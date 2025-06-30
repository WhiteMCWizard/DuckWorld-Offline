using System;
using SLAM.Slinq;

namespace SLAM.Avatar;

[Serializable]
public class AvatarConfigurationData : ICloneable
{
	public AvatarSystem.Gender Gender;

	public AvatarSystem.Race Race;

	[AvatarItemLibrary.AvatarSkinColor]
	public JsonColor SkinColor;

	[AvatarItemLibrary.AvatarItemGUID]
	public string[] Items;

	public void ReplaceItem(AvatarItemLibrary.AvatarItem newItem, AvatarItemLibrary itemLibrary)
	{
		for (int i = 0; i < Items.Length; i++)
		{
			AvatarItemLibrary.AvatarItem itemByGUID = itemLibrary.GetItemByGUID(Items[i]);
			if (itemByGUID != null && itemByGUID.Category == newItem.Category)
			{
				Items[i] = newItem.GUID;
				break;
			}
		}
	}

	public AvatarItemLibrary.AvatarItem GetItemByCategory(AvatarSystem.ItemCategory category, AvatarItemLibrary itemLibrary)
	{
		for (int i = 0; i < Items.Length; i++)
		{
			AvatarItemLibrary.AvatarItem itemByGUID = itemLibrary.GetItemByGUID(Items[i]);
			if (itemByGUID != null && itemByGUID.Category == category)
			{
				return itemByGUID;
			}
		}
		return null;
	}

	public object Clone()
	{
		AvatarConfigurationData avatarConfigurationData = new AvatarConfigurationData();
		avatarConfigurationData.Gender = Gender;
		avatarConfigurationData.Race = Race;
		avatarConfigurationData.SkinColor = SkinColor;
		AvatarConfigurationData avatarConfigurationData2 = avatarConfigurationData;
		avatarConfigurationData2.Items = new string[Items.Length];
		Array.Copy(Items, avatarConfigurationData2.Items, Items.Length);
		return avatarConfigurationData2;
	}

	public bool HasItem(string guid)
	{
		return Items.Contains(guid);
	}
}
