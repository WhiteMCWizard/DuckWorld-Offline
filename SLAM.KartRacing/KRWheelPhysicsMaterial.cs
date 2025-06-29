using UnityEngine;

namespace SLAM.KartRacing;

public class KRWheelPhysicsMaterial : MonoBehaviour
{
	[SerializeField]
	private KRPhysicsMaterialType materialType;

	public KRPhysicsMaterialType MaterialType => materialType;
}
