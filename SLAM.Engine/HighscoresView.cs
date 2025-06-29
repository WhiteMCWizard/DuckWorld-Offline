using SLAM.Webservices;
using UnityEngine;

namespace SLAM.Engine;

public class HighscoresView : View
{
	[SerializeField]
	private UILabel titleLabel;

	[SerializeField]
	private UIGrid grid;

	[SerializeField]
	private GameObject highscoreRowNormal;

	[SerializeField]
	private GameObject highscoreRowYou;

	[SerializeField]
	private GameObject loadingRow;

	[SerializeField]
	private bool isTimeBased;

	[SerializeField]
	private UIScrollView scrollView;

	[SerializeField]
	private UITexture avatarTexture;

	public void SetInfo(GameController.LevelSetting level)
	{
		titleLabel.text = StringFormatter.GetLocalizationFormatted("UI_WINDOW_TITLE", level.Difficulty);
		grid.transform.DestroyChildren();
		grid.transform.DetachChildren();
		NGUITools.AddChild(grid.gameObject, loadingRow);
	}

	public void SetHighscores(HighScore[] highscores)
	{
		grid.transform.DestroyChildren();
		grid.transform.DetachChildren();
		float num = 0f;
		float num2 = 0f;
		foreach (HighScore highScore in highscores)
		{
			HighscoreRow highscoreRow = ((!highScore.You) ? NGUITools.AddChild(grid.gameObject, highscoreRowNormal).GetComponent<HighscoreRow>() : NGUITools.AddChild(grid.gameObject, highscoreRowYou).GetComponent<HighscoreRow>());
			highscoreRow.lblRank.text = highScore.Rank + ".";
			num2 = (float)highscoreRow.lblRank.text.Length * 16f - 5f;
			num = highscoreRow.lblRank.transform.localPosition.x;
			highscoreRow.lblName.transform.localPosition = new Vector3(num + num2, 0f, 0f);
			highscoreRow.lblName.text = highScore.Username;
			highscoreRow.lblScore.text = ((!isTimeBased) ? highScore.Score.ToString() : StringFormatter.GetFormattedTimeFromMiliseconds(highScore.Score));
			highscoreRow.name = highScore.Rank.ToString();
		}
		scrollView.ResetPosition();
		grid.enabled = true;
		grid.repositionNow = true;
		grid.Reposition();
		scrollView.ResetPosition();
	}

	public virtual void OnBackClicked()
	{
		Controller<GameController>().CloseHighscores();
	}

	public override void Close()
	{
		grid.transform.DestroyChildren();
		scrollView.ResetPosition();
		base.Close();
	}
}
