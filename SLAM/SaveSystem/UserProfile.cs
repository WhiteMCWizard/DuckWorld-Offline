using System;
using System.Collections.Generic;
using LitJson;
using SLAM.Webservices;
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
			downloadMugshot();
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

	private void downloadMugshot()
	{
		if (string.IsNullOrEmpty(MugShotUrl))
		{
			return;
		}
		if (imageCache.ContainsKey(MugShotUrl))
		{
			Webservice.WaitFor(delegate
			{
				MugShot = imageCache[MugShotUrl];
			}, MugShotUrl);
			return;
		}
		imageCache.Add(MugShotUrl, null);
		SingletonMonobehaviour<Webservice>.Instance.DoRequest("GET", MugShotUrl, delegate(WebResponse response)
		{
			if (response.Request.error == null)
			{
				MugShot = response.Request.textureNonReadable;
				MugShot = new Texture2D(MugShot.width, MugShot.height, TextureFormat.ARGB32, mipmap: true);
				MugShot.wrapMode = TextureWrapMode.Clamp;
				response.Request.LoadImageIntoTexture(MugShot);
				imageCache[MugShotUrl] = MugShot;
			}
			else
			{
				imageCache[MugShotUrl] = null;
			}
		});
	}

	public void SetMugShot(Texture2D mugshot)
	{
		MugShot = mugshot;
	}
}
