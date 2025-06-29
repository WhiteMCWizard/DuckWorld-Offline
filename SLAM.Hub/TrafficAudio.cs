using UnityEngine;

namespace SLAM.Hub;

public class TrafficAudio : MonoBehaviour
{
	[SerializeField]
	private string clipName;

	private void Start()
	{
		if ((bool)SingletonMonoBehaviour<AudioController>.DoesInstanceExist())
		{
			AudioController.Play(clipName, base.transform);
		}
	}
}
