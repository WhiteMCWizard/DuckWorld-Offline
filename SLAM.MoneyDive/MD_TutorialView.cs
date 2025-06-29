using System.Collections;
using SLAM.Engine;
using SLAM.ToolTips;
using UnityEngine;

namespace SLAM.MoneyDive;

public class MD_TutorialView : TutorialView
{
	[SerializeField]
	private ToolTip clickHereToolTip;

	[SerializeField]
	private ToolTip pointerToolTip;

	[SerializeField]
	private MD_HUDView hudView;

	private bool playerUnderstandMouseControls;

	private void OnEnable()
	{
		GameEvents.Subscribe<MD_Controller.EquationVisibleEvent>(onEquationVisible);
		GameEvents.Subscribe<MD_Controller.EquationAnsweredEvent>(onEquationAnswered);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<MD_Controller.EquationVisibleEvent>(onEquationVisible);
		GameEvents.Unsubscribe<MD_Controller.EquationAnsweredEvent>(onEquationAnswered);
	}

	protected override void Start()
	{
		base.Start();
	}

	private IEnumerator step1(Transform buttonWithCorrectAnswer)
	{
		pointerToolTip.Show(buttonWithCorrectAnswer);
		clickHereToolTip.Show(pointerToolTip.GO.transform);
		yield return CoroutineUtils.WaitForGameEvent<MD_Controller.EquationAnsweredEvent>();
		pointerToolTip.Hide();
		clickHereToolTip.Hide();
	}

	private void onEquationVisible(MD_Controller.EquationVisibleEvent evt)
	{
		if (!playerUnderstandMouseControls)
		{
			StartCoroutine(step1(evt.ButtonWithCorrectAnwer.transform));
		}
	}

	private void onEquationAnswered(MD_Controller.EquationAnsweredEvent evt)
	{
		playerUnderstandMouseControls = playerUnderstandMouseControls || evt.IsCorrectAnswer;
	}
}
