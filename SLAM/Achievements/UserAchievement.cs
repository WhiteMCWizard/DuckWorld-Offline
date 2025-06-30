using LitJson;

namespace SLAM.Achievements;

public class UserAchievement
{
	[JsonName("achievement")]
	public AchievementInfo Info;

	[JsonName("progress")]
	public float Progress;

	[JsonName("completed")]
	public bool Completed;
}
