using UnityEngine;

namespace SLAM.AssemblyLine;

public class ALAvatarController : MonoBehaviour
{
	[SerializeField]
	private Animator controlpanel;

	private Animator animator;

	private void Start()
	{
		animator = GetComponent<Animator>();
	}

	private void Update()
	{
	}

	private void OnEnable()
	{
		GameEvents.Subscribe<AssemblyLineGame.RobotCompletedEvent>(robotCompletedEvent);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<AssemblyLineGame.RobotCompletedEvent>(robotCompletedEvent);
	}

	private void robotCompletedEvent(AssemblyLineGame.RobotCompletedEvent evt)
	{
		animator.SetTrigger("PullHandle");
		controlpanel.SetTrigger("PullHandle");
	}
}
