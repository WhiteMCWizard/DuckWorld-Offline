using System;
using SLAM.Engine;
using UnityEngine;

namespace SLAM.BeatTheBeagleBoys;

[Serializable]
public class BTBDifficultySetting : GameController.LevelSetting
{
	[SerializeField]
	private AnimationCurve monitorVisibleTime;

	[SerializeField]
	private AnimationCurve thiefActiveCount;

	[SerializeField]
	private AnimationCurve thiefIdleTime;

	[SerializeField]
	private AnimationCurve thiefStealingTime;

	[SerializeField]
	private AnimationCurve cageResetTime;

	[SerializeField]
	private int levelDuration;

	private Alarm gameTimer;

	private float timeProgression => gameTimer.Progress;

	public int LevelDuration => levelDuration;

	public void SetGameTimer(Alarm gameTimer)
	{
		this.gameTimer = gameTimer;
	}

	public float GetMonitorVisibleTime()
	{
		return monitorVisibleTime.Evaluate(timeProgression);
	}

	public int GetThiefIdlingTime()
	{
		return (int)thiefIdleTime.Evaluate(timeProgression);
	}

	public int GetThiefActiveCount()
	{
		return (int)thiefActiveCount.Evaluate(timeProgression);
	}

	public float GetThiefStealingTime()
	{
		return thiefStealingTime.Evaluate(timeProgression);
	}

	public float GetCageResetTime()
	{
		return cageResetTime.Evaluate(timeProgression);
	}
}
