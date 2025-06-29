using System.Collections;
using SLAM.Engine;
using SLAM.Slinq;
using SLAM.ToolTips;
using UnityEngine;

namespace SLAM.Fruityard;

public class FY_TutorialView : TutorialView
{
	[SerializeField]
	private ToolTip helperToolTip;

	[SerializeField]
	private ToolTip spotToolTip;

	[SerializeField]
	private ToolTip seedToolTip;

	[SerializeField]
	private Vector3 spotOffset;

	[SerializeField]
	private Vector3 helperOffset;

	[SerializeField]
	private Transform cherryTarget;

	private FYSpot spot;

	private FYHelper[] helpers;

	private FYHelper selectedHelper;

	private FYHelper correctHelper;

	private void OnEnable()
	{
		GameEvents.Subscribe<FruityardGame.TreeTaskFailedEvent>(onTaskFailed);
		GameEvents.Subscribe<FruityardGame.HelperSelectedEvent>(onHelperSelected);
		GameEvents.Subscribe<FruityardGame.TreeTaskSucceededEvent>(onTaskSucceeded);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<FruityardGame.TreeTaskFailedEvent>(onTaskFailed);
		GameEvents.Unsubscribe<FruityardGame.HelperSelectedEvent>(onHelperSelected);
		GameEvents.Unsubscribe<FruityardGame.TreeTaskSucceededEvent>(onTaskSucceeded);
	}

	protected override void Start()
	{
		spot = Object.FindObjectOfType<FYSpot>();
		helpers = Object.FindObjectsOfType<FYHelper>();
		StartCoroutine(step1());
		StartCoroutine(step2());
	}

	private IEnumerator step1()
	{
		while (spot.RequiredActionIndex < spot.RequiredActions.Length)
		{
			if (helpers.Count((FYHelper h) => h.IsPerformingAction) > 0)
			{
				helperToolTip.Hide();
				spotToolTip.Hide();
			}
			else if (selectedHelper == correctHelper)
			{
				if (!spotToolTip.IsVisible)
				{
					helperToolTip.Hide();
					spotToolTip.Show(spot.transform, spotOffset);
				}
			}
			else if (!helperToolTip.IsVisible)
			{
				spotToolTip.Hide();
				helperToolTip.Show(correctHelper.transform, helperOffset);
			}
			correctHelper = helpers.FirstOrDefault((FYHelper h) => h.GetAppropiateAction(spot) != FruityardGame.FYTreeAction.None);
			yield return null;
		}
	}

	private IEnumerator step2()
	{
		yield return CoroutineUtils.WaitForGameEvent<FruityardGame.ShowSeedViewEvent>();
		seedToolTip.Show(cherryTarget);
		yield return CoroutineUtils.WaitForGameEvent<FruityardGame.SeedTreeEvent>();
		seedToolTip.Hide();
		StartCoroutine(step2());
	}

	private void onHelperSelected(FruityardGame.HelperSelectedEvent evt)
	{
		selectedHelper = evt.Helper;
	}

	private void onTaskSucceeded(FruityardGame.TreeTaskSucceededEvent evt)
	{
		helperToolTip.Hide();
		spotToolTip.Hide();
	}

	private void onTaskFailed(FruityardGame.TreeTaskFailedEvent evt)
	{
		helperToolTip.Hide();
		spotToolTip.Hide();
	}
}
