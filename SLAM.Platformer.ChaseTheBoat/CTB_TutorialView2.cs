using System.Collections;
using SLAM.Engine;
using SLAM.ToolTips;
using UnityEngine;

namespace SLAM.Platformer.ChaseTheBoat;

public class CTB_TutorialView2 : TutorialView
{
	[SerializeField]
	private ToolTip ladderToolTip;

	[SerializeField]
	private ToolTip pointerToolTip;

	[SerializeField]
	private Collider ladderCollider;

	private CC2DPlayer avatar;

	protected override void Start()
	{
		base.Start();
		avatar = Object.FindObjectOfType<CC2DPlayer>();
		StartCoroutine(explainLadders());
	}

	private IEnumerator explainLadders()
	{
		ladderCollider.enabled = true;
		while (!ladderCollider.bounds.Contains(avatar.transform.position))
		{
			yield return null;
		}
		pointerToolTip.Show(ladderToolTip.TargetGO.transform);
		ladderToolTip.Show(pointerToolTip.GO.transform);
		while (ladderCollider.bounds.Contains(avatar.transform.position))
		{
			yield return null;
		}
		pointerToolTip.Hide();
		ladderToolTip.Hide();
	}
}
