using System;
using UnityEngine;

namespace SLAM.Performance;

public class QualitySettings : MonoBehaviour
{
	[Serializable]
	public class QualityProfile
	{
		public int pixelLightCount;

		public AnisotropicFiltering anisotropicFiltering;

		public int antiAliasing;

		public ShadowProjection shadowProjection;

		public int shadowCascades;

		public float shadowDistance;

		public float lodBias;

		public int maximumLODLevel;

		public bool enableSoftParticles;
	}

	[SerializeField]
	private QualityProfile high;

	[SerializeField]
	private QualityProfile medium;

	[SerializeField]
	private QualityProfile low;

	[Header("Original Settings")]
	[SerializeField]
	[HideInInspector]
	private int pixelLightCount;

	[SerializeField]
	[HideInInspector]
	private AnisotropicFiltering anisotropicFiltering;

	[SerializeField]
	[HideInInspector]
	private int antiAliasing;

	[SerializeField]
	[HideInInspector]
	private ShadowProjection shadowProjection;

	[SerializeField]
	[HideInInspector]
	private int shadowCascades;

	[HideInInspector]
	[SerializeField]
	private float shadowDistance;

	[HideInInspector]
	[SerializeField]
	private float lodBias;

	[HideInInspector]
	[SerializeField]
	private int maximumLODLevel;

	[HideInInspector]
	[SerializeField]
	private bool enableSoftParticles;

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
	}

	private void Reset()
	{
		pixelLightCount = UnityEngine.QualitySettings.pixelLightCount;
		anisotropicFiltering = UnityEngine.QualitySettings.anisotropicFiltering;
		antiAliasing = UnityEngine.QualitySettings.antiAliasing;
		shadowProjection = UnityEngine.QualitySettings.shadowProjection;
		shadowCascades = UnityEngine.QualitySettings.shadowCascades;
		shadowDistance = UnityEngine.QualitySettings.shadowDistance;
		lodBias = UnityEngine.QualitySettings.lodBias;
		maximumLODLevel = UnityEngine.QualitySettings.maximumLODLevel;
	}

	public void RestoreOrigninalValues()
	{
		high.pixelLightCount = pixelLightCount;
		high.anisotropicFiltering = anisotropicFiltering;
		high.antiAliasing = antiAliasing;
		high.shadowProjection = shadowProjection;
		high.shadowCascades = shadowCascades;
		high.shadowDistance = shadowDistance;
		high.lodBias = lodBias;
		high.maximumLODLevel = maximumLODLevel;
		high.enableSoftParticles = enableSoftParticles;
		medium.pixelLightCount = pixelLightCount;
		medium.anisotropicFiltering = anisotropicFiltering;
		medium.antiAliasing = antiAliasing;
		medium.shadowProjection = shadowProjection;
		medium.shadowCascades = shadowCascades;
		medium.shadowDistance = shadowDistance;
		medium.lodBias = lodBias;
		medium.maximumLODLevel = maximumLODLevel;
		medium.enableSoftParticles = enableSoftParticles;
		low.pixelLightCount = pixelLightCount;
		low.anisotropicFiltering = anisotropicFiltering;
		low.antiAliasing = antiAliasing;
		low.shadowProjection = shadowProjection;
		low.shadowCascades = shadowCascades;
		low.shadowDistance = shadowDistance;
		low.lodBias = lodBias;
		low.maximumLODLevel = maximumLODLevel;
		low.enableSoftParticles = enableSoftParticles;
	}

	private void onPerformanceProfileChanged(PerformanceProfileChangedEvent evt)
	{
		switch (evt.NewQuality)
		{
		case Quality.High:
			applySettings(high);
			break;
		case Quality.Medium:
			applySettings(medium);
			break;
		default:
			applySettings(low);
			break;
		}
	}

	private void applySettings(QualityProfile settings)
	{
		if (settings.enableSoftParticles)
		{
			Camera.main.depthTextureMode = DepthTextureMode.Depth;
		}
		UnityEngine.QualitySettings.pixelLightCount = settings.pixelLightCount;
		UnityEngine.QualitySettings.anisotropicFiltering = settings.anisotropicFiltering;
		UnityEngine.QualitySettings.antiAliasing = settings.antiAliasing;
		UnityEngine.QualitySettings.shadowProjection = settings.shadowProjection;
		UnityEngine.QualitySettings.shadowCascades = settings.shadowCascades;
		UnityEngine.QualitySettings.shadowDistance = settings.shadowDistance;
		UnityEngine.QualitySettings.lodBias = settings.lodBias;
		UnityEngine.QualitySettings.maximumLODLevel = settings.maximumLODLevel;
	}
}
