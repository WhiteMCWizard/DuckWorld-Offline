using System.Collections;
using SLAM;
using SLAM.Slinq;
using UnityEngine;

public class FYTreeBlossom : MonoBehaviour
{
	[Tooltip("How many seconds it takes to show the blossom")]
	[SerializeField]
	private float blossomDuration;

	[SerializeField]
	private AnimationCurve blossomCurve;

	private bool isToggled;

	[ContextMenu("Toggle blossom")]
	private void ToggleBlossom()
	{
		if (isToggled)
		{
			StartCoroutine(fadeBlossom(1f, 0f));
		}
		else
		{
			StartCoroutine(fadeBlossom(0f, 1f));
		}
		isToggled = !isToggled;
	}

	private IEnumerator fadeBlossom(float startBlend, float endBlend)
	{
		Material[] mats = (from m in GetComponentsInChildren<Renderer>().SelectMany((Renderer r) => r.materials)
			where m.HasProperty("_Blend")
			select m).ToArray();
		Stopwatch sw = new Stopwatch(blossomDuration);
		while (!sw.Expired)
		{
			yield return null;
			float blend = Mathf.Lerp(startBlend, endBlend, blossomCurve.Evaluate(sw.Progress));
			for (int i = 0; i < mats.Length; i++)
			{
				mats[i].SetFloat("_Blend", blend);
			}
		}
	}
}
