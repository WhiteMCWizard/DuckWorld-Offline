using UnityEngine;

namespace SLAM.Utilities;

public class CameraSpecificRenderSettings : MonoBehaviour
{
	[SerializeField]
	private RenderSettingsProfile customSettings;

	private void OnPreRender()
	{
		RenderSettingsProfile.Apply(customSettings);
	}

	private void OnPostRender()
	{
		RenderSettingsProfile.Restore();
	}

	[ContextMenu("Copy from scene")]
	private void copyFromScene()
	{
		RenderSettingsProfile.Copy(RenderSettingsProfile.Current, customSettings);
	}
}
