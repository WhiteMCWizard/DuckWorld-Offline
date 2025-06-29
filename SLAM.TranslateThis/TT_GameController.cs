using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using SLAM.Avatar;
using SLAM.Engine;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.TranslateThis;

public class TT_GameController : GameController
{
	public enum TranslateFromTo
	{
		LOCALE_TO_TARGET,
		TARGET_TO_LOCALE
	}

	[Serializable]
	public class TTLevelSetting : LevelSetting
	{
		public TranslateFromTo TranslateDirection;

		public int TargetWordCount;

		public string TranslationKey;

		public float TimePerLetter;
	}

	[Serializable]
	public class LanguageFlag
	{
		[Popup(new string[] { "English", "Dutch", "Spanish" })]
		public string LocaleLanguage;

		public Texture2D LocaleFlag;

		public Texture2D TargetFlag;
	}

	public class WordPickedEvent
	{
		public string Answer;
	}

	public class LetterGuessedEvent
	{
		public char letter;
	}

	private const string STATE_GUESSING_ANSWER = "GuessingAnswer";

	private const string STATE_SHOWING_ANSWER = "ShowingAnswer";

	private const string WORDLIST_PREFIX = "TT_WORDLIST_";

	private const int WORD_SCORE = 10;

	private const int LETTER_SCORE = 1;

	private const int MAX_INCORRECT_WORDS = 3;

	private const int MAX_GUESSES_PER_WORD = 7;

	private const int MAX_LETTERS_ON_BOARD = 24;

	[SerializeField]
	[Header("Translate This properties")]
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
	private LanguageFlag[] flagMapping;

	private TT_GameView gameView;

	private string answer;

	private string answerNoSpace;

	private string answerNormalised;

	private char[] knownLetters;

	private List<char> guessedLetters;

	private int totalCorrectLetters;

	private int totalWrongLetters;

	private int wrongLetters;

	private int score;

	private int correctWords;

	private int incorrectWords;

	private Alarm letterTimer;

	private List<string> wordsUsed;

	private Dictionary<string, string> wordsTranslations;

	[SerializeField]
	private TTLevelSetting[] settings;

	public override int GameId => 33;

