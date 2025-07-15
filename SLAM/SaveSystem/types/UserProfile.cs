using System;
using System.Collections.Generic;
using System.IO;
using LitJson;
using UnityEngine;

namespace SLAM.SaveSystem;

[Serializable]
public class UserProfile
{
	[NonSerialized]
	private static Dictionary<string, Texture2D> imageCache = new Dictionary<string, Texture2D>();

	[JsonName("id")]
	public int Id = 0;

	[JsonName("name")]
	public string Name = "";

	[JsonName("address")]
	public string Address = "";

	[JsonName("is_free")]
	public bool IsFree = false;

	[JsonName("is_sa")]
	public bool IsSA = false;

	private string _mugshotUrl = "";

	[NonSerialized]
	public Texture2D MugShot;

	public static UserProfile Current { get; private set; }

	[JsonName("mugshot")]
	public string MugShotUrl
	{
		get
		{
			return _mugshotUrl;
		}
		set
		{
			_mugshotUrl = value;
			loadMugshot();
		}
	}

	public string FirstName
	{
		get
		{
			if (Name.IndexOf(' ') > 0)
			{
				return Name.Substring(0, Name.IndexOf(' '));
			}
			return Name;
		}
	}

	public string LastName
	{
		get
		{
			if (Name.Length > 0)
			{
				return Name.Substring(Name.IndexOf(" "));
			}
			return Name;
		}
	}

	public static void GetCurrentProfileData(Action<UserProfile> callback)
	{
		if (SaveManager.Instance.IsLoaded)
		{
			Current = SaveManager.Instance.GetSaveData().profile;
			Current.loadMugshot();
		}
		else
		{
			Current = null;
		}

		callback?.Invoke(Current);
	}

	public static void UnsetCurrentProfileData()
	{
		Current = null;
	}

	private void loadMugshot()
	{
		// Instead of downloading, load from persistentDataPath/avatar_mugshot.png
		string mugshotPath = Path.Combine(Application.persistentDataPath, "avatar_mugshot.png");
		if (File.Exists(mugshotPath))
		{
			byte[] fileData = File.ReadAllBytes(mugshotPath);
			Texture2D tex = new Texture2D(2, 2, TextureFormat.ARGB32, mipmap: true);
			if (tex.LoadImage(fileData))
			{
				tex.wrapMode = TextureWrapMode.Clamp;
				MugShot = tex;
			}
			else
			{
				MugShot = null;
			}
		}
		else
		{
			MugShot = null;
		}
	}

	public void SetMugShot(Texture2D mugshot)
	{
		MugShot = mugshot;
	}
}