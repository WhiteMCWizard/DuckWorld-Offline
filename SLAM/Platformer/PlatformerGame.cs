using System.Collections;
using SLAM.Engine;
using UnityEngine;

namespace SLAM.Platformer;

public abstract class PlatformerGame : GameController
{
	protected const int MAX_HEARTS = 3;

	[SerializeField]
	[Header("Platformer Properties")]
	protected CC2DPlayer player;

	[SerializeField]
	private SidescrollerBehaviour sidescrollCam;

	protected int hearts = 3;

	protected P_Checkpoint lastCheckpoint;

	protected bool isDoingSinkRoutine;

	private bool invulnerable;

	protected override void Start()
	{
		base.Start();
		player.AreControlsLocked = true;
	}

	protected virtual void OnEnable()
	{
		GameEvents.Subscribe<CheckPointReachedEvent>(onCheckpointReachedEvent);
		GameEvents.Subscribe<DoorEnterEvent>(onDoorEnterEvent);
		GameEvents.Subscribe<PlayerHitEvent>(onPlayerHit);
		GameEvents.Subscribe<PickupCollectedEvent>(onPickupCollectedEvent);
		GameEvents.Subscribe<FinishReachedEvent>(onFinishReachedEvent);
		GameEvents.Subscribe<PlayerFallInWaterEvent>(onFallInWaterEvent);
	}

	protected virtual void OnDisable()
	{
		GameEvents.Unsubscribe<CheckPointReachedEvent>(onCheckpointReachedEvent);
		GameEvents.Unsubscribe<DoorEnterEvent>(onDoorEnterEvent);
		GameEvents.Unsubscribe<PlayerHitEvent>(onPlayerHit);
		GameEvents.Unsubscribe<PickupCollectedEvent>(onPickupCollectedEvent);
		GameEvents.Unsubscribe<FinishReachedEvent>(onFinishReachedEvent);
		GameEvents.Unsubscribe<PlayerFallInWaterEvent>(onFallInWaterEvent);
	}

	public override void Play(LevelSetting selectedLevel)
	{
		base.Play(selectedLevel);
		player.AreControlsLocked = false;
		OpenView<HeartsView>().SetTotalHeartCount(hearts);
	}

	public void FadeToBlack(float seconds, View.Callback callback)
	{
		FadeView fadeView = OpenView<FadeView>(callback);
		fadeView.GetComponentInChildren<TweenAlpha>().duration = seconds;
	}

	public bool CollectHeart(Heart heart)
	{
		if (hearts < 3)
		{
			hearts++;
			hearts = Mathf.Clamp(hearts, 0, 3);
			GetView<HeartsView>().FoundHeart(heart.transform.position);
			return true;
		}
		return false;
	}

	public override void Pause()
	{
		base.Pause();
		player.Pause();
	}

	public override void Resume()
	{
		base.Resume();
		player.Resume();
	}

	public override void Finish(bool succes)
	{
		base.Finish(succes);
		player.Pause();
	}

	public void RespawnAtLastCheckpoint()
	{
		player.ResetPlayer(lastCheckpoint.transform.position);
	}

	protected void registerCheckpoint(P_Checkpoint cp)
	{
		lastCheckpoint = cp;
	}

	protected IEnumerator doFinishAtEndOfFrame(bool succes)
	{
		yield return new WaitForEndOfFrame();
		Finish(succes);
	}

	private IEnumerator doSinkRoutine()
	{
		isDoingSinkRoutine = true;
		player.Drown();
		yield return new WaitForSeconds(0.5f);
		if (!invulnerable)
		{
			hearts--;
			GetView<HeartsView>().LoseHeart();
		}
		float delay = ((hearts != 0) ? 0.5f : 1.1f);
		yield return new WaitForSeconds(delay);
		if (hearts == 0)
		{
			sidescrollCam.Weight = 0f;
			Finish(succes: false);
		}
		else
		{
			player.ResetToIdle();
			RespawnAtLastCheckpoint();
		}
		isDoingSinkRoutine = false;
	}

	private IEnumerator doFinish(float afterSeconds, bool finishSucces)
	{
		yield return new WaitForSeconds(afterSeconds);
		Finish(finishSucces);
	}

	protected void onCheckpointReachedEvent(CheckPointReachedEvent evt)
	{
		registerCheckpoint(evt.checkpoint);
	}

	protected void onDoorEnterEvent(DoorEnterEvent evt)
	{
	}

	protected void onPlayerHit(PlayerHitEvent evt)
	{
		if (!invulnerable)
		{
			hearts--;
			GetView<HeartsView>().LoseHeart();
			StartCoroutine(BecomeInvulnerable());
			if (hearts == 0)
			{
				player.Collapse();
				StartCoroutine(doFinish(2f, finishSucces: false));
			}
			else
			{
				player.Hit();
			}
		}
	}

	protected virtual void onPickupCollectedEvent(PickupCollectedEvent evt)
	{
	}

	protected abstract void onFinishReachedEvent(FinishReachedEvent evt);

	protected virtual void onFallInWaterEvent(PlayerFallInWaterEvent evt)
	{
		if (!isDoingSinkRoutine)
		{
			StartCoroutine(doSinkRoutine());
		}
	}

	public IEnumerator BecomeInvulnerable()
	{
		invulnerable = true;
		SkinnedMeshRenderer[] skinnedMeshRenderers = player.GetComponentsInChildren<SkinnedMeshRenderer>();
		for (float t = 0f; t < 2f; t += 0.1f)
		{
			yield return new WaitForSeconds(0.1f);
			SkinnedMeshRenderer[] array = skinnedMeshRenderers;
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in array)
			{
				skinnedMeshRenderer.enabled = !skinnedMeshRenderer.enabled;
			}
		}
		invulnerable = false;
	}
}
