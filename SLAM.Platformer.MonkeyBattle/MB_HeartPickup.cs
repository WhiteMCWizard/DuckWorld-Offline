using UnityEngine;

namespace SLAM.Platformer.MonkeyBattle;

public class MB_HeartPickup : P_Pickup
{
	protected override void Start()
	{
		GetComponent<Collider>().isTrigger = true;
	}

	protected override void OnTriggerEnter(Collider other)
	{
		base.OnTriggerEnter(other);
		MonkeyBattleGame monkeyBattleGame = Object.FindObjectOfType<MonkeyBattleGame>();
		if (monkeyBattleGame.CollectHeart(this))
		{
			MonkeyBattleGame.HeartPickedUp heartPickedUp = new MonkeyBattleGame.HeartPickedUp();
			heartPickedUp.Pickup = this;
			GameEvents.Invoke(heartPickedUp);
		}
	}
}
