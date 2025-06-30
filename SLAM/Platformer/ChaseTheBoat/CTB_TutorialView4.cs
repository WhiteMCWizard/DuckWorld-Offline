using System.Collections;
using SLAM.Engine;
using SLAM.ToolTips;
using UnityEngine;

namespace SLAM.Platformer.ChaseTheBoat;

public class CTB_TutorialView4 : TutorialView
{
	[SerializeField]
	private ToolTip sewerToolTip;

	[SerializeField]
	private ToolTip sewerArrowToolTip;

	[SerializeField]
	private Collider sewerCollider;

	private CC2DPlayer avatar;

	protected override void Start()
	{
		base.Start();
		avatar = Object.FindObjectOfType<CC2DPlayer>();
		StartCoroutine(explainSewers());
	}

	private IEnumerator explainSewers()
	{
		sewerCollider.enabled = true;
		while (!sewerCollider.bounds.Contains(avatar.transform.position))
		{
			yield return null;
		}
		sewerToolTip.Show();
		sewerArrowToolTip.Show();
		while (!SLAMInput.Provider.GetButtonDown(SLAMInput.Button.UpOrAction))
		{
			yield return null;
		}
		sewerToolTip.Hide();
		sewerArrowToolTip.Hide();
	}
}
