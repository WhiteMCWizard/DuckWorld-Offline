using UnityEngine;

namespace SLAM.FollowTheTruck;

public class FTTCargo<T> : FTTInteractable<Component> where T : class
{
	[SerializeField]
	private PrefabSpawner impactGroundPrefab;

	[SerializeField]
	private string impactGroundSound;

	[SerializeField]
	private string loopSound;

	protected bool hitTheGround;

	protected bool hitThePlayer;

	public override void OnInteract(Component component)
	{
		FTTAvatarController component2 = component.gameObject.GetComponent<FTTAvatarController>();
		hitThePlayer = component2 != null;
		if (!hitThePlayer && !hitTheGround && GetComponent<Rigidbody>().useGravity)
		{
			hitTheGround = true;
			if (!string.IsNullOrEmpty(impactGroundSound))
			{
				AudioController.Play(impactGroundSound, base.transform);
			}
			if (!string.IsNullOrEmpty(loopSound))
			{
				AudioController.Play(loopSound, base.transform);
			}
			if (impactGroundPrefab != null)
			{
				Vector3 position = GetComponent<Collider>().ClosestPointOnBounds(component.transform.position);
				impactGroundPrefab.SpawnAt(position);
			}
		}
	}

	public void OnWaterHit(FTTWater water)
	{
		GetComponent<Collider>().enabled = false;
		Object.Destroy(base.gameObject);
	}
}
