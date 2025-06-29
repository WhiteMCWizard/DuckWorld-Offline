using System.Collections;
using SLAM.Engine;
using SLAM.ToolTips;
using UnityEngine;

namespace SLAM.Platformer.BatCave;

public class BC_TutorialView1 : TutorialView
{
	[SerializeField]
	private ToolTip moveForwardToolTip;

	[SerializeField]
	private ToolTip pointerToolTip;

	[SerializeField]
	private ToolTip creatureToolTip;

	[SerializeField]
	private ToolTip ladderToolTip;

	[SerializeField]
	private ToolTip spiderToolTip;

	[SerializeField]
	private ToolTip doubleJumpToolTip;

	[SerializeField]
	private ToolTip infoboard1ToolTip;

	[SerializeField]
	private ToolTip infoboard2ToolTip;

	[SerializeField]
	private Collider creatureCollider;

	[SerializeField]
	private Collider ladderCollider;

	[SerializeField]
	private Collider spiderCollider;

	[SerializeField]
	private Collider doubleJumpCollider;

	[SerializeField]
	private Collider infoboard1Collider;

	[SerializeField]
	private Collider infoboard2Collider;

	private CC2DPlayer avatar;

	protected override void Start()
	{
		base.Start();
		avatar = Object.FindObjectOfType<CC2DPlayer>();
		StartCoroutine(explainMoving());
		StartCoroutine(explainCreatures());
		StartCoroutine(explainSpiders());
		StartCoroutine(explainLadders());
		StartCoroutine(explainDoubleJump());
		StartCoroutine(explainInfoboard1());
		StartCoroutine(explainInfoboard2());
	}

	private IEnumerator explainMoving()
	{
		while (0.9f > Vector3.Dot(Camera.main.transform.forward.normalized, (avatar.transform.position - Camera.main.transform.position).normalized))
		{
			yield return null;
		}
		pointerToolTip.Show(avatar.transform);
		moveForwardToolTip.Show(pointerToolTip.GO.transform);
		while (!SLAMInput.Provider.GetButton(SLAMInput.Button.Right))
		{
			yield return null;
		}
		pointerToolTip.Hide();
		moveForwardToolTip.Hide();
	}

	private IEnumerator explainDoubleJump()
	{
		doubleJumpCollider.enabled = true;
		while (!doubleJumpCollider.bounds.Contains(avatar.transform.position))
		{
			yield return null;
		}
		doubleJumpToolTip.Show();
		while (doubleJumpCollider.bounds.Contains(avatar.transform.position))
		{
			yield return null;
		}
		doubleJumpToolTip.Hide();
	}

	private IEnumerator explainInfoboard1()
	{
		infoboard1Collider.enabled = true;
		while (!infoboard1Collider.bounds.Contains(avatar.transform.position))
		{
			yield return null;
		}
		infoboard1ToolTip.Show();
		while (infoboard1Collider.bounds.Contains(avatar.transform.position))
		{
			yield return null;
		}
		infoboard1ToolTip.Hide();
	}

	private IEnumerator explainInfoboard2()
	{
		infoboard2Collider.enabled = true;
		while (!infoboard2Collider.bounds.Contains(avatar.transform.position))
		{
			yield return null;
		}
		infoboard2ToolTip.Show();
		while (infoboard2Collider.bounds.Contains(avatar.transform.position))
		{
			yield return null;
		}
		infoboard2ToolTip.Hide();
	}

	private IEnumerator explainCreatures()
	{
		creatureCollider.enabled = true;
		while (!creatureCollider.bounds.Contains(avatar.transform.position))
		{
			yield return null;
		}
		creatureToolTip.Show();
		while (creatureCollider.bounds.Contains(avatar.transform.position))
		{
			yield return null;
		}
		creatureToolTip.Hide();
	}

	private IEnumerator explainSpiders()
	{
		spiderCollider.enabled = true;
		while (!spiderCollider.bounds.Contains(avatar.transform.position))
		{
			yield return null;
		}
		spiderToolTip.Show();
		while (spiderCollider.bounds.Contains(avatar.transform.position))
		{
			yield return null;
		}
		spiderToolTip.Hide();
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
