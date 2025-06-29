using System;
using UnityEngine;

namespace SLAM.Avatar;

public class AvatarSpawn : MonoBehaviour
{
	[Serializable]
	private struct ItemOverride
	{
		public AvatarSystem.Race Race;

		public AvatarSystem.Gender Gender;

		[AvatarItemLibrary.AvatarItemGUID]
		public string GUID;
	}

	[SerializeField]
	private bool spawnImmediately = true;

	[SerializeField]
	[Tooltip("Temporary items the avatar should equip for this game.")]
	private ItemOverride[] itemOverrides;

	private void Awake()
	{
		if (spawnImmediately)
		{
			SpawnAvatar();
		}
	}

	public GameObject SpawnAvatar()
	{
		GameObject gameObject;
		if (itemOverrides != null && itemOverrides.Length > 0)
		{
			AvatarConfigurationData avatarConfigurationData = AvatarSystem.GetPlayerConfiguration().Clone() as AvatarConfigurationData;
			AvatarItemLibrary itemLibrary = AvatarItemLibrary.GetItemLibrary(avatarConfigurationData);
			for (int i = 0; i < itemOverrides.Length; i++)
			{
				if (itemOverrides[i].Race == avatarConfigurationData.Race && itemOverrides[i].Gender == avatarConfigurationData.Gender)
				{
					AvatarItemLibrary.AvatarItem itemByGUID = itemLibrary.GetItemByGUID(itemOverrides[i].GUID);
					if (itemByGUID != null)
					{
						avatarConfigurationData.ReplaceItem(itemByGUID, itemLibrary);
					}
				}
			}
			gameObject = AvatarSystem.SpawnAvatar(avatarConfigurationData);
		}
		else
		{
			gameObject = AvatarSystem.SpawnPlayerAvatar();
		}
		gameObject.transform.parent = base.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.SetLayerRecursively(LayerMask.NameToLayer("Avatar"));
		SkinnedMeshRenderer[] componentsInChildren = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
		for (int j = 0; j < componentsInChildren.Length; j++)
		{
			componentsInChildren[j].updateWhenOffscreen = true;
		}
		if ((bool)GetComponent<Animator>())
		{
			GetComponent<Animator>().Rebind();
		}
		return gameObject;
	}
}
