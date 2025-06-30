using System;
using SLAM.Engine;
using UnityEngine;

namespace SLAM.BatCave;

[Serializable]
public class BatCaveDifficultySettings : GameController.LevelSetting
{
	public GameObject LevelRoot;

	public Texture BackgroundTexture;
}
