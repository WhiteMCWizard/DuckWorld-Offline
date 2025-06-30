using LitJson;

namespace SLAM.Achievements;

public class AchievementInfo
{
	[JsonName("id")]
	public int Id;

	[JsonName("name")]
	public string Name;

	[JsonName("description")]
	public string Description;

	[JsonName("hidden")]
	public bool Hidden;

	public AchievementManager.AchievementId Type => (AchievementManager.AchievementId)Id;

	public int SortOrder => Id;

	public int Target { get; set; }
}
