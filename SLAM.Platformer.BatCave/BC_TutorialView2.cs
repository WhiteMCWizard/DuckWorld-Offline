using System.Collections;
using SLAM.Engine;
using SLAM.ToolTips;
using UnityEngine;

namespace SLAM.Platformer.BatCave;

public class BC_TutorialView2 : TutorialView
{
	[SerializeField]
	private ToolTip switchToolTip;

	[SerializeField]
	private ToolTip pointerToolTip;

	[SerializeField]
	private Collider switchCollider;

	private CC2DPlayer avatar;

	protected override void Start()
	{
		base.Start();
		avatar = Object.FindObjectOfType<CC2DPlayer>();
		StartCoroutine(explainSwitches());
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
}
