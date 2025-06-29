using SLAM.Engine;
using UnityEngine;

namespace SLAM.DuckQuiz;

public class DQCorrectAnswerView : View
{
	[SerializeField]
	private UILabel lblScoreCount;

	[SerializeField]
	private UILabel lblScoreCountShadow;

	public void SetInfo(int scoreCount)
	{
		UILabel uILabel = lblScoreCount;
		string text = $"{scoreCount} punten";
		lblScoreCountShadow.text = text;
		uILabel.text = text;
	}

	private void OnDisable()
	{
		UITweener[] componentsInChildren = GetComponentsInChildren<UITweener>(includeInactive: true);
		foreach (UITweener uITweener in componentsInChildren)
		{
			uITweener.ResetToBeginning();
			uITweener.enabled = true;
		}
	}
}
