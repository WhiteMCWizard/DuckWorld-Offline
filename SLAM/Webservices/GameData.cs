using LitJson;

namespace SLAM.Webservices;

public class GameData
{
	public class StickerData
	{
		[JsonName("title")]
		public string Name;

		[JsonName("description")]
		public string Description;

		[JsonName("image")]
		public string Image;

		[JsonName("is_earned")]
		public bool Earned;

		[JsonName("special")]
		public bool IsSpecial;

		[JsonName("stars")]
		public int Stars;
	}

	[JsonName("game")]
	public int GameId;

	[JsonName("name")]
	public string Name;

	[JsonName("image")]
	public string Image;

	[JsonName("is_unlocked")]
	public bool IsUnlocked;

	[JsonName("is_unlocked_sa")]
	public bool IsUnlockedSA;

	[JsonName("stickers")]
	public StickerData[] Stickers;
}
