using SLAM.Engine;
using SLAM.ToolTips;
using UnityEngine;

namespace SLAM.Hangman;

public class HM_TutorialView : TutorialView
{
	[SerializeField]
	private ToolTip mouseClickToolTip;

	private string answer;

	private HM_Key[] keys;

	private void OnEnable()
	{
		GameEvents.Subscribe<HangmanGame.WordPickedEvent>(onWordPicked);
		GameEvents.Subscribe<HangmanGame.LetterGuessedEvent>(onLetterGuessed);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<HangmanGame.WordPickedEvent>(onWordPicked);
		GameEvents.Unsubscribe<HangmanGame.LetterGuessedEvent>(onLetterGuessed);
	}

	private void pickKey()
	{
		if (keys == null)
		{
			keys = Object.FindObjectsOfType<HM_Key>();
		}
		mouseClickToolTip.Hide();
		for (int i = 0; i < answer.Length; i++)
		{
			string text = answer.Substring(i, 1);
			int num = 25 - (text.ToLower().ToCharArray()[0] - 97);
			if (keys[num].GetComponent<Collider>().enabled)
			{
				mouseClickToolTip.Show(keys[num].transform);
				break;
			}
		}
	}

	private void onWordPicked(HangmanGame.WordPickedEvent evt)
	{
		keys = null;
		answer = evt.Answer;
		pickKey();
	}

	private void onLetterGuessed(HangmanGame.LetterGuessedEvent evt)
	{
		pickKey();
	}
}
