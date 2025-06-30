using System;
using System.Collections;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.Fruityard;

public class FYSpot : MonoBehaviour
{
	[Flags]
	private enum LevelSet
	{
		Level1 = 1,
		Level2 = 2,
		Level3 = 4,
		Level4 = 8,
		Level5 = 0x10,
		Level6 = 0x20,
		Level7 = 0x40,
		Level8 = 0x80,
		Level9 = 0x100,
		Level10 = 0x200,
		Level11 = 0x400,
		Level12 = 0x800,
		Level13 = 0x1000,
		Level14 = 0x2000,
		Level15 = 0x4000
	}

	[SerializeField]
	private FruityardGame fruityardGame;

	[SerializeField]
	[BitMask(typeof(LevelSet))]
	private LevelSet activeInLevels;

	private FruityardGame.FYTree tree;

	private bool hasTree;

	private Material groundMaterial;

	private float treeTaskTime;

	public FruityardGame.FYTreeAction[] RequiredActions { get; protected set; }

	public int RequiredActionIndex { get; protected set; }

	public FruityardGame.FYTreeAction CurrentRequiredAction => RequiredActions[RequiredActionIndex];

	public Alarm CurrentActionTimer { get; protected set; }

	private void Start()
	{
		RequiredActions = new FruityardGame.FYTreeAction[2]
		{
			FruityardGame.FYTreeAction.Dig,
			FruityardGame.FYTreeAction.Seed
		};
		RequiredActionIndex = 0;
		CurrentActionTimer = base.gameObject.GetComponent<Alarm>() ?? base.gameObject.AddComponent<Alarm>();
		CurrentActionTimer.StartCountdown(treeTaskTime, null, destroyWhenFinished: false);
		CurrentActionTimer.Pause();
		groundMaterial = base.gameObject.GetComponentInChildren<Renderer>().material;
		groundMaterial.SetFloat("_Blend", 1f);
	}

	private void OnEnable()
	{
		GameEvents.Subscribe<FruityardGame.LevelCompletedEvent>(onLevelCompleted);
		GameEvents.Subscribe<FruityardGame.LevelInitEvent>(onLevelInit);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<FruityardGame.LevelCompletedEvent>(onLevelCompleted);
		GameEvents.Unsubscribe<FruityardGame.LevelInitEvent>(onLevelInit);
	}

	private void onLevelInit(FruityardGame.LevelInitEvent evt)
	{
		treeTaskTime = evt.AllowedTimePerTask;
		base.gameObject.SetActive(((uint)activeInLevels & (uint)(1 << evt.Level)) == (uint)(1 << evt.Level));
	}

	private void onLevelCompleted(FruityardGame.LevelCompletedEvent obj)
	{
		RequiredActions = new FruityardGame.FYTreeAction[2]
		{
			FruityardGame.FYTreeAction.Dig,
			FruityardGame.FYTreeAction.Seed
		};
		RequiredActionIndex = 0;
		if (hasTree)
		{
			UnityEngine.Object.Destroy(tree.TreeModel.gameObject);
			hasTree = false;
		}
		((CapsuleCollider)GetComponent<Collider>()).radius = 0.5f;
		((CapsuleCollider)GetComponent<Collider>()).height = 3.26f;
		GetComponent<Collider>().enabled = true;
	}

	private void Update()
	{
		if (CurrentActionTimer.TimerDuration != 0f && CurrentActionTimer.Expired)
		{
			failTask();
		}
	}

	public IEnumerator PerformAction(FYHelper helper, FruityardGame.FYTreeAction action)
	{
		bool finished = false;
		bool correctAction = action == CurrentRequiredAction;
		if (correctAction)
		{
			if (action == FruityardGame.FYTreeAction.Seed)
			{
				CurrentActionTimer.Pause();
				yield return StartCoroutine(PerformSeed());
			}
			else if (hasTree)
			{
				if (tree.GrowMoments.Contains(RequiredActionIndex + 1))
				{
					GrowTree(Array.IndexOf(tree.GrowMoments, RequiredActionIndex + 1) + 1);
				}
			}
			else if (action == FruityardGame.FYTreeAction.Dig)
			{
				StartCoroutine(PlowEarth());
			}
			fruityardGame.PlayTreeAudio(5, base.transform.position);
			helper.PlayAnimation(action);
			RequiredActionIndex++;
			if (hasTree && RequiredActionIndex >= RequiredActions.Length)
			{
				finished = true;
			}
			FruityardGame.TreeTaskSucceededEvent treeTaskSucceededEvent = new FruityardGame.TreeTaskSucceededEvent();
			treeTaskSucceededEvent.Tree = tree;
			treeTaskSucceededEvent.Spot = this;
			GameEvents.Invoke(treeTaskSucceededEvent);
		}
		else
		{
			failTask();
		}
		CurrentActionTimer.StartCountdown(treeTaskTime, null, destroyWhenFinished: false);
		CurrentActionTimer.Pause();
		if (correctAction)
		{
			yield return new WaitForSeconds(3f);
		}
		fruityardGame.PlayTreeAudio(6, base.transform.position);
		if (RequiredActionIndex != 0)
		{
			CurrentActionTimer.Restart();
		}
		if (finished)
		{
			tree.TreeModel.transform.FindChild("FY_Fruit").gameObject.SetActive(value: false);
			GetComponent<Collider>().enabled = false;
			FruityardGame.TreeCompletedEvent treeCompletedEvent = new FruityardGame.TreeCompletedEvent();
			treeCompletedEvent.Tree = tree;
			treeCompletedEvent.Spot = this;
			GameEvents.Invoke(treeCompletedEvent);
			CurrentActionTimer.Pause();
		}
	}

