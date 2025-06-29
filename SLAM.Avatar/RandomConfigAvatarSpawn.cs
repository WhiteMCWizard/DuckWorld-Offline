using UnityEngine;

namespace SLAM.Avatar;

public class RandomConfigAvatarSpawn : MonoBehaviour
{
	[SerializeField]
	private bool spawnImmediately = true;

	[SerializeField]
	private AvatarConfigurationData[] configs;

	private void Awake()
	{
		if (spawnImmediately)
		{
			SpawnAvatar();
		}
	}

	public GameObject SpawnAvatar()
	{
		AvatarConfigurationData random = configs.GetRandom();
		GameObject gameObject = AvatarSystem.SpawnAvatar(random);
		gameObject.transform.parent = base.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.SetLayerRecursively(LayerMask.NameToLayer("Avatar"));
		if ((bool)GetComponent<Animator>())
		{
			GetComponent<Animator>().Rebind();
		}
		return gameObject;
	}
}
