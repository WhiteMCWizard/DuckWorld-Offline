using System.Collections;
using SLAM.Engine;
using SLAM.ToolTips;
using UnityEngine;

namespace SLAM.Platformer.BatCave;

public class BC_TutorialView4 : TutorialView
{
	[SerializeField]
	private ToolTip batToolTip;

	[SerializeField]
	private Collider batCollider;

	private CC2DPlayer avatar;

	protected override void Start()
	{
		base.Start();
		avatar = Object.FindObjectOfType<CC2DPlayer>();
		StartCoroutine(explainScorpions());
	}

	private IEnumerator explainScorpions()
	{
		batCollider.enabled = true;
		while (!batCollider.bounds.Contains(avatar.transform.position))
		{
			yield return null;
		}
		batToolTip.Show();
		while (batCollider.bounds.Contains(avatar.transform.position))
		{
			yield return null;
		}
		batToolTip.Hide();
	}
}
