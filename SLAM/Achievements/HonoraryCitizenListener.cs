using UnityEngine;

namespace SLAM.Achievements;

public class HonoraryCitizenListener : MonoBehaviour
{
	private void OnEnable()
	{
		GameEvents.Subscribe<AchievementCompletedEvent>(onAchievementCompleted);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<AchievementCompletedEvent>(onAchievementCompleted);
	}

	private void onAchievementCompleted(AchievementCompletedEvent evt)
	{
		if (evt.Achievement.Info.Type != AchievementManager.AchievementId.COMPLETE_ADVENTURE_2)
		{
		}
	}
}
