using System.Collections;
using UnityEngine;

namespace SLAM.DuckQuiz;

public class DQ_AudioManager : MonoBehaviour
{
	private void OnEnable()
	{
		GameEvents.Subscribe<DQGameController.QuestionProposedEvent>(onQuestionProposedEvent);
		GameEvents.Subscribe<DQGameController.QuestionAnsweredEvent>(onQuestionAnsweredEvent);
		GameEvents.Subscribe<DQGameController.GuideClickedEvent>(onGuideClickedEvent);
		GameEvents.Subscribe<DQGameController.ScoreGainedEvent>(onScoreGainedEvent);
		GameEvents.Subscribe<DQGameController.ScoreCounterFinishedEvent>(onScoreCounterFinishedEvent);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<DQGameController.QuestionProposedEvent>(onQuestionProposedEvent);
		GameEvents.Unsubscribe<DQGameController.QuestionAnsweredEvent>(onQuestionAnsweredEvent);
		GameEvents.Unsubscribe<DQGameController.GuideClickedEvent>(onGuideClickedEvent);
		GameEvents.Unsubscribe<DQGameController.ScoreGainedEvent>(onScoreGainedEvent);
		GameEvents.Unsubscribe<DQGameController.ScoreCounterFinishedEvent>(onScoreCounterFinishedEvent);
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void onQuestionProposedEvent(DQGameController.QuestionProposedEvent evt)
	{
		StopAllCoroutines();
		AudioController.Stop("DQ_SFX_countdown_tick");
		AudioController.Stop("DQ_SFX_countdown_3count");
		AudioController.Play("DQ_SFX_question_appears");
		StartCoroutine(doPlayTimer(evt.QuestionTime));
	}

	private void onQuestionAnsweredEvent(DQGameController.QuestionAnsweredEvent evt)
	{
		StopAllCoroutines();
		AudioController.Stop("DQ_SFX_countdown_tick");
		AudioController.Stop("DQ_SFX_countdown_3count");
		if (evt.Answer != null && evt.Answer.Correct)
		{
			AudioController.Play("DQ_SFX_answer_correct");
			AudioController.Play("DQ_SFX_audience_answer_correct_1");
		}
		else
		{
			AudioController.Play("DQ_SFX_answer_wrong");
			AudioController.Play("DQ_SFX_audience_answer_wrong_1");
		}
	}

	private void onGuideClickedEvent(DQGameController.GuideClickedEvent evt)
	{
		AudioController.Play("DQ_SFX_button_generic");
		if (evt.Guide == DQGameController.GuideType.Clock)
		{
			StopAllCoroutines();
			AudioController.Stop("DQ_SFX_countdown_tick");
			AudioController.Stop("DQ_SFX_countdown_3count");
		}
	}

	private void onScoreGainedEvent(DQGameController.ScoreGainedEvent evt)
	{
		AudioController.Play("DQ_SFX_counter_points");
	}

	private void onScoreCounterFinishedEvent(DQGameController.ScoreCounterFinishedEvent evt)
	{
		AudioController.Stop("DQ_SFX_counter_points");
		AudioController.Play("DQ_SFX_answer_correct_points_followup");
	}

	private IEnumerator playAudioAfterDelay(string audio, float delay)
	{
		yield return new WaitForSeconds(delay);
		AudioController.Play(audio);
	}

	private IEnumerator doPlayTimer(float seconds)
	{
		AudioController.Play("DQ_SFX_countdown_tick");
		yield return new WaitForSeconds(seconds - 3f);
		AudioController.Stop("DQ_SFX_countdown_tick");
		AudioController.Play("DQ_SFX_countdown_3count");
		yield return new WaitForSeconds(3f);
		AudioController.Stop("DQ_SFX_countdown_3count");
	}
}
