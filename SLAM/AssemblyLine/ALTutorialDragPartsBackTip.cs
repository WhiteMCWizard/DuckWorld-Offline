using System.Collections;
using SLAM.Engine;
using SLAM.ToolTips;
using UnityEngine;

namespace SLAM.AssemblyLine;

public class ALTutorialDragPartsBackTip : TutorialView
{
	[SerializeField]
	private MessageToolTip messageToolTip;

	[SerializeField]
	private ToolTip partToolTip;

	[SerializeField]
	private string localizationKey;

	private int displayCount;

	private void OnEnable()
	{
		GameEvents.Subscribe<AssemblyLineGame.PartSpawnedEvent>(onPartSpawned);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<AssemblyLineGame.PartSpawnedEvent>(onPartSpawned);
	}

	private void onPartSpawned(AssemblyLineGame.PartSpawnedEvent obj)
	{
		if (displayCount < 5)
		{
			StartCoroutine(monitorPart(obj.Part));
		}
	}

	private IEnumerator monitorPart(ALRobotPart part)
	{
		while (part != null)
		{
			if (!part.IsDragging && (double)part.Timer.Progress > 0.7)
			{
				messageToolTip.ShowText(Localization.Get(localizationKey));
				displayCount++;
				partToolTip.Hide();
				partToolTip.Show(part.transform, part.GetComponentInChildren<Renderer>().bounds.center - part.transform.position);
				yield return CoroutineUtils.WaitForGameEvent<AssemblyLineGame.PartPickedUpEvent>();
				partToolTip.Hide();
				messageToolTip.Hide();
				break;
			}
			yield return null;
		}
	}
}
