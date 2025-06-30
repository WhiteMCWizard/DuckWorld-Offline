using UnityEngine;

namespace SLAM.Shared;

public class SpeechBalloonTargetMarker : MonoBehaviour
{
	[SerializeField]
	private GameObject sceneTarget;

	[SerializeField]
	private bool copyRotation;

	private void Start()
	{
	}

	private void Update()
	{
		if (sceneTarget != null)
		{
			base.gameObject.transform.position = sceneTarget.transform.position;
			if (copyRotation)
			{
				base.gameObject.transform.rotation = sceneTarget.transform.rotation;
			}
		}
	}
}
