using UnityEngine;

namespace SLAM.DuckQuiz;

public class DQFlagQuestionView : DQQuestionView
{
	[SerializeField]
	private UITexture txtrFlag;

	public override void SetInfo(DQQuestion question, Alarm alarm)
	{
		base.SetInfo(question, alarm);
		txtrFlag.mainTexture = ((DQFlagQuestion)question).Flag;
	}
}
