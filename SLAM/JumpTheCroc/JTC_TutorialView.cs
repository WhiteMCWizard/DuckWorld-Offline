using System.Collections;
using SLAM.Engine;
using SLAM.ToolTips;
using UnityEngine;

namespace SLAM.JumpTheCroc;

public class JTC_TutorialView : TutorialView
{
	[SerializeField]
	private ToolTip clickHereToolTip;

	[SerializeField]
	private ToolTip pointerToolTip;

	private bool playerUnderstandMouseControls;

	private bool answeredCorrect;

	private int hintLimit = 3;

	private int hintCount;

	private void OnEnable()
	{
		GameEvents.Subscribe<JumpTheCrocGame.EquationVisibleEvent>(onEquationVisible);
		GameEvents.Subscribe<JumpTheCrocGame.EquationAnsweredEvent>(onEquationAnswered);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<JumpTheCrocGame.EquationVisibleEvent>(onEquationVisible);
		GameEvents.Unsubscribe<JumpTheCrocGame.EquationAnsweredEvent>(onEquationAnswered);
	}

	protected override void Start()
	{
		base.Start();
	}

	private IEnumerator step1(Transform buttonWithCorrectAnswer)
	{
		pointerToolTip.Show(buttonWithCorrectAnswer);
		clickHereToolTip.Show(pointerToolTip.GO.transform);
		while (!answeredCorrect)
		{
			yield return null;
		}
		pointerToolTip.Hide();
		clickHereToolTip.Hide();
	}

	private void onEquationVisible(JumpTheCrocGame.EquationVisibleEvent evt)
	{
		answeredCorrect = false;
		if (!playerUnderstandMouseControls)
		{
			StartCoroutine(step1(evt.PlatformWithCorrectAnwer.transform));
		}
	}

	private void onEquationAnswered(JumpTheCrocGame.EquationAnsweredEvent evt)
	{
		answeredCorrect = evt.IsCorrectAnswer;
		playerUnderstandMouseControls = evt.IsCorrectAnswer && ++hintCount >= hintLimit;
	}
}
