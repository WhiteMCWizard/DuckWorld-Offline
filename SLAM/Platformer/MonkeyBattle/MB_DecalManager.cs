using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace SLAM.Platformer.MonkeyBattle;

public class MB_DecalManager : SingletonMonobehaviour<MB_DecalManager>
{
	[SerializeField]
	private int maxDecalCount;

	[SerializeField]
	private float fadeOutDuration = 0.5f;

	[SerializeField]
	private bool castShadows;

	[SerializeField]
	private Material material;

	private List<GameObject> decalObjects = new List<GameObject>();

	public GameObject SpawnDecalAt(GameObject decalPrefab, Vector3 position, Quaternion rotation, Vector3 minScale, Vector3 maxScale)
	{
		while (decalObjects.Count > maxDecalCount)
		{
			StartCoroutine(doFadeOutAndDestroy(decalObjects[0]));
			decalObjects.RemoveAt(0);
		}
		GameObject gameObject = Object.Instantiate(decalPrefab);
		gameObject.transform.position = position;
		gameObject.transform.localScale = Vector3.Lerp(minScale, maxScale, Random.value);
		gameObject.transform.rotation = rotation;
		decalObjects.Add(gameObject);
		if (material != null)
		{
			gameObject.GetComponent<Renderer>().material.shader = material.shader;
		}
		gameObject.GetComponent<Renderer>().shadowCastingMode = (castShadows ? ShadowCastingMode.On : ShadowCastingMode.Off);
		return gameObject;
	}

	private IEnumerator doFadeOutAndDestroy(GameObject gameObject)
	{
		Stopwatch sw = new Stopwatch(fadeOutDuration);
		SpriteRenderer sprite = gameObject.GetComponent<SpriteRenderer>();
		while ((bool)sw)
		{
			yield return null;
			sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1f - sw.Progress);
		}
		Object.Destroy(gameObject);
	}
}
