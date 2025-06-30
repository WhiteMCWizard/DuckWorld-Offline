using System;
using LitJson;
using SLAM.Avatar;

namespace SLAM.SaveSystem;

[Serializable]
public class PlayerAvatarData
{
	[JsonName("mugshot")]
	public string MugShot;

	[JsonName("config")]
	public AvatarConfigurationData Config;
}
