using System;
using LitJson;
using SLAM.Avatar;
using UnityEngine;

namespace SLAM.SaveSystem;

[Serializable]
public class PlayerAvatarData
{
	[JsonName("mugshot")]
	public string MugShot;
	[JsonName("config")]
	public AvatarConfigurationData Config;
	public static PlayerAvatarData Current
	{
		get
		{
			if (!SaveManager.Instance.IsLoaded)
			{
				Debug.LogWarning("Requesting PlayerAvatarData before SaveManager is loaded.");
				return null;
			}
			var saveData = SaveManager.Instance.GetSaveData();
			if (saveData.avatar == null)
			{
				saveData.avatar = new PlayerAvatarData();
			}
			return saveData.avatar;
		}
	}
	public static void SetCurrentAvatarData(PlayerAvatarData data)
	{
		if (data == null)
		{
			throw new ArgumentNullException(nameof(data), "Cannot set current avatar data to null.");
		}
		var saveData = SaveManager.Instance.GetSaveData();
		if (saveData != null)
		{
			saveData.avatar = data;
			SaveManager.Instance.MarkDirty();
		}
	}
	public static void SaveMugShot(Texture2D mugshot)
	{
		if (mugshot == null)
		{
			Debug.LogWarning("Attempted to save null mugshot.");
			return;
		}
		SaveManager.Instance.SaveTextureToFile(mugshot, "avatar_mugshot.png");
	}
}