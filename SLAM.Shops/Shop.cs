using System;
using System.Collections.Generic;
using SLAM.Analytics;
using SLAM.Webservices;

namespace SLAM.Shops;

public class Shop : Inventory
{
	public class Feedback
	{
		public bool WasSuccesfull { get; private set; }

		public string Message { get; private set; }

		public Feedback(bool succes, string message)
		{
			WasSuccesfull = succes;
			Message = message;
		}
	}

	protected List<ShopVariationDefinition> shoppingCart;

	public int ShoppingCartValue
	{
		get
		{
			int num = 0;
			for (int i = 0; i < shoppingCart.Count; i++)
			{
				num += shoppingCart[i].Price;
			}
			return num;
		}
	}

	public ShopVariationDefinition[] ShoppingCart => shoppingCart.ToArray();

	protected override void Start()
	{
		base.Start();
		shoppingCart = new List<ShopVariationDefinition>();
	}

	public void AddToCart(ShopVariationDefinition item)
	{
		for (int num = shoppingCart.Count - 1; num >= 0; num--)
		{
			if (shoppingCart[num].Item.LibraryItem.Category == item.Item.LibraryItem.Category)
			{
				shoppingCart.RemoveAt(num);
			}
		}
		if (!item.HasBeenBoughtByPlayer && !shoppingCart.Contains(item))
		{
			shoppingCart.Add(item);
		}
	}

	public void RemoveFromCart(ShopVariationDefinition item)
	{
		if (shoppingCart.Contains(item))
		{
			shoppingCart.Remove(item);
		}
	}

	public void PurchaseShoppingCartContents(Action<Feedback> callback, Filter filter)
	{
		int[] array = new int[shoppingCart.Count];
		for (int i = 0; i < shoppingCart.Count; i++)
		{
			if (!shoppingCart[i].HasBeenBoughtByPlayer)
			{
				array[i] = shoppingCart[i].Item.ShopItem.Id;
				continue;
			}
			callback(new Feedback(succes: false, Localization.Get("WR_ERROR_ITEM_ALREADY_BOUGHT") + " " + shoppingCart[i].Texture.name));
			return;
		}
		ApiClient.PurchaseItems(array, 1, delegate(bool succes)
		{
			if (succes)
			{
				foreach (ShopVariationDefinition item in shoppingCart)
				{
					GameEvents.Invoke(new TrackingEvent
					{
						Type = TrackingEvent.TrackingType.ItemBought,
						Arguments = new Dictionary<string, object>
						{
							{
								"ItemGUID",
								item.Item.LibraryItem.GUID
							},
							{ "Price", item.Price }
						}
					});
					item.HasBeenBoughtByPlayer = true;
				}
				if (!filter.OnlyShowBoughtItems)
				{
					ShopCategoryDefinition[] array2 = categoryDefinitions;
					foreach (ShopCategoryDefinition shopCategoryDefinition in array2)
					{
						List<ShopVariationDefinition> list = new List<ShopVariationDefinition>(shopCategoryDefinition.Items);
						bool flag = false;
						foreach (ShopVariationDefinition item2 in shoppingCart)
						{
							if (list.Contains(item2))
							{
								list.Remove(item2);
								flag = true;
							}
						}
						if (flag)
						{
							shopCategoryDefinition.Items = list.ToArray();
						}
					}
				}
				callback(new Feedback(succes: true, string.Empty));
				shoppingCart.Clear();
			}
			else
			{
				callback(new Feedback(succes: true, StringFormatter.GetLocalizationFormatted("WR_ERROR_PURCHASE_FAILED", shoppingCart.Count)));
			}
		});
	}
}
