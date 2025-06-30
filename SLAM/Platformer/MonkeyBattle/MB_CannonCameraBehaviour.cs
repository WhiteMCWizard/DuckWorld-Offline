using SLAM.CameraSystem;
using UnityEngine;

namespace SLAM.Platformer.MonkeyBattle;

public class MB_CannonCameraBehaviour : MB_PlayerCameraBehaviour
{
	[SerializeField]
	private CameraBehaviour defaultBehaviour;

	protected override void OnEnable()
	{
		base.OnEnable();
		GameEvents.Subscribe<MonkeyBattleGame.TurretEnteredEvent>(onTurretEntered);
		GameEvents.Subscribe<MonkeyBattleGame.TurretExitedEvent>(onTurretExited);
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		GameEvents.Unsubscribe<MonkeyBattleGame.TurretEnteredEvent>(onTurretEntered);
		GameEvents.Unsubscribe<MonkeyBattleGame.TurretExitedEvent>(onTurretExited);
	}

	private void onTurretEntered(MonkeyBattleGame.TurretEnteredEvent obj)
	{
		if (base.CameraManager.CurrentBehaviour(Layer) == defaultBehaviour)
		{
			base.CameraManager.CrossFade(this, 0.5f);
		}
	}

	private void onTurretExited(MonkeyBattleGame.TurretExitedEvent obj)
	{
		if (base.CameraManager.CurrentBehaviour(Layer) == this)
		{
			base.CameraManager.CrossFade(defaultBehaviour, 0.5f);
		}
	}
}
