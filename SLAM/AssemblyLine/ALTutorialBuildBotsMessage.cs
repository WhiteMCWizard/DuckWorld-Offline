using System.Collections;
using SLAM.Engine;
using SLAM.ToolTips;
using UnityEngine;

namespace SLAM.AssemblyLine;

public class ALTutorialBuildBotsMessage : TutorialView
{
	[SerializeField]
	private MessageToolTip messageTooltip;

	[SerializeField]
	private string welcomeLocalizationKey = "In dit level moet je {0} robots bouwen om te winnen!";

	[SerializeField]
	private string robotCountLocalizationKey = "Goed gedaan! Je moet nog {0} robots bouwen om te winnen.";

	private new IEnumerator Start()
	{
		AssemblyLineGame controller = Controller<AssemblyLineGame>();
		int botsLeft = controller.RequiredRobotCount;
		messageTooltip.ShowText(StringFormatter.GetLocalizationFormatted(welcomeLocalizationKey, controller.RequiredRobotCount));
		if (controller.SelectedLevel<GameController.LevelSetting>().Index <= 5)
		{
			while (botsLeft > 1)
			{
				yield return CoroutineUtils.WaitForGameEvent<AssemblyLineGame.RobotCompletedEvent>();
				botsLeft--;
				messageTooltip.ShowText(StringFormatter.GetLocalizationFormatted(robotCountLocalizationKey, botsLeft));
			}
		}
	}
}
