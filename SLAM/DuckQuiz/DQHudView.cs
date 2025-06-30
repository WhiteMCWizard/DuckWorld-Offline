using SLAM.Engine;
using UnityEngine;

namespace SLAM.DuckQuiz;

public class DQHudView : HUDView
{
	[SerializeField]
	private UILabel lblQuestionCount;

	private int totalQuestionCount;

	private void OnEnable()
	{
		GameEvents.Subscribe<DQGameController.QuestionProposedEvent>(onQuestionProposed);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<DQGameController.QuestionProposedEvent>(onQuestionProposed);
	}

	public void SetTotalQuestionCount(int totalCount)
	{
		totalQuestionCount = totalCount;
		updateQuestionCountLabel(1);
	}

	private void onQuestionProposed(DQGameController.QuestionProposedEvent evt)
	{
		updateQuestionCountLabel(evt.QuestionCount);
	}

	private void updateQuestionCountLabel(int questionCount)
	{
		lblQuestionCount.text = $"{questionCount}/{totalQuestionCount}";
	}
}
