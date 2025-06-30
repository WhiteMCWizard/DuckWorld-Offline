using System.Collections;
using AnimationOrTween;
using SLAM.Engine;
using SLAM.Utils;
using UnityEngine;

namespace SLAM.MoneyDive;

public class MD_HUDView : HUDView
{
	[SerializeField]
	private GameObject pointsPlingPrefab;

	[SerializeField]
	private GameObject pointsParent;

	[SerializeField]
	private GameObject bonusPlingPrefab;

	[SerializeField]
	private Transform bonusParent;

	[SerializeField]
	private UILabel bonusValue;

	[SerializeField]
	private UILabel currentScoreLabel;

	[SerializeField]
	private UILabel targetTrickCountLabel;

	[SerializeField]
	private UITweener trickCountChangeTween;

	[SerializeField]
	private float bonusHideTime = 0.75f;

	[SerializeField]
	private float pointsPerSecond = 800f;

	[SerializeField]
	private UITweener equationHider;

	[SerializeField]
	private UILabel sumValue;

	[SerializeField]
	private UILabel answerValue1;

	[SerializeField]
	private UILabel answerValue2;

	[SerializeField]
	private UILabel answerValue3;

	private float score;

	private int endScore;

	private UIButton btnA;

	private UIButton btnB;

	private UIButton btnC;

	private bool equationVisible;

	private UIButton btnWithCorrectAnswer;

	private Equation currentEquation;

	private void Awake()
	{
		btnA = answerValue1.GetComponentInParent<UIButton>();
		btnB = answerValue2.GetComponentInParent<UIButton>();
		btnC = answerValue3.GetComponentInParent<UIButton>();
	}

	protected override void Start()
	{
		base.Start();
		currentScoreLabel.text = Mathf.CeilToInt(score).ToString();
		equationHider.AddOnFinished(EquationVisible);
	}

	protected override void Update()
	{
		base.Update();
		if (score < (float)endScore)
		{
			float num = pointsPerSecond * Time.deltaTime;
			score = Mathf.Clamp(score + num, 0f, endScore);
			currentScoreLabel.text = Mathf.CeilToInt(score).ToString();
		}
		if (SLAMInput.Provider.GetButtonDown(SLAMInput.Button.Left))
		{
			OnAnswerOneClicked();
		}
		else if (SLAMInput.Provider.GetButtonDown(SLAMInput.Button.Down))
		{
			OnAnswerTwoClicked();
		}
		else if (SLAMInput.Provider.GetButtonDown(SLAMInput.Button.Right))
		{
			OnAnswerThreeClicked();
		}
	}

	public void DoTrickPointsPling(int score)
	{
		GameObject gameObject = NGUITools.AddChild(pointsParent, pointsPlingPrefab);
		gameObject.GetComponent<MD_PointsPling>().Pling(score, currentScoreLabel.transform, this);
	}

	public void UpdateTrickProgress(int currentTrickCount, int targetTrickCount)
	{
		targetTrickCountLabel.text = currentTrickCount + "/" + targetTrickCount;
		trickCountChangeTween.ResetToBeginning();
		trickCountChangeTween.PlayForward();
	}

	public void UpdateBonusCounter(int bonus)
	{
		bonusValue.text = "+" + bonus;
		StartCoroutine(HideBonus(bonusHideTime));
	}

	public void PlayBonusLost(int bonus)
	{
		CreateNewBonusPling().BonusLostPling(bonus);
	}

	public void PlayBonusReceived(int bonus)
	{
		CreateNewBonusPling().BonusReceivedPling(bonus, currentScoreLabel.transform, this);
	}

	public void AddPoints(int points)
	{
		endScore += points;
	}

	public void HideEquation()
	{
		equationHider.PlayForward();
	}

	public void ShowEquation()
	{
		equationHider.PlayReverse();
	}

	public void SetEquation(Equation equation)
	{
		float num = Random.Range(0, 3);
		sumValue.text = $"{equation.NumberA} {equation.SignString} {equation.NumberB} =";
		answerValue1.text = ((num != 0f) ? equation.WrongAnswer.ToString() : equation.CorrectAnswer.ToString());
		answerValue2.text = ((num != 1f) ? equation.WrongAnswer.ToString() : equation.CorrectAnswer.ToString());
		answerValue3.text = ((num != 2f) ? equation.WrongAnswer.ToString() : equation.CorrectAnswer.ToString());
		currentEquation = equation;
		btnWithCorrectAnswer = ((num == 0f) ? btnA : ((num != 1f) ? btnC : btnB));
	}

	private MD_BonusPling CreateNewBonusPling()
	{
		GameObject gameObject = NGUITools.AddChild(bonusParent.gameObject, bonusPlingPrefab);
		return gameObject.GetComponent<MD_BonusPling>();
	}

	private IEnumerator HideBonus(float duration)
	{
		bonusValue.cachedGameObject.SetActive(value: false);
		yield return new WaitForSeconds(duration);
		bonusValue.cachedGameObject.SetActive(value: true);
		UITweener tween = bonusValue.GetComponent<UITweener>();
		tween.ResetToBeginning();
		tween.PlayForward();
	}

	private void EquationVisible()
	{
		if (equationHider.direction == Direction.Reverse)
		{
			equationVisible = true;
			MD_Controller.EquationVisibleEvent equationVisibleEvent = new MD_Controller.EquationVisibleEvent();
			equationVisibleEvent.ButtonWithCorrectAnwer = btnWithCorrectAnswer;
			GameEvents.Invoke(equationVisibleEvent);
		}
	}

	private void OnAnswerOneClicked()
	{
		if (equationVisible)
		{
			equationVisible = false;
			MD_Controller.EquationAnsweredEvent equationAnsweredEvent = new MD_Controller.EquationAnsweredEvent();
			equationAnsweredEvent.ButtonPressed = btnA;
			equationAnsweredEvent.IsCorrectAnswer = currentEquation.CorrectAnswer == int.Parse(answerValue1.text);
			equationAnsweredEvent.Answer = int.Parse(answerValue1.text);
			GameEvents.Invoke(equationAnsweredEvent);
		}
	}

	private void OnAnswerTwoClicked()
	{
		if (equationVisible)
		{
			equationVisible = false;
			MD_Controller.EquationAnsweredEvent equationAnsweredEvent = new MD_Controller.EquationAnsweredEvent();
			equationAnsweredEvent.ButtonPressed = btnB;
			equationAnsweredEvent.IsCorrectAnswer = currentEquation.CorrectAnswer == int.Parse(answerValue2.text);
			equationAnsweredEvent.Answer = int.Parse(answerValue2.text);
			GameEvents.Invoke(equationAnsweredEvent);
		}
	}

	private void OnAnswerThreeClicked()
	{
		if (equationVisible)
		{
			equationVisible = false;
			MD_Controller.EquationAnsweredEvent equationAnsweredEvent = new MD_Controller.EquationAnsweredEvent();
			equationAnsweredEvent.ButtonPressed = btnC;
			equationAnsweredEvent.IsCorrectAnswer = currentEquation.CorrectAnswer == int.Parse(answerValue3.text);
			equationAnsweredEvent.Answer = int.Parse(answerValue3.text);
			GameEvents.Invoke(equationAnsweredEvent);
		}
	}
}
