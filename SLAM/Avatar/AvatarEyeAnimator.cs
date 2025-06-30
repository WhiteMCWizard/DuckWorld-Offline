using System.Collections;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.Avatar;

public class AvatarEyeAnimator : MonoBehaviour
{
	[SerializeField]
	private Transform animationNullObject;

	[SerializeField]
	private Material material;

	[SerializeField]
	private Vector2 uvOffset;

	[SerializeField]
	private int tileXCount = 4;

	[SerializeField]
	private int tileYCount = 4;

	private Animator animator;

	private Renderer rendererItem;

	private int materialIndex = -1;

	private float nextBlinkTime;

	private float blinkTime = 0.2f;

	private float minBlinkInterval = 2f;

	private float maxBlinkInterval = 10f;

	private void OnEnable()
	{
		StartCoroutine(doEyeAnimationSequence());
	}

	private IEnumerator doEyeAnimationSequence()
	{
		findCorrectMaterialIndex();
		yield return null;
		yield return null;
		if (rendererItem != null && materialIndex > -1)
		{
			material = rendererItem.materials[materialIndex];
		}
		if (material == null)
		{
			Renderer eyeRenderer = GetComponentsInChildren<Renderer>().FirstOrDefault((Renderer r) => r.name.Contains("Eyes"));
			if (eyeRenderer != null)
			{
				eyeRenderer.enabled = false;
			}
			Debug.LogWarning("No material so i quit!", this);
			yield break;
		}
		animator = GetComponentInParent<Animator>();
		float spriteWidth = material.mainTexture.width;
		float spriteHeight = material.mainTexture.height;
		float frameWidth = spriteWidth / (float)tileXCount;
		float frameHeight = spriteHeight / (float)tileYCount;
		nextBlinkTime = Time.time + 5f;
		float x = 0f;
		float y = 0f;
		while (true)
		{
			if (animationNullObject == null || animationNullObject.localPosition.x < 0f || animationNullObject.localPosition.y < 0f)
			{
				doEyeIdle(out x, out y, material);
			}
			else if (!isAnimatorTransitioning())
			{
				x = animationNullObject.localPosition.x;
				y = 0f - animationNullObject.localPosition.y;
				material.mainTextureScale = new Vector2(animationNullObject.localPosition.z, material.mainTextureScale.y);
				if (animationNullObject.localPosition.z < 0f)
				{
					x += 1f;
				}
			}
			material.mainTextureOffset = new Vector2(x * frameWidth / spriteWidth, y * frameHeight / spriteHeight);
			yield return null;
		}
	}

	private bool isAnimatorTransitioning()
	{
		for (int i = 0; i < animator.layerCount; i++)
		{
			if (animator.IsInTransition(i))
			{
				return true;
			}
		}
		return false;
	}

	private void findCorrectMaterialIndex()
	{
		Renderer[] componentsInChildren = GetComponentsInChildren<Renderer>();
		foreach (Renderer renderer in componentsInChildren)
		{
			for (int j = 0; j < renderer.sharedMaterials.Length; j++)
			{
				if (renderer.sharedMaterials[j] == material || (material == null && renderer.sharedMaterials[j].name.ToLower().Contains("eye")))
				{
					rendererItem = renderer;
					materialIndex = j;
					return;
				}
			}
		}
		Debug.LogError("Couldnt find " + animationNullObject.name + " material for " + base.name, this);
	}

	private void doEyeIdle(out float x, out float y, Material mat)
	{
		mat.mainTextureScale = new Vector2(1f, mat.mainTextureScale.y);
		y = 0f;
		if (Time.time > nextBlinkTime)
		{
			float num = (Time.time - nextBlinkTime) / blinkTime;
			x = Mathf.Round((float)tileXCount * num);
			if (num > 1f)
			{
				nextBlinkTime = Time.time + Random.Range(minBlinkInterval, maxBlinkInterval);
			}
		}
		else
		{
			x = 0f;
		}
		x += uvOffset.x;
		y += uvOffset.y;
	}
}
