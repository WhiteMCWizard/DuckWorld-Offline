using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SLAM.Fruityard;

public class FYHelperManager : MonoBehaviour
{
	[SerializeField]
	private FYHelper startingHelper;

	[SerializeField]
	private Vector3 activeIndicatorOffset;

	[SerializeField]
	private GameObject activeIndicator;

	[SerializeField]
	private AudioClip helperSelectedAudio;

	private List<Transform> busyTransforms;

	private FYHelper clickedHelper;

	private bool inputAllowed;

	private void Start()
	{
		busyTransforms = new List<Transform>();
	}

	private void OnEnable()
	{
		GameEvents.Subscribe<FruityardGame.LevelStartedEvent>(onLevelStarted);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<FruityardGame.LevelStartedEvent>(onLevelStarted);
	}

	private void Update()
	{
		if (!inputAllowed || !Input.GetMouseButtonDown(0))
		{
			return;
		}
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (!Physics.Raycast(ray, out var hitInfo))
		{
			return;
		}
		if (hitInfo.transform.HasComponent<FYSpot>() && clickedHelper != null && !clickedHelper.IsPerformingAction && !isSpotBusy(hitInfo.transform))
		{
			clickedHelper.PerformActionAtSpot(hitInfo.transform.GetComponent<FYSpot>());
			StartCoroutine(trackSpotBusy(clickedHelper, hitInfo.transform));
			return;
		}
		if (hitInfo.transform.HasComponent<FYHelper>())
		{
			setActiveHelper(hitInfo.transform.GetComponent<FYHelper>());
			return;
		}
		if (hitInfo.transform.HasComponent<FYIconListener>())
		{
			FYIconListener component = hitInfo.transform.GetComponent<FYIconListener>();
			if (component.helper != null)
			{
				setActiveHelper(hitInfo.transform.GetComponent<FYIconListener>().helper);
			}
			return;
		}
		ray = UICamera.currentCamera.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out hitInfo) && hitInfo.collider.transform.HasComponent<FYIconListener>())
		{
			FYIconListener component2 = hitInfo.collider.transform.GetComponent<FYIconListener>();
			if (component2.spot != null && !isSpotBusy(component2.spot.transform))
			{
				clickedHelper.PerformActionAtSpot(component2.spot);
				StartCoroutine(trackSpotBusy(clickedHelper, component2.spot.transform));
			}
		}
	}

	private void setActiveHelper(FYHelper helper)
	{
		if (clickedHelper != helper)
		{
			if (clickedHelper != null)
			{
				Transform transform = clickedHelper.transform.FindChildRecursively("head_jnt");
				transform.FindChild("task_balloon").GetComponent<SpriteRenderer>().enabled = true;
			}
			clickedHelper = helper;
			activeIndicator.transform.parent = helper.transform.FindChildRecursively("head_jnt");
			activeIndicator.transform.localPosition = activeIndicatorOffset;
			activeIndicator.transform.parent.FindChild("task_balloon").GetComponent<SpriteRenderer>().enabled = false;
			AudioController.Play(helperSelectedAudio.name, base.transform);
			FruityardGame.HelperSelectedEvent helperSelectedEvent = new FruityardGame.HelperSelectedEvent();
			helperSelectedEvent.Helper = helper;
			GameEvents.Invoke(helperSelectedEvent);
		}
	}

	private void onLevelStarted(FruityardGame.LevelStartedEvent evt)
	{
		setActiveHelper(startingHelper);
		inputAllowed = true;
	}

	private bool isSpotBusy(Transform transform)
	{
		return busyTransforms.Contains(transform);
	}

	private IEnumerator trackSpotBusy(FYHelper clickedHelper, Transform transform)
	{
		busyTransforms.Add(transform);
		while (clickedHelper.IsPerformingAction)
		{
			yield return null;
		}
		busyTransforms.Remove(transform);
	}
}
