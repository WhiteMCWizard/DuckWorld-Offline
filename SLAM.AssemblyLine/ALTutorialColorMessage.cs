using System.Collections;
using SLAM.Engine;
using SLAM.ToolTips;
using UnityEngine;

namespace SLAM.AssemblyLine;

public class ALTutorialColorMessage : TutorialView
{
	[SerializeField]
	private MessageToolTip messageTooltip;

	[SerializeField]
	private string welcomeTranslationKey = "Je kunt enkel onderdelen van dezelfde kleur in een buis plaatsen.";

	[SerializeField]
	private string secondTranslationKey = "Plaats dit onderdeel in de lege buis.";

	private new IEnumerator Start()
	{
		messageTooltip.ShowText(Localization.Get(welcomeTranslationKey));
		ALRobotPart part = null;
		ALRobotPart newPart = null;
		yield return CoroutineUtils.WaitForGameEvent(delegate(AssemblyLineGame.PartSpawnedEvent p)
		{
			part = p.Part;
		});
		yield return part.WaitForPartToBeVisible();
		bool moreDifferentColorSpawned = false;
		while (!moreDifferentColorSpawned)
		{
			yield return CoroutineUtils.WaitForGameEvent(delegate(AssemblyLineGame.PartSpawnedEvent p)
			{
				moreDifferentColorSpawned = p.Part.Kind != part.Kind;
				newPart = p.Part;
			});
		}
		yield return newPart.WaitForPartToBeVisible();
		messageTooltip.ShowText(Localization.Get(secondTranslationKey));
	}
}
