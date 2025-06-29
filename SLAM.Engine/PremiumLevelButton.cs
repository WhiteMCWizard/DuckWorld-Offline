using SLAM.Webservices;

namespace SLAM.Engine;

public class PremiumLevelButton : LevelButton
{
	private bool premiumLocked;

	private int gameId;

	public override void SetInfo(GameController.LevelSetting data, int index, StartView view, Game game)
	{
		gameId = game.Id;
		base.SetInfo(data, index, view, game);
		lblLevelName.text = string.Empty;
	}

	public override void OnLevelSelected()
	{
		GameEvents.Invoke(new PopupEvent(Localization.Get("UI_STARTMENU_POPUP_LOCKED_TITLE"), Localization.Get("UI_STARTMENU_POPUP_LOCKED_DESCRIPTION"), Localization.Get("UI_STARTMENU_POPUP_LOCKED_BUY"), Localization.Get("UI_STARTMENU_POPUP_LOCKED_CONTINUE"), delegate
		{
			ApiClient.OpenPropositionPage(gameId);
		}, null));
	}
}
