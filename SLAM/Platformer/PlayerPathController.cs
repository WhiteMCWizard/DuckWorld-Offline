using System;
using SLAM.Utilities.BezierCurve;
using UnityEngine;

namespace SLAM.Platformer;

[RequireComponent(typeof(CharacterControllerPath))]
public class PlayerPathController : MonoBehaviour
{
	[SerializeField]
	protected BezierPath pathToFollow;

	[SerializeField]
	private float gravity = -25f;

	[SerializeField]
	private float runSpeed = 8f;

	[SerializeField]
	private float crouchSpeed = 4f;

	[SerializeField]
	private float airSpeed = 8f;

	[SerializeField]
	private float jumpHeight = 3f;

	[SerializeField]
	private float doubleJumpHeight = 1.5f;

	private float distanceOnPath;

	private int jumpCount;

	private float inputScale = -1f;

	private CharacterControllerPath controller;

	protected Animator animator;

	private Func<Vector3, Vector3> upAction;

	public bool AreControlsLocked { get; set; }

	public bool IsJumping => !controller.isGrounded;

	public void RegisterAction(Func<Vector3, Vector3> action)
	{
		upAction = action;
	}

	public void UnregisterAction()
	{
		upAction = doJumpAction;
	}

	public Vector3 GetPositionOnPath()
	{
		return pathToFollow.GetPositionByDistance(distanceOnPath);
	}

	private Vector3 doJumpAction(Vector3 velocity)
	{
		if (jumpCount == 0)
		{
			AudioController.Play("CTT_avatar_jump", base.transform);
		}
		else
		{
			AudioController.Play("Avatar_double_jump", base.transform);
		}
		float currentJumpHeight = getCurrentJumpHeight();
		if (currentJumpHeight > 0f)
		{
			velocity.y = currentJumpHeight;
		}
		jumpCount++;
		return velocity;
	}

	protected virtual void Start()
	{
		controller = GetComponentInChildren<CharacterControllerPath>();
		distanceOnPath = pathToFollow.GetDistanceByPosition(base.transform.position);
		applyPath(Vector3.zero);
		animator = GetComponent<Animator>();
		UnregisterAction();
	}

	protected virtual void Update()
	{
		Vector3 velocity = controller.velocity;
		if (controller.isGrounded)
		{
			velocity.y = 0f;
			jumpCount = 0;
		}
		applyInput(ref velocity);
		applyGravity(ref velocity);
		Vector3 rhs = base.transform.TransformDirection(new Vector3(velocity.x, 0f, velocity.z).normalized);
		float num = Vector3.Dot(base.transform.forward, rhs);
		if (num <= -0.9f)
		{
			base.transform.rotation = Quaternion.Euler(base.transform.eulerAngles.x, base.transform.eulerAngles.y - 180f, base.transform.eulerAngles.z);
			inputScale *= -1f;
			velocity = new Vector3(0f - velocity.x, velocity.y, 0f - velocity.z);
		}
		controller.move(velocity * Time.deltaTime);
		setAnimatorValues();
		applyPath(controller.velocity);
	}

	private void setAnimatorValues()
	{
		animator.SetFloat("horizontalVelocity", new Vector3(controller.velocity.x, 0f, controller.velocity.z).sqrMagnitude);
		animator.SetFloat("verticalVelocity", controller.velocity.y);
		animator.SetInteger("jumpCount", jumpCount);
		RaycastHit hitInfo;
		if (controller.isGrounded)
		{
			animator.SetFloat("groundDistance", 0f);
			animator.SetBool("isCrouching", !AreControlsLocked && SLAMInput.Provider.GetButton(SLAMInput.Button.Down));
		}
		else if (Physics.Raycast(base.transform.position, Vector3.down, out hitInfo, float.MaxValue))
		{
			animator.SetFloat("groundDistance", hitInfo.distance);
		}
	}

	private void applyPath(Vector3 velocity)
	{
		Debug.DrawLine(base.transform.position, pathToFollow.GetPositionByDistance(distanceOnPath), Color.blue);
		Vector3 vector = new Vector3(velocity.x, 0f, velocity.z);
		if (vector.sqrMagnitude > 0f)
		{
			distanceOnPath += vector.magnitude * Time.deltaTime * inputScale;
			Vector3 positionOnPath = GetPositionOnPath();
			positionOnPath.y = base.transform.position.y;
			Vector3 vector2 = positionOnPath - base.transform.position;
			base.transform.rotation = Quaternion.LookRotation(vector2.normalized, Vector3.up);
		}
	}

	private void applyGravity(ref Vector3 velocity)
	{
		velocity.y += gravity * Time.deltaTime;
	}

	protected void applyInput(ref Vector3 velocity)
	{
		Vector3 movementInput = getMovementInput();
		velocity.z = movementInput.z * getCurrentMovementSpeed();
		if (movementInput.y > 0f)
		{
			velocity = upAction(velocity);
		}
	}

	private float getCurrentJumpHeight()
	{
		if (controller.isGrounded)
		{
			return Mathf.Sqrt(2f * jumpHeight * (0f - gravity));
		}
		if (jumpCount == 1)
		{
			return Mathf.Sqrt(2f * doubleJumpHeight * (0f - gravity));
		}
		return 0f;
	}

	private float getCurrentMovementSpeed()
	{
		if (!controller.isGrounded)
		{
			return airSpeed;
		}
		if (SLAMInput.Provider.GetButton(SLAMInput.Button.Down))
		{
			return crouchSpeed;
		}
		return runSpeed;
	}

	protected Vector3 getMovementInput()
	{
		Vector3 zero = Vector3.zero;
		if (!AreControlsLocked)
		{
			if (SLAMInput.Provider.GetButton(SLAMInput.Button.Right))
			{
				zero += Vector3.back * inputScale;
			}
			if (SLAMInput.Provider.GetButton(SLAMInput.Button.Left))
			{
				zero += Vector3.forward * inputScale;
			}
			if (SLAMInput.Provider.GetButtonDown(SLAMInput.Button.UpOrAction))
			{
				zero += Vector3.up;
			}
			if (SLAMInput.Provider.GetButton(SLAMInput.Button.Down))
			{
				zero += Vector3.down;
			}
		}
		return zero.normalized;
	}
}
