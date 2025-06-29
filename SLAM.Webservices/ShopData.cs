using LitJson;

namespace SLAM.Webservices;

public class ShopData
{
	[JsonName("id")]
	public int Id;

	[JsonName("title")]
	public string Title;

	[JsonName("items")]
	public ShopItemData[] Items;
}
