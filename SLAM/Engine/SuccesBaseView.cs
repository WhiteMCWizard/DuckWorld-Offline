using SLAM.Avatar;
using SLAM.SaveSystem;
using SLAM.Slinq;
using SLAM.Webservices;
using UnityEngine;

namespace SLAM.Engine;

public class SuccesBaseView : View
{
	[SerializeField]
	protected UIGrid buttonGrid;

	[SerializeField]
	protected UIButton btnReplay;

	[SerializeField]
	protected UIButton btnMenu;

	[SerializeField]
	protected UIButton btnNextLevel;

	[SerializeField]
	private UILabel titlebarLabel;

	[SerializeField]
	protected UITexture avatarTexture;

	public override void Open(Callback callback, bool immediately)
	{
		base.Open(callback, immediately);
		avatarTexture.mainTexture = SingletonMonobehaviour<PhotoBooth>.Instance.StartFilming(PhotoBooth.Pose.Cheer);
	}

	public override void Close(Callback callback, bool immediately)
	{
		base.Close(callback, immediately);
		SingletonMonobehaviour<PhotoBooth>.Instance.StopFilming();
	}

	public void SetInfo(Game gameInfo, string difficulty, bool isAllowedToChallengeFriend)
	{
		titlebarLabel.text = StringFormatter.GetLocalizationFormatted("UI_WINDOW_TITLE", difficulty);
		int num = Controller<GameController>().SelectedLevel<GameController.LevelSetting>().Index + 1;
		int num2 = Controller<GameController>().Levels.Length;
		bool isNextLevelAvailable = ((!UserProfile.Current.IsFree) ? (num < num2) : gameInfo.FreeLevels.Contains(num + 1));
		bool isJob = gameInfo.Type == Game.GameType.Job;
		updateStuff(isJob, isNextLevelAvailable, isAllowedToChallengeFriend);
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

	public virtual void OnNextLevelClicked()
	{
		Controller<GameController>().PlayNextLevel();
	}

	protected virtual void updateStuff(bool isJob, bool isNextLevelAvailable, bool canChallengeFriend)
	{
		btnReplay.gameObject.SetActive(value: true);
		btnMenu.gameObject.SetActive(value: true);
		btnNextLevel.gameObject.SetActive(isNextLevelAvailable);
		buttonGrid.enabled = true;
		buttonGrid.Reposition();
	}
}
