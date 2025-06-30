using UnityEngine;

namespace SLAM.AssemblyLine;

[RequireComponent(typeof(Animator))]
public class ALGyroAnimation : MonoBehaviour
{
	private Animator animator;

	private void OnEnable()
	{
		GameEvents.Subscribe<AssemblyLineGame.LifeLostEvent>(onLifeLost);
		GameEvents.Subscribe<AssemblyLineGame.RobotCompletedEvent>(onRobotCompleted);
		animator = GetComponent<Animator>();
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<AssemblyLineGame.LifeLostEvent>(onLifeLost);
		GameEvents.Unsubscribe<AssemblyLineGame.RobotCompletedEvent>(onRobotCompleted);
	}

	private void onLifeLost(AssemblyLineGame.LifeLostEvent evt)
	{
		animator.SetTrigger("LifeLost");
	}

	private void onRobotCompleted(AssemblyLineGame.RobotCompletedEvent evt)
	{
		animator.SetTrigger("RobotCompleted");
	}
}
