using UnityEngine;

namespace SLAM.Shops;

public class ShopVariationDefinition
{
	public ShopLibraryItem Item;

	public Texture2D Texture => Item.LibraryItem.Icon;

	public int Price => Item.ShopItem.Price;

	public bool HasBeenBoughtByPlayer { get; set; }
}
