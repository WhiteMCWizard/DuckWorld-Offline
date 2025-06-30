using System;
using System.Collections;
using System.Collections.Generic;
using SLAM.Engine;
using SLAM.Slinq;
using SLAM.Utils;
using UnityEngine;

namespace SLAM.JumpTheCroc;

public class JumpTheCrocGame : GameController
{
	[Serializable]
	public class JumpTheCrocDifficultySettings : LevelSetting
	{
		public int NumberOfQuestions = 10;

		public float Time = 30f;

		public EquationSettings[] Equations;
	}

	[Serializable]
	public class EquationSettings
	{
		public Manipulator Manipulator;

		public bool IsRestrictedToTenths;

		public int MaximumTable;

		public List<int> ExcludedTables;
	}

	public class EquationVisibleEvent
	{
		public Equation Equation;

		public Platform PlatformWithCorrectAnwer;
	}

	public class EquationAnsweredEvent
	{
		public Platform PlatformPressed;

		public bool IsCorrectAnswer;

		public int Answer;
	}

	[SerializeField]
	[Header("Jump the Croc properties")]
	private bool cheatMode;

	[SerializeField]
	private JumpTheCrocDifficultySettings[] settings;

	[SerializeField]
	private GameObject platformParent;

	[SerializeField]
	private GameObject crocParent;

	[SerializeField]
	private Platform StartPlatform;

	[SerializeField]
	private GameObject[] platformPrefabs;

	[SerializeField]
	private GameObject[] edgePrefabs;

	[SerializeField]
	private GameObject FinishPlatformPrefab;

	[SerializeField]
	private GameObject crocSpawnerPrefab;

	[SerializeField]
	private int lives = 3;

	[SerializeField]
	private int pointsPerGoodAnswer = 10;

	[SerializeField]
	private int pointsPerSecondLeft = 10;

	[SerializeField]
	private int platformsInOneRow = 4;

	[SerializeField]
	private int saboteurHeadStart = 3;

	[SerializeField]
	private AnimationCurve moveCurve;

	[SerializeField]
	private AnimationCurve sinkCurve;

	[SerializeField]
	private float jumpDuration = 1f;

	[SerializeField]
	private float sinkDuration = 2f;

	[SerializeField]
	private AnimationCurve landCurve;

	[SerializeField]
	private float landDuration = 0.5f;

	[SerializeField]
	private float horizontalSpacing = 3f;

	[SerializeField]
	private float verticalSpacing = 5f;

	[SerializeField]
	private float horizontalOffsetRange = 1f;

	[SerializeField]
	private float verticalOffsetRange = 1f;

	[SerializeField]
	private float randomRotationRange = 5f;

	[SerializeField]
	private float saboteurTimePerPlatform = 1.5f;

	[SerializeField]
	private PrefabSpawner jumpTakeOffParticles;

	[SerializeField]
	private PrefabSpawner sinkWithPlatformParticles;

	[SerializeField]
	private PrefabSpawner sinkGameOverParticles;

	public GameObject avatarModel;

	public AnimationCurve JumpCurve;

	public GameObject FlintheartBlomgold;

	public JumpTheCrocCameraManager CameraObject;

	private GameObject[,] platformInstances;

	private Animator avatarAnimator;

	private Alarm gameTimer;

	private bool[] correctAnswers;

	[SerializeField]
	private bool lockControls;

	private int sinkCount;

	private int currentQuestion;

	private Platform lastReachedPlatform;

	private Platform finishPlatform;

	private List<List<Platform>> instantiatedPlatforms = new List<List<Platform>>();

	private JTC_CrocSpawner deathCrocSpawner;

	private JumpTheCrocHUDView hud;

	private int saboteurQuestion;

	private Animator flintAnimator;

	private Platform saboteurLastPlatform;

	private JTC_CrocSpawner[] crocs;

	private bool isFinishingSuccesfully;

	private List<Equation> equations;

	private EquationGenerator equationGenerator;

	public override LevelSetting[] Levels => settings;

	private bool AreCheatsEnabled => Application.isEditor && cheatMode;

	private JumpTheCrocDifficultySettings currentSettings => SelectedLevel<JumpTheCrocDifficultySettings>();

