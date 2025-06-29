using SLAM.Webservices;

namespace SLAM.Engine;

public class LockedLevelSanomaAccountButton : LevelButton
{
	private bool isLocked;

	public override void SetInfo(GameController.LevelSetting data, int index, StartView view, Game game)
	{
		base.SetInfo(data, index, view, game);
		lblLevelName.text = string.Empty;
	}

	public override void OnLevelSelected()
	{
		GameEvents.Invoke(new PopupEvent(Localization.Get("UI_LEVELSELECT_GAME_LOCKED_SA_TITLE"), Localization.Get("UI_LEVELSELECT_GAME_LOCKED_SA_BODY"), Localization.Get("UI_OK"), null, null, null));
	}
}
