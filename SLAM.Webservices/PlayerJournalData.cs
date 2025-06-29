using LitJson;

namespace SLAM.Webservices;

public class PlayerJournalData
{
	[JsonName("profile")]
	public UserProfile Profile;

	[JsonName("friends")]
	public UserProfile[] Friends;

	[JsonName("behaviour_badges")]
	public BadgeData[] Badges;

	[JsonName("adventures")]
	public AdventureData[] Adventures;

	[JsonName("games")]
	public GameData[] StickerGames;
}
