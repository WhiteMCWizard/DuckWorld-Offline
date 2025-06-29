using System.Collections;
using SLAM.Platformer;
using UnityEngine;

namespace SLAM.BatCave;

public class BC_ExoticDoor : P_Trigger
{
	[SerializeField]
	private int animalsToUnlockCount;

	[SerializeField]
	private Transform doorPivot;

	[SerializeField]
	private float closedAngle;

	[SerializeField]
	private float openAngle;

	[SerializeField]
	private AnimationCurve openAnimCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

	[SerializeField]
	private float openAnimTime = 0.8f;

	[SerializeField]
	private GameObject infoPanel;

	[SerializeField]
	private GameObject[] objectsActiveOnOpen;

	[SerializeField]
	private GameObject[] objectsActiveOnClose;

	private BatCaveGame game;

	private bool isOpen;

	protected override void Start()
	{
		base.Start();
		game = Object.FindObjectOfType<BatCaveGame>();
		if (infoPanel != null)
		{
			Renderer component = infoPanel.GetComponent<Renderer>();
			Material[] materials = component.materials;
			foreach (Material material in materials)
			{
				if (material.name.Contains("BC_Numbers"))
				{
					material.SetTextureOffset("_MainTex", new Vector2((float)animalsToUnlockCount * 0.1f, 0f));
				}
			}
		}
		if (animalsToUnlockCount > 0)
		{
			close();
		}
	}

	protected override void OnTriggerEnter(Collider other)
	{
		base.OnTriggerEnter(other);
		if (!isOpen && animalsToUnlockCount <= game.ExoticAnimalCount)
		{
			open();
		}
	}

	private void open()
	{
		isOpen = true;
		AudioController.Play("BC_door_open", base.transform);
		GameObject[] array = objectsActiveOnOpen;
		foreach (GameObject gameObject in array)
		{
			gameObject.SetActive(value: true);
		}
		GameObject[] array2 = objectsActiveOnClose;
		foreach (GameObject gameObject2 in array2)
		{
			gameObject2.SetActive(value: false);
		}
		StartCoroutine(doDoorRoutine(closedAngle, openAngle, openAnimTime, openAnimCurve, doorPivot));
	}

	private void close()
	{
		isOpen = false;
		GameObject[] array = objectsActiveOnOpen;
		foreach (GameObject gameObject in array)
		{
			gameObject.SetActive(value: false);
		}
		GameObject[] array2 = objectsActiveOnClose;
		foreach (GameObject gameObject2 in array2)
		{
			gameObject2.SetActive(value: true);
		}
		StartCoroutine(doDoorRoutine(openAngle, closedAngle, 0.01f, openAnimCurve, doorPivot));
	}

	private IEnumerator doDoorRoutine(float fromAngle, float toAngle, float duration, AnimationCurve curve, Transform target)
	{
		float time = 0f;
		while (time < duration)
		{
			time += Time.deltaTime;
			target.localRotation = Quaternion.AngleAxis(Mathf.Lerp(fromAngle, toAngle, curve.Evaluate(time / duration)), target.up);
			yield return null;
		}
	}
}
