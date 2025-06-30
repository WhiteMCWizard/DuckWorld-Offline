using UnityEngine;

namespace SLAM.FollowTheTruck;

public class FTTBarrel : FTTCargo<FTTBarrel>
{
	[SerializeField]
	private float rollSpeed = 10f;

	[SerializeField]
	private PrefabSpawner impactPlayerPrefab;

	[SerializeField]
	private string impactPlayerSound;

	private bool isRolling;

	public override void OnInteract(Component component)
	{
		base.OnInteract(component);
		if (hitThePlayer)
		{
			if ((bool)impactPlayerPrefab)
			{
				impactPlayerPrefab.SpawnAt(component.transform.position);
			}
			if (!string.IsNullOrEmpty(impactPlayerSound))
			{
				AudioController.Play(impactPlayerSound);
			}
			Object.Destroy(base.gameObject);
		}
		if (hitTheGround && !isRolling)
		{
			isRolling = true;
			Vector3 force = Vector3.back * rollSpeed;
			GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
		}
	}
}
