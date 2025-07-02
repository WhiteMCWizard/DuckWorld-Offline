using LitJson;
using SLAM.Webservices;
using System.Collections.Generic;

public class AllShops
{
    private static readonly ShopItemData[] _allItems = CombineAll();
    public static ShopItemData[] AllItems => _allItems;

    private static ShopItemData[] CombineAll()
    {
        var list = new List<ShopItemData>();
        list.AddRange(FashionStoreItems.All);
        list.AddRange(KartshopItems.All);
        return list.ToArray();
    }


    public static ShopData FashionStore = new ShopData
    {
        Id = 1,
        Title = "FashionStore",
        Items = FashionStoreItems.All
    };

    public static ShopData Kartshop = new ShopData
    {
        Id = 3,
        Title = "Kartshop",
        Items = KartshopItems.All
    };
}