	private IEnumerator PlowEarth()
	{
		StopCoroutine("UnplowEarth");
		float timer = 0f;
		while (timer < 1f)
		{
			timer += Time.deltaTime / 3f;
			if (timer >= 0.85f)
			{
				groundMaterial.SetFloat("_Blend", 0f);
			}
			else if (timer >= 0.5f)
			{
				groundMaterial.SetFloat("_Blend", 0.5f);
			}
			yield return null;
		}
	}

	private IEnumerator UnplowEarth()
	{
		float timer = 0f;
		while (timer < 1f)
		{
			timer += Time.deltaTime;
			groundMaterial.SetFloat("_Blend", timer);
			yield return null;
		}
	}

	public IEnumerator PerformSeed()
	{
		FruityardGame.ShowSeedViewEvent showSeedViewEvent = new FruityardGame.ShowSeedViewEvent();
		showSeedViewEvent.Spot = this;
		GameEvents.Invoke(showSeedViewEvent);
		while (!hasTree)
		{
			yield return null;
		}
	}

	public void TreeSelected(FruityardGame.FYTreeType selectedTree)
	{
		tree = fruityardGame.GetTree(selectedTree);
		Animator component = ((GameObject)UnityEngine.Object.Instantiate(tree.TreePrefab, base.transform.position, base.transform.GetChild(0).rotation)).GetComponent<Animator>();
		tree.SetTreeModel(component);
		component.Rebind();
		RequiredActions = tree.Actions;
		GrowTree(0);
		hasTree = true;
	}

	private void failTask()
	{
		FruityardGame.TreeTaskFailedEvent treeTaskFailedEvent = new FruityardGame.TreeTaskFailedEvent();
		treeTaskFailedEvent.Tree = tree;
		treeTaskFailedEvent.Spot = this;
		GameEvents.Invoke(treeTaskFailedEvent);
		fruityardGame.PlayTreeAudio(4, base.transform.position);
		if (RequiredActionIndex > 0)
		{
			RequiredActionIndex--;
			if (RequiredActionIndex == 0)
			{
				StartCoroutine(UnplowEarth());
			}
			if (CurrentRequiredAction == FruityardGame.FYTreeAction.Seed)
			{
				UnityEngine.Object.Destroy(tree.TreeModel.gameObject);
				hasTree = false;
			}
			else if (hasTree && tree.GrowMoments.Contains(RequiredActionIndex + 1))
			{
				GrowTree(Array.IndexOf(tree.GrowMoments, RequiredActionIndex + 1) + 1, backwards: true);
			}
			CurrentActionTimer.StartCountdown(treeTaskTime, null, destroyWhenFinished: false);
			if (RequiredActionIndex == 0)
			{
				CurrentActionTimer.Pause();
			}
		}
	}

	private void GrowTree(int growPhase, bool backwards = false)
	{
		StartCoroutine(DoGrowTree(growPhase, backwards));
	}

	private IEnumerator DoGrowTree(int growPhase, bool backwards = false)
	{
		if (!backwards)
		{
			yield return new WaitForSeconds(2f);
		}
		fruityardGame.PlayTreeAudio((!backwards) ? 1 : 0, base.transform.position);
		foreach (Transform t in tree.TreeModel.transform)
		{
			if (t.name.Contains("FY_TrunkState"))
			{
				t.gameObject.SetActive(t.name.EndsWith(growPhase.ToString()));
			}
		}
		string trigger = "Phase" + growPhase;
		if (backwards)
		{
			trigger += "R";
		}
		tree.TreeModel.SetTrigger(trigger);
		if (growPhase >= 4)
		{
			tree.TreeModel.SendMessage("ToggleBlossom");
		}
		if (growPhase == 5)
		{
			Transform fruit = tree.TreeModel.transform.FindChild("FY_Fruit");
			fruit.gameObject.SetActive(!backwards);
			if (!backwards)
			{
				fruityardGame.PlayTreeAudio(3, base.transform.position);
			}
		}
		float growFactor = ((!backwards) ? 1.1f : 0.9090909f);
		((CapsuleCollider)GetComponent<Collider>()).radius *= growFactor;
		((CapsuleCollider)GetComponent<Collider>()).height *= growFactor;
	}

	public void PauseTimer()
	{
		CurrentActionTimer.Pause();
	}

	public int GetGrowPhase()
	{
		if (hasTree)
		{
			for (int num = RequiredActionIndex; num > 0; num--)
			{
				if (tree.GrowMoments.Contains(num + 1))
				{
					return Array.IndexOf(tree.GrowMoments, num + 1);
				}
			}
		}
		return 0;
	}
}
