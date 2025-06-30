using System;
using SLAM.Engine;

namespace SLAM.Fruityard;

[Serializable]
public class FYpickupList : GameController.LevelSetting
{
	public float allowedTime;

	public float allowedTimePerTask = 10f;

	public FruityardGame.FYTreeType[] objectives;
}
