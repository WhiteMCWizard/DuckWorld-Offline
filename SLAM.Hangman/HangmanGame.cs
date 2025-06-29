using System;
using System.Collections;
using System.Collections.Generic;
using SLAM.Avatar;
using SLAM.Engine;
using UnityEngine;

namespace SLAM.Hangman;

public class HangmanGame : GameController
{
	[Serializable]
	public class HMLevelSetting : LevelSetting
	{
		[Serializable]
		public struct HMCategorySetting
		{
			public string TitleTranslationKey;

			public string WordsTranslationKey;
		}

		public int TargetWordCount;

		public float TimePerLetter;

		public HMCategorySetting[] Categories;

		public int HelperMin;

		public int HelperThreshold;

		public float HelperMultiplier;
	}

	public class WordPickedEvent
	{
		public string Answer;
	}

	public class LetterGuessedEvent
	{
	}

	private const string STATE_GUESSING_ANSWER = "GuessingAnswer";

	private const string STATE_SHOWING_ANSWER = "ShowingAnswer";

	private const int WORD_SCORE = 10;

	private const int LETTER_SCORE = 1;

	[SerializeField]
	[Header("Hangman properties")]
	private Camera introCam;

	[SerializeField]
	private AvatarSpawn gameAvatar;

	[SerializeField]
	private AudioClip correctClip;

	[SerializeField]
	private AudioClip incorrectClip;

	[SerializeField]
	private AudioClip levelupClip;

	[SerializeField]
	private AudioClip timeupClip;

	[SerializeField]
	private AudioClip removeLettersClip;

	private HM_GameView gameView;

	private string answerOrig;

	private string answer;

	private char[] known;

	private List<char> guessedLetters;

	private int totalCorrectLetters;

	private int totalWrongLetters;

	private int wrongLetters;

	private int wordCount = -1;

	private int correctWords;

	private int incorrectWords;

	private Dictionary<string, string[]> wordsPerCategory;

	private Alarm letterTimer;

	private List<string> wordsUsed;

	[SerializeField]
	private HMLevelSetting[] settings;

	public override int GameId => 24;

