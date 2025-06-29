using System;
using System.Collections;
using System.Collections.Generic;
using SLAM.CameraSystem;
using SLAM.Engine;
using SLAM.Notifications;
using SLAM.Slinq;
using SLAM.Utils;
using UnityEngine;

namespace SLAM.MoneyDive;

public class MD_Controller : GameController
{
	public class EquationAnsweredEvent
	{
		public UIButton ButtonPressed;

		public bool IsCorrectAnswer;

		public int Answer;
	}

	public class EquationVisibleEvent
	{
		public Equation Equation;

		public UIButton ButtonWithCorrectAnwer;
	}

	private const int POINTS_PER_EQUATION = 100;

	[SerializeField]
	private MD_LevelSpawner levelSpawner;

	[SerializeField]
	private MD_Cinematics cinematics;

	[SerializeField]
	private MD_AvatarController avatarController;

	[SerializeField]
	private CameraManager cameraManager;

	[SerializeField]
	private CameraBehaviour gameCamera;

	[SerializeField]
	private Trick[] tricks;

	[SerializeField]
	private DifficultySettings[] settings;

	[SerializeField]
	private int[] comboPointsLookup;

	[SerializeField]
	private PrefabSpawner easyTrickParticleSpawner;

	[SerializeField]
	private PrefabSpawner mediumTrickParticleSpawner;

	[SerializeField]
	private PrefabSpawner hardTrickParticleSpawner;

	[SerializeField]
	private MD_SpeedFX speedFX;

	public static bool ShouldSkipLongIntro;

	private DifficultySettings currentSettings;

	private int score;

	private int trickScore;

	private int bonusScore;

	private int equationCount;

	private int correctAnswers;

	private Alarm gameTimer;

	private List<Equation> equations;

	private EquationGenerator equationGenerator;

	private Action<Action> introDelegate;

	public override LevelSetting[] Levels => settings;

	private Equation CurrentEquation
	{
		get
		{
			if (equations.Count <= 0)
			{
				PopulateEquations();
			}
			return equations.FirstOrDefault();
		}
	}

	private bool HasCompletedLevel => equationCount >= currentSettings.RequiredEquationCount;

	public override int GameId => 2;

	public override string IntroNPCKey => "NPC_NAME_SCROOGE";

	public override string IntroTextKey => "MD_CINEMATICINTRO_TEXT";

