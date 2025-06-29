using System;
using SLAM.Achievements;

namespace SLAM.Smartphone;

public class AchievementsApp : AppController
{
	public override void Open()
	{
		OpenView<AchievementsMainView>().SetData(SingletonMonobehaviour<AchievementManager>.Instance.Achievements);
	}

	protected override void checkForNotifications(Action<AppChangedEvent> eventCallback)
	{
	}
}
