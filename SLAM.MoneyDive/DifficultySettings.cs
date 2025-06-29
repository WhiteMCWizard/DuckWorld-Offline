using System;
using SLAM.Engine;

namespace SLAM.MoneyDive;

[Serializable]
public class DifficultySettings : GameController.LevelSetting
{
	public float GameDuration = 30f;

	public int RequiredEquationCount = 10;

	public string[] TrickIds;

	public EquationSettings[] Equations;
}
