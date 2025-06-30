using SLAM.Engine;
using SLAM.ToolTips;
using UnityEngine;

namespace SLAM.HigherThan;

public class HTtutorialView : TutorialView
{
	[SerializeField]
	private ToolTip clickHereToolTip;

	[SerializeField]
	private ToolTip pointerToolTip;

	private bool playerUnderstandMouseControls;

	private void OnEnable()
	{
		GameEvents.Subscribe<HigherThanGame.NewEquationEvent>(onEquationVisible);
		GameEvents.Subscribe<HigherThanGame.EquationAnsweredEvent>(onEquationAnswered);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<HigherThanGame.NewEquationEvent>(onEquationVisible);
		GameEvents.Unsubscribe<HigherThanGame.EquationAnsweredEvent>(onEquationAnswered);
	}

	private void step1(Transform buttonWithCorrectAnswer)
	{
		pointerToolTip.Hide();
		clickHereToolTip.Hide();
		pointerToolTip.Show(buttonWithCorrectAnswer);
		clickHereToolTip.Show(pointerToolTip.GO.transform);
	}

	private void onEquationVisible(HigherThanGame.NewEquationEvent evt)
	{
		if (!playerUnderstandMouseControls)
		{
			step1(evt.ButtonWithCorrectAnwer.transform);
			return;
		}
		pointerToolTip.Hide();
		clickHereToolTip.Hide();
	}

	private void onEquationAnswered(HigherThanGame.EquationAnsweredEvent evt)
	{
		playerUnderstandMouseControls = playerUnderstandMouseControls || evt.IsCorrectAnswer;
	}
}
