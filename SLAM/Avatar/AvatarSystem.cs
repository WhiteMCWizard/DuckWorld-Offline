using SLAM.SaveSystem;
using SLAM.Slinq;
using SLAM.Webservices;
using UnityEngine;

namespace SLAM.Avatar;

public static class AvatarSystem
{
	public enum Gender
	{
		Boy,
		Girl
	}

	public enum Race
	{
		Dog,
		Cat,
		Duck
	}

	public enum ItemCategory
	{
		Skin,
		Hair,
		Eyes,
		Torso,
		Legs,
		Feet
	}

	public const string defaultConfigurationJson = "{\"Gender\":\"Boy\",\"Race\":\"Dog\",\"SkinColor\":{\"r\":0.945098042488098,\"g\":0.788235306739807,\"b\":0.709803938865662},\"Items\":[\"31ea5849-b3b6-47e9-b6b8-0a606cee53ac\",\"d3167c5e-42f6-43fe-9a1a-6c4045270499\",\"32599b5a-9c8a-4283-8439-f3dd85fd465b\",\"9c675a13-b379-455a-a2a5-e92b87af1f26\",\"d40d736d-e224-4d34-b102-793adee32d95\"]}";

	private static AvatarConfigurationData playerAvatarConfiguration;

	static AvatarSystem()
	{
		SaveManager.Instance.OnDataLoaded += LoadPlayerConfiguration;
	}

	public static void SavePlayerConfiguration(AvatarConfigurationData config, Texture2D mugshot)
	{
		playerAvatarConfiguration = config;
		if(UserProfile.Current != null) UserProfile.Current.SetMugShot(mugshot);

		var avatarData = PlayerAvatarData.Current;
		if (avatarData != null)
		{
			avatarData.Config = config;
			PlayerAvatarData.SetCurrentAvatarData(avatarData);
			PlayerAvatarData.SaveMugShot(mugshot);
		}
		else
		{
			Debug.LogWarning("PlayerAvatarData.Current is null. SaveManager may not be loaded yet.");
		}
	}

	public static AvatarConfigurationData GetPlayerConfiguration()
	{
		if (playerAvatarConfiguration == null)
		{
			LoadPlayerConfiguration();
		}
		return playerAvatarConfiguration;
	}

	public static void UnsetPlayerConfiguration()
	{
		playerAvatarConfiguration = null;
	}

	public static void LoadPlayerConfiguration()
	{
		if (!SaveManager.Instance.IsLoaded)
        {
            Debug.LogWarning("AvatarSystem.LoadPlayerConfiguration called before SaveManager was loaded. This may result in using default avatar.");
        }
		// Load from offline SaveData
		var data = PlayerAvatarData.Current;
		if (data != null && data.Config != null)
		{
			playerAvatarConfiguration = data.Config;
		}
	}

	public static GameObject SpawnPlayerAvatar()
	{
		return SpawnAvatar(GetPlayerConfiguration());
	}

	public static GameObject SpawnAvatar(AvatarConfigurationData config)
	{
		GameObject gameObject = loadModel(config.Race, config.Gender);
		UpdateAvatar(gameObject, config);
		return gameObject;
	}

	public static void UpdateAvatar(GameObject avatarRoot, AvatarConfigurationData config)
	{
		AvatarItemLibrary itemLibrary = AvatarItemLibrary.GetItemLibrary(config);
		string[] items = config.Items;
		foreach (string text in items)
		{
			AvatarItemLibrary.AvatarItem itemByGUID = itemLibrary.GetItemByGUID(text);
			if (itemByGUID != null)
			{
				applyItem(avatarRoot.transform.FindChildRecursively(itemByGUID.MeshName), itemByGUID.Material);
			}
			else
			{
				Debug.LogError("Unkown item guid " + text);
			}
		}
		foreach (Material item in avatarRoot.GetComponentsInChildren<Renderer>().SelectMany((Renderer r) => r.materials))
		{
			if (item != null && item.HasProperty("_SkinColor"))
			{
				item.SetColor("_SkinColor", config.SkinColor);
			}
		}
	}

	private static void applyItem(Transform itemMesh, Material material)
	{
		foreach (Transform item in itemMesh.parent)
		{
			item.gameObject.SetActive(item == itemMesh);
		}
		Renderer component = itemMesh.GetComponent<Renderer>();
		Material[] materials = component.materials;
		for (int i = 0; i < materials.Length; i++)
		{
			Material material2 = materials[i];
			if (material2 != null && !material2.name.Contains("Skin") && !material2.name.Contains("DontReplace"))
			{
				materials[i] = material;
			}
			if (material2.name.ToLower().Contains("helmet"))
			{
				break;
			}
		}
		component.materials = materials;
	}

	private static GameObject loadModel(Race race, Gender gender)
	{
		return Object.Instantiate(Resources.Load($"{gender}_{race}_Model_Master")) as GameObject;
	}
}
