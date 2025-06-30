using System;
using System.Collections;
using UnityEngine;

namespace SLAM.Platformer.MonkeyBattle;

public class MB_TurretManager : MonoBehaviour
{
	[SerializeField]
	private MB_Turret[] turrets;

	private float appearanceIntervalMin;

	private float appearanceIntervalMax;

	private void OnEnable()
	{
		GameEvents.Subscribe<MonkeyBattleGame.GameStartedEvent>(onGameStart);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<MonkeyBattleGame.GameStartedEvent>(onGameStart);
	}

	private void onGameStart(MonkeyBattleGame.GameStartedEvent evt)
	{
		appearanceIntervalMin = evt.settings.turretAppearanceIntervalMin;
		appearanceIntervalMax = evt.settings.turretAppearanceIntervalMax;
		MB_Turret random = turrets.GetRandom();
		MB_Turret[] array = turrets;
		foreach (MB_Turret mB_Turret in array)
		{
			if (random != mB_Turret)
			{
				mB_Turret.Disappear();
			}
		}
	}

	private IEnumerator activateTurret(MB_Turret turret)
	{
		float delay = UnityEngine.Random.Range(appearanceIntervalMin, appearanceIntervalMax);
		yield return new WaitForSeconds(delay);
		turret.Appear();
	}

	public void SwitchToNewTurret(MB_Turret turret)
	{
		int num = Array.IndexOf(turrets, turret);
		int num2 = num + turrets.Length / 2 + UnityEngine.Random.Range(-1, 2);
		num2 %= turrets.Length;
		turret.Disappear();
		StartCoroutine(activateTurret(turrets[num2]));
	}
}
