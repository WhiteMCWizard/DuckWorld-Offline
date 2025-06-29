using SLAM.CameraSystem;
using UnityEngine;

namespace SLAM.ZooTransport;

public class ZTcameraManager : MonoBehaviour
{
	[SerializeField]
	private CameraBehaviour[] behaviours;

	private CameraManager manager;

	public CameraBehaviour[] Behaviours => behaviours;

	private void Start()
	{
		manager = GetComponent<CameraManager>();
	}

	public void CrossFade(int behaviour)
	{
		behaviours[behaviour].enabled = true;
		manager.CrossFade(behaviours[behaviour], 0f);
	}

	public void WarpTo()
	{
		((FollowObjectBehaviour)manager.CurrentBehaviour(1)).WarpTo();
	}
}
