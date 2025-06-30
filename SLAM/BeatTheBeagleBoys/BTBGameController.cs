using System.Collections.Generic;
using System.Reflection;
using SLAM.Engine;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.BeatTheBeagleBoys;

public class BTBGameController : GameController
{
	public class ThiefExitedEvent
	{
		public BTBThief Thief;

		public BTBArea Area;
	}

	public class AnimalStolenEvent
	{
		public BTBThief Thief;

		public BTBArea Area;
	}

	public class CageClosedEvent
	{
	}

	public class MonitorAlarmedEvent
	{
		public BTBMonitor monitor;
	}

	private const int POINTS_HEARTS_LEFT = 100;

	private const int POINTS_CAGES_CLOSED = 25;

	[SerializeField]
	[HideInInspector]
	private BTBArea[] areas;

	[SerializeField]
	private BTBMonitor[] monitors;

	[SerializeField]
	private GameObject[] thiefPrefabs;

	[SerializeField]
	private BTBGuard guard;

	[SerializeField]
	private BTBDifficultySetting[] levels;

	[SerializeField]
	private Alarm gameTimer;

	private int heartCount = 3;

	private int cagesClosed;

	private List<BTBThief> activeThieves = new List<BTBThief>();

	private List<BTBGuard> activeGuards = new List<BTBGuard>();

	private float nextMonitorSwitchTime;

	[SerializeField]
	private int gizmoDifficulty;

	[SerializeField]
	private float gizmoWidth = 10f;

	public override int GameId => 16;