	public override Dictionary<string, int> ScoreCategories
	{
		get
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			dictionary.Add(StringFormatter.GetLocalizationFormatted("TT_VICTORYWINDOW_SCORE_CORRECT_WORDS", correctWords, 10), correctWords * 10);
			dictionary.Add(StringFormatter.GetLocalizationFormatted("TT_VICTORYWINDOW_SCORE_INCORRECT_LETTERS", totalWrongLetters, 1), totalWrongLetters * -1 * 1);
			return dictionary;
		}
	}

	public override string IntroNPCKey => "NPC_NAME_WARBOL";

	public override string IntroTextKey => "TT_CINEMATICINTRO_TEXT";

	public override Portrait DuckCharacter => Portrait.Warbol;

	public override LevelSetting[] Levels => settings;

	protected override void Start()
	{
		base.Start();
		letterTimer = Alarm.Create();
		wordsUsed = new List<string>();
		gameView = OpenView<TT_GameView>();
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
		gameAvatar.SpawnAvatar();
		introCam.gameObject.SetActive(value: false);
		wordsTranslations = GetWordsForDifficulty();
		LanguageFlag languageFlag = flagMapping.FirstOrDefault((LanguageFlag l) => l.LocaleLanguage == Localization.language);
		if (languageFlag != null)
		{
			if (SelectedLevel<TTLevelSetting>().TranslateDirection == TranslateFromTo.LOCALE_TO_TARGET)
			{
				gameView.SetFlag(languageFlag.LocaleFlag);
			}
			else
			{
				gameView.SetFlag(languageFlag.TargetFlag);
			}
		}
		else
		{
			Debug.LogError("Hey Buddy, there is not flag info for language " + Localization.language);
		}
		base.StateMachine.SwitchTo("GuessingAnswer");
		CreateNewWord();
	}

	protected void WhileStateGuessing()
	{
		string inputString = Input.inputString;
		foreach (char c in inputString)
		{
			GuessLetter(c);
		}
		gameView.UpdateTimer(letterTimer.TimeLeft / letterTimer.TimerDuration);
	}

	private Dictionary<string, string> GetWordsForDifficulty()
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		string text = Localization.Get(SelectedLevel<TTLevelSetting>().TranslationKey);
		string[] array = text.Split(',');
		int num = (int)(1 - SelectedLevel<TTLevelSetting>().TranslateDirection);
		string[] array2 = array;
		foreach (string text2 in array2)
		{
			if (text2.Length > 0)
			{
				string[] array3 = text2.Split(':');
				if (array3.Length != 2)
				{
					Debug.LogWarning("Hey Buddy, I expected two values while parsing: " + text2);
				}
				else if (dictionary.ContainsKey(array3[num]))
				{
					Debug.LogWarning("Hey Buddy, You added word '" + array3[num] + "' twice.");
				}
				else if (array3[0].Length + array3[1].Length >= 24)
				{
					Debug.LogWarning("Hey Buddy, the word '" + array3[num] + "' is too long for the board.");
				}
				else
				{
					dictionary.Add(array3[num], array3[1 - num]);
				}
			}
		}
		return dictionary;
	}

	public char RemoveDiacritics(char c)
	{
		return c.ToString().Normalize(NormalizationForm.FormD)[0];
	}

	public string RemoveDiacritics(string s)
	{
		string text = string.Empty;
		foreach (char c in s)
		{
			text += RemoveDiacritics(c);
		}
		return text;
	}

	private void CreateNewWord()
	{
		PickAnswer();
		knownLetters = new char[answerNoSpace.Length];
		wrongLetters = 0;
		guessedLetters = new List<char>();
		gameView.InitKeys(this);
		gameView.InitSpots(answer);
		gameView.SetLevel(correctWords, SelectedLevel<TTLevelSetting>().TargetWordCount);
		gameView.SetBeagleBoy(wrongLetters);
		WordPickedEvent wordPickedEvent = new WordPickedEvent();
		wordPickedEvent.Answer = answer;
		GameEvents.Invoke(wordPickedEvent);
		letterTimer.StartCountdown(SelectedLevel<TTLevelSetting>().TimePerLetter, WrongLetter, destroyWhenFinished: false);
	}

	private void PickAnswer()
	{
		KeyValuePair<string, string> random = wordsTranslations.GetRandom();
		gameView.SetCategory(random.Key);
		answer = random.Value;
		answerNoSpace = answer.Replace(" ", string.Empty);
		answerNormalised = RemoveDiacritics(answerNoSpace);
		if (wordsUsed.Count >= wordsTranslations.Count)
		{
			wordsUsed.Clear();
		}
		if (wordsUsed.Contains(answer))
		{
			PickAnswer();
		}
		else
		{
			wordsUsed.Add(answer);
		}
	}

	private void CorrectLetter(char c)
	{
		AudioController.Play(correctClip.name);
		totalCorrectLetters++;
		score++;
		gameView.SetScore(score);
		string text = ((RemoveDiacritics(c) == c) ? answerNormalised.ToLower() : answerNoSpace.ToLower());
		text = text.Replace(" ", string.Empty);
		for (int i = 0; i < text.Length; i++)
		{
			if (text[i] == c)
			{
				knownLetters[i] = answerNoSpace[i];
			}
		}
		gameView.UpdateSpots(knownLetters);
		gameView.SetButtonState(c, correct: true);
		if (!knownLetters.Contains('\0'))
		{
			WordCompleted();
		}
		else
		{
			letterTimer.StartCountdown(SelectedLevel<TTLevelSetting>().TimePerLetter, WrongLetter, destroyWhenFinished: false);
		}
	}

	private void WrongLetter()
	{
		AudioController.Play(incorrectClip.name);
		wrongLetters++;
		totalWrongLetters++;
		score--;
		gameView.SetScore(score);
		gameView.SetBeagleBoy(wrongLetters);
		if (wrongLetters >= 7)
		{
			WordFailed();
		}
		letterTimer.StartCountdown(SelectedLevel<TTLevelSetting>().TimePerLetter, WrongLetter, destroyWhenFinished: false);
	}

	private void WordCompleted()
	{
		correctWords++;
		score += 10;
		gameView.SetLevel(correctWords, SelectedLevel<TTLevelSetting>().TargetWordCount);
		gameView.SetScore(score);
		gameAvatar.GetComponent<Animator>().SetTrigger("Succes");
		base.StateMachine.SwitchTo("ShowingAnswer");
	}

	private void WordFailed()
	{
		GetView<HeartsView>().LoseHeart();
		gameView.VictoriousBeagleBoy(incorrectWords);
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
		bool correct = !knownLetters.Contains('\0');
		knownLetters = answer.Replace(" ", string.Empty).ToCharArray();
		gameView.UpdateSpots(knownLetters);
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
		if (correctWords >= SelectedLevel<TTLevelSetting>().TargetWordCount)
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
		if (base.StateMachine.CurrentState.Name != "GuessingAnswer")
		{
			return;
		}
		c = char.ToLower(c);
		if (!guessedLetters.Contains(c))
		{
			guessedLetters.Add(c);
			if (answer.IndexOf(c.ToString(), StringComparison.OrdinalIgnoreCase) >= 0 || answerNormalised.IndexOf(c.ToString(), StringComparison.OrdinalIgnoreCase) >= 0)
			{
				CorrectLetter(c);
			}
			else
			{
				gameView.SetButtonState(c, correct: false);
				WrongLetter();
			}
			LetterGuessedEvent letterGuessedEvent = new LetterGuessedEvent();
			letterGuessedEvent.letter = c;
			GameEvents.Invoke(letterGuessedEvent);
		}
	}
}
