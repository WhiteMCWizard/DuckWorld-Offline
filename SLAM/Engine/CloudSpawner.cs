using System.Collections.Generic;
using UnityEngine;

namespace SLAM.Engine;

[RequireComponent(typeof(BoxCollider))]
public class CloudSpawner : MonoBehaviour
{
	private struct ActiveCloud
	{
		public GameObject Object;

		public Vector3 Velocity;

		public Renderer renderr;
	}

	[SerializeField]
	[HideInInspector]
	private Bounds cloudArea;

	[SerializeField]
	private GameObject[] cloudPrefabs;

	[SerializeField]
	private float spawnIntervalMax;

	[SerializeField]
	private float spawnIntervalMin;

	[Range(0.1f, 100f)]
	[SerializeField]
	private float cloudSpeed;

	[SerializeField]
	private bool flip = true;

	[SerializeField]
	private bool rightToLeft;

	private List<ActiveCloud> activeClouds = new List<ActiveCloud>();

	private BoxCollider cloudCollider;

	private float ScaledSpeed => cloudSpeed * base.transform.lossyScale.x;

	private void Start()
	{
		cloudCollider = GetComponent<BoxCollider>();
		if (cloudCollider == null)
		{
			Debug.LogError("Hey buddy, you need to upgrade the CloudSpawner using the context menu", this);
			return;
		}
		cloudCollider.isTrigger = true;
		cloudArea = cloudCollider.bounds;
		prewarm();
	}

	private void OnEnable()
	{
		CancelInvoke("spawnCloud");
		Invoke("spawnCloud", Random.Range(spawnIntervalMin, spawnIntervalMax));
	}

	private void Update()
	{
		for (int i = 0; i < activeClouds.Count; i++)
		{
			ActiveCloud item = activeClouds[i];
			item.Object.transform.position += item.Velocity * Time.deltaTime;
			if (!item.renderr.bounds.Intersects(cloudArea))
			{
				Object.Destroy(item.Object);
				activeClouds.Remove(item);
				i--;
			}
		}
	}

	[ContextMenu("Update to use BoxCollider")]
	private void upgrade()
	{
		base.transform.position = Vector3.zero;
		BoxCollider boxCollider = GetComponent<BoxCollider>();
		if (boxCollider == null)
		{
			boxCollider = base.gameObject.AddComponent<BoxCollider>();
		}
		Vector3 center = cloudArea.center;
		center.x /= base.transform.lossyScale.x;
		center.y /= base.transform.lossyScale.y;
		center.z /= base.transform.lossyScale.z;
		boxCollider.center = center;
		Vector3 size = cloudArea.size;
		size.x /= base.transform.lossyScale.x;
		size.y /= base.transform.lossyScale.y;
		size.z /= base.transform.lossyScale.z;
		boxCollider.size = size;
		boxCollider.isTrigger = true;
		cloudSpeed /= 2f;
	}

	private void OnDrawGizmosSelected()
	{
		if (GetComponent<BoxCollider>() == null)
		{
			Gizmos.DrawWireCube(cloudArea.center, cloudArea.size);
		}
	}

	private void prewarm()
	{
		float num = 0f;
		for (float num2 = cloudArea.size.x / ScaledSpeed; num < num2; num += Random.Range(spawnIntervalMin, spawnIntervalMax))
		{
			Vector3 position = new Vector3(cloudArea.min.x + num * ScaledSpeed, Random.Range(cloudArea.min.y, cloudArea.max.y), Random.Range(cloudArea.min.z, cloudArea.max.z));
			Vector2 velocity = new Vector2(ScaledSpeed, 0f);
			if (rightToLeft)
			{
				velocity.x = 0f - velocity.x;
			}
			activeClouds.Add(createCloud(position, velocity));
		}
	}

	private void spawnCloud()
	{
		if (base.gameObject.activeSelf)
		{
			Vector3 position = new Vector3(cloudArea.min.x, Random.Range(cloudArea.min.y, cloudArea.max.y), Random.Range(cloudArea.min.z, cloudArea.max.z));
			Vector2 velocity = new Vector2(ScaledSpeed, 0f);
			if (rightToLeft)
			{
				position.x = cloudArea.max.x;
				velocity.x = 0f - velocity.x;
			}
			activeClouds.Add(createCloud(position, velocity));
			Invoke("spawnCloud", Random.Range(spawnIntervalMin, spawnIntervalMax));
		}
	}

	private ActiveCloud createCloud(Vector3 position, Vector2 velocity)
	{
		GameObject gameObject = Object.Instantiate(cloudPrefabs.GetRandom());
		ScaleSize(gameObject.transform);
		gameObject.transform.parent = base.transform;
		gameObject.transform.position = position;
		if (flip && Random.value > 0.5f)
		{
			gameObject.transform.localScale = new Vector3(0f - gameObject.transform.localScale.x, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
		}
		Object.Destroy(gameObject.GetComponent<BoxCollider>());
		return new ActiveCloud
		{
			Object = gameObject,
			Velocity = velocity,
			renderr = gameObject.GetComponentInChildren<Renderer>()
		};
	}

	private void ScaleSize(Transform t)
	{
		Vector3 localScale = t.localScale;
		localScale.x *= base.transform.lossyScale.x;
		localScale.y *= base.transform.lossyScale.y;
		localScale.z *= base.transform.lossyScale.z;
		t.localScale = localScale;
	}
}
