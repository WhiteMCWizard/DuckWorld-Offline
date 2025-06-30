using LitJson;

namespace SLAM.Webservices;

public class ShopItemData
{
	[JsonName("id")]
	public int Id;

	public string GUID;

	[JsonName("title")]
	public string Title;

	[JsonName("description")]
	public string Description;

	[JsonName("price")]
	public int Price;

	[JsonName("visible_in_shop")]
	public bool VisibleInShop;

	[JsonName("meta")]
	public string InternalName;
}
