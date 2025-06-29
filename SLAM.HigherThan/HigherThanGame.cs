using System;
using System.Collections.Generic;
using SLAM.Avatar;
using SLAM.Engine;
using SLAM.Notifications;
using UnityEngine;

namespace SLAM.HigherThan;

public class HigherThanGame : GameController
{
	public enum Answer
	{
		Lower,
		Higher
	}

	public class EquationAnsweredEvent
	{
		public Equation Equation;

		public Answer Answer;

		public bool IsCorrectAnswer;

		public int Progress;
	}

	public class NewEquationEvent
	{
		public Equation Equation;

		public UIButton ButtonWithCorrectAnwer;

		public int Level;

		public int LevelToReach;
	}

	[Serializable]
	public class HTDifficultySettings : LevelSetting
	{
		public float Duration = 50f;

		public int LevelToReach = 5;

		public EquationDifficultySetting[] EquationSettings;
	}

	public class ScoreChangedEvent
	{
		public int Score;

		public int DeltaChange;
	}

	public class TimeChangedEvent
	{
		public float Time;

		public float DeltaChange;
	}

	public const int ANSWERS_PER_LEVEL = 5;

	public const int POINTS_PER_LEVEL = 50;

	public const int POINTS_PER_GOOD_ANSWER = 10;

	public const float EXTRA_TIME_PER_LEVEL = 5f;

	public const float TIME_PENALTY_PER_WRONG_ANSWER = -5f;

	[Header("Higher Than properties")]
	[SerializeField]
	private HTDifficultySettings[] levels;

	[SerializeField]
	private HTequationGenerator generator;

	[SerializeField]
	private Camera introCam;

	[SerializeField]
	private AvatarSpawn gameAvatar;

	[SerializeField]
	private UIButton leftSideButton;

	[SerializeField]
	private UIButton rightSideButton;

	[SerializeField]
	private AudioClip correctClip;

	[SerializeField]
	private AudioClip incorrectClip;

	[SerializeField]
	private AudioClip levelupClip;

	private int score;

	private int progress;

	private int wrongAnswers;

	private int rightAnswers;

	private Alarm timer;

	private HTDifficultySettings currentSettings;

	public override Dictionary<string, int> ScoreCategories
	{
		get
		{
			int value = rightAnswers * 10;
			int value2 = LevelUpIndex * 50;
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			dictionary.Add(StringFormatter.GetLocalizationFormatted("HT_VICTORYWINDOW_CORRECT_ANSWERS", rightAnswers, 10), value);
			dictionary.Add(StringFormatter.GetLocalizationFormatted("HT_VICTORYWINDOW_LEVEL", LevelUpIndex, 50), value2);
			return dictionary;
		}
	}

	public override string IntroNPCKey => "NPC_NAME_WARBOL";

	public override string IntroTextKey => "HT_CINEMATICINTRO_TEXT";

	public int LevelUpIndex { get; protected set; }

	public Equation CurrentEquation { get; protected set; }

	public int CorrectAnswers => rightAnswers;

	public int CorrectAnswersToReach => currentSettings.LevelToReach * 5;

	public override int GameId => 23;

	public override LevelSetting[] Levels => levels;

	public override Portrait DuckCharacter => Portrait.Warbol;

	public override void Play(LevelSetting selectedLevel)
	{
		base.Play(selectedLevel);
		ScoreChangedEvent scoreChangedEvent = new ScoreChangedEvent();
		scoreChangedEvent.Score = 0;
		GameEvents.Invoke(scoreChangedEvent);
	}

	protected override void OnEnterStateRunning()
	{
		base.OnEnterStateRunning();
		OpenView<HTgameView>();
		currentSettings = SelectedLevel<HTDifficultySettings>();
		GetView<HTgameView>().UpdateMaxTime(currentSettings.Duration);
		introCam.gameObject.SetActive(value: false);
		gameAvatar.SpawnAvatar();
		int num = (LevelUpIndex = (wrongAnswers = (rightAnswers = 0)));
		score = (progress = num);
		timer = Alarm.Create();
		timer.StartCountdown(currentSettings.Duration, OnGameOver);
		CurrentEquation = generator.GetEquation(currentSettings.EquationSettings.GetRandom());
		NewEquationEvent newEquationEvent = new NewEquationEvent();
		newEquationEvent.Equation = CurrentEquation;
		newEquationEvent.ButtonWithCorrectAnwer = ((!(CurrentEquation.leftAnswer > CurrentEquation.rightAnswer)) ? rightSideButton : leftSideButton);
		newEquationEvent.Level = LevelUpIndex;
		newEquationEvent.LevelToReach = currentSettings.LevelToReach;
		GameEvents.Invoke(newEquationEvent);
	}

