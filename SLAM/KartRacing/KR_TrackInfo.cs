using System;
using SLAM.Engine;
using UnityEngine;

namespace SLAM.KartRacing;

[Serializable]
public class KR_TrackInfo : GameController.LevelSetting
{
	public KRGameMode mode;

	public GameObject trackObject;

	public KR_AISettings[] AISettings;

	public AudioClip ambienceAudio;
}
