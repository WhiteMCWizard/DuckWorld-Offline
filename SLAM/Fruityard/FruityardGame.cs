using System;
using System.Collections;
using System.Collections.Generic;
using SLAM.Engine;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.Fruityard;

public class FruityardGame : GameController
{
	[Flags]
	public enum FYTreeAction
	{
		None = 0,
		Dig = 1,
		Seed = 2,
		Water = 4,
		Prune = 8,
		Harvest = 0x10,
		Spade = 0x20
	}

	[Flags]
	public enum FYTreeType
	{
		Cherry = 1,
		Pear = 2,
		Plum = 4,
		Apple = 8,
		Nectarine = 0x10
	}

	[Serializable]
	public struct FYTree
	{
		public FYTreeType Type;

		public int Score;

		public FYTreeAction[] Actions;

		[Tooltip("The value is the stage, key + 1 is the grow phase")]
		public int[] GrowMoments;

		public UnityEngine.Object TreePrefab;

		private Animator treeModel;

		public Animator TreeModel => treeModel;

		public void SetTreeModel(Animator treeModel)
		{
			this.treeModel = treeModel;
		}
	}

	public class HelperSelectedEvent
	{
		public FYHelper Helper;
	}

	public class TreeCompletedEvent
	{
		public FYTree Tree;

		public FYSpot Spot;
	}

	public class TreeTaskFailedEvent
	{
		public FYTree Tree;

		public FYSpot Spot;
	}

	public class TreeTaskSucceededEvent
	{
		public FYTree Tree;

		public FYSpot Spot;
	}

	public class LevelCompletedEvent
	{
	}

	public class LevelInitEvent
	{
		public int Level;

		public List<FYTreeType> PickupList;

		public float AllowedTime;

		public float AllowedTimePerTask;
	}

	public class LevelStartedEvent
	{
	}

	public class ShowSeedViewEvent
	{
		public FYSpot Spot;
	}

	public class SeedTreeEvent
	{
		public FYTreeType TreeType;
	}

	[SerializeField]
	[Header("Fruityard properties")]
	private FYTree[] treeDefinitions;

	[SerializeField]
	private FYpickupList[] objectivesPerDifficulty;

	[SerializeField]
	private AudioClip[] workAudio;

	[SerializeField]
	private AudioClip[] treeAudio;

	private List<FYTreeType> currentPickupList;

	private int score;

	private Dictionary<FYTreeType, int> treePlantCount;

	private Alarm levelTimer;

	public override int GameId => 10;

	public override LevelSetting[] Levels => objectivesPerDifficulty;

