using System;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.BuildSystem;

public class SceneDataLibrary : ScriptableObject
{
	[Serializable]
	public class Version
	{
		public int Major;

		public int Minor;

		public int Build;

		public override string ToString()
		{
			return $"{Major}.{Minor}.{Build}";
		}
	}

	[Serializable]
	public class LevelAssetVersion
	{
		public string LevelName;

		public int CRC;

		public int Version;

		[Popup(new string[] { "hub_loading_screen", "chapter1_loading_screen", "chapter2_loading_screen", "job_loading_screen", "none" })]
		public string LoadingScreenName;
	}

	[SerializeField]
	public Version GameVersion;

	[SerializeField]
	private LevelAssetVersion[] levelAssets;

	public static SceneDataLibrary GetSceneDataLibrary()
	{
		return Resources.Load<SceneDataLibrary>("SceneDataLibrary");
	}

	public LevelAssetVersion GetVersionData(string levelName)
	{
		return levelAssets.FirstOrDefault((LevelAssetVersion l) => l.LevelName == levelName);
	}
}
