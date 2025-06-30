using System.Collections;
using SLAM.Engine;
using SLAM.ToolTips;
using UnityEngine;

namespace SLAM.AssemblyLine;

public class ALTutorialDragDrop : TutorialView
{
	[SerializeField]
	private ALConveyorBelt belt;

	[SerializeField]
	private ToolTip clickOnPartTooltip;

	[SerializeField]
	private ToolTip clickOnBeamToolTip;

	private int robotsBuild;

	private ALRobotPart availablePart;

	private ALRobotPart pickedUpPart;

	private void OnEnable()
	{
		GameEvents.Subscribe<AssemblyLineGame.RobotCompletedEvent>(onRobotCompleted);
		GameEvents.Subscribe<AssemblyLineGame.PartSpawnedEvent>(onPartSpawned);
		GameEvents.Subscribe<AssemblyLineGame.PartPickedUpEvent>(onPartPickedUp);
		GameEvents.Subscribe<AssemblyLineGame.PartReleasedEvent>(onPartReleased);
		GameEvents.Subscribe<AssemblyLineGame.PartDroppedEvent>(onPartDropped);
		GameEvents.Subscribe<AssemblyLineGame.LifeLostEvent>(onPartLost);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<AssemblyLineGame.RobotCompletedEvent>(onRobotCompleted);
		GameEvents.Unsubscribe<AssemblyLineGame.PartSpawnedEvent>(onPartSpawned);
		GameEvents.Unsubscribe<AssemblyLineGame.PartPickedUpEvent>(onPartPickedUp);
		GameEvents.Unsubscribe<AssemblyLineGame.PartReleasedEvent>(onPartReleased);
		GameEvents.Unsubscribe<AssemblyLineGame.PartDroppedEvent>(onPartDropped);
		GameEvents.Unsubscribe<AssemblyLineGame.LifeLostEvent>(onPartLost);
	}

	protected override void Start()
	{
		base.Start();
		StartCoroutine(doExplainCycle());
	}

	private void onRobotCompleted(AssemblyLineGame.RobotCompletedEvent evt)
	{
		robotsBuild++;
	}

	private void onPartSpawned(AssemblyLineGame.PartSpawnedEvent evt)
	{
		availablePart = evt.Part;
	}

	private void onPartPickedUp(AssemblyLineGame.PartPickedUpEvent evt)
	{
		pickedUpPart = evt.Part;
	}

	private void onPartReleased(AssemblyLineGame.PartReleasedEvent evt)
	{
		pickedUpPart = null;
	}

	private void onPartDropped(AssemblyLineGame.PartDroppedEvent evt)
	{
		clickOnBeamToolTip.Hide();
		pickedUpPart = null;
		availablePart = null;
	}

	private void onPartLost(AssemblyLineGame.LifeLostEvent evt)
	{
		pickedUpPart = null;
		availablePart = null;
	}

	private IEnumerator doExplainCycle()
	{
		while (robotsBuild < 1)
		{
			if (availablePart != null)
			{
				belt.PauseSpawningParts();
				if (pickedUpPart == null)
				{
					yield return StartCoroutine(doExplainAvailablePart());
				}
				else if (availablePart == pickedUpPart)
				{
					yield return StartCoroutine(doExplainAvailableBeam());
				}
			}
			else
			{
				belt.ResumeSpawningParts();
			}
			yield return null;
		}
	}

	private IEnumerator doExplainAvailablePart()
	{
		clickOnBeamToolTip.Hide();
		yield return availablePart.WaitForPartToBeVisible();
		if (pickedUpPart == null && availablePart != null)
		{
			clickOnPartTooltip.Show(availablePart.transform, availablePart.GetComponentInChildren<Renderer>().bounds.center - availablePart.transform.position);
		}
		while (pickedUpPart == null && availablePart != null)
		{
			yield return null;
		}
	}

	private IEnumerator doExplainAvailableBeam()
	{
		clickOnPartTooltip.Hide();
		clickOnBeamToolTip.Hide();
		ALDropZone beam = Object.FindObjectOfType<ALDropZone>();
		clickOnBeamToolTip.Show(beam.transform);
		while (availablePart != null && availablePart == pickedUpPart)
		{
			yield return null;
		}
	}
}
