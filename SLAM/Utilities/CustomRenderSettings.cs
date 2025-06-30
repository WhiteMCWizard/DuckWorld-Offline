using UnityEngine;

namespace SLAM.Utilities;

public class CustomRenderSettings : MonoBehaviour
{
	public RenderSettingsProfile customSettings;

	private void OnEnable()
	{
		RenderSettingsProfile.Apply(customSettings);
	}

	private void OnDisable()
	{
		RenderSettingsProfile.Restore();
	}
}
