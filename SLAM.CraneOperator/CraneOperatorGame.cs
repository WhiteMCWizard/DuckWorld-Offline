using System.Collections;
using System.Collections.Generic;
using SLAM.Engine;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.CraneOperator;

public class CraneOperatorGame : GameController
{
	public class TruckCompletedEvent
	{
	}

	public class GameStartedEvent
	{
		public CODifficultySettings Settings;
	}

	private const int ROWS = 6;

	private const int COLUMNS = 8;

	private const int POINTS_PER_TRUCK = 100;

	private const int POINTS_PER_SECOND_LEFT = 10;

	[Header("Crane Operator properties")]
	[SerializeField]
	private CODifficultySettings[] levels;

	[SerializeField]
	private CrateGenerator generator;

	[SerializeField]
	private Crate[] crates;

	[SerializeField]
	private Crane crane;

	[SerializeField]
	private CraneOperatorCrateView leftTruckView;

	[SerializeField]
	private CraneOperatorCrateView rightTruckView;

	[SerializeField]
	private Transform[] stackTransforms;

	[SerializeField]
	private GameObject truckPrefab;

	[SerializeField]
	private GameObject lastTruckPrefab;

	[SerializeField]
	private Transform[] truckSpawnPositions;

	[SerializeField]
	private float rightTruckSpawnWaitTime = 0.5f;

	private List<Crate[]> pickupLists;

	private int pickupListIndex;

	private Crate[] leftTruckPickupList;

	private Crate[] rightTruckPickupList;

	private TruckDropZone leftTruck;

	private TruckDropZone rightTruck;

	private Alarm levelTimer;

	public override int GameId => 5;

	public override string IntroNPCKey => "NPC_NAME_SCROOGE";

	public override string IntroTextKey => "CO_CINEMATICINTRO_TEXT";

