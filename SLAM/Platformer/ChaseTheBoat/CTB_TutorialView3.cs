using System.Collections;
using SLAM.Engine;
using SLAM.ToolTips;
using UnityEngine;

namespace SLAM.Platformer.ChaseTheBoat;

public class CTB_TutorialView3 : TutorialView
{
	[SerializeField]
	private ToolTip switchToolTip;

	[SerializeField]
	private ToolTip doorToolTip;

	[SerializeField]
	private ToolTip pointerToolTip;

	[SerializeField]
	private Collider switchCollider;

	[SerializeField]
	private Collider doorCollider;

	private CC2DPlayer avatar;

	protected override void Start()
	{
		base.Start();
		avatar = Object.FindObjectOfType<CC2DPlayer>();
		StartCoroutine(explainSwitches());
		StartCoroutine(explainDoors());
	}

	private IEnumerator explainSwitches()
	{
		switchCollider.enabled = true;
		while (!switchCollider.bounds.Contains(avatar.transform.position))
		{
			yield return null;
		}
		pointerToolTip.Show(switchToolTip.TargetGO.transform);
		switchToolTip.Show(pointerToolTip.GO.transform);
		while (switchCollider.bounds.Contains(avatar.transform.position))
		{
			yield return null;
		}
		pointerToolTip.Hide();
		switchToolTip.Hide();
	}

	private IEnumerator explainDoors()
	{
		doorCollider.enabled = true;
		while (!doorCollider.bounds.Contains(avatar.transform.position))
		{
			yield return null;
		}
		pointerToolTip.Show(doorToolTip.TargetGO.transform);
		doorToolTip.Show(pointerToolTip.GO.transform);
		while (doorCollider.bounds.Contains(avatar.transform.position))
		{
			yield return null;
		}
		pointerToolTip.Hide();
		doorToolTip.Hide();
	}
}
