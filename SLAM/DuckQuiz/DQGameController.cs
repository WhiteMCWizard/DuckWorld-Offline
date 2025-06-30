using System;
using System.Collections.Generic;
using SLAM.Engine;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.DuckQuiz;

public class DQGameController : GameController
{
	[Flags]
	public enum QuestionCategory
	{
		Animals = 1,
		Disney = 2,
		Duck = 4,
		History = 8,
		Geography = 0x10,
		Capitals = 0x20,
		Flags = 0x40
	}

	[Flags]
	public enum QuestionDifficulty
	{
		Easy = 1,
		Medium = 2,
		Hard = 4
	}

	public enum GuideType
	{
		Clock,
		Clover,
		Feather
	}

	[Serializable]
	public class DifficultySettings : LevelSetting
	{
		[BitMask(typeof(QuestionCategory))]
		public QuestionCategory Categories;

		[BitMask(typeof(QuestionDifficulty))]
		public QuestionDifficulty Difficulties;

		public int QuestionCount;

		public bool HasBonusRound;

		public bool HasGuides = true;

		public float TimeInSecondsPerAnswer;
	}

	public class GameStartedEvent
	{
		public DifficultySettings settings;
	}

	public class QuestionProposedEvent
	{
		public DQQuestion Question;

		public int QuestionCount;

		public float QuestionTime;
	}

	public class QuestionAnsweredEvent
	{
		public DQQuestion Question;

		public DQAnswer Answer;
	}

	public class GuideClickedEvent
	{
		public GuideType Guide;
	}

	public class ScoreGainedEvent
	{
		public int ScoreGained;

		public int TotalScore;
	}

	public class ScoreCounterFinishedEvent
	{
	}

	public const string STATE_SHOW_CATEGORY = "Show_Category";

	public const string STATE_SHOW_QUESTION = "Show_Question";

	public const string STATE_SHOW_ANSWER = "Show_Answer";

	public const string STATE_QUESTION_CORRECT = "Question_Correct";

	public const string STATE_QUESTION_INCORRECT = "Question_Incorrect";

	public const string STATE_SHOW_BONUSROUND = "Show_Bonusround";

	public const int BONUS_ROUND_QUESTION_COUNT = 5;

	[SerializeField]
	private DQQuestionGenerator questionGenerator;

	[SerializeField]
	private DQCameraShotManager cameraShotManager;

	[SerializeField]
	private DifficultySettings[] settings;

	[SerializeField]
	private float showCategoryVisibleTime = 2f;

	[SerializeField]
	private float showQuestionResultVisibleTime = 2f;

	[SerializeField]
	private float showQuestionOutcomeResponseVisibleTime = 2f;

	[SerializeField]
	private float showBonusRoundIntroVisibleTime = 2f;

	private IEnumerator<DQQuestion> questionSet;

	private Alarm alarm;

	private DQAnswer lastGivenAnswer;

	private int score;

	private int bonusRoundScore;

	private int lives;

	private int questionCount;

	private DQQuestionView questionView;

	private bool isInBonusRound;

	public override LevelSetting[] Levels => settings;

	public override int GameId => 36;

	public override Portrait DuckCharacter => Portrait.DonaldDuck;

