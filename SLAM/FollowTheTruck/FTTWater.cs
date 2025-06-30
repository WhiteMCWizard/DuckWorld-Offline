using UnityEngine;

namespace SLAM.FollowTheTruck;

public class FTTWater : FTTInteractable<Component>
{
	[SerializeField]
	private GameObject splashPrefab;

	public override void OnInteract(Component component)
	{
		Vector3 pos = GetComponent<Collider>().ClosestPointOnBounds(component.transform.position);
		spawnParticle(splashPrefab, pos);
		component.SendMessage("OnWaterHit", this, SendMessageOptions.DontRequireReceiver);
	}

	private void spawnParticle(GameObject particlePrefab, Vector3 pos)
	{
		GameObject gameObject = Object.Instantiate(particlePrefab);
		gameObject.transform.parent = base.transform;
		gameObject.transform.position = pos;
	}
}
