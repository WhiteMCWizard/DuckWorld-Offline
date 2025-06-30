using UnityEngine;

namespace SLAM.Shared;

public class SpeechBalloon : MonoBehaviour
{
	private Camera UICamera;

	private Camera MainCamera;

	private UILabel speechLabel;

	private GameObject target;

	private void Awake()
	{
		speechLabel = GetComponentInChildren<UILabel>();
	}

	private void Start()
	{
		UICamera = GetComponentInParent<Camera>();
	}

	private void Update()
	{
		followTarget(target);
	}

	public void SetInfo(string text, GameObject target, bool append, Camera cam = null)
	{
		this.target = target;
		MainCamera = ((!(cam == null)) ? cam : Camera.main);
		speechLabel.text = ((!append) ? text : (speechLabel.text + "\n" + text).Trim());
	}

	private void followTarget(GameObject target)
	{
		if (MainCamera == null)
		{
			Debug.LogWarning("Hey Buddy, make sure the scene camera's are tagged as \"MainCamera\" and only one at a time is active.");
		}
		else if (UICamera != null && target != null)
		{
			base.transform.position = (Vector2)UICamera.ScreenToWorldPoint(MainCamera.WorldToScreenPoint(target.transform.position));
		}
	}
}
