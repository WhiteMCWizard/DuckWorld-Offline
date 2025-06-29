using System.Collections;
using SLAM.Engine;
using SLAM.ToolTips;
using UnityEngine;

namespace SLAM.Platformer.ChaseTheBoat;

public class CTB_TutorialView1 : TutorialView
{
	[SerializeField]
	private ToolTip moveForwardToolTip;

	[SerializeField]
	private ToolTip jumpToolTip;

	[SerializeField]
	private ToolTip jumpArrowToolTip;

	[SerializeField]
	private ToolTip crouchToolTip;

	[SerializeField]
	private ToolTip crouchArrowToolTip;

	[SerializeField]
	private ToolTip doubleJumpToolTip;

	[SerializeField]
	private ToolTip pointerToolTip;

	[SerializeField]
	private ToolTip featherToolTip;

	[SerializeField]
	private ToolTip floorboardToolTip;

	[SerializeField]
	private Collider jumpCollider;

	[SerializeField]
	private Collider crouchCollider;

	[SerializeField]
	private Collider featherCollider;

	[SerializeField]
	private Collider floorboardCollider;

	[SerializeField]
	private Collider doubleJumpCollider;

	private CC2DPlayer avatar;

	protected override void Start()
	{
		base.Start();
		avatar = Object.FindObjectOfType<CC2DPlayer>();
		StartCoroutine(explainMoving());
		StartCoroutine(explainFeathers());
		StartCoroutine(explainJumping());
		StartCoroutine(explainCrouching());
		StartCoroutine(explainDoubleJumping());
		StartCoroutine(explainFloorboards());
	}

	private IEnumerator explainMoving()
	{
		while (0.9f > Vector3.Dot(Camera.main.transform.forward.normalized, (avatar.transform.position - Camera.main.transform.position).normalized))
		{
			yield return null;
		}
		yield return new WaitForEndOfFrame();
		pointerToolTip.Show(avatar.transform);
		moveForwardToolTip.Show(pointerToolTip.GO.transform);
		while (!SLAMInput.Provider.GetButton(SLAMInput.Button.Right))
		{
			yield return null;
		}
		pointerToolTip.Hide();
		moveForwardToolTip.Hide();
	}

	private IEnumerator explainFeathers()
	{
		featherCollider.enabled = true;
		while (!featherCollider.bounds.Contains(avatar.transform.position))
		{
			yield return null;
		}
		featherToolTip.Show();
		while (featherCollider.bounds.Contains(avatar.transform.position))
		{
			yield return null;
		}
		featherToolTip.Hide();
	}

	private IEnumerator explainJumping()
	{
		jumpCollider.enabled = true;
		while (!jumpCollider.bounds.Contains(avatar.transform.position))
		{
			yield return null;
		}
		jumpToolTip.Show();
		jumpArrowToolTip.Show();
		while (!SLAMInput.Provider.GetButtonDown(SLAMInput.Button.UpOrAction))
		{
			yield return null;
		}
		jumpToolTip.Hide();
		jumpArrowToolTip.Hide();
	}

	private IEnumerator explainCrouching()
	{
		crouchCollider.enabled = true;
		while (!crouchCollider.bounds.Contains(avatar.transform.position))
		{
			yield return null;
		}
		crouchToolTip.Show();
		crouchArrowToolTip.Show();
		while (!SLAMInput.Provider.GetButtonDown(SLAMInput.Button.Down))
		{
			yield return null;
		}
		crouchToolTip.Hide();
		crouchArrowToolTip.Hide();
	}

	private IEnumerator explainDoubleJumping()
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

	private IEnumerator explainFloorboards()
	{
		floorboardCollider.enabled = true;
		while (!floorboardCollider.bounds.Contains(avatar.transform.position))
		{
			yield return null;
		}
		floorboardToolTip.Show();
		while (floorboardCollider.bounds.Contains(avatar.transform.position))
		{
			yield return null;
		}
		floorboardToolTip.Hide();
	}
}
