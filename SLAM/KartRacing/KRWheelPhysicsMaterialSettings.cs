using UnityEngine;

namespace SLAM.KartRacing;

public class KRWheelPhysicsMaterialSettings
{
	private float brakePower;

	private float forwardStiffness;

	private float sidewaysStiffness;

	[SerializeField]
	private KRPhysicsMaterialType materialType;

	public static KRWheelPhysicsMaterialSettings DefaultDirtSettings = new KRWheelPhysicsMaterialSettings
	{
		brakePower = 0f,
		forwardStiffness = 1f,
		sidewaysStiffness = 2.5f,
		materialType = KRPhysicsMaterialType.Dirt
	};

	public static KRWheelPhysicsMaterialSettings DefaultSnowSettings = new KRWheelPhysicsMaterialSettings
	{
		brakePower = 0f,
		forwardStiffness = 0.66f,
		sidewaysStiffness = 1f,
		materialType = KRPhysicsMaterialType.Snow
	};

	public static KRWheelPhysicsMaterialSettings DefaultIceSettings = new KRWheelPhysicsMaterialSettings
	{
		brakePower = 0f,
		forwardStiffness = 0f,
		sidewaysStiffness = 0f,
		materialType = KRPhysicsMaterialType.Ice
	};

	public static KRWheelPhysicsMaterialSettings DefaultMudSettings = new KRWheelPhysicsMaterialSettings
	{
		brakePower = 0.8f,
		forwardStiffness = 0.7f,
		sidewaysStiffness = 0.7f,
		materialType = KRPhysicsMaterialType.Mud
	};

	public static KRWheelPhysicsMaterialSettings DefaultOilSettings = new KRWheelPhysicsMaterialSettings
	{
		brakePower = 0f,
		forwardStiffness = 0f,
		sidewaysStiffness = 0f,
		materialType = KRPhysicsMaterialType.Oil
	};

	public static KRWheelPhysicsMaterialSettings DefaultWoodSettings = new KRWheelPhysicsMaterialSettings
	{
		brakePower = 0f,
		forwardStiffness = 1f,
		sidewaysStiffness = 2.5f,
		materialType = KRPhysicsMaterialType.Wood
	};

	public float BrakePower => brakePower;

	public float ForwardStiffness => forwardStiffness;

	public float SidewaysStiffness => sidewaysStiffness;

	public KRPhysicsMaterialType MaterialType => materialType;

	public static KRWheelPhysicsMaterialSettings GetSettingsForMaterial(KRPhysicsMaterialType type)
	{
		return type switch
		{
			KRPhysicsMaterialType.Dirt => DefaultDirtSettings, 
			KRPhysicsMaterialType.Ice => DefaultIceSettings, 
			KRPhysicsMaterialType.Mud => DefaultMudSettings, 
			KRPhysicsMaterialType.Oil => DefaultOilSettings, 
			KRPhysicsMaterialType.Snow => DefaultSnowSettings, 
			KRPhysicsMaterialType.Wood => DefaultWoodSettings, 
			_ => DefaultDirtSettings, 
		};
	}
}