	public override Dictionary<string, int> ScoreCategories
	{
		get
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			dictionary.Add(StringFormatter.GetLocalizationFormatted("MD_VICTORYWINDOW_CORRECT_ANSWERS"), correctAnswers * 100);
			dictionary.Add(Localization.Get("MD_VICTORYWINDOW_SCORE_BONUS"), bonusScore);
			return dictionary;
		}
	}

	public override Portrait DuckCharacter => Portrait.ScroogeDuck;

	private void OnEnable()
	{
		GameEvents.Subscribe<EquationAnsweredEvent>(onEquationAnswered);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<EquationAnsweredEvent>(onEquationAnswered);
	}

	protected override void Awake()
	{
		base.Awake();
		equations = new List<Equation>();
		equationGenerator = new EquationGenerator();
	}

	protected override void Start()
	{
		base.Start();
		gameTimer = Alarm.Create();
	}

	public override void Play(LevelSetting selectedLevel)
	{
		base.Play(selectedLevel);
		CloseView<HUDView>();
		currentSettings = selectedLevel as DifficultySettings;
		Trick[] array = new Trick[currentSettings.TrickIds.Length];
		int i;
		for (i = 0; i < currentSettings.TrickIds.Length; i++)
		{
			array[i] = tricks.FirstOrDefault((Trick t) => t.Id == currentSettings.TrickIds[i]);
		}
		avatarController.Init(array);
	}

	public override void Pause()
	{
		base.Pause();
		avatarController.Pause();
	}

	public override void Resume()
	{
		base.Resume();
		avatarController.UnPause();
	}

	public void FadeInAndOut(Action fadeInComplete, Action fadeOutComplete)
	{
		OpenView<FadeView>(delegate
		{
			if (fadeInComplete != null)
			{
				fadeInComplete();
			}
			CloseView<FadeView>(delegate
			{
				if (fadeOutComplete != null)
				{
					fadeOutComplete();
				}
			});
		});
	}

	public void TrickPerformed(bool succes, Trick trick, int comboLevel)
	{
		MD_HUDView view = GetView<MD_HUDView>();
		if (succes)
		{
			int num = 100;
			int bonus = GetBonus(comboLevel);
			trickScore += num;
			bonusScore += bonus;
			score += bonus + num;
			equationCount++;
			view.UpdateTrickProgress(equationCount, currentSettings.RequiredEquationCount);
			view.DoTrickPointsPling(num);
			view.PlayBonusReceived(GetBonus(comboLevel));
			view.UpdateBonusCounter(GetBonus(comboLevel + 1));
			switch (trick.Complexity)
			{
			default:
				easyTrickParticleSpawner.Spawn();
				AudioController.Play("MD_moveSimple_executed");
				break;
			case TrickComplexity.Medium:
				mediumTrickParticleSpawner.Spawn();
				AudioController.Play("MD_moveMedium_executed");
				break;
			case TrickComplexity.Hard:
				hardTrickParticleSpawner.Spawn();
				AudioController.Play("MD_moveHard_executed");
				break;
			}
			switch (trick.Name)
			{
			case "Peekaboo":
				AudioController.Play("MD_move_showHands");
				break;
			case "Roll":
				AudioController.Play("MD_move_salto");
				break;
			case "Corkscrew":
				AudioController.Play("MD_move_corkscrew");
				break;
			case "Ballet":
				AudioController.Play("MD_move_pirouette");
				break;
			case "Swim":
				AudioController.Play("MD_move_swim");
				break;
			case "Turbulence":
				AudioController.Play("MD_move_glide");
				break;
			case "Pretzel":
				AudioController.Play("MD_move_pretzel");
				break;
			case "TeaParty":
				AudioController.Play("MD_move_drink");
				break;
			}
		}
		else
		{
			int bonus2 = GetBonus(comboLevel + 1);
			view.PlayBonusLost(bonus2);
			view.UpdateBonusCounter(0);
			AudioController.Play("MD_move_fail");
		}
	}

	private IEnumerator WaitForAvatar()
	{
		while (avatarController.AreControlsLocked)
		{
			yield return null;
		}
		GetView<MD_HUDView>().SetEquation(CurrentEquation);
		GetView<MD_HUDView>().ShowEquation();
		AudioController.Play("MD_new_combo_appears");
	}

	private void onEquationAnswered(EquationAnsweredEvent evt)
	{
		if (!avatarController.AreControlsLocked)
		{
			GetView<MD_HUDView>().HideEquation();
			if (evt.IsCorrectAnswer)
			{
				correctAnswers++;
				avatarController.DoGoodTrick();
			}
			else
			{
				avatarController.DoFailTrick();
			}
			equations.RemoveAt(0);
			StartCoroutine(WaitForAvatar());
		}
		if (evt.IsCorrectAnswer && equationCount == currentSettings.RequiredEquationCount)
		{
			NotificationEvent notificationEvent = new NotificationEvent();
			notificationEvent.Title = StringFormatter.GetLocalizationFormatted("UI_GAME_THRESHOLD_REACHED_TITLE", currentSettings.Difficulty);
			notificationEvent.Body = Localization.Get("UI_GAME_THRESHOLD_REACHED_BODY");
			notificationEvent.IconSpriteName = "Achiev_Default_HighestLevel";
			GameEvents.Invoke(notificationEvent);
		}
	}

	private void PopulateEquations(int amount = 5)
	{
		EquationSettings[] array = currentSettings.Equations;
		foreach (EquationSettings equationSettings in array)
		{
			equationGenerator.Tables = new List<int>();
			equationGenerator.Manipulators = new List<Manipulator>();
			for (int j = 0; j <= equationSettings.MaximumTable; j++)
			{
				if (!equationSettings.ExcludedTables.Contains(j))
				{
					equationGenerator.Tables.Add(j);
				}
			}
			equationGenerator.Manipulators.Add(equationSettings.Manipulator);
			equationGenerator.RestrictedToTenths = equationSettings.IsRestrictedToTenths;
			List<Equation> list = equationGenerator.GetEquations(amount);
			foreach (Equation item in list)
			{
				equations.Add(item);
			}
		}
		equations.Shuffle();
	}

	protected override void OnEnterStateRunning()
	{
		base.OnEnterStateRunning();
		if (ShouldSkipLongIntro)
		{
			introDelegate = cinematics.PlayShortIntro;
		}
		else
		{
			introDelegate = cinematics.PlayIntro;
		}
		introDelegate(delegate
		{
			OpenView<HUDView>();
			cameraManager.CrossFade(gameCamera, 0f);
			gameTimer.StartCountdown(currentSettings.GameDuration, OnGameTimerEnded);
			speedFX.Show(currentSettings.GameDuration);
			float num = levelSpawner.ChunkHeight / 15f;
			float num2 = Mathf.Abs(avatarController.transform.position.y - levelSpawner.ChunkHeight / 2f) / 15f;
			float waitFor = num2 + num * 2f;
			levelSpawner.StartSpawning(waitFor, num);
			MD_HUDView view = GetView<MD_HUDView>();
			view.SetEquation(CurrentEquation);
			view.ShowEquation();
			view.UpdateTrickProgress(equationCount, currentSettings.RequiredEquationCount);
			view.UpdateBonusCounter(GetBonus(0));
		});
		ShouldSkipLongIntro = true;
	}

	private void OnGameTimerEnded()
	{
		CloseView<MD_HUDView>();
		cinematics.PlayOutro(HasCompletedLevel, delegate
		{
			Finish(HasCompletedLevel);
		});
	}

	private int GetBonus(int forComboLevel)
	{
		return comboPointsLookup[Mathf.Clamp(forComboLevel - 1, 0, comboPointsLookup.Length - 1)];
	}
}
