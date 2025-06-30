using System.Collections;
using SLAM.Engine;
using SLAM.ToolTips;
using UnityEngine;

namespace SLAM.Platformer.BatCave;

public class BC_TutorialView3 : TutorialView
{
	[SerializeField]
	private ToolTip scorpionToolTip;

	[SerializeField]
	private Collider scorpionCollider;

	private CC2DPlayer avatar;

	protected override void Start()
	{
		base.Start();
		avatar = Object.FindObjectOfType<CC2DPlayer>();
		StartCoroutine(explainScorpions());
	}

	private IEnumerator explainScorpions()
	{
		scorpionCollider.enabled = true;
		while (!scorpionCollider.bounds.Contains(avatar.transform.position))
		{
			yield return null;
		}
		scorpionToolTip.Show();
		while (scorpionCollider.bounds.Contains(avatar.transform.position))
		{
			yield return null;
		}
		scorpionToolTip.Hide();
	}
}
