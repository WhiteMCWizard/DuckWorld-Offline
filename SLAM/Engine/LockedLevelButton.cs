using SLAM.Webservices;

namespace SLAM.Engine;

public class LockedLevelButton : LevelButton
{
	private bool isLocked;

	public override void SetInfo(GameController.LevelSetting data, int index, StartView view, Game game)
	{
		base.SetInfo(data, index, view, game);
		lblLevelName.text = string.Empty;
	}

	public override void OnLevelSelected()
	{
		GameEvents.Invoke(new PopupEvent(Localization.Get("UI_LEVELSELECT_GAME_LOCKED_TITLE"), Localization.Get("UI_LEVELSELECT_GAME_LOCKED_BODY"), Localization.Get("UI_OK"), null, null, null));
	}
}
