using SLAM.Engine;
using UnityEngine;

namespace SLAM.JumpTheCroc;

public class JumpTheCrocHUDView : HUDView
{
	[SerializeField]
	private UILabel QuestionLabel;

	public void SetQuestion(string question)
	{
		QuestionLabel.text = question;
	}
}