	public override Dictionary<string, int> ScoreCategories
	{
		get
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			dictionary.Add(Localization.Get("DQ_SCOREWINDOW_TOTALSCORE"), score);
			Dictionary<string, int> dictionary2 = dictionary;
			if (SelectedLevel<DifficultySettings>().HasBonusRound)
			{
				dictionary2.Add(Localization.Get("DQ_SCOREWINDOW_BONUS_ROUND"), bonusRoundScore);
			}
			return dictionary2;
		}
	}

	public override string IntroNPCKey => "NPC_NAME_DONALD";

	public override string IntroTextKey => "DQ_CINEMATICINTRO_TEXT";

	private void OnEnable()
	{
		GameEvents.Subscribe<QuestionAnsweredEvent>(onQuestionAnswered);
		GameEvents.Subscribe<GuideClickedEvent>(onGuideClicked);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<QuestionAnsweredEvent>(onQuestionAnswered);
		GameEvents.Unsubscribe<GuideClickedEvent>(onGuideClicked);
	}

	public override void Play(LevelSetting selectedLevel)
	{
		base.Play(selectedLevel);
		GameStartedEvent gameStartedEvent = new GameStartedEvent();
		gameStartedEvent.settings = SelectedLevel<DifficultySettings>();
		GameEvents.Invoke(gameStartedEvent);
	}

	private void onTimeUp()
	{
		QuestionAnsweredEvent questionAnsweredEvent = new QuestionAnsweredEvent();
		questionAnsweredEvent.Answer = null;
		questionAnsweredEvent.Question = questionSet.Current;
		GameEvents.Invoke(questionAnsweredEvent);
	}

	private void doPauseTimeClicked()
	{
		alarm.Pause();
	}

	private void doFiftyFiftyClicked()
	{
		List<int> list = new List<int>();
		for (int i = 0; i < questionSet.Current.Answers.Length; i++)
		{
			if (!questionSet.Current.Answers[i].Correct)
			{
				list.Add(i);
			}
		}
		while (list.Count > 2)
		{
			list.RemoveAt(UnityEngine.Random.Range(0, list.Count));
		}
		questionView.DisableAnswers(list);
	}

	private void doSkipQuestionClicked()
	{
		questionCount--;
		questionSet = getExtraQuestionSet(questionSet);
		base.StateMachine.SwitchTo("Show_Question");
	}

	private IEnumerator<DQQuestion> getExtraQuestionSet(IEnumerator<DQQuestion> set)
	{
		while (set.MoveNext())
		{
			yield return set.Current;
		}
		yield return questionGenerator.GetQuestions(Localization.language, questionSet.Current.Category, SelectedLevel<DifficultySettings>().Difficulties).First();
	}

	private bool shouldGoToBonusRound()
	{
		return !isInBonusRound && SelectedLevel<DifficultySettings>().HasBonusRound && lives > 1;
	}

	protected override void AddStates()
	{
		base.AddStates();
		base.StateMachine.AddState("Show_Category", (Action)Delegate.Combine(new Action(OnEnterStateShowCategory), new Action(SwitchCameraToCurrentState)), null, OnExitStateShowCategory);
		base.StateMachine.AddState("Show_Question", (Action)Delegate.Combine(new Action(OnEnterStateShowQuestion), new Action(SwitchCameraToCurrentState)), WhileStateShowQuestion, OnExitStateShowQuestion);
		base.StateMachine.AddState("Show_Answer", (Action)Delegate.Combine(new Action(OnEnterStateShowAnswer), new Action(SwitchCameraToCurrentState)), WhileStateShowAnswer, OnExitStateShowAnswer);
		base.StateMachine.AddState("Show_Bonusround", (Action)Delegate.Combine(new Action(OnEnterStateShowBonusRound), new Action(SwitchCameraToCurrentState)), null, OnExitStateShowBonusRound);
		base.StateMachine.AddState("Question_Correct", (Action)Delegate.Combine(new Action(OnEnterStateQuestionCorrect), new Action(SwitchCameraToCurrentState)), WhileStateQuestionCorrect, OnExitStateQuestionCorrect);
		base.StateMachine.AddState("Question_Incorrect", (Action)Delegate.Combine(new Action(OnEnterStateQuestionIncorrect), new Action(SwitchCameraToCurrentState)), WhileStateQuestionIncorrect, OnExitStateQuestionIncorrect);
	}

	protected override void OnEnterStateRunning()
	{
		base.OnEnterStateRunning();
		alarm = Alarm.Create();
		OpenView<HeartsView>().SetTotalHeartCount(lives = 3);
		GetView<DQHudView>().SetTotalQuestionCount(SelectedLevel<DifficultySettings>().QuestionCount);
		base.StateMachine.SwitchTo("Show_Category");
	}

	private void SwitchCameraToCurrentState()
	{
		if (isInBonusRound && base.StateMachine.CurrentState.Name == "Show_Question")
		{
			cameraShotManager.SwitchToCameraShot("Show_Bonusround");
		}
		else
		{
			cameraShotManager.SwitchToCameraShot(base.StateMachine.CurrentState.Name);
		}
	}

	private void OnEnterStateShowCategory()
	{
		questionSet = questionGenerator.GetQuestions(Localization.language, SelectedLevel<DifficultySettings>().Categories, SelectedLevel<DifficultySettings>().Difficulties).Take(SelectedLevel<DifficultySettings>().QuestionCount).GetEnumerator();
		questionView = GetView<DQQuestionView>();
		OpenView<DQCategoryView>().SetInfo(SelectedLevel<DifficultySettings>().Categories);
		AudioController.Stop("DuckQuiz - introscreen", 0.2f);
		AudioController.Play("DQ_SFX_intro");
		base.StateMachine.SwitchTo("Show_Question", showCategoryVisibleTime);
	}

	private void OnExitStateShowCategory()
	{
		CloseView<DQCategoryView>();
	}

	private void OnEnterStateShowQuestion()
	{
		if (IsViewOpen<HeartsView>() && lives <= 0)
		{
			Finish(succes: false);
			return;
		}
		if (!questionSet.MoveNext())
		{
			if (shouldGoToBonusRound())
			{
				base.StateMachine.SwitchTo("Show_Bonusround");
			}
			else
			{
				Finish(succes: true);
			}
			return;
		}
		Debug.Log("OnEnterStateShowQuestion: " + questionSet.Current);
		alarm.StartCountdown(SelectedLevel<DifficultySettings>().TimeInSecondsPerAnswer, onTimeUp, destroyWhenFinished: false);
		questionCount++;
		QuestionProposedEvent questionProposedEvent = new QuestionProposedEvent();
		questionProposedEvent.Question = questionSet.Current;
		questionProposedEvent.QuestionCount = questionCount;
		questionProposedEvent.QuestionTime = SelectedLevel<DifficultySettings>().TimeInSecondsPerAnswer;
		GameEvents.Invoke(questionProposedEvent);
		questionView.Open();
		questionView.SetInfo(questionSet.Current, alarm);
	}

	private void WhileStateShowQuestion()
	{
		if (cameraShotManager.TimeSinceLastCameraSwitch > alarm.TimeLeft && alarm.TimeLeft > 1f)
		{
			SwitchCameraToCurrentState();
		}
	}

	private void OnExitStateShowQuestion()
	{
		alarm.Pause();
	}

	private void OnEnterStateShowAnswer()
	{
		questionView.ShowResult(lastGivenAnswer);
		if (lastGivenAnswer != null && lastGivenAnswer.Correct)
		{
			base.StateMachine.SwitchTo("Question_Correct", showQuestionResultVisibleTime);
		}
		else
		{
			base.StateMachine.SwitchTo("Question_Incorrect", showQuestionResultVisibleTime);
		}
	}

	private void WhileStateShowAnswer()
	{
		if (SLAMInput.Provider.AnyKeyDown())
		{
			if (lastGivenAnswer != null && lastGivenAnswer.Correct)
			{
				base.StateMachine.SwitchTo("Question_Correct", 0f, stopOtherTimedSwitches: true);
			}
			else
			{
				base.StateMachine.SwitchTo("Question_Incorrect", 0f, stopOtherTimedSwitches: true);
			}
		}
	}

	private void OnExitStateShowAnswer()
	{
		questionView.Close();
	}

	private void OnEnterStateQuestionCorrect()
	{
		int num = (int)alarm.TimeLeft * 10;
		if (isInBonusRound)
		{
			num *= 2;
			bonusRoundScore += num;
		}
		else
		{
			score += num;
		}
		ScoreGainedEvent scoreGainedEvent = new ScoreGainedEvent();
		scoreGainedEvent.ScoreGained = num;
		scoreGainedEvent.TotalScore = score + bonusRoundScore;
		GameEvents.Invoke(scoreGainedEvent);
		base.StateMachine.SwitchTo("Show_Question", showQuestionOutcomeResponseVisibleTime);
	}

	private void WhileStateQuestionCorrect()
	{
		if (SLAMInput.Provider.AnyKeyDown())
		{
			base.StateMachine.SwitchTo("Show_Question", 0f, stopOtherTimedSwitches: true);
		}
	}

	private void OnExitStateQuestionCorrect()
	{
	}

	private void OnEnterStateQuestionIncorrect()
	{
		GetView<HeartsView>().LoseHeart();
		lives--;
		base.StateMachine.SwitchTo("Show_Question", showQuestionOutcomeResponseVisibleTime);
	}

	private void WhileStateQuestionIncorrect()
	{
		if (SLAMInput.Provider.AnyKeyDown())
		{
			base.StateMachine.SwitchTo("Show_Question", 0f, stopOtherTimedSwitches: true);
		}
	}

	private void OnExitStateQuestionIncorrect()
	{
	}

	private void OnEnterStateShowBonusRound()
	{
		CloseView<HeartsView>();
		isInBonusRound = true;
		questionCount = 0;
		QuestionCategory questionCategory = QuestionCategory.Flags;
		OpenView<DQBonusRoundView>().SetInfo(questionCategory);
		int index = SelectedLevel<DifficultySettings>().Index;
		QuestionDifficulty difficulty = ((index <= 5) ? QuestionDifficulty.Easy : ((index > 10) ? QuestionDifficulty.Hard : QuestionDifficulty.Medium));
		questionSet = questionGenerator.GetQuestions(Localization.language, questionCategory, difficulty).Take(5).GetEnumerator();
		switch (questionCategory)
		{
		case QuestionCategory.Flags:
			questionView = GetView<DQFlagQuestionView>();
			break;
		case QuestionCategory.Capitals:
			questionView = GetView<DQQuestionView>();
			break;
		}
		questionView.DisableGuides();
		GetView<DQHudView>().SetTotalQuestionCount(5);
		AudioController.Stop("DQ_SFX_intro");
		AudioController.Play("DQ_SFX_intro_bonusround");
		base.StateMachine.SwitchTo("Show_Question", showBonusRoundIntroVisibleTime);
	}

	private void OnExitStateShowBonusRound()
	{
		CloseView<DQBonusRoundView>();
	}

	private void onQuestionAnswered(QuestionAnsweredEvent evt)
	{
		lastGivenAnswer = evt.Answer;
		base.StateMachine.SwitchTo("Show_Answer");
	}

	private void onGuideClicked(GuideClickedEvent evt)
	{
		switch (evt.Guide)
		{
		case GuideType.Clock:
			doPauseTimeClicked();
			break;
		case GuideType.Clover:
			doFiftyFiftyClicked();
			break;
		case GuideType.Feather:
			doSkipQuestionClicked();
			break;
		default:
			Debug.LogWarning(string.Concat("Hey buddy, i dont know this guide ", evt.Guide, "! Create a method in the DQGameController to call."));
			break;
		}
	}
}