	public override Dictionary<string, int> ScoreCategories
	{
		get
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			dictionary.Add(StringFormatter.GetLocalizationFormatted("UI_VICTORYWINDOW_COINS_EARNED", getCoinRewardForThisLevel()), getCoinRewardForThisLevel());
			return dictionary;
		}
	}

	public override string IntroNPCKey => "NPC_NAME_GRANDMA_DUCK";

	public override string IntroTextKey => "FY_CINEMATICINTRO_TEXT";

	public override Portrait DuckCharacter => Portrait.GrandmaDuck;

	protected override void Start()
	{
		base.Start();
		levelTimer = Alarm.Create();
		levelTimer.Pause();
		treePlantCount = new Dictionary<FYTreeType, int>();
	}

	private void OnEnable()
	{
		GameEvents.Subscribe<TreeCompletedEvent>(onTreeCompleted);
		GameEvents.Subscribe<TreeTaskSucceededEvent>(onTreeTaskSucceeded);
		GameEvents.Subscribe<LevelInitEvent>(OnLevelInit);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<TreeCompletedEvent>(onTreeCompleted);
		GameEvents.Unsubscribe<TreeTaskSucceededEvent>(onTreeTaskSucceeded);
		GameEvents.Unsubscribe<LevelInitEvent>(OnLevelInit);
	}

	protected override void OnEnterStateRunning()
	{
		OpenView<FYseedView>();
		OpenView<FYSpotView>();
		base.OnEnterStateRunning();
		AudioSource[] componentsInChildren = SingletonMonoBehaviour<AudioController>.Instance.GetComponentsInChildren<AudioSource>();
		AudioSource[] array = componentsInChildren;
		foreach (AudioSource audioSource in array)
		{
			audioSource.maxDistance = 50f;
		}
		LevelSetting levelSetting = SelectedLevel<LevelSetting>();
		currentPickupList = objectivesPerDifficulty[levelSetting.Index].objectives.ToList();
		float allowedTime = objectivesPerDifficulty[levelSetting.Index].allowedTime * 60f;
		float allowedTimePerTask = objectivesPerDifficulty[levelSetting.Index].allowedTimePerTask;
		LevelInitEvent levelInitEvent = new LevelInitEvent();
		levelInitEvent.Level = levelSetting.Index;
		levelInitEvent.AllowedTime = allowedTime;
		levelInitEvent.AllowedTimePerTask = allowedTimePerTask;
		levelInitEvent.PickupList = currentPickupList;
		GameEvents.Invoke(levelInitEvent);
	}

	protected void OnLevelInit(LevelInitEvent evt)
	{
		OpenView<TimerView>().SetTimer(levelTimer);
		levelTimer.Restart();
		levelTimer.StartCountdown(evt.AllowedTime, delegate
		{
			Finish(succes: false);
		});
	}

	protected override void OnEnterStateFinished()
	{
		CloseView<FYseedView>();
		levelTimer.Pause();
		base.OnEnterStateFinished();
	}

	public void PlayTreeAudio(int audio, Vector3 pos)
	{
		AudioController.Play(treeAudio[audio].name, pos);
	}

	private void onTreeTaskSucceeded(TreeTaskSucceededEvent evt)
	{
		int num = 0;
		for (int num2 = (int)evt.Spot.RequiredActions[evt.Spot.RequiredActionIndex - 1]; num2 > 1; num2 /= 2)
		{
			num++;
		}
		AudioController.Play(workAudio[num].name, evt.Spot.transform);
	}

	private void onTreeCompleted(TreeCompletedEvent evt)
	{
		PlayTreeAudio(2, evt.Spot.transform.position);
		FYTreeType type = evt.Tree.Type;
		if (treePlantCount.ContainsKey(type))
		{
			Dictionary<FYTreeType, int> dictionary2;
			Dictionary<FYTreeType, int> dictionary = (dictionary2 = treePlantCount);
			FYTreeType key2;
			FYTreeType key = (key2 = type);
			int num = dictionary2[key2];
			dictionary[key] = num + 1;
		}
		else
		{
			treePlantCount.Add(type, 1);
		}
		score += evt.Tree.Score;
		if (!checkLevelCompleted(type))
		{
			return;
		}
		FYSpot[] array = UnityEngine.Object.FindObjectsOfType<FYSpot>();
		bool flag = false;
		FYSpot[] array2 = array;
		foreach (FYSpot fYSpot in array2)
		{
			if (fYSpot.RequiredActionIndex > 0 && fYSpot.RequiredActionIndex < fYSpot.RequiredActions.Length)
			{
				flag = true;
			}
		}
		if (!flag)
		{
			levelTimer.Pause();
			StartCoroutine(FinishAfterDelay(1f));
		}
	}

	private IEnumerator FinishAfterDelay(float delay)
	{
		yield return new WaitForSeconds(delay);
		Finish(succes: true);
	}

	private bool checkLevelCompleted(FYTreeType completedTree)
	{
		if (!currentPickupList.Remove(completedTree))
		{
			score += GetTree(completedTree).Score;
		}
		return currentPickupList.Count == 0;
	}

	public FYTree GetTree(FYTreeType treeType)
	{
		return treeDefinitions.First((FYTree td) => td.Type == treeType);
	}
}
