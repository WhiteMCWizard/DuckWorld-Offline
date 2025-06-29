using System;
using System.Collections;
using SLAM.Avatar;
using SLAM.Engine;
using SLAM.Webservices;
using UnityEngine;

namespace SLAM.KartRacing;

public class KRresultView : View
{
	[SerializeField]
	private UILabel lblTop;

	[SerializeField]
	private UILabel lblTitle;

	[SerializeField]
	private UILabel lblChallengeResult;

	[SerializeField]
	private UIGrid timeGrid;

	[SerializeField]
	private GameObject timeRowPrefab;

	[SerializeField]
	private GameObject youTimeRowPrefab;

	[SerializeField]
	private UIButton highscoresButton;

	[SerializeField]
	private UIButton nextLevelButton;

	[SerializeField]
	private UIButton btnChallengeFriend;

	[SerializeField]
	private UITexture avatarTexture;

	public override void Close(Callback callback, bool immediately)
	{
		base.Close(callback, immediately);
		SingletonMonobehaviour<PhotoBooth>.Instance.StopFilming();
	}

	public void ShowResult(bool succes, Game gameInfo, KR_TrackInfo settings)
	{
		btnChallengeFriend.gameObject.SetActive(value: false);
		PhotoBooth.Pose pose = (succes ? PhotoBooth.Pose.Cheer : PhotoBooth.Pose.Sad);
		avatarTexture.mainTexture = SingletonMonobehaviour<PhotoBooth>.Instance.StartFilming(pose);
		lblTop.text = StringFormatter.GetLocalizationFormatted("UI_WINDOW_TITLE", settings.Difficulty);
		lblTitle.text = Localization.Get((!succes) ? "KR_RESULTWINDOW_LOSE" : "KR_RESULTWINDOW_VICTORY");
		int num = ((!UserProfile.Current.IsFree) ? Controller<GameController>().Levels.Length : gameInfo.FreeLevelTo);
		bool flag = gameInfo.Type != Game.GameType.Job && Controller<GameController>().SelectedLevel<GameController.LevelSetting>().Index + 1 < num;
		nextLevelButton.gameObject.SetActive(succes && flag);
		highscoresButton.gameObject.SetActive(succes);
		if (GameController.ChallengeAccepted != null)
		{
			StartCoroutine(doChallengeResultSequence(GameController.ChallengeAccepted));
		}
	}

	public virtual void OnHubClicked()
	{
		Controller<GameController>().GoToHub();
	}

	public virtual void OnNextClicked()
	{
		Controller<KR_GameController>().PlayNextLevel();
	}

	public virtual void OnRestartClicked()
	{
		Controller<GameController>().Restart();
	}

	public virtual void OnMenuClicked()
	{
		Controller<GameController>().GoToMenu();
	}

	public virtual void OnHighscoresClicked()
	{
		Controller<GameController>().OpenHighscores();
	}

	public virtual void OnNextLevelClicked()
	{
		Controller<GameController>().PlayNextLevel();
	}

	public virtual void OnChallengeFriendClicked()
	{
		Controller<GameController>().OpenChallenge();
	}

	public void AddFinishedKart(KR_KartBase kart, int podiumPosition)
	{
		if (GameController.ChallengeAccepted == null || timeGrid.GetChildList().Count < 5)
		{
			GameObject prefab = ((!(kart is KR_HumanKart)) ? timeRowPrefab : youTimeRowPrefab);
			GameObject gameObject = NGUITools.AddChild(timeGrid.gameObject, prefab);
			float timeInMS = (float)TimeSpan.FromSeconds(kart.Timer.CurrentTime).TotalMilliseconds;
			if (kart is KR_GhostKart && GameController.ChallengeAccepted != null)
			{
				timeInMS = GameController.ChallengeAccepted.ScoreSender;
			}
			gameObject.GetComponent<KRTimeRow>().SetData(podiumPosition, kart.PlayerName, timeInMS);
			timeGrid.enabled = true;
			timeGrid.Reposition();
		}
	}

	private IEnumerator doChallengeResultSequence(Message challenge)
	{
		challenge.ScoreRecipient = Controller<GameController>().TotalScore;
		if (challenge.WasGameTie())
		{
			string text = "SF_CHALLENGE_TIE";
			lblChallengeResult.text = string.Format(Localization.Get(text), challenge.Sender.Name, StringFormatter.GetFormattedTimeFromMiliseconds(challenge.ScoreSender));
		}
		else
		{
			string text2 = ((!challenge.HasRecipientWon()) ? "SF_CHALLENGE_DEFEAT" : "SF_CHALLENGE_VICTORY");
			lblChallengeResult.text = string.Format(Localization.Get(text2), challenge.Sender.Name, StringFormatter.GetFormattedTimeFromMiliseconds(challenge.ScoreSender), StringFormatter.GetFormattedTimeFromMiliseconds(challenge.ScoreRecipient));
		}
		yield return null;
	}
}
