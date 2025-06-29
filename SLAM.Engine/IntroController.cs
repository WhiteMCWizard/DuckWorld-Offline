using System.Collections.Generic;
using SLAM.Shared;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.Engine;

public class IntroController : ViewController
{
	[SerializeField]
	private BalloonType balloonType;

	[SerializeField]
	private GameObject balloonTarget;

	[SerializeField]
	private Camera balloonCamera;

	private SpeechBalloon balloon;

	private IEnumerable<Light> sceneLights;

	private IEnumerable<Camera> sceneCameras;

	public void Enable(BalloonView v, string introText)
	{
		base.gameObject.SetActive(value: true);
		sceneLights = from l in Object.FindObjectsOfType<Light>()
			where l != null && l.gameObject.layer != LayerMask.NameToLayer("GUI") && l.gameObject.layer != LayerMask.NameToLayer("PhotoBooth")
			select l;
		sceneCameras = from c in Object.FindObjectsOfType<Camera>()
			where c != null && c.gameObject.layer != LayerMask.NameToLayer("GUI") && c.gameObject.layer != LayerMask.NameToLayer("ErrorUI") && c.gameObject.layer != LayerMask.NameToLayer("PhotoBooth")
			select c;
		foreach (Light sceneLight in sceneLights)
		{
			sceneLight.enabled = (sceneLight.transform.IsChildOf(base.transform) ? true : false);
		}
		foreach (Camera sceneCamera in sceneCameras)
		{
			sceneCamera.enabled = (sceneCamera.transform.IsChildOf(base.transform) ? true : false);
		}
		balloon = v.CreateBalloon(balloonType);
		balloon.SetInfo(introText, balloonTarget, append: false, balloonCamera);
	}

	public void Disable()
	{
		Object.Destroy(balloon);
		foreach (Light sceneLight in sceneLights)
		{
			sceneLight.enabled = !sceneLight.transform.IsChildOf(base.transform);
		}
		foreach (Camera sceneCamera in sceneCameras)
		{
			sceneCamera.enabled = !sceneCamera.transform.IsChildOf(base.transform);
		}
		base.gameObject.SetActive(value: false);
	}
}
