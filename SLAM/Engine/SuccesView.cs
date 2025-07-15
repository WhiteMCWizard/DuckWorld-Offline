using System.Collections;
using System.Collections.Generic;
using SLAM.Webservices;
using UnityEngine;

namespace SLAM.Engine;

public class SuccesView : SuccesBaseView
{
	private const float startDelay = 1f;

	private const float waitInBetweenScoreTime = 0f;

	[SerializeField]
	private UIGrid grdScoreContainer;

	[SerializeField]
	private ScoreRow scoreRowPrefab;

	[SerializeField]
	private UILabel lblTotalScores;

	[SerializeField]
	private UILabel lblChallengeResult;

	[SerializeField]
	private UIButton btnHighscores;

	[SerializeField]
	private UIButton btnChallengeFriend;

	protected override void updateStuff(bool isJob, bool isNextLevelAvailable, bool canChallengeFriend)
	{
		btnHighscores.gameObject.SetActive(false);
		btnNextLevel.gameObject.SetActive(isNextLevelAvailable);
		btnChallengeFriend.gameObject.SetActive(value: false);
		lblTotalScores.text = "0";
		base.updateStuff(isJob, isNextLevelAvailable, canChallengeFriend);
	}

	public virtual void OnHighscoresClicked()
	{
		Controller<GameController>().OpenHighscores();
	}

	public virtual void OnChallengeFriendClicked()
	{
		Controller<GameController>().OpenChallenge();
	}

	protected override void onOpenFinished()
	{
		base.onOpenFinished();
		StartCoroutine(doGameOverSequence());
	}

	private IEnumerator doGameOverSequence()
	{
		yield return StartCoroutine(doScoreSequence());
		if (GameController.ChallengeAccepted != null)
		{
			yield return StartCoroutine(doChallengeResultSequence(GameController.ChallengeAccepted));
		}
	}

	private IEnumerator doChallengeResultSequence(Message challenge)
	{
		challenge.ScoreRecipient = Controller<GameController>().TotalScore;
		if (challenge.WasGameTie())
		{
			string text = "SF_CHALLENGE_TIE";
			lblChallengeResult.text = string.Format(Localization.Get(text), challenge.Sender.Name, challenge.ScoreSender);
		}
		else
		{
			string text2 = ((!challenge.HasRecipientWon()) ? "SF_CHALLENGE_DEFEAT" : "SF_CHALLENGE_VICTORY");
			lblChallengeResult.text = string.Format(Localization.Get(text2), challenge.Sender.Name, challenge.ScoreSender, challenge.ScoreRecipient);
		}
		yield return null;
	}

	private IEnumerator doScoreSequence()
	{
		yield return new WaitForSeconds(1f);
		int counter = 0;
		int totalCategories = Controller<GameController>().ScoreCategories.Count;
		foreach (KeyValuePair<string, int> scoreCategory in Controller<GameController>().ScoreCategories)
		{
			if (scoreCategory.Value != 0)
			{
				ScoreRow row = NGUITools.AddChild(grdScoreContainer.gameObject, scoreRowPrefab.gameObject).GetComponent<ScoreRow>();
				row.name = totalCategories - counter + ". ScoreRow";
				grdScoreContainer.enabled = true;
				grdScoreContainer.Reposition();
				yield return StartCoroutine(row.AnimateScoreRow(scoreCategory.Key, scoreCategory.Value, lblTotalScores));
				yield return new WaitForSeconds(0f);
				counter++;
			}
		}
	}

	public void OnFriendChallenged()
	{
		btnChallengeFriend.gameObject.SetActive(value: false);
	}
}
