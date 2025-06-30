using System.Collections.Generic;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.Avatar;

public class RandomAvatarSpawn : MonoBehaviour
{
	[SerializeField]
	private bool spawnImmediately = true;

	[SerializeField]
	private AvatarSystem.Gender Gender;

	[AvatarItemLibrary.AvatarItemGUID]
	[Tooltip("Items the avatar shouldnt wear.")]
	[SerializeField]
	private string[] excludedItems;

	private AvatarConfigurationData config;

	private void Awake()
	{
		if (spawnImmediately)
		{
			SpawnRandomAvatar();
		}
	}

	[ContextMenu("Spawn Random Avatar")]
	public void SpawnRandomAvatar()
	{
		foreach (Transform item in base.transform)
		{
			if (item.name.Contains("Model_Master"))
			{
				Object.Destroy(item.gameObject);
			}
		}
		CreateConfig();
		SpawnAvatar();
	}

	public void CreateConfig()
	{
		config = new AvatarConfigurationData();
		config.Gender = Gender;
		config.Race = (AvatarSystem.Race)Random.Range(0, 3);
		AvatarItemLibrary itemLibrary = AvatarItemLibrary.GetItemLibrary(config.Race, config.Gender);
		List<string>[] array = new List<string>[5];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = new List<string>();
		}
		foreach (AvatarItemLibrary.AvatarItem item in itemLibrary.Items)
		{
			if (!excludedItems.Contains(item.GUID))
			{
				array[(int)(item.Category - 1)].Add(item.GUID);
			}
		}
		config.SkinColor = itemLibrary.SkinColors.GetRandom();
		config.Items = new string[array.Length];
		for (int j = 0; j < array.Length; j++)
		{
			int index = Random.Range(0, array[j].Count);
			config.Items[j] = array[j][index];
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
}
