using UnityEngine;

namespace SLAM.Avatar;

public class CustomAvatarSpawn : MonoBehaviour
{
	[SerializeField]
	private bool spawnImmediately = true;

	[SerializeField]
	private AvatarConfigurationData config;

	private void Awake()
	{
		if (spawnImmediately)
		{
			SpawnAvatar();
		}
	}

	public GameObject SpawnAvatar()
	{
		GameObject gameObject = AvatarSystem.SpawnAvatar(config);
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

	public void SetConfiguration(AvatarConfigurationData conf)
	{
		config = conf;
	}
}