	public override Dictionary<string, int> ScoreCategories
	{
		get
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			dictionary.Add(StringFormatter.GetLocalizationFormatted("BTB_VICTORYWINDOW_SCORE_HEARTS_LEFT", heartCount, 100), heartCount * 100);
			dictionary.Add(StringFormatter.GetLocalizationFormatted("BTB_VICTORYWINDOW_CAGES_CLOSED", cagesClosed, 25), cagesClosed * 25);
			return dictionary;
		}
	}

	public override string IntroNPCKey => "NPC_NAME_DONALD";

	public override string IntroTextKey => "BTB_CINEMATICINTRO_TEXT";

	public BTBDifficultySetting CurrentDifficultySetting => SelectedLevel<BTBDifficultySetting>();

	public override LevelSetting[] Levels => levels;

	public override Portrait DuckCharacter => Portrait.DonaldDuck;

	protected override void Start()
	{
		base.Start();
		disableControls();
	}

	public override void Play(LevelSetting selectedLevel)
	{
		base.Play(selectedLevel);
		enableControls();
		for (int i = 0; i < monitors.Length; i++)
		{
			BTBArea area = areas.FirstOrDefault((BTBArea a) => !a.IsMonitored);
			monitors[i].DisplayArea(area);
		}
		gameTimer = Alarm.Create();
		gameTimer.StartCountUp(CurrentDifficultySetting.LevelDuration, onGameTimeUp);
		CurrentDifficultySetting.SetGameTimer(gameTimer);
		GetView<BTBHudView>().SetInfo(gameTimer);
		OpenView<HeartsView>().SetTotalHeartCount(heartCount);
	}

	private void OnEnable()
	{
		GameEvents.Subscribe<AnimalStolenEvent>(onAnimalStolen);
		GameEvents.Subscribe<CageClosedEvent>(onCageClosed);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<AnimalStolenEvent>(onAnimalStolen);
		GameEvents.Unsubscribe<CageClosedEvent>(onCageClosed);
	}

	private void OnDrawGizmos()
	{
		BTBDifficultySetting obj = levels[gizmoDifficulty];
		AnimationCurve[] array = new AnimationCurve[2]
		{
			GetPrivateField<AnimationCurve>(obj, "thiefActiveCount"),
			GetPrivateField<AnimationCurve>(obj, "thiefIdleTime")
		};
		Color[] array2 = new Color[5]
		{
			Color.red,
			Color.green,
			Color.blue,
			Color.magenta,
			Color.yellow
		};
		for (int i = 0; i < array.Length; i++)
		{
			AnimationCurve animationCurve = array[i];
			for (int j = 1; j < animationCurve.keys.Length; j++)
			{
				Gizmos.color = array2[i];
				Gizmos.DrawLine(base.transform.position + Vector3.right * animationCurve.keys[j - 1].time * gizmoWidth + Vector3.up * animationCurve.keys[j - 1].value, base.transform.position + Vector3.right * animationCurve.keys[j].time * gizmoWidth + Vector3.up * animationCurve.keys[j].value);
			}
		}
	}

	private static T GetPrivateField<T>(object obj, string fieldname)
	{
		return (T)obj.GetType().GetField(fieldname, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(obj);
	}

	public override void Pause()
	{
		base.Pause();
		disableControls();
	}

	public override void Resume()
	{
		base.Resume();
		enableControls();
	}

	private void onGameTimeUp()
	{
		Finish(heartCount > 0);
	}

	protected override void WhileStateRunning()
	{
		base.WhileStateRunning();
		updateThiefs();
		updateMonitors();
		handleInput();
	}

	public override void Finish(bool succes)
	{
		BTBArea[] array = areas;
		foreach (BTBArea bTBArea in array)
		{
			if (bTBArea.Cage.IsOpened)
			{
				bTBArea.Cage.OnReset();
			}
		}
		for (int j = 0; j < activeThieves.Count; j++)
		{
			activeThieves[j].stopAI();
		}
		disableControls();
		gameTimer.Pause();
		base.Finish(succes);
	}

	private void onAnimalStolen(AnimalStolenEvent evt)
	{
		if (heartCount > 0)
		{
			GetView<HeartsView>().LoseHeart();
		}
		heartCount--;
		if (heartCount <= 0)
		{
			CoroutineUtils.WaitForObjectDestroyed(evt.Thief.gameObject, delegate
			{
				Finish(succes: false);
			});
		}
	}

	private void onCageClosed(CageClosedEvent obj)
	{
		cagesClosed++;
	}

	private void updateMonitors()
	{
		IEnumerable<BTBArea> enumerable = areas.Where((BTBArea ar) => !ar.IsMonitored && ar.HasThief);
		Enumerable.IOrderedEnumerable<BTBMonitor> collection = from m in monitors
			where !m.Area.HasThief
			orderby m.UsedCount
			select m;
		if (enumerable != null && monitors == null)
		{
			Debug.LogWarning("Hey budddy, there are not enought monitors to display all thiefs!");
		}
		foreach (BTBArea item in enumerable)
		{
			collection.Where((BTBMonitor m) => m.Area != null).GetRandom().DisplayArea(item);
		}
		if (Time.time > nextMonitorSwitchTime)
		{
			nextMonitorSwitchTime = Time.time + CurrentDifficultySetting.GetMonitorVisibleTime();
			switchMonitor();
		}
	}

	private void handleInput()
	{
		if (!Input.GetMouseButtonDown(0))
		{
			return;
		}
		RaycastHit hitInfo = default(RaycastHit);
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out hitInfo) && hitInfo.collider.HasComponent<BTBMonitor>())
		{
			BTBMonitor component = hitInfo.collider.GetComponent<BTBMonitor>();
			if (component.Area != null && !component.AreControlsLocked && component.Area.OnInteract())
			{
				AudioController.Play("BTB_button_press");
				AudioController.Play("cage_close", component.transform);
			}
		}
	}

	private void disableControls()
	{
		BTBMonitor[] array = monitors;
		foreach (BTBMonitor bTBMonitor in array)
		{
			bTBMonitor.AreControlsLocked = true;
		}
	}

	private void enableControls()
	{
		BTBMonitor[] array = monitors;
		foreach (BTBMonitor bTBMonitor in array)
		{
			bTBMonitor.AreControlsLocked = false;
		}
	}

	private void switchMonitor()
	{
		BTBMonitor monitor = monitors.Where((BTBMonitor m) => !m.Area.HasThief).GetRandom();
		if (monitor != null)
		{
			BTBArea random = areas.Where((BTBArea ar) => !ar.IsMonitored && !ar.HasThief && ar != monitor.Area).GetRandom();
			monitor.DisplayArea(random);
		}
	}

	private void updateThiefs()
	{
		for (int i = 0; i < activeThieves.Count; i++)
		{
			if (!activeThieves[i].enabled)
			{
				Object.Destroy(activeThieves[i].gameObject);
				activeThieves.RemoveAt(i);
				i--;
			}
		}
		if (activeThieves.Count >= CurrentDifficultySetting.GetThiefActiveCount())
		{
			return;
		}
		BTBArea bTBArea = areas.Where((BTBArea ar) => !ar.Cage.IsHacked && ar.Cage.IsOpened && !ar.HasGuard && !ar.HasThief).GetRandom();
		if (bTBArea == null)
		{
			bTBArea = areas.OrderBy((BTBArea ar) => ar.ThiefsEncountered).FirstOrDefault((BTBArea ar) => !ar.HasGuard && !ar.HasThief && !ar.IsMonitored);
		}
		if (bTBArea == null)
		{
			Debug.LogError("Couldn't spawn a thief because all areas are full!");
			return;
		}
		BTBThief componentInChildren = Object.Instantiate(thiefPrefabs.GetRandom()).GetComponentInChildren<BTBThief>();
		componentInChildren.name += Time.time;
		bTBArea.OnThiefEntered(componentInChildren);
		componentInChildren.Initialize(bTBArea, CurrentDifficultySetting);
		addObjectToLayer(componentInChildren.gameObject, "Avatar", recursivly: true);
		activeThieves.Add(componentInChildren);
	}

	private void addObjectToLayer(GameObject go, string layerName, bool recursivly)
	{
		go.layer = LayerMask.NameToLayer(layerName);
		if (!recursivly)
		{
			return;
		}
		foreach (Transform item in go.transform)
		{
			item.gameObject.layer = LayerMask.NameToLayer(layerName);
		}
	}

	private void updateGuard()
	{
		for (int i = 0; i < activeGuards.Count; i++)
		{
			if (!activeGuards[i].enabled)
			{
				Object.Destroy(activeGuards[i].gameObject);
				activeGuards.RemoveAt(i);
				i--;
			}
		}
		if (activeGuards.Count == 0)
		{
			BTBArea random = areas.Where((BTBArea ar) => !ar.HasGuard && !ar.HasThief).GetRandom();
			if (random == null)
			{
				Debug.LogError("Couldnt spawn a guard because all areas are full!");
				return;
			}
			BTBGuard componentInChildren = Object.Instantiate(guard.gameObject).GetComponentInChildren<BTBGuard>();
			componentInChildren.name += Time.time;
			random.OnGuardEntered(componentInChildren);
			componentInChildren.Initialize(random, CurrentDifficultySetting);
			addObjectToLayer(componentInChildren.gameObject, "Avatar", recursivly: true);
			activeGuards.Add(componentInChildren);
		}
	}
}
