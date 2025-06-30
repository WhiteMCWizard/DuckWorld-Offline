using SLAM.Engine;
using UnityEngine;

namespace SLAM.HigherThan;

public class HTgameView : View
{
	[SerializeField]
	private UITexture startView;

	[SerializeField]
	private UILabel leftEquationSide;

	[SerializeField]
	private UILabel rightEquationSide;

	[SerializeField]
	private UILabel lblLevel;

	[SerializeField]
	private UIProgressBar timer;

	[SerializeField]
	private UIToggle[] correctAnswersInLevel;

	[SerializeField]
	private GameObject answerPlingPrefab;

	[SerializeField]
	private GameObject timePlingPrefab;

	[SerializeField]
	private GameObject boardPanel;

	private float maxTime;

	private GameObject answerPling;

	protected override void Start()
	{
		startView.gameObject.SetActive(value: false);
	}

	private void OnEnable()
	{
		GameEvents.Subscribe<HigherThanGame.EquationAnsweredEvent>(onEquationAnswered);
		GameEvents.Subscribe<HigherThanGame.NewEquationEvent>(onNewEquation);
		GameEvents.Subscribe<HigherThanGame.TimeChangedEvent>(onTimeChanged);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<HigherThanGame.EquationAnsweredEvent>(onEquationAnswered);
		GameEvents.Unsubscribe<HigherThanGame.NewEquationEvent>(onNewEquation);
		GameEvents.Unsubscribe<HigherThanGame.TimeChangedEvent>(onTimeChanged);
	}

	private void onNewEquation(HigherThanGame.NewEquationEvent evt)
	{
		leftEquationSide.text = evt.Equation.leftText;
		rightEquationSide.text = evt.Equation.rightText;
		startView.gameObject.SetActive(value: false);
		lblLevel.text = $"{Controller<HigherThanGame>().CorrectAnswers}/{Controller<HigherThanGame>().CorrectAnswersToReach}";
	}

	private void onEquationAnswered(HigherThanGame.EquationAnsweredEvent evt)
	{
		if (answerPling != null)
		{
			Object.Destroy(answerPling);
		}
		answerPling = NGUITools.AddChild(boardPanel, answerPlingPrefab);
		HTanswerPling component = answerPling.GetComponent<HTanswerPling>();
		component.DoIt(evt);
		for (int i = 0; i < correctAnswersInLevel.Length; i++)
		{
			correctAnswersInLevel[i].value = i < evt.Progress;
		}
	}

	private void onTimeChanged(HigherThanGame.TimeChangedEvent evt)
	{
		GameObject gameObject = NGUITools.AddChild(boardPanel, timePlingPrefab);
		gameObject.GetComponent<HTtimePling>().DoIt(evt.DeltaChange);
	}

	public void UpdateTimer(float timeLeft)
	{
		timer.value = timeLeft / maxTime;
	}

	public void UpdateMaxTime(float newMaxTime)
	{
		maxTime = newMaxTime;
	}

	public void OnLowerThenClicked()
	{
		Controller<HigherThanGame>().AnswerEquation(HigherThanGame.Answer.Lower);
	}

	public void OnHigherThenClicked()
	{
		Controller<HigherThanGame>().AnswerEquation(HigherThanGame.Answer.Higher);
	}
}
