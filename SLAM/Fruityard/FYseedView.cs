using System.Collections;
using SLAM.Engine;
using UnityEngine;

namespace SLAM.Fruityard;

public class FYseedView : View
{
	[SerializeField]
	private float fadeLength;

	[SerializeField]
	private AnimationCurve fadeCurve;

	[SerializeField]
	private AudioClip appearSound;

	private FYSpot spot;

	public void OnIconClicked(FruityardGame.FYTreeType treeType)
	{
		spot.TreeSelected(treeType);
		StartCoroutine(fadeAlpha(1f, 0f));
		GetComponentInChildren<UIPanel>().alpha = 0f;
		FruityardGame.SeedTreeEvent seedTreeEvent = new FruityardGame.SeedTreeEvent();
		seedTreeEvent.TreeType = treeType;
		GameEvents.Invoke(seedTreeEvent);
	}

	private void OnEnable()
	{
		GameEvents.Subscribe<FruityardGame.ShowSeedViewEvent>(onShowSeedView);
		GetComponentInChildren<UIPanel>().alpha = 0f;
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<FruityardGame.ShowSeedViewEvent>(onShowSeedView);
	}

	private void onShowSeedView(FruityardGame.ShowSeedViewEvent obj)
	{
		spot = obj.Spot;
		reposition();
		AudioController.Play(appearSound.name);
		StartCoroutine(fadeAlpha(0f, 1f));
	}

	private IEnumerator fadeAlpha(float srcAlpha, float dstAlpha)
	{
		Stopwatch sw = new Stopwatch(fadeLength);
		UIPanel panel = GetComponentInChildren<UIPanel>();
		while (!sw.Expired)
		{
			yield return null;
			panel.alpha = Mathf.Lerp(srcAlpha, dstAlpha, fadeCurve.Evaluate(sw.Progress));
		}
		if (dstAlpha < 0.5f)
		{
			base.transform.position = new Vector3(-Screen.width, -Screen.height);
		}
	}

	private void reposition()
	{
		Vector3 position = UICamera.currentCamera.ScreenToWorldPoint(Camera.main.WorldToScreenPoint(spot.transform.position + Vector3.up * 1f));
		position.z = 0f;
		base.transform.position = position;
	}
}
