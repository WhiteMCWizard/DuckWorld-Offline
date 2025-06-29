using LitJson;

public class WebConfiguration
{
	[JsonName("version_number")]
	public string Version = "1.0";

	[JsonName("installer_win_url")]
	public string WindowsInstallerUrl = "http://localhost/SLAM-32-installer.exe";

	[JsonName("installer_win_checksum")]
	public string WindowsInstallerChecksum = "185f07337256844ab7d8e272a60d074d";

	[JsonName("installer_mac_url")]
	public string MacInstallerUrl = "http://localhost/installer.zip";

	[JsonName("installer_mac_checksum")]
	public string MacInstallerChecksum = "185f07337256844ab7d8e272a60d074d";

	[JsonName("installer_linux_url")]
	public string LinuxInstallerUrl = "http://localhost/installer.deb";

	[JsonName("installer_linux_checksum")]
	public string LinuxInstallerChecksum = "185f07337256844ab7d8e272a60d074d";

	[JsonName("download_page_url")]
	public string DownloadPageUrl = "http://duckworld.com/download";

	[JsonName("proposition_url")]
	public string PropositionUrl = "http://duckworld.com/prijzen";

	[JsonName("register_url")]
	public string RegisterUrl = "http://www.google.com";

	[JsonName("help_url")]
	public string HelpUrl = "http://www.google.com";

	[JsonName("password_forget_url")]
	public string PasswordForgetUrl = "http://www.google.com";

	[JsonName("cheats_allowed")]
	public bool CheatsEnabled = true;

	[JsonName("freeplay_allowed")]
	public bool FreeplayerEnabled;

	[JsonName("google_analytics_tracking_code")]
	public string GoogleAnalyticsTrackingCode = "UA-55680043-2";

	[JsonName("keepalive_interval")]
	public int KeepaliveInterval;
}
