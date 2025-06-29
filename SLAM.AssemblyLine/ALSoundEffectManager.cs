using UnityEngine;

namespace SLAM.AssemblyLine;

public class ALSoundEffectManager : MonoBehaviour
{
	[SerializeField]
	private AudioClip partDropInTrashClip;

	[SerializeField]
	private AudioClip partPickupClip;

	[SerializeField]
	private AudioClip partPutDownClip;

	[SerializeField]
	private AudioClip partReleasedClip;

	[SerializeField]
	private AudioClip partHoverClip;

	private void OnEnable()
	{
		GameEvents.Subscribe<AssemblyLineGame.LifeLostEvent>(onLifeLost);
		GameEvents.Subscribe<AssemblyLineGame.PartDroppedEvent>(onPartDropped);
		GameEvents.Subscribe<AssemblyLineGame.RobotCompletedEvent>(onRobotCompleted);
		GameEvents.Subscribe<AssemblyLineGame.PartPickedUpEvent>(onPartPickedUp);
		GameEvents.Subscribe<AssemblyLineGame.PartReleasedEvent>(onPartReleased);
		GameEvents.Subscribe<AssemblyLineGame.PartHoverEvent>(onPartHover);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<AssemblyLineGame.LifeLostEvent>(onLifeLost);
		GameEvents.Unsubscribe<AssemblyLineGame.PartDroppedEvent>(onPartDropped);
		GameEvents.Unsubscribe<AssemblyLineGame.RobotCompletedEvent>(onRobotCompleted);
		GameEvents.Unsubscribe<AssemblyLineGame.PartPickedUpEvent>(onPartPickedUp);
		GameEvents.Unsubscribe<AssemblyLineGame.PartReleasedEvent>(onPartReleased);
		GameEvents.Unsubscribe<AssemblyLineGame.PartHoverEvent>(onPartHover);
	}

	private void onPartPickedUp(AssemblyLineGame.PartPickedUpEvent obj)
	{
		AudioController.Play(partPickupClip.name);
	}

	private void onRobotCompleted(AssemblyLineGame.RobotCompletedEvent evt)
	{
		AudioController.Play("AL_robot_complete");
		AudioController.Play("AL_pull_lever");
		AudioController.Play("AL_robot_complete_points");
	}

	private void onPartHover(AssemblyLineGame.PartHoverEvent evt)
	{
		AudioController.Play(partHoverClip.name);
	}

	private void onPartDropped(AssemblyLineGame.PartDroppedEvent evt)
	{
		AudioController.Play(partPutDownClip.name);
	}

	private void onLifeLost(AssemblyLineGame.LifeLostEvent evt)
	{
		AudioController.Play(partDropInTrashClip.name);
	}

	private void onPartReleased(AssemblyLineGame.PartReleasedEvent evt)
	{
		AudioController.Play(partReleasedClip.name);
	}
}
