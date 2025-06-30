using SLAM.SaveSystem;
using SLAM.Webservices;
using UnityEngine;

namespace SLAM.Engine;

public class StartView : View
{
	[SerializeField]
	private UILabel titleLabel;

	[SerializeField]
	private UIGrid gridLevels;

	[SerializeField]
	private GameObject levelButtonPrefab;

	[SerializeField]
	private GameObject premiumLevelButtonPrefab;

	[SerializeField]
	private GameObject lockedLevelButtonPrefab;

	[SerializeField]
	private GameObject lockedLevelSanomaAccountButtonPrefab;

	public virtual void SetInfo(GameController.LevelSetting[] levels, Game game)
	{
		titleLabel.text = Localization.Get(game.Name);
		buildLevelGrid(levels, game);
	}

	private void buildLevelGrid(GameController.LevelSetting[] levels, Game game)
	{
		if (gridLevels == null)
		{
			Debug.LogWarning("Hey buddy, there is no level grid in StartView", this);
			return;
		}
		gridLevels.transform.DestroyChildren();
		for (int i = 0; i < levels.Length; i++)
		{
			GameController.LevelSetting levelSetting = levels[i];
			GameObject gameObject = NGUITools.AddChild(prefab: (UserProfile.Current.IsFree && !game.CanPlayLevel(levelSetting)) ? premiumLevelButtonPrefab : ((UserProfile.Current.IsSA && !game.IsUnlockedSA) ? lockedLevelSanomaAccountButtonPrefab : ((!UserProfile.Current.IsFree && !levelSetting.IsUnlocked) ? lockedLevelButtonPrefab : levelButtonPrefab)), parent: gridLevels.gameObject);
			gameObject.GetComponent<LevelButton>().SetInfo(levelSetting, i, this, game);
		}
		gridLevels.Reposition();
		gridLevels.repositionNow = true;
	}

	public virtual void OnHubClicked()
	{
		Controller<GameController>().GoToHub();
	}

	public virtual void OnChallengeClicked()
	{
		Controller<GameController>().OpenChallenge();
	}

	public virtual void OnHighscoresClicked()
	{
		Controller<GameController>().OpenHighscores();
	}

	public void SelectLevel(GameController.LevelSetting levelSettings)
	{
		Controller<GameController>().Play(levelSettings);
	}

	public GameController GetController()
	{
		return Controller<GameController>();
	}
}
