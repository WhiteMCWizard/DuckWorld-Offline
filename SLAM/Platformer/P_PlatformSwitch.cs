using UnityEngine;

namespace SLAM.Platformer;

public class P_PlatformSwitch : P_Toggle
{
	[SerializeField]
	private MovingPlatform platform;

	protected override void Start()
	{
		base.gameObject.layer = 2;
		GetComponent<Collider>().isTrigger = true;
		animator = GetComponent<Animator>();
	}

	protected override void OnTogglePressed(bool state)
	{
		platform.Toggle(state);
	}
}
