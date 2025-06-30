using UnityEngine;

namespace SLAM.HigherThan;

public class HTanswerPling : MonoBehaviour
{
	[SerializeField]
	private UITweener moveUpTween;

	[SerializeField]
	private UIToggle toggle;

	[SerializeField]
	private UILabel yourAnswerLabel;

	[SerializeField]
	private UILabel correctAnswerLabel;

	public void DoIt(HigherThanGame.EquationAnsweredEvent answer)
	{
		moveUpTween.PlayForward();
		toggle.value = answer.IsCorrectAnswer;
		correctAnswerLabel.text = answer.Equation.leftText + " " + getSign(answer.Equation) + " " + answer.Equation.rightText;
		if (answer.IsCorrectAnswer)
		{
			yourAnswerLabel.text = string.Empty;
			return;
		}
		yourAnswerLabel.text = answer.Equation.leftText + " " + getSign(answer.Answer) + " " + answer.Equation.rightText;
	}

	private string getSign(Equation e)
	{
		return (!(e.leftAnswer < e.rightAnswer)) ? ">" : "<";
	}

	private string getSign(HigherThanGame.Answer e)
	{
		return (e != HigherThanGame.Answer.Higher) ? "<" : ">";
	}
}
