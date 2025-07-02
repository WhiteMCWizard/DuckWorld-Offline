using System;
using LitJson;

namespace SLAM.Webservices;

[Serializable]
public class PurchasedShopItemData
{
	[JsonName("shopitem")]
	public int ShopItemId;
}