	public override Dictionary<string, int> ScoreCategories
	{
		get
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			dictionary.Add(StringFormatter.GetLocalizationFormatted("HM_VICTORYWINDOW_SCORE_CORRECT_WORDS", correctWords, 10), correctWords * 10);
			dictionary.Add(StringFormatter.GetLocalizationFormatted("HM_VICTORYWINDOW_SCORE_INCORRECT_LETTERS", totalWrongLetters, 1), totalWrongLetters * -1 * 1);
			return dictionary;
		}
	}

	public override string IntroNPCKey => "NPC_NAME_WARBOL";

	public override string IntroTextKey => "HM_CINEMATICINTRO_TEXT";

	public override Portrait DuckCharacter => Portrait.Warbol;

	public override LevelSetting[] Levels => settings;

	protected override void Start()
	{
		base.Start();
		wordsUsed = new List<string>();
		letterTimer = Alarm.Create();
		gameView = OpenView<HM_GameView>();
		gameView.InitKeys(this);
	}

	public override void Finish(bool succes)
	{
		base.Finish(succes);
		letterTimer.Destroy();
	}

	protected override void AddStates()
	{
		base.AddStates();
		base.StateMachine.AddState("GuessingAnswer", null, WhileStateGuessing, null);
		base.StateMachine.AddState("ShowingAnswer", OnShowAnswer, null, null);
	}

	protected override void OnEnterStateRunning()
	{
		base.OnEnterStateRunning();
		OpenView<HeartsView>().SetTotalHeartCount(3);
		introCam.gameObject.SetActive(value: false);
		gameAvatar.SpawnAvatar();
		wordsPerCategory = new Dictionary<string, string[]>();
		HMLevelSetting.HMCategorySetting[] categories = SelectedLevel<HMLevelSetting>().Categories;
		for (int i = 0; i < categories.Length; i++)
		{
			HMLevelSetting.HMCategorySetting hMCategorySetting = categories[i];
			wordsPerCategory.Add(Localization.Get(hMCategorySetting.TitleTranslationKey), Localization.Get(hMCategorySetting.WordsTranslationKey).Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries));
		}
		base.StateMachine.SwitchTo("GuessingAnswer");
		CreateNewWord();
	}

	protected void WhileStateGuessing()
	{
		string inputString = Input.inputString;
		foreach (char c in inputString)
		{
			if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z'))
			{
				GuessLetter(c);
			}
		}
		gameView.UpdateTimer(letterTimer.TimeLeft / letterTimer.TimerDuration);
	}

	private void CreateNewWord()
	{
		wordCount++;
		PickAnswer();
		answer = answerOrig.Replace(" ", string.Empty);
		known = new char[answer.Length];
		for (int i = 0; i < known.Length; i++)
		{
			known[i] = ' ';
		}
		guessedLetters = ExcludeLetters(answer);
		wrongLetters = 0;
		gameView.SetLevel(correctWords, SelectedLevel<HMLevelSetting>().TargetWordCount);
		gameView.SetBeagleBoy(wrongLetters);
		gameView.InitSpots(answerOrig);
		gameView.InitKeys(this);
		gameView.DisableButtons(guessedLetters);
		WordPickedEvent wordPickedEvent = new WordPickedEvent();
		wordPickedEvent.Answer = answer;
		GameEvents.Invoke(wordPickedEvent);
		letterTimer.StartCountdown(SelectedLevel<HMLevelSetting>().TimePerLetter, WrongLetter, destroyWhenFinished: false);
	}

	private void PickAnswer()
	{
		KeyValuePair<string, string[]> random = wordsPerCategory.GetRandom();
		gameView.SetCategory(random.Key);
		answerOrig = random.Value[UnityEngine.Random.Range(0, random.Value.Length)];
		if (wordsUsed.Contains(answerOrig))
		{
			PickAnswer();
		}
		else
		{
			wordsUsed.Add(answerOrig);
		}
	}

	private void CorrectLetter(char c)
	{
		AudioController.Play(correctClip.name);
		totalCorrectLetters++;
		for (int i = 0; i < answer.Length; i++)
		{
			if (string.Compare(c.ToString(), answer.Substring(i, 1), ignoreCase: true) == 0)
			{
				known[i] = answer.Substring(i, 1).ToCharArray()[0];
			}
		}
		gameView.UpdateSpots(known);
		gameView.SetButtonState(c, correct: true);
		bool flag = true;
		char[] array = known;
		foreach (char c2 in array)
		{
			if (c2.ToString() == " ")
			{
				flag = false;
			}
		}
		if (flag)
		{
			WordCompleted();
		}
		else
		{
			letterTimer.StartCountdown(SelectedLevel<HMLevelSetting>().TimePerLetter, WrongLetter, destroyWhenFinished: false);
		}
	}

	private void WrongLetter()
	{
		AudioController.Play(incorrectClip.name);
		wrongLetters++;
		totalWrongLetters++;
		gameView.SetBeagleBoy(wrongLetters);
		if (wrongLetters >= 7)
		{
			WordFailed();
		}
		letterTimer.StartCountdown(SelectedLevel<HMLevelSetting>().TimePerLetter, WrongLetter, destroyWhenFinished: false);
	}

	private void WordCompleted()
	{
		correctWords++;
		gameView.SetLevel(correctWords, SelectedLevel<HMLevelSetting>().TargetWordCount);
		gameAvatar.GetComponent<Animator>().SetTrigger("Succes");
		base.StateMachine.SwitchTo("ShowingAnswer");
	}

	private void WordFailed()
	{
		GetView<HeartsView>().LoseHeart();
		gameAvatar.GetComponent<Animator>().SetTrigger("Failure");
		base.StateMachine.SwitchTo("ShowingAnswer");
		incorrectWords++;
	}

	private void OnShowAnswer()
	{
		StartCoroutine(DoShowAnswer());
	}

	private IEnumerator DoShowAnswer()
	{
		bool correct = true;
		char[] array = known;
		foreach (char check in array)
		{
			if (check.ToString() == " ")
			{
				correct = false;
			}
		}
		for (int j = 0; j < answer.Length; j++)
		{
			known[j] = answer[j];
		}
		gameView.UpdateSpots(known);
		gameView.UpdateTimer(0f);
		gameView.SetWordFinished(correct);
		if (correct)
		{
			AudioController.Play(levelupClip.name);
		}
		else
		{
			AudioController.Play(timeupClip.name);
		}
		yield return new WaitForSeconds(2.5f);
		if (incorrectWords >= 3)
		{
			Finish(succes: false);
			yield break;
		}
		if (correctWords >= SelectedLevel<HMLevelSetting>().TargetWordCount)
		{
			Finish(succes: true);
			yield break;
		}
		base.StateMachine.SwitchTo("GuessingAnswer");
		CreateNewWord();
	}

	private void TimeUp()
	{
		AudioController.Play(timeupClip.name);
		Finish(succes: false);
	}

	public void GuessLetter(char c)
	{
		if (!(base.StateMachine.CurrentState.Name != "GuessingAnswer") && !guessedLetters.Contains(c))
		{
			string text = c.ToString();
			text = text.ToUpper() + text.ToLower();
			char[] array = text.ToCharArray();
			guessedLetters.Add(array[0]);
			guessedLetters.Add(array[1]);
			if (answer.Contains(c.ToString().ToUpper()) || answer.Contains(c.ToString().ToLower()))
			{
				CorrectLetter(c);
			}
			else
			{
				gameView.SetButtonState(c, correct: false);
				WrongLetter();
			}
			GameEvents.Invoke(new LetterGuessedEvent());
		}
	}

	private List<char> ExcludeLetters(string word)
	{
		List<char> list = new List<char>();
		HMLevelSetting hMLevelSetting = SelectedLevel<HMLevelSetting>();
		int b = (int)((float)hMLevelSetting.HelperMin + Mathf.Max(0f, (float)(hMLevelSetting.HelperThreshold - word.Length) * hMLevelSetting.HelperMultiplier));
		b = Mathf.Min(23 - word.Length, b);
		if (b > 0)
		{
			AudioController.Play(removeLettersClip.name);
		}
		for (int i = 0; i < b; i++)
		{
			while (list.Count < i + 1)
			{
				char item = (char)UnityEngine.Random.Range(97, 122);
				if (!list.Contains(item) && !answer.Contains(item.ToString().ToUpper()) && !answer.Contains(item.ToString().ToLower()))
				{
					list.Add(item);
				}
			}
		}
		return list;
	}
}
