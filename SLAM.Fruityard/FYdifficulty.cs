using System;
using SLAM.Engine;

namespace SLAM.Fruityard;

[Serializable]
public class FYdifficulty : GameController.LevelSetting
{
	public FYpickupList[] objectivesPerLevel;
}
