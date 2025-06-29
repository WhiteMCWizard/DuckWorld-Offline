using SLAM.Avatar;
using UnityEngine;

namespace SLAM.Engine;

public class PauseView : View
{
	[SerializeField]
	private UILabel titleLabel;

	[SerializeField]
	private UITexture avatarTexture;

	private bool isInChallengeMode;

	private bool wasInvitedForGame;

	public override void Open(Callback callback, bool immediately)
	{
		base.Open(callback, immediately);
		avatarTexture.mainTexture = SingletonMonobehaviour<PhotoBooth>.Instance.StartFilming(PhotoBooth.Pose.Idle);
	}

	public override void Close(Callback callback, bool immediately)
	{
		base.Close(callback, immediately);
		SingletonMonobehaviour<PhotoBooth>.Instance.StopFilming();
	}

	public virtual void SetInfo(GameController.LevelSetting level, bool isInChallengeMode, bool wasInvitedForGame)
	{
		titleLabel.text = StringFormatter.GetLocalizationFormatted("UI_WINDOW_TITLE", level.Difficulty);
		this.isInChallengeMode = isInChallengeMode;
		this.wasInvitedForGame = wasInvitedForGame;
	}

	public virtual void OnBackClicked()
	{
		Controller<GameController>().Resume();
	}

	public virtual void OnMenuClicked()
	{
		if (isInChallengeMode)
		{
			GameEvents.Invoke(new PopupEvent(Localization.Get("SF_CHALLENGE_ABORT_TITLE"), Localization.Get("SF_CHALLENGE_ABORT_BODY"), Localization.Get("UI_YES"), Localization.Get("UI_NO"), Controller<GameController>().GoToMenu, null));
		}
		else if (wasInvitedForGame)
		{
			GameEvents.Invoke(new PopupEvent(Localization.Get("SF_INVITE_ABORT_TITLE"), Localization.Get("SF_INVITE_ABORT_BODY"), Localization.Get("UI_YES"), Localization.Get("UI_NO"), Controller<GameController>().GoToMenu, null));
		}
		else
		{
			Controller<GameController>().GoToMenu();
		}
	}

	public virtual void OnRestartClicked()
	{
		Controller<GameController>().Restart();
	}

	public virtual void OnHubClicked()
	{
		if (isInChallengeMode)
		{
			GameEvents.Invoke(new PopupEvent(Localization.Get("SF_CHALLENGE_ABORT_TITLE"), Localization.Get("SF_CHALLENGE_ABORT_BODY"), Localization.Get("UI_YES"), Localization.Get("UI_NO"), Controller<GameController>().GoToHub, null));
		}
		else if (wasInvitedForGame)
		{
			GameEvents.Invoke(new PopupEvent(Localization.Get("SF_INVITE_ABORT_TITLE"), Localization.Get("SF_INVITE_ABORT_BODY"), Localization.Get("UI_YES"), Localization.Get("UI_NO"), Controller<GameController>().GoToHub, null));
		}
		else
		{
			Controller<GameController>().GoToHub();
		}
	}
}
