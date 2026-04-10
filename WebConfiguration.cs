using LitJson;

public class WebConfiguration
{
	[JsonName("version_number")]
	public string Version = "1.0";

	[JsonName("cheats_allowed")]
	public bool CheatsEnabled = true;

	[JsonName("freeplay_allowed")]
	public bool FreeplayerEnabled;
}
