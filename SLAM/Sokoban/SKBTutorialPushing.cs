using System;
using System.Collections;
using SLAM.Engine;
using SLAM.Slinq;
using SLAM.ToolTips;
using UnityEngine;

namespace SLAM.Sokoban;

public class SKBTutorialPushing : TutorialView
{
	[SerializeField]
	private ToolTip arrowTooltip;

	[SerializeField]
	private ToolTip moveTooltip;

	[SerializeField]
	private SKBAvatarController avatarController;

	[SerializeField]
	private int maxCrateCount = 3;

	private int crateCount;

	private void OnEnable()
	{
		GameEvents.Subscribe<SokobanGameController.MarkerCompletedEvent>(onMarkerCompleted);
		GameEvents.Subscribe<SokobanGameController.MarkerRemovedEvent>(onMarkerRemoved);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<SokobanGameController.MarkerCompletedEvent>(onMarkerCompleted);
		GameEvents.Unsubscribe<SokobanGameController.MarkerRemovedEvent>(onMarkerRemoved);
	}

	private void onMarkerCompleted(SokobanGameController.MarkerCompletedEvent obj)
	{
		if (++crateCount >= maxCrateCount)
		{
			Close();
		}
	}

	private void onMarkerRemoved(SokobanGameController.MarkerRemovedEvent obj)
	{
		crateCount--;
	}

	public override void Open(Callback callback, bool immediately)
	{
		base.Open(callback, immediately);
		StartCoroutine(step1());
	}

	private IEnumerator step1()
	{
		moveTooltip.Show(avatarController.transform);
		while (!avatarController.IsMoving)
		{
			yield return null;
		}
		moveTooltip.Hide();
		StartCoroutine(step2());
	}

	private IEnumerator step2()
	{
		SKBCrate crate = findNearestComponent(avatarController.transform.position, (SKBCrate c) => !c.Completed);
		if (crate == null)
		{
			yield break;
		}
		SKBMarker marker = findNearestComponent(crate.transform.position, (SKBMarker m) => !m.Completed && m.MarkerType == crate.MarkerType);
		if (!(marker == null))
		{
			arrowTooltip.Show(crate.transform);
			float angle = 0f;
			if (crate.transform.position.x > marker.transform.position.x)
			{
				angle = 180f;
			}
			arrowTooltip.GO.transform.rotation = Quaternion.Euler(0f, 0f, angle);
			while (crate != null && Mathf.Abs(crate.transform.position.x - marker.transform.position.x) > 0.5f && !crate.Completed)
			{
				yield return null;
			}
			arrowTooltip.Hide();
			if (crate != null)
			{
				StartCoroutine(step3(crate, marker));
			}
			else
			{
				StartCoroutine(step2());
			}
		}
	}

	private IEnumerator step3(SKBCrate crate, SKBMarker marker)
	{
		arrowTooltip.Show(crate.transform);
		float angle = Mathf.Sign(crate.transform.position.z - marker.transform.position.z) * -90f;
		arrowTooltip.GO.transform.localRotation = Quaternion.Euler(0f, 0f, angle);
		while (crate != null && Mathf.Abs(crate.transform.position.z - marker.transform.position.z) > 0.5f && !crate.Completed)
		{
			yield return null;
		}
		arrowTooltip.Hide();
		StartCoroutine(step2());
	}

	private T findNearestComponent<T>(Vector3 pos, Func<T, bool> compare) where T : Component
	{
		return UnityEngine.Object.FindObjectsOfType<T>().Where(compare).Min((T c) => (pos - c.transform.position).sqrMagnitude);
	}
}
