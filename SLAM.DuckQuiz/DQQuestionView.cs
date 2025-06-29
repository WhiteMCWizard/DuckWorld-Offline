using System.Collections;
using System.Collections.Generic;
using SLAM.Engine;
using UnityEngine;

namespace SLAM.DuckQuiz;

public class DQQuestionView : View
{
	[SerializeField]
	private UILabel lblQuestionText;

	[SerializeField]
	private DQAnswerButton[] answerButtons;

	[SerializeField]
	private UIProgressBar timer;

	[SerializeField]
	private UISprite sprtQuestionBackground;

	[SerializeField]
	private UIButton clockIcon;

	[SerializeField]
	private UIButton cloverIcon;

	[SerializeField]
	private UIButton featherIcon;

	private DQQuestion question;

	public virtual void SetInfo(DQQuestion question, Alarm alarm)
	{
		this.question = question;
		lblQuestionText.text = question.Text;
		sprtQuestionBackground.spriteName = getSpriteNameForQuestionCategory(question.Category);
		for (int i = 0; i < this.question.Answers.Length; i++)
		{
			answerButtons[i].SetInfo(this.question.Answers[i]);
		}
		StartCoroutine(updateProgressBar(alarm));
	}

	public void DisableGuides()
	{
		clockIcon.gameObject.SetActive(value: false);
		cloverIcon.gameObject.SetActive(value: false);
		featherIcon.gameObject.SetActive(value: false);
	}

	private IEnumerator updateProgressBar(Alarm alarm)
	{
		while (!alarm.Expired)
		{
			yield return null;
			timer.value = alarm.Progress;
		}
	}

	public void OnGuideFeatherClicked()
	{
		DQGameController.GuideClickedEvent guideClickedEvent = new DQGameController.GuideClickedEvent();
		guideClickedEvent.Guide = DQGameController.GuideType.Feather;
		GameEvents.Invoke(guideClickedEvent);
		featherIcon.isEnabled = false;
		StartCoroutine(doHide(featherIcon.GetComponent<UISprite>()));
	}

	public void OnGuideCloverClicked()
	{
		DQGameController.GuideClickedEvent guideClickedEvent = new DQGameController.GuideClickedEvent();
		guideClickedEvent.Guide = DQGameController.GuideType.Clover;
		GameEvents.Invoke(guideClickedEvent);
		cloverIcon.isEnabled = false;
		StartCoroutine(doHide(cloverIcon.GetComponent<UISprite>()));
	}

	public void OnGuideClockClicked()
	{
		DQGameController.GuideClickedEvent guideClickedEvent = new DQGameController.GuideClickedEvent();
		guideClickedEvent.Guide = DQGameController.GuideType.Clock;
		GameEvents.Invoke(guideClickedEvent);
		clockIcon.isEnabled = false;
		StartCoroutine(doHide(clockIcon.GetComponent<UISprite>()));
	}

	private string getSpriteNameForQuestionCategory(DQGameController.QuestionCategory questionCategory)
	{
		return questionCategory switch
		{
			DQGameController.QuestionCategory.Animals => "categorie_Animals", 
			DQGameController.QuestionCategory.Disney => "categorie_Disney", 
			DQGameController.QuestionCategory.Duck => "categorie_Donald", 
			DQGameController.QuestionCategory.History => "categorie_History", 
			DQGameController.QuestionCategory.Geography => "categorie_Geo", 
			_ => "categorie_Mix", 
		};
	}

	public void OnAnswerClicked()
	{
		DQAnswerButton component = UIButton.current.transform.GetComponent<DQAnswerButton>();
		DQGameController.QuestionAnsweredEvent questionAnsweredEvent = new DQGameController.QuestionAnsweredEvent();
		questionAnsweredEvent.Question = question;
		questionAnsweredEvent.Answer = component.Answer;
		GameEvents.Invoke(questionAnsweredEvent);
	}

	public void ShowResult(DQAnswer answer)
	{
		for (int i = 0; i < question.Answers.Length; i++)
		{
			if (answerButtons[i].Answer.Correct)
			{
				answerButtons[i].SetCorrect();
			}
			else if (answerButtons[i].Answer == answer || answer == null)
			{
				answerButtons[i].SetIncorrect();
			}
			answerButtons[i].DisableInteraction();
		}
	}

	public void DisableAnswers(List<int> answersToDisable)
	{
		foreach (int item in answersToDisable)
		{
			answerButtons[item].DisableInteraction();
			answerButtons[item].SetIncorrect();
			StartCoroutine(doHide(answerButtons[item].GetComponent<UISprite>()));
		}
	}

	private IEnumerator doHide(UISprite sprite)
	{
		yield return new WaitForSeconds(0.2f);
		for (float t = 1f; t > 0f; t -= Time.deltaTime)
		{
			Color col = sprite.color;
			col.a = t;
			sprite.color = col;
			yield return null;
		}
		sprite.gameObject.SetActive(value: false);
	}
}
