using UnityEngine;

namespace SLAM.FollowTheTruck;

public class FTTCrate : FTTCargo<FTTCrate>
{
	[SerializeField]
	private PrefabSpawner impactPlayerPrefab;

	[SerializeField]
	private string impactPlayerSound;

	private bool isStationary;

	private bool hitCrateFrontal;

	public override void OnInteract(Component component)
	{
		base.OnInteract(component);
		if (hitThePlayer && hitCrateFrontal)
		{
			if ((bool)impactPlayerPrefab)
			{
				Vector3 position = GetComponent<Collider>().ClosestPointOnBounds(component.transform.position);
				impactPlayerPrefab.SpawnAt(position);
			}
			if (!string.IsNullOrEmpty(impactPlayerSound))
			{
				AudioController.Play(impactPlayerSound);
			}
		}
		if (hitTheGround && !isStationary)
		{
			isStationary = true;
			GetComponent<Rigidbody>().isKinematic = true;
		}
	}

	public override void OnCollisionEnter(Collision other)
	{
		base.OnCollisionEnter(other);
		hitCrateFrontal = false;
		if (!other.collider.HasComponent<FTTAvatarController>())
		{
			return;
		}
		for (int i = 0; i < other.contacts.Length; i++)
		{
			if (base.transform.position.y > -0.5f && Vector3.Dot(base.transform.up * -1f, other.contacts[i].normal) < 0.5f)
			{
				hitCrateFrontal = true;
				break;
			}
		}
	}
}
