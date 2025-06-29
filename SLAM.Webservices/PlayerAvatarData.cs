using LitJson;
using SLAM.Avatar;

namespace SLAM.Webservices;

public class PlayerAvatarData
{
	[JsonName("mugshot")]
	public string MugShot;

	[JsonName("config")]
	public AvatarConfigurationData Config;
}
