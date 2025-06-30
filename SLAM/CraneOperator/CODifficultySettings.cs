using System;
using System.Collections.Generic;
using SLAM.Engine;

namespace SLAM.CraneOperator;

[Serializable]
public class CODifficultySettings : GameController.LevelSetting
{
	public int AmountOfTrucks = 5;

	public float LevelDuration = 60f;

	public List<COManifest> TruckManifests;

	public Crate.CrateType[] ExtraCrates;
}
