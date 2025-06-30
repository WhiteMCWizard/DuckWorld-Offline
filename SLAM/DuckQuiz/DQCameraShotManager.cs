using System;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.DuckQuiz;

public class DQCameraShotManager : MonoBehaviour
{
	[Serializable]
	public class CameraShotSetting
	{
		[SerializeField]
		[Popup(new string[] { "Show_Category", "Show_Question", "Show_Answer", "Question_Correct", "Question_Incorrect", "Show_Bonusround" })]
		private string stateName;

		[SerializeField]
		private Camera[] cameras;

		public string StateName => stateName;

		public Camera[] Cameras => cameras;
	}

	[SerializeField]
	private CameraShotSetting[] settings;

	private Camera oldCamera;

	private float lastSwitchTime;

	public float TimeSinceLastCameraSwitch => Time.timeSinceLevelLoad - lastSwitchTime;

	private void OnEnable()
	{
		for (int i = 0; i < settings.Length; i++)
		{
			for (int j = 0; j < settings[i].Cameras.Length; j++)
			{
				settings[i].Cameras[j].gameObject.SetActive(value: false);
			}
		}
	}

	public void SwitchToCameraShot(string stateName)
	{
		CameraShotSetting cameraShotSetting = settings.FirstOrDefault((CameraShotSetting s) => s.StateName == stateName);
		if (cameraShotSetting == null || cameraShotSetting.Cameras.Length <= 0)
		{
			return;
		}
		Camera random = cameraShotSetting.Cameras.Where((Camera c) => !c.gameObject.activeInHierarchy).GetRandom();
		if (!(random == null))
		{
			if (oldCamera != null)
			{
				oldCamera.gameObject.SetActive(value: false);
			}
			random.gameObject.SetActive(value: true);
			oldCamera = random;
			lastSwitchTime = Time.timeSinceLevelLoad;
		}
	}
}
