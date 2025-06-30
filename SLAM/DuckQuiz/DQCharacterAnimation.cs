using UnityEngine;

namespace SLAM.DuckQuiz;

public class DQCharacterAnimation : MonoBehaviour
{
	private const string CORRECT_ANSWER_TRIGGER = "CorrectAnswer";

	private const string INCORRECT_ANSWER_TRIGGER = "IncorrectAnswer";

	private const string QUESTION_PROPOSED_TRIGGER = "QuestionProposed";

	[SerializeField]
	private Animator animator;

	[Range(0f, 1f)]
	[SerializeField]
	private float responseChance = 1f;

	private void OnValidate()
	{
		animator = animator ?? GetComponentInChildren<Animator>();
	}

	private void OnEnable()
	{
		GameEvents.Subscribe<DQGameController.GameStartedEvent>(onGameStarted);
		GameEvents.Subscribe<DQGameController.QuestionAnsweredEvent>(onQuestionAnswered);
		GameEvents.Subscribe<DQGameController.QuestionProposedEvent>(onQuestionProposed);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<DQGameController.GameStartedEvent>(onGameStarted);
		GameEvents.Unsubscribe<DQGameController.QuestionAnsweredEvent>(onQuestionAnswered);
		GameEvents.Unsubscribe<DQGameController.QuestionProposedEvent>(onQuestionProposed);
	}

	private void onGameStarted(DQGameController.GameStartedEvent evt)
	{
		animator.SetBool("Idle", value: true);
	}

	private void onQuestionProposed(DQGameController.QuestionProposedEvent evt)
	{
		animator.SetBool("Idle", value: true);
	}

	private void onQuestionAnswered(DQGameController.QuestionAnsweredEvent evt)
	{
		if (Random.value < responseChance)
		{
			animator.SetBool("Idle", value: false);
			animator.SetTrigger((evt.Answer != null && evt.Answer.Correct) ? "CorrectAnswer" : "IncorrectAnswer");
		}
		else
		{
			animator.SetBool("Idle", value: true);
		}
	}
}
