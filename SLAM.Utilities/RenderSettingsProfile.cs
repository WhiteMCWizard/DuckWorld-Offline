using System;
using System.Collections.Generic;
using UnityEngine;

namespace SLAM.Utilities;

[Serializable]
public class RenderSettingsProfile
{
	public bool fog;

	public Color fogColor;

	public FogMode fogMode;

	public float fogDensity;

	public float fogStartDistance;

	public float fogEndDistance;

	public Color ambientLight;

	public Material skybox;

	private static Stack<RenderSettingsProfile> rspStack = new Stack<RenderSettingsProfile>();

	public static RenderSettingsProfile Current
	{
		get
		{
			RenderSettingsProfile renderSettingsProfile = new RenderSettingsProfile();
			renderSettingsProfile.ambientLight = RenderSettings.ambientLight;
			renderSettingsProfile.fog = RenderSettings.fog;
			renderSettingsProfile.fogMode = RenderSettings.fogMode;
			renderSettingsProfile.fogColor = RenderSettings.fogColor;
			renderSettingsProfile.fogDensity = RenderSettings.fogDensity;
			renderSettingsProfile.fogStartDistance = RenderSettings.fogStartDistance;
			renderSettingsProfile.fogEndDistance = RenderSettings.fogEndDistance;
			renderSettingsProfile.skybox = RenderSettings.skybox;
			return renderSettingsProfile;
		}
		set
		{
			RenderSettings.ambientLight = value.ambientLight;
			RenderSettings.fog = value.fog;
			RenderSettings.fogMode = value.fogMode;
			RenderSettings.fogColor = value.fogColor;
			RenderSettings.fogDensity = value.fogDensity;
			RenderSettings.fogStartDistance = value.fogStartDistance;
			RenderSettings.fogEndDistance = value.fogEndDistance;
			RenderSettings.skybox = value.skybox;
		}
	}

	public static void Apply(RenderSettingsProfile settings)
	{
		rspStack.Push(Current);
		Current = settings;
	}

	public static void Restore()
	{
		RenderSettingsProfile current = rspStack.Pop();
		Current = current;
	}

	public static void Copy(RenderSettingsProfile from, RenderSettingsProfile to)
	{
		to.ambientLight = from.ambientLight;
		to.fog = from.fog;
		to.fogMode = from.fogMode;
		to.fogColor = from.fogColor;
		to.fogDensity = from.fogDensity;
		to.fogStartDistance = from.fogStartDistance;
		to.fogEndDistance = from.fogEndDistance;
		to.skybox = from.skybox;
	}
}
