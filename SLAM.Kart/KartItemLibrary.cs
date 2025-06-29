using System;
using SLAM.KartRacing;
using SLAM.Shops;
using UnityEngine;

namespace SLAM.Kart;

public class KartItemLibrary : ItemLibrary<KartItemLibrary.KartItem>
{
	[Serializable]
	public class KartItem : Item
	{
		public GameObject Prefab;

		public Texture2D Icon;

		[Range(0f, 1f)]
		public float TopSpeed;

		[Range(0f, 1f)]
		public float Acceleration;

		[Range(0f, 1f)]
		public float Handling;

		public bool Snow;

		public bool Oil;

		public KartSystem.ItemCategory Category;

		public override string CategoryName => Category.ToString();

		public float GetStat(KartSystem.ItemStat stat, KRPhysicsMaterialType materialType = KRPhysicsMaterialType.Dirt)
		{
			return stat switch
			{
				KartSystem.ItemStat.TopSpeed => TopSpeed, 
				KartSystem.ItemStat.Acceleration => Acceleration, 
				_ => Handling, 
			};
		}
	}

	public class KartItemGUID : PropertyAttribute
	{
	}

	[SerializeField]
	private Color[] primaryColorPalette;

	[SerializeField]
	private Color[] secondaryColorPalette;

	[SerializeField]
	private KartConfigurationData[] defaultConfigurations;

	public Color[] PrimaryColorPalette => primaryColorPalette;

	public Color[] SecondaryColorPalette => secondaryColorPalette;

	public KartConfigurationData[] DefaultConfigurations => defaultConfigurations;

	public static KartItemLibrary GetItemLibrary()
	{
		return Resources.Load<KartItemLibrary>("Kart_Items");
	}
}
