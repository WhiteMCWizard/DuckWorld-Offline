using SLAM.Achievements;
using UnityEngine;

namespace SLAM.Smartphone;

public class AchievementRow : MonoBehaviour
{
	[SerializeField]
	private UISprite icon;

	[SerializeField]
	private UILabel titleLabel;

	[SerializeField]
	private UILabel descriptionLabel;

	[SerializeField]
	private UIProgressBar progressbar;

	[SerializeField]
	private UILabel progressLabel;

	public void SetData(UserAchievement data)
	{
		titleLabel.text = Localization.Get(data.Info.Name);
		descriptionLabel.text = StringFormatter.GetLocalizationFormatted(data.Info.Description, data.Info.Target);
		progressbar.value = data.Progress;
		progressLabel.text = $"{Mathf.CeilToInt(data.Progress * (float)data.Info.Target)}/{data.Info.Target}";
		icon.spriteName = AchievementManager.AchievementToIcon(data);
	}
}
