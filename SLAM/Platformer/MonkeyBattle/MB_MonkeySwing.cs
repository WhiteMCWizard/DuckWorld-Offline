using UnityEngine;

namespace SLAM.Platformer.MonkeyBattle;

public class MB_MonkeySwing : MonoBehaviour
{
	private void OnEnable()
	{
		GameEvents.Subscribe<MonkeyBattleGame.GameStartedEvent>(onGameStartedEvent);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<MonkeyBattleGame.GameStartedEvent>(onGameStartedEvent);
	}

	private void onGameStartedEvent(MonkeyBattleGame.GameStartedEvent evt)
	{
		GetComponent<Animator>().speed = evt.settings.monkeySwingRotationSpeed;
	}
}