	protected override void WhileStateRunning()
	{
		if (SLAMInput.Provider.GetButtonDown(SLAMInput.Button.Left))
		{
			AnswerEquation(Answer.Higher);
		}
		else if (SLAMInput.Provider.GetButtonDown(SLAMInput.Button.Right))
		{
			AnswerEquation(Answer.Lower);
		}
		GetView<HTgameView>().UpdateTimer(timer.TimeLeft);
	}

	public void AnswerEquation(Answer answer)
	{
		bool flag = isCorrectAnswer(answer);
		if (flag)
		{
			CorrectAnswer();
		}
		else
		{
			IncorrectAnswer();
		}
		EquationAnsweredEvent equationAnsweredEvent = new EquationAnsweredEvent();
		equationAnsweredEvent.IsCorrectAnswer = flag;
		equationAnsweredEvent.Answer = answer;
		equationAnsweredEvent.Equation = CurrentEquation;
		equationAnsweredEvent.Progress = progress;
		GameEvents.Invoke(equationAnsweredEvent);
		CurrentEquation = generator.GetEquation(currentSettings.EquationSettings.GetRandom());
		NewEquationEvent newEquationEvent = new NewEquationEvent();
		newEquationEvent.Equation = CurrentEquation;
		newEquationEvent.ButtonWithCorrectAnwer = ((!(CurrentEquation.leftAnswer > CurrentEquation.rightAnswer)) ? rightSideButton : leftSideButton);
		newEquationEvent.Level = LevelUpIndex;
		newEquationEvent.LevelToReach = currentSettings.LevelToReach;
		GameEvents.Invoke(newEquationEvent);
	}

	private bool isCorrectAnswer(Answer userAnswer)
	{
		return (CurrentEquation.leftAnswer > CurrentEquation.rightAnswer && userAnswer == Answer.Higher) || (CurrentEquation.leftAnswer < CurrentEquation.rightAnswer && userAnswer == Answer.Lower);
	}

	private void CorrectAnswer()
	{
		AudioController.Play(correctClip.name);
		rightAnswers++;
		progress++;
		score += 10;
		ScoreChangedEvent scoreChangedEvent = new ScoreChangedEvent();
		scoreChangedEvent.Score = score;
		scoreChangedEvent.DeltaChange = 10;
		GameEvents.Invoke(scoreChangedEvent);
		if (progress >= 5)
		{
			AudioController.Play(levelupClip.name);
			gameAvatar.GetComponent<Animator>().SetTrigger("Succes");
			LevelUpIndex++;
			progress = 0;
			float num = 5f - Mathf.Clamp(LevelUpIndex - currentSettings.LevelToReach, 0f, 5f);
			timer.TimeLeft += num;
			score += 50;
			if (num > 0f)
			{
				TimeChangedEvent timeChangedEvent = new TimeChangedEvent();
				timeChangedEvent.Time = timer.TimeLeft;
				timeChangedEvent.DeltaChange = num;
				GameEvents.Invoke(timeChangedEvent);
			}
			scoreChangedEvent = new ScoreChangedEvent();
			scoreChangedEvent.Score = score;
			scoreChangedEvent.DeltaChange = 50;
			GameEvents.Invoke(scoreChangedEvent);
		}
		if (rightAnswers == currentSettings.LevelToReach * 5)
		{
			NotificationEvent notificationEvent = new NotificationEvent();
			notificationEvent.Title = StringFormatter.GetLocalizationFormatted("UI_GAME_THRESHOLD_REACHED_TITLE", currentSettings.Difficulty);
			notificationEvent.Body = Localization.Get("UI_GAME_THRESHOLD_REACHED_BODY");
			notificationEvent.IconSpriteName = "Achiev_Default_HighestLevel";
			GameEvents.Invoke(notificationEvent);
		}
	}

	private void IncorrectAnswer()
	{
		AudioController.Play(incorrectClip.name);
		gameAvatar.GetComponent<Animator>().SetTrigger("Failure");
		wrongAnswers++;
		timer.TimeLeft += -5f;
		TimeChangedEvent timeChangedEvent = new TimeChangedEvent();
		timeChangedEvent.Time = timer.TimeLeft;
		timeChangedEvent.DeltaChange = -5f;
		GameEvents.Invoke(timeChangedEvent);
	}

	private void OnGameOver()
	{
		Finish(LevelUpIndex >= currentSettings.LevelToReach);
	}
}
