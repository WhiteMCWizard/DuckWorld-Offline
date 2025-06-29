using UnityEngine;

namespace SLAM.FollowTheTruck;

public abstract class FTTInteractable<T> : MonoBehaviour where T : Component
{
	public virtual void OnCollisionEnter(Collision other)
	{
		if (other.collider.HasComponent<T>())
		{
			OnInteract(other.collider.GetComponent<T>());
		}
	}

	public virtual void OnTriggerEnter(Collider other)
	{
		if (other.HasComponent<T>())
		{
			OnInteract(other.GetComponent<T>());
		}
	}

	public abstract void OnInteract(T controller);
}
