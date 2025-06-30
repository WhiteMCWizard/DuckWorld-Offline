using UnityEngine;

namespace SLAM.Avatar;

public class AvatarFootsteps : MonoBehaviour
{
	[SerializeField]
	private string audioId;

	private void OnFootstep()
	{
		if ((bool)SingletonMonoBehaviour<AudioController>.DoesInstanceExist() && !string.IsNullOrEmpty(audioId))
		{
			AudioController.Play(audioId, base.transform);
		}
	}
}