	public override Dictionary<string, int> ScoreCategories
	{
		get
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			dictionary.Add(StringFormatter.GetLocalizationFormatted("CO_VICTORYWINDOW_SCORE_TRUCKS_COMPLETED", TruckCount, 100), TruckCount * 100);
			dictionary.Add(StringFormatter.GetLocalizationFormatted("CO_VICTORYWINDOW_SCORE_TIME_LEFT", (int)levelTimer.TimeLeft, 10), (int)levelTimer.TimeLeft * 10);
			return dictionary;
		}
	}

	public int TruckCount => pickupLists.Count;

	public override LevelSetting[] Levels => levels;

	private CODifficultySettings currentDifficultySetting => SelectedLevel<CODifficultySettings>();

	public override Portrait DuckCharacter => Portrait.ScroogeDuck;

	protected override void Start()
	{
		base.Start();
		levelTimer = Alarm.Create();
		levelTimer.Pause();
		crane.enabled = false;
	}

	public void CratesInTruckChanged(TruckDropZone truck, List<Crate> crates)
	{
		List<Crate> list = new List<Crate>(crates);
		CraneOperatorCrateView craneOperatorCrateView;
		List<Crate> currentPickupList;
		if (truck == leftTruck)
		{
			craneOperatorCrateView = leftTruckView;
			currentPickupList = new List<Crate>(leftTruckPickupList);
		}
		else
		{
			craneOperatorCrateView = rightTruckView;
			currentPickupList = new List<Crate>(rightTruckPickupList);
		}
		KeyValuePair<Crate, bool>[] array = new KeyValuePair<Crate, bool>[currentPickupList.Count];
		int num = 0;
		for (int i = 0; i < currentPickupList.Count; i++)
		{
			bool value = false;
			Crate crate = list.FirstOrDefault((Crate a) => a.Type == currentPickupList[i].Type);
			if (crate != null)
			{
				value = true;
				list.Remove(crate);
				num++;
			}
			ref KeyValuePair<Crate, bool> reference = ref array[i];
			reference = new KeyValuePair<Crate, bool>(currentPickupList[i], value);
		}
		if (num >= currentPickupList.Count)
		{
			truckCompleted(truck, craneOperatorCrateView);
		}
		else
		{
			craneOperatorCrateView.UpdateUI(array);
		}
	}

	public bool CurrentPickupListsContainCrate(Crate crate)
	{
		if (crate == null)
		{
			return false;
		}
		return (leftTruckPickupList != null && leftTruckPickupList.Count((Crate c) => c.Type == crate.Type) > 0) || (rightTruckPickupList != null && rightTruckPickupList.Count((Crate c) => c.Type == crate.Type) > 0);
	}

	private void truckCompleted(TruckDropZone truck, CraneOperatorCrateView hud)
	{
		GameEvents.Invoke(new TruckCompletedEvent());
		AudioController.Play("CO_truck_full_hornBlast");
		pickupListIndex++;
		truck.Animator.SetTrigger("shouldDriveAway");
		Crate[] componentsInChildren = truck.GetComponentsInChildren<Crate>();
		foreach (Crate crate in componentsInChildren)
		{
			crate.Collider.enabled = false;
		}
		GetView<CraneOperatorHUDView>().UpdateTrucksFinished(pickupListIndex - 1, truck.transform.position);
		if (pickupListIndex > pickupLists.Count)
		{
			levelTimer.Pause();
			CoroutineUtils.WaitForObjectDestroyed(truck.gameObject, delegate
			{
				Finish(succes: true);
			});
		}
		hud.gameObject.SetActive(value: false);
		if (pickupListIndex < pickupLists.Count)
		{
			StartCoroutine(spawnTruck(1f, truck == leftTruck));
		}
	}

	private void spawnCrates()
	{
		List<COManifest> truckManifests = currentDifficultySetting.TruckManifests;
		pickupLists = new List<Crate[]>();
		Transform[] array = stackTransforms;
		foreach (Transform transform in array)
		{
			foreach (Transform item in transform.transform)
			{
				Object.Destroy(item.gameObject);
			}
		}
		for (int j = 0; j < currentDifficultySetting.AmountOfTrucks; j++)
		{
			COManifest manifest = truckManifests.GetRandom();
			if (manifest == null)
			{
				Debug.Log("Hey Buddy, you have more trucks than manifests!", this);
				break;
			}
			truckManifests.Remove(manifest);
			Crate[] array2 = new Crate[manifest.CrateTypes.Length];
			for (int k = 0; k < manifest.CrateTypes.Length; k++)
			{
				array2[k] = crates.FirstOrDefault((Crate c) => c.Type == manifest.CrateTypes[k]);
			}
			pickupLists.Add(array2);
		}
		Crate[] array3 = new Crate[currentDifficultySetting.ExtraCrates.Length];
		int j2;
		for (j2 = 0; j2 < currentDifficultySetting.ExtraCrates.Length; j2++)
		{
			array3[j2] = crates.FirstOrDefault((Crate c) => c.Type == currentDifficultySetting.ExtraCrates[j2]);
		}
		generator.SpawnCrates(stackTransforms, pickupLists, array3, 6, 8);
	}

	private TruckDropZone spawnTruck(bool spawnTruckLeft = false)
	{
		Transform transform = ((!spawnTruckLeft) ? truckSpawnPositions[1] : truckSpawnPositions[0]);
		GameObject gameObject = Object.Instantiate((pickupListIndex != pickupLists.Count - 1) ? truckPrefab : lastTruckPrefab, transform.position, transform.rotation) as GameObject;
		TruckDropZone componentInChildren = gameObject.GetComponentInChildren<TruckDropZone>();
		componentInChildren.Animator.SetBool("driveLeft", spawnTruckLeft);
		componentInChildren.SetOperator(this);
		if (spawnTruckLeft)
		{
			leftTruckPickupList = pickupLists[pickupListIndex];
			leftTruckView.InitialisePickupList(leftTruckPickupList);
			leftTruck = componentInChildren;
		}
		else
		{
			rightTruckPickupList = pickupLists[pickupListIndex];
			rightTruckView.InitialisePickupList(rightTruckPickupList);
			rightTruck = componentInChildren;
		}
		return componentInChildren;
	}

	private TruckDropZone spawnTruck()
	{
		return spawnTruck(spawnTruckLeft: false);
	}

	private IEnumerator spawnTruck(float waitTime, bool leftTruck)
	{
		yield return new WaitForSeconds(waitTime);
		spawnTruck(leftTruck);
	}

	public override void Play(LevelSetting selectedLevel)
	{
		base.Play(selectedLevel);
		GameStartedEvent gameStartedEvent = new GameStartedEvent();
		gameStartedEvent.Settings = currentDifficultySetting;
		GameEvents.Invoke(gameStartedEvent);
		if (currentDifficultySetting.LevelDuration > 0f)
		{
			OpenView<TimerView>().SetTimer(levelTimer);
		}
	}

	public override void Pause()
	{
		base.Pause();
		crane.AreControlsLocked = true;
	}

	public override void Resume()
	{
		base.Resume();
		crane.AreControlsLocked = false;
	}

	public override void Finish(bool succes)
	{
		crane.AreControlsLocked = true;
		levelTimer.Pause();
		GetView<TimerView>().SetTimer(null);
		base.Finish(succes);
	}

	protected override void OnEnterStateRunning()
	{
		base.OnEnterStateRunning();
		spawnCrates();
		crane.enabled = true;
		crane.AreControlsLocked = false;
		levelTimer.Restart();
		if (currentDifficultySetting.LevelDuration > 0f)
		{
			levelTimer.StartCountdown(currentDifficultySetting.LevelDuration, delegate
			{
				Finish(succes: false);
			});
		}
		spawnTruck(spawnTruckLeft: true);
		pickupListIndex++;
		if (currentDifficultySetting.AmountOfTrucks > 1)
		{
			Invoke("spawnTruck", rightTruckSpawnWaitTime);
		}
		CraneOperatorHUDView view = GetView<CraneOperatorHUDView>();
		view.CreateTruckChecks(pickupLists.Count);
	}

	public static RayOrigin[] CalculateRaysFromBox(BoxCollider box, float skinWidth)
	{
		List<RayOrigin> list = new List<RayOrigin>();
		skinWidth += 0.01f;
		float x = (0f - box.bounds.size.x) / 2f + skinWidth;
		float x2 = box.bounds.size.x / 2f - skinWidth;
		float y = box.bounds.size.y / 2f - skinWidth;
		float y2 = (0f - box.bounds.size.y) / 2f + skinWidth;
		float num = 0f;
		float z = box.bounds.size.z / 2f - skinWidth;
		float z2 = (0f - box.bounds.size.z) / 2f + skinWidth;
		float z3 = box.transform.localPosition.z;
		Vector3 origin = new Vector3(x, y, z3) + box.bounds.center;
		Vector3 origin2 = new Vector3(x2, y, z3) + box.bounds.center;
		Vector3 origin3 = new Vector3(x, y2, z3) + box.bounds.center;
		Vector3 origin4 = new Vector3(x2, y2, z3) + box.bounds.center;
		Vector3 origin5 = new Vector3(num, y, z3) + box.bounds.center;
		Vector3 origin6 = new Vector3(num, y2, z3) + box.bounds.center;
		Vector3 origin7 = new Vector3(x, num, z3) + box.bounds.center;
		Vector3 origin8 = new Vector3(x2, num, z3) + box.bounds.center;
		Vector3 origin9 = new Vector3(num, num, z) + box.bounds.center;
		Vector3 origin10 = new Vector3(x2, num, z2) + box.bounds.center;
		list.Add(new RayOrigin(new Ray(origin, Vector3.up), Direction.Up));
		list.Add(new RayOrigin(new Ray(origin2, Vector3.up), Direction.Up));
		list.Add(new RayOrigin(new Ray(origin5, Vector3.up), Direction.Up));
		list.Add(new RayOrigin(new Ray(origin3, Vector3.down), Direction.Down));
		list.Add(new RayOrigin(new Ray(origin4, Vector3.down), Direction.Down));
		list.Add(new RayOrigin(new Ray(origin6, Vector3.down), Direction.Down));
		list.Add(new RayOrigin(new Ray(origin, Vector3.left), Direction.Left));
		list.Add(new RayOrigin(new Ray(origin3, Vector3.left), Direction.Left));
		list.Add(new RayOrigin(new Ray(origin7, Vector3.left), Direction.Left));
		list.Add(new RayOrigin(new Ray(origin2, Vector3.right), Direction.Right));
		list.Add(new RayOrigin(new Ray(origin4, Vector3.right), Direction.Right));
		list.Add(new RayOrigin(new Ray(origin8, Vector3.right), Direction.Right));
		list.Add(new RayOrigin(new Ray(origin9, Vector3.forward), Direction.Forward));
		list.Add(new RayOrigin(new Ray(origin10, Vector3.back), Direction.Back));
		return list.ToArray();
	}
}