	public override Dictionary<string, int> ScoreCategories
	{
		get
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			dictionary.Add(StringFormatter.GetLocalizationFormatted("JTC_VICTORYWINDOW_SCORE_CORRECT_ANSWERS", correctAnswers.Count((bool t) => t), pointsPerGoodAnswer), correctAnswers.Length * pointsPerGoodAnswer);
			dictionary.Add(StringFormatter.GetLocalizationFormatted("JTC_VICTORYWINDOW_SCORE_TIME_LEFT", Mathf.RoundToInt(gameTimer.TimeLeft), pointsPerSecondLeft), Mathf.RoundToInt(gameTimer.TimeLeft) * pointsPerSecondLeft);
			return dictionary;
		}
	}

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

	public override string IntroNPCKey => "NPC_NAME_DONALD";

	public override string IntroTextKey => "JTC_CINEMATICINTRO_TEXT";

	public override int GameId => 1;

	public override Portrait DuckCharacter => Portrait.ScroogeDuck;

	protected override void Start()
	{
		base.Start();
		gameTimer = Alarm.Create();
		equations = new List<Equation>();
		equationGenerator = new EquationGenerator();
		avatarAnimator = avatarModel.GetComponent<Animator>();
		avatarModel.transform.localEulerAngles = Vector3.zero;
		avatarModel.SetActive(value: false);
		flintAnimator = FlintheartBlomgold.GetComponent<Animator>();
		lastReachedPlatform = StartPlatform;
		CameraObject.FlyTo(StartPlatform.transform.position);
	}

	protected override void OnEnterStateRunning()
	{
		base.OnEnterStateRunning();
		lockControls = true;
		StartCoroutine(PlayIntro(delegate
		{
			hud = OpenView<JumpTheCrocHUDView>();
			hud.SetQuestion(CurrentEquation.EquationString);
			OpenView<TimerView>().SetTimer(gameTimer);
			showCurrentRow();
			lockControls = false;
		}));
	}

	protected override void WhileStateRunning()
	{
		base.WhileStateRunning();
		UpdateClickInput();
	}

	public override void Finish(bool succes)
	{
		base.Finish(succes);
		gameTimer.Pause();
		if (succes)
		{
			int num = 0;
			for (int i = 0; i < correctAnswers.Length; i++)
			{
				if (correctAnswers[i])
				{
					num++;
				}
			}
		}
		else
		{
			lockControls = true;
			StopCoroutine(Jump(Vector3.zero, null));
			StopCoroutine(OnGoodAnswer(null));
			StopCoroutine(OnBadAnswer(null));
		}
	}

	public override void Play(LevelSetting selectedLevel)
	{
		base.Play(selectedLevel);
		GenerateLevel();
		SpawnCrocs();
		CloseView<HUDView>();
		correctAnswers = new bool[currentSettings.NumberOfQuestions];
		for (int i = 0; i < correctAnswers.Length; i++)
		{
			correctAnswers[i] = true;
		}
		OpenView<HeartsView>().SetTotalHeartCount(lives);
	}

	public override void Pause()
	{
		lockControls = true;
		gameTimer.Pause();
		base.Pause();
	}

	public override void Resume()
	{
		lockControls = false;
		gameTimer.Resume();
		base.Resume();
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

	private void DestroyCollider()
	{
		if ((bool)avatarModel.GetComponent<Rigidbody>())
		{
			avatarModel.GetComponent<Rigidbody>().useGravity = false;
		}
		if ((bool)avatarModel.GetComponent<Collider>())
		{
			avatarModel.GetComponent<Collider>().enabled = false;
		}
	}

	private void SpawnCrocs()
	{
		UnityEngine.Object.Destroy(deathCrocSpawner);
		crocParent.transform.DestroyChildren();
		deathCrocSpawner = UnityEngine.Object.Instantiate(crocSpawnerPrefab).GetComponent<JTC_CrocSpawner>();
		deathCrocSpawner.name = "KillerCroc";
		crocs = new JTC_CrocSpawner[currentSettings.NumberOfQuestions];
		Vector3 vector = new Vector3((0f - horizontalSpacing) * (float)(platformsInOneRow - 1) / 2f, 0f, verticalSpacing / 1.5f);
		Vector3 vector2 = StartPlatform.transform.position + vector;
		for (int i = 0; i < currentSettings.NumberOfQuestions; i++)
		{
			Vector3 left = vector2 + new Vector3(0f, -0.5f, ((float)i + 0.5f) * verticalSpacing);
			Vector3 right = vector2 + new Vector3((float)platformsInOneRow * horizontalSpacing / 2f, -0.5f, ((float)i + 0.5f) * verticalSpacing);
			GameObject gameObject = UnityEngine.Object.Instantiate(crocSpawnerPrefab);
			gameObject.transform.parent = crocParent.transform;
			JTC_CrocSpawner component = gameObject.GetComponent<JTC_CrocSpawner>();
			component.SpawnBetween(left, right);
			crocs[i] = component;
		}
	}

	private void GenerateLevel()
	{
		platformParent.transform.DestroyChildren();
		if (finishPlatform != null)
		{
			UnityEngine.Object.Destroy(finishPlatform.gameObject);
		}
		instantiatedPlatforms.Clear();
		Vector3 vector = new Vector3((0f - horizontalSpacing) * (float)(platformsInOneRow - 1) / 2f, 0f, verticalSpacing / 1.5f);
		Vector3 vector2 = StartPlatform.transform.position + vector;
		platformInstances = new GameObject[currentSettings.NumberOfQuestions, platformsInOneRow];
		for (int i = 0; i < currentSettings.NumberOfQuestions; i++)
		{
			List<Platform> list = new List<Platform>();
			for (int j = 0; j < platformsInOneRow; j++)
			{
				float x = (float)j * horizontalSpacing + UnityEngine.Random.Range(0f - horizontalOffsetRange, horizontalOffsetRange);
				float y = -0.16f;
				float z = (float)i * verticalSpacing + UnityEngine.Random.Range(0f - verticalOffsetRange, verticalOffsetRange);
				Vector3 position = vector2 + new Vector3(x, y, z);
				GameObject original = platformPrefabs[UnityEngine.Random.Range(0, platformPrefabs.Length - 1)];
				Quaternion rotation = Quaternion.AngleAxis(UnityEngine.Random.Range(0f - randomRotationRange, randomRotationRange), Vector3.up);
				platformInstances[i, j] = UnityEngine.Object.Instantiate(original, position, rotation) as GameObject;
				platformInstances[i, j].transform.parent = platformParent.transform;
				Platform component = platformInstances[i, j].GetComponent<Platform>();
				component.Init(string.Empty, landDuration, landCurve);
				list.Add(component);
			}
			instantiatedPlatforms.Add(list);
		}
		Vector3 position2 = StartPlatform.transform.position + new Vector3(0f, 0f, ((float)currentSettings.NumberOfQuestions + 0.65f) * verticalSpacing);
		finishPlatform = (UnityEngine.Object.Instantiate(FinishPlatformPrefab, position2, Quaternion.AngleAxis(180f, Vector3.up)) as GameObject).GetComponent<Platform>();
		finishPlatform.Init(string.Empty, landDuration, landCurve);
	}

	private IEnumerator PlayIntro(Action callback)
	{
		float levelTime = saboteurTimePerPlatform * (float)currentSettings.NumberOfQuestions + currentSettings.Time;
		yield return new WaitForSeconds(0.5f);
		gameTimer.StartCountdown(levelTime, OnTimeOver, destroyWhenFinished: false);
		StartCoroutine(SaboteurContinuesJumpRoutine());
		avatarModel.SetActive(value: true);
		AudioController.Play("JTC_charAnim_run_pier");
		while (saboteurQuestion < saboteurHeadStart)
		{
			yield return null;
		}
		while (avatarAnimator.GetCurrentAnimatorStateInfo(0).IsName("Run and Stop"))
		{
			yield return null;
		}
		DestroyCollider();
		callback?.Invoke();
	}

	private IEnumerator SaboteurJumpNext()
	{
		if (saboteurQuestion > currentSettings.NumberOfQuestions)
		{
			yield break;
		}
		if (saboteurLastPlatform != null)
		{
			StartCoroutine(saboteurLastPlatform.RestoreOriginalPosition());
		}
		if (saboteurQuestion == currentSettings.NumberOfQuestions)
		{
			yield return StartCoroutine(Jump(finishPlatform.LandingStrip.position, flintAnimator));
			yield return new WaitForSeconds(3f);
			UnityEngine.Object.Destroy(FlintheartBlomgold);
			saboteurLastPlatform = null;
		}
		else
		{
			int nextColumn = UnityEngine.Random.Range(1, 3);
			Platform nextPlatform = (saboteurLastPlatform = instantiatedPlatforms[saboteurQuestion][nextColumn]);
			yield return StartCoroutine(Jump(nextPlatform.transform.position, flintAnimator));
			yield return StartCoroutine(nextPlatform.LandAt(FlintheartBlomgold.transform));
			nextColumn = UnityEngine.Random.Range(nextColumn - 1, nextColumn + 2);
			if (nextColumn < 0)
			{
				nextColumn = 1;
			}
			else if (nextColumn == instantiatedPlatforms[saboteurQuestion].Count)
			{
				nextColumn = instantiatedPlatforms[saboteurQuestion].Count - 2;
			}
		}
		saboteurQuestion++;
	}

	private IEnumerator SaboteurContinuesJumpRoutine()
	{
		yield return StartCoroutine(SaboteurJumpNext());
		if (saboteurQuestion <= currentSettings.NumberOfQuestions)
		{
			StartCoroutine(SaboteurContinuesJumpRoutine());
		}
	}

	private IEnumerator Jump(Vector3 to, Animator animator)
	{
		avatarModel.transform.rotation = Quaternion.identity;
		AudioController.Play("JTC_avatar_jump", animator.transform);
		jumpTakeOffParticles.SpawnAt(animator.transform.position);
		float time = 0f;
		Vector3 from = animator.transform.position;
		float delta = to.x - from.x;
		if (delta > horizontalSpacing * 2f - 2f * horizontalOffsetRange)
		{
			animator.SetTrigger("JumpRightFar");
		}
		else if (delta < 0f - (horizontalSpacing * 2f - 2f * horizontalOffsetRange))
		{
			animator.SetTrigger("JumpLeftFar");
		}
		else if (delta > horizontalOffsetRange * 2f)
		{
			animator.SetTrigger("JumpRight");
		}
		else if (delta < horizontalOffsetRange * 2f && delta > (0f - horizontalOffsetRange) * 2f)
		{
			animator.SetTrigger("JumpStraight");
		}
		else if (delta < horizontalOffsetRange * 2f)
		{
			animator.SetTrigger("JumpLeft");
		}
		while (time < jumpDuration)
		{
			time += Time.deltaTime;
			animator.transform.position = Vector3.Lerp(from, to, moveCurve.Evaluate(time / jumpDuration)) + new Vector3(0f, JumpCurve.Evaluate(time / jumpDuration), 0f);
			yield return null;
		}
	}

	private IEnumerator OnGoodAnswer(Platform targetPlatform)
	{
		lockControls = true;
		bool isLastQuestion = (isFinishingSuccesfully = currentQuestion >= currentSettings.NumberOfQuestions - 1);
		Platform previousPlatform = lastReachedPlatform.GetComponent<Platform>();
		if (previousPlatform != null)
		{
			StartCoroutine(previousPlatform.RestoreOriginalPosition());
		}
		if (!isLastQuestion)
		{
			crocs[currentQuestion].Scare();
		}
		yield return StartCoroutine(Jump(targetPlatform.LandingStrip.position, avatarAnimator));
		hideCurrentRowSigns();
		if (!isLastQuestion)
		{
			currentQuestion++;
			hud.SetQuestion(CurrentEquation.EquationString);
			showCurrentRow();
		}
		yield return StartCoroutine(targetPlatform.LandAt(avatarModel.transform));
		if (isLastQuestion)
		{
			CameraObject.FlyTo(finishPlatform.LandingStrip.position);
			yield return StartCoroutine(Jump(finishPlatform.LandingStrip.position, avatarAnimator));
			yield return new WaitForSeconds(1f);
			Finish(succes: true);
		}
		lockControls = false;
	}

	private IEnumerator OnBadAnswer(Platform badPlatform)
	{
		lockControls = true;
		correctAnswers[currentQuestion] = false;
		yield return StartCoroutine(Jump(badPlatform.transform.position, avatarAnimator));
		yield return StartCoroutine(badPlatform.LandAt(avatarModel.transform));
		AudioController.Play("JTC_avatar_sink", badPlatform.transform);
		if (currentQuestion - 1 >= 0)
		{
			crocs[currentQuestion - 1].Hide();
		}
		crocs[currentQuestion].Hide();
		yield return StartCoroutine(SinkWithPlatform(badPlatform));
		lives--;
		GetView<HeartsView>().LoseHeart();
		Vector3 pos = badPlatform.LandingStrip.position;
		pos.y = -0.5f;
		yield return StartCoroutine(deathCrocSpawner.SpawnKillerCroc(pos));
		if (lives > 0)
		{
			avatarModel.transform.position = lastReachedPlatform.transform.position;
			avatarModel.transform.forward = Vector3.forward;
		}
		else
		{
			Finish(succes: false);
		}
		lockControls = false;
	}

	private IEnumerator SinkWithPlatform(Platform platform)
	{
		sinkCount++;
		avatarAnimator.SetInteger("SinkCount", sinkCount);
		avatarAnimator.SetTrigger("Sink");
		if (sinkCount == 3)
		{
			sinkGameOverParticles.SpawnAt(platform.transform.position);
			yield return new WaitForSeconds(1f);
		}
		else
		{
			sinkWithPlatformParticles.SpawnAt(platform.transform.position);
		}
		float time = 0f;
		Vector3 from = platform.LandingStrip.position;
		Vector3 to = from + Vector3.down * 2f;
		while (time < sinkDuration)
		{
			time += Time.deltaTime;
			Transform obj = avatarModel.transform;
			Vector3 position = Vector3.Lerp(from, to, sinkCurve.Evaluate(time / sinkDuration));
			platform.transform.position = position;
			obj.position = position;
			yield return null;
		}
		avatarAnimator.Play("Idle");
	}

	private void OnTimeOver()
	{
		if (!isFinishingSuccesfully)
		{
			Finish(succes: false);
		}
	}

	private void UpdateClickInput()
	{
		if (lockControls)
		{
			return;
		}
		Platform platform = null;
		if (Input.GetMouseButtonDown(0) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hitInfo, 100f))
		{
			GameObject gameObject = hitInfo.collider.gameObject;
			platform = gameObject.GetComponent<Platform>();
		}
		if (SLAMInput.Provider.GetButtonDown(SLAMInput.Button.Left))
		{
			platform = instantiatedPlatforms[currentQuestion][0];
		}
		if (SLAMInput.Provider.GetButtonDown(SLAMInput.Button.Up) || SLAMInput.Provider.GetButtonDown(SLAMInput.Button.Down))
		{
			platform = instantiatedPlatforms[currentQuestion][1];
		}
		if (SLAMInput.Provider.GetButtonDown(SLAMInput.Button.Right))
		{
			platform = instantiatedPlatforms[currentQuestion][2];
		}
		if (!instantiatedPlatforms[currentQuestion].Contains(platform))
		{
			platform = null;
		}
		if (!(platform != null))
		{
			return;
		}
		bool flag = CurrentEquation.CorrectAnswer == platform.Answer;
		EquationAnsweredEvent equationAnsweredEvent = new EquationAnsweredEvent();
		equationAnsweredEvent.PlatformPressed = platform;
		equationAnsweredEvent.IsCorrectAnswer = flag;
		equationAnsweredEvent.Answer = platform.Answer;
		GameEvents.Invoke(equationAnsweredEvent);
		if (flag || AreCheatsEnabled)
		{
			CameraObject.FlyTo(platform.LandingStrip.position);
			StartCoroutine(OnGoodAnswer(platform));
			lastReachedPlatform = platform;
			equations.RemoveAt(0);
			return;
		}
		for (int i = 0; i < instantiatedPlatforms[currentQuestion].Count; i++)
		{
			if (instantiatedPlatforms[currentQuestion][i] == platform)
			{
				instantiatedPlatforms[currentQuestion][i] = null;
			}
		}
		StartCoroutine(OnBadAnswer(platform));
	}

	private void showCurrentRow()
	{
		int num = UnityEngine.Random.Range(0, platformsInOneRow);
		for (int i = 0; i < platformsInOneRow; i++)
		{
			Platform component = platformInstances[currentQuestion, i].GetComponent<Platform>();
			if (num == i)
			{
				component.SetAnswer(CurrentEquation.CorrectAnswer);
				EquationVisibleEvent equationVisibleEvent = new EquationVisibleEvent();
				equationVisibleEvent.Equation = CurrentEquation;
				equationVisibleEvent.PlatformWithCorrectAnwer = component;
				GameEvents.Invoke(equationVisibleEvent);
			}
			else
			{
				component.SetAnswer(CurrentEquation.WrongAnswer);
			}
		}
	}

	private void hideCurrentRowSigns()
	{
		for (int i = 0; i < platformsInOneRow; i++)
		{
			Platform component = platformInstances[currentQuestion, i].GetComponent<Platform>();
			component.HideSign();
		}
	}
}
