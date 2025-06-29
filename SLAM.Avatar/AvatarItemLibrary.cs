using System;
using LitJson;
using SLAM.Shops;
using UnityEngine;

namespace SLAM.Avatar;

public class AvatarItemLibrary : ItemLibrary<AvatarItemLibrary.AvatarItem>
{
	[Serializable]
	public class AvatarItem : Item
	{
		public Material Material;

		public string MeshName;

		public AvatarSystem.ItemCategory Category;

		public Texture2D Icon;

		public override string CategoryName => Category.ToString();
	}

	public class AvatarItemGUID : PropertyAttribute
	{
	}

	public class AvatarSkinColor : PropertyAttribute
	{
	}

	[SerializeField]
	[JsonIgnore]
	private Color[] skinColors;

	[SerializeField]
	private AvatarConfigurationData[] defaultConfigurations;

	public Color[] SkinColors => skinColors;

	public AvatarConfigurationData[] DefaultConfigurations => defaultConfigurations;

	public static AvatarItemLibrary GetItemLibrary(AvatarConfigurationData configData)
	{
		return GetItemLibrary(configData.Race, configData.Gender);
	}

	public static AvatarItemLibrary GetItemLibrary(AvatarSystem.Race race, AvatarSystem.Gender gender)
	{
		return Resources.Load<AvatarItemLibrary>($"{gender}_{race}_Items");
	}
}
