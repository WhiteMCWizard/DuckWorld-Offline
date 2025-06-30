using SLAM.Avatar;
using UnityEngine;

namespace SLAM.Engine;

public class FailedView : View
{
	[SerializeField]
	private UILabel titleLabel;

	[SerializeField]
	private UITexture avatarTexture;

	public override void Open(Callback callback, bool immediately)
	{
		base.Open(callback, immediately);
		avatarTexture.mainTexture = SingletonMonobehaviour<PhotoBooth>.Instance.StartFilming(PhotoBooth.Pose.Sad);
	}

	public override void Close(Callback callback, bool immediately)
	{
		base.Close(callback, immediately);
		SingletonMonobehaviour<PhotoBooth>.Instance.StopFilming();
	}

	public void SetInfo(GameController.LevelSetting level)
	{
		titleLabel.text = StringFormatter.GetLocalizationFormatted("UI_WINDOW_TITLE", level.Difficulty);
	}

	public virtual void OnHubClicked()
	{
		Controller<GameController>().GoToHub();
	}

	public virtual void OnRestartClicked()
	{
		Controller<GameController>().Restart();
	}

	public virtual void OnMenuClicked()
	{
		Controller<GameController>().GoToMenu();
	}
}
