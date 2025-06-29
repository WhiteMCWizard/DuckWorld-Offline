using System;
using UnityEngine;

namespace SLAM.Performance;

public class PerformanceAdjuster : MonoBehaviour
{
	[Serializable]
	public class PerformanceProfiles
	{
		public Adjustment Option;

		public PerformanceProfile low;

		public PerformanceProfile medium;

		public PerformanceProfile high;
	}

	[Serializable]
	public class PerformanceProfile
	{
		public Adjustment Option;

		public Material[] Materials;

		public bool IncludeChildren;

		public Component[] Components;

		public LightShadows Shadows;

		public bool AreComponentsEnabled;

		public bool AreGameObjectsEnabled;
	}

	[SerializeField]
	private PerformanceProfiles profiles;

	private void OnEnable()
	{
		GameEvents.Subscribe<PerformanceProfileChangedEvent>(onPerformanceProfileChanged);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<PerformanceProfileChangedEvent>(onPerformanceProfileChanged);
	}

	private void OnValidate()
	{
		if (profiles.low != null)
		{
			profiles.low.Option = profiles.Option;
		}
		if (profiles.medium != null)
		{
			profiles.medium.Option = profiles.Option;
		}
		if (profiles.high != null)
		{
			profiles.high.Option = profiles.Option;
		}
	}

	private void onPerformanceProfileChanged(PerformanceProfileChangedEvent evt)
	{
		switch (evt.NewQuality)
		{
		case Quality.High:
			apply(profiles.high);
			break;
		case Quality.Medium:
			apply(profiles.medium);
			break;
		default:
			apply(profiles.low);
			break;
		}
	}

	private void apply(PerformanceProfile p)
	{
		switch (p.Option)
		{
		case Adjustment.None:
			break;
		case Adjustment.ToggleGameObjectChildren:
		{
			foreach (Transform item in base.transform)
			{
				item.gameObject.SetActive(p.AreGameObjectsEnabled);
			}
			break;
		}
		case Adjustment.ChangeMaterials:
		{
			MeshRenderer[] array = ((!p.IncludeChildren) ? new MeshRenderer[1] { GetComponent<MeshRenderer>() } : GetComponentsInChildren<MeshRenderer>());
			if (array.Length > 0)
			{
				for (int i = 0; i < array.Length; i++)
				{
					array[i].materials = p.Materials;
				}
			}
			else
			{
				Debug.LogError(string.Concat("Cannot apply quality option ", p.Option, " because this object doesnot have a renderer!"), this);
			}
			break;
		}
		case Adjustment.ToggleComponents:
		{
			for (int j = 0; j < p.Components.Length; j++)
			{
				p.Components[j].GetType().GetProperty("enabled")?.SetValue(p.Components[j], p.AreComponentsEnabled, null);
			}
			break;
		}
		case Adjustment.Shadows:
			if (GetComponent<Light>() != null)
			{
				GetComponent<Light>().shadows = p.Shadows;
			}
			else
			{
				Debug.LogError(string.Concat("Cannot apply quality option ", p.Option, " because this object doesnot have a light!"), this);
			}
			break;
		}
	}
}
