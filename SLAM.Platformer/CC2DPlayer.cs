using System.Collections;
using SLAM.Avatar;
using SLAM.Engine;
using UnityEngine;

namespace SLAM.Platformer;

public class CC2DPlayer : FiniteStateMachine
{
	public delegate UpAction ActionKeyPressed();

	private const string IDLE = "Idle";

	private const string WALKING = "Walking";

	private const string JUMPING = "Jumping";

	private const string Falling = "Falling";

	private const string CLIMBING = "Climbing";

	private const string CROUCHING = "Crouching";

	private const string SLOPE_SLIDE = "Slope Slide";

	private const string DROWN = "Drown";

	private const string COLLAPSE = "Collapse";

	public float gravity = -25f;

	public float runSpeed = 8f;

	public float crouchSpeed = 4f;

	public float climbSpeed = 1.1f;

	public float groundDamping = 20f;

	public float inAirDamping = 5f;

	public float ladderDamping = 40f;

	public float jumpHeight = 3f;

	public float doubleJumpHeight = 1.5f;

	public float terminalVelocity = -3f;

	public float shortJumpVelocity = 3f;

	public float slopeRaycastLength = 0.3f;

	public float slopeSlideSpeed = 3f;

	public ActionKeyPressed OnActionKeyPressed;

	private Vector3 pausedVelocity = Vector3.zero;

	public bool AreControlsLocked;

	[SerializeField]
	private UpAction currentAction;

	[SerializeField]
	private float standHeight = 1.3f;

	[SerializeField]
	private float crouchHeight = 0.7f;

	[SerializeField]
	private float prepareForLandingHeight = 0.7f;

	private float normalizedHorizontalSpeed;

	private float normalizedVerticalSpeed;

	private GameObject avatar;

	private CharacterController2D _controller;

	private Animator _animator;

	[SerializeField]
	private Vector3 _velocity;

	private int jumpCount;

	private bool impulseJump;

	private Vector3 vImpulseJump = Vector3.zero;

	private bool isGroundedDelayed;

	private int JumpCount
	{
		get
		{
			return jumpCount;
		}
		set
		{
			if (jumpCount != value)
			{
				jumpCount = value;
				_animator.SetInteger("JumpCount", jumpCount);
			}
		}
	}

	private bool PrepareLanding
	{
		set
		{
			_animator.SetBool("PrepareLanding", value);
		}
	}

	private void Awake()
	{
		_animator = GetComponent<Animator>();
		_animator.Rebind();
		_controller = GetComponent<CharacterController2D>();
		AvatarSpawn component = GetComponent<AvatarSpawn>();
		if (component != null)
		{
			avatar = component.SpawnAvatar();
		}
		else
		{
			Debug.LogError("Cannot spawn avatar as the AvatarSpawn component is missing on " + base.name);
		}
		AddState("Idle", EnterIdle, UpdateIdle, ExitIdle);
		AddState("Walking", EnterWalking, UpdateWalking, ExitWalking);
		AddState("Jumping", EnterJumping, UpdateJumping, ExitJumping);
		AddState("Falling", EnterFalling, UpdateFalling, ExitFalling);
		AddState("Slope Slide", EnterSlopeSlide, UpdateSlopeSlide, ExitSlopeSlide);
		AddState("Climbing", EnterClimbing, UpdateClimbing, ExitClimbing);
		AddState("Crouching", EnterCrouching, UpdateCrouching, ExitCrouching);
		AddState("Drown", EnterDrown, UpdateDrown, ExitDrown);
		AddState("Collapse", EnterCollapse, UpdateCollapse, ExitCollapse);
		SwitchTo("Idle");
	}

	private void OnDestroy()
	{
		OnActionKeyPressed = null;
	}

	protected override void Update()
	{
		_velocity = _controller.velocity;
		if (_controller.isGrounded)
		{
			_velocity.y = 0f;
		}
		if (impulseJump)
		{
			impulseJump = false;
			vImpulseJump.y = Mathf.Sqrt(2f * vImpulseJump.y * (0f - gravity));
			_velocity = vImpulseJump;
			JumpCount = 1;
			SwitchTo("Jumping");
		}
		CheckStateSwitch();
		base.Update();
		_controller.move(_velocity * Time.deltaTime);
	}

	private void CheckStateSwitch()
	{
		if (!AreControlsLocked)
		{
			switch (base.CurrentState.Name)
			{
			case "Idle":
				if (!ShouldPrepareForLanding())
				{
					SwitchTo("Falling");
				}
				if ((SLAMInput.Provider.GetButton(SLAMInput.Button.Left) && CanWalkToLeft()) || (SLAMInput.Provider.GetButton(SLAMInput.Button.Right) && CanWalkToRight()))
				{
					SwitchTo("Walking");
				}
				if (SLAMInput.Provider.GetButton(SLAMInput.Button.Down))
				{
					SwitchTo("Crouching");
				}
				currentAction = UpAction.NoAction;
				if (SLAMInput.Provider.GetButtonDown(SLAMInput.Button.UpOrAction))
				{
					if (OnActionKeyPressed != null)
					{
						currentAction = OnActionKeyPressed();
					}
					if (currentAction.Type == Action.None)
					{
						SwitchTo("Jumping");
					}
					else if (currentAction.Type == Action.PressSwitch)
					{
						PressSwitch();
					}
					else if (currentAction.Type == Action.Climbing)
					{
						SwitchTo("Climbing");
					}
					else if (currentAction.Type == Action.EnterDoor)
					{
						StartCoroutine(LockControls(currentAction.Duration));
					}
				}
				if (isGroundedDelayed && IsOnTooSteepSlope())
				{
					SwitchTo("Slope Slide");
				}
				break;
			case "Walking":
				if (!SLAMInput.Provider.GetButton(SLAMInput.Button.Left) && !SLAMInput.Provider.GetButton(SLAMInput.Button.Right))
				{
					SwitchTo("Idle");
				}
				if (SLAMInput.Provider.GetButton(SLAMInput.Button.Left) && !CanWalkToLeft())
				{
					SwitchTo("Idle");
				}
				if (SLAMInput.Provider.GetButton(SLAMInput.Button.Right) && !CanWalkToRight())
				{
					SwitchTo("Idle");
				}
				if (SLAMInput.Provider.GetButtonDown(SLAMInput.Button.UpOrAction))
				{
					SwitchTo("Jumping");
				}
				if (SLAMInput.Provider.GetButton(SLAMInput.Button.Down))
				{
					SwitchTo("Crouching");
				}
				if (!ShouldPrepareForLanding())
				{
					SwitchTo("Falling");
				}
				if (isGroundedDelayed && IsOnTooSteepSlope())
				{
					SwitchTo("Slope Slide");
				}
				break;
			case "Jumping":
				if ((_velocity.y < -0.01f || _controller.isGrounded) && !SLAMInput.Provider.GetButtonDown(SLAMInput.Button.UpOrAction))
				{
					SwitchTo("Falling");
				}
				break;
			case "Falling":
				if (SLAMInput.Provider.GetButtonDown(SLAMInput.Button.UpOrAction))
				{
					SwitchTo("Jumping");
				}
				if (isGroundedDelayed)
				{
					SwitchTo("Idle");
				}
				if (isGroundedDelayed && (SLAMInput.Provider.GetButton(SLAMInput.Button.Left) || SLAMInput.Provider.GetButton(SLAMInput.Button.Right)))
				{
					SwitchTo("Walking");
				}
				break;
			case "Slope Slide":
				if (isGroundedDelayed && !IsOnTooSteepSlope())
				{
					SwitchTo("Idle");
				}
				break;
			case "Crouching":
				if (!SLAMInput.Provider.GetButton(SLAMInput.Button.Down) && HasSpaceToStand())
				{
					if (SLAMInput.Provider.GetButton(SLAMInput.Button.Left) || SLAMInput.Provider.GetButton(SLAMInput.Button.Right))
					{
						SwitchTo("Walking");
					}
					else
					{
						SwitchTo("Idle");
					}
				}
				if (SLAMInput.Provider.GetButtonDown(SLAMInput.Button.UpOrAction) && HasSpaceToStand())
				{
					SwitchTo("Jumping");
				}
				if (!ShouldPrepareForLanding())
				{
					SwitchTo("Walking");
				}
				break;
			case "Climbing":
				if (SLAMInput.Provider.GetButton(SLAMInput.Button.Right) || SLAMInput.Provider.GetButton(SLAMInput.Button.Left))
				{
					SwitchTo("Falling");
				}
				break;
			}
		}
		isGroundedDelayed = _controller.isGrounded;
	}

	private void EnterIdle()
	{
		PrepareLanding = true;
		JumpCount = 0;
	}

	private void UpdateIdle()
	{
		ApplyHorizontalMovement(ref _velocity, crouch: false);
		ApplyGravity(ref _velocity);
	}

	private void ExitIdle()
	{
	}

	private void EnterWalking()
	{
		_animator.SetBool("Running", value: true);
		_animator.SetBool("JumpFalling", value: false);
		JumpCount = 0;
	}

	private void UpdateWalking()
	{
		if (AreControlsLocked)
		{
			return;
		}
		if (SLAMInput.Provider.GetButton(SLAMInput.Button.Right))
		{
			normalizedHorizontalSpeed = 1f;
			if (base.transform.localEulerAngles.y > 269f)
			{
				base.transform.localRotation = Quaternion.AngleAxis(90f, Vector3.up);
			}
		}
		else if (SLAMInput.Provider.GetButton(SLAMInput.Button.Left))
		{
			normalizedHorizontalSpeed = -1f;
			if (base.transform.localEulerAngles.y < 91f)
			{
				base.transform.localRotation = Quaternion.AngleAxis(270f, Vector3.up);
			}
		}
		else
		{
			normalizedHorizontalSpeed = 0f;
		}
		ApplyHorizontalMovement(ref _velocity, crouch: false);
		ApplyGravity(ref _velocity);
	}

	private void ExitWalking()
	{
		normalizedHorizontalSpeed = 0f;
		_animator.SetBool("Running", value: false);
	}

	private void EnterJumping()
	{
		PrepareLanding = false;
	}

	private void UpdateJumping()
	{
		if (AreControlsLocked)
		{
			return;
		}
		_animator.SetBool("Running", value: false);
		if (SLAMInput.Provider.GetButton(SLAMInput.Button.Right))
		{
			normalizedHorizontalSpeed = 1f;
			if (base.transform.localEulerAngles.y > 269f)
			{
				base.transform.localRotation = Quaternion.AngleAxis(90f, Vector3.up);
			}
			_animator.SetBool("Running", value: true);
		}
		else if (SLAMInput.Provider.GetButton(SLAMInput.Button.Left))
		{
			normalizedHorizontalSpeed = -1f;
			if (base.transform.localEulerAngles.y < 91f)
			{
				base.transform.localRotation = Quaternion.AngleAxis(270f, Vector3.up);
			}
			_animator.SetBool("Running", value: true);
		}
		if (SLAMInput.Provider.GetButtonDown(SLAMInput.Button.UpOrAction) && JumpCount == 1)
		{
			AudioController.Play("Avatar_double_jump", base.transform);
			_velocity.y = Mathf.Sqrt(2f * doubleJumpHeight * (0f - gravity));
			JumpCount = 2;
		}
		if (SLAMInput.Provider.GetButtonDown(SLAMInput.Button.UpOrAction) && JumpCount == 0)
		{
			AudioController.Play("CTT_avatar_jump", base.transform);
			_velocity.y = Mathf.Sqrt(2f * jumpHeight * (0f - gravity));
			JumpCount = 1;
		}
		if (SLAMInput.Provider.GetButtonUp(SLAMInput.Button.UpOrAction) && JumpCount == 1)
		{
			_velocity.y = Mathf.Min(_velocity.y, shortJumpVelocity);
		}
		ApplyHorizontalMovement(ref _velocity, crouch: false);
		ApplyGravity(ref _velocity);
	}

	private void ExitJumping()
	{
		normalizedHorizontalSpeed = 0f;
		_animator.SetBool("JumpFalling", value: true);
		_animator.SetBool("Running", value: false);
	}

	private void EnterFalling()
	{
		PrepareLanding = false;
		_animator.SetBool("JumpFalling", value: true);
	}

	private void UpdateFalling()
	{
		if (AreControlsLocked)
		{
			return;
		}
		if (SLAMInput.Provider.GetButton(SLAMInput.Button.Right))
		{
			normalizedHorizontalSpeed = 1f;
			if (base.transform.localEulerAngles.y > 269f)
			{
				base.transform.localRotation = Quaternion.AngleAxis(90f, Vector3.up);
			}
		}
		else if (SLAMInput.Provider.GetButton(SLAMInput.Button.Left))
		{
			normalizedHorizontalSpeed = -1f;
			if (base.transform.localEulerAngles.y < 91f)
			{
				base.transform.localRotation = Quaternion.AngleAxis(270f, Vector3.up);
			}
		}
		if (ShouldPrepareForLanding() || _controller.isGrounded)
		{
			PrepareLanding = true;
		}
		ApplyHorizontalMovement(ref _velocity, crouch: false);
		ApplyGravity(ref _velocity);
	}

	private void ExitFalling()
	{
		normalizedHorizontalSpeed = 0f;
		_animator.SetBool("JumpFalling", value: false);
	}

	private void EnterSlopeSlide()
	{
	}

	private void UpdateSlopeSlide()
	{
		if (!AreControlsLocked && IsOnTooSteepSlope(out var slopeDown))
		{
			_velocity = slopeDown * slopeSlideSpeed;
		}
	}

	private void ExitSlopeSlide()
	{
	}

	private void EnterClimbing()
	{
		_animator.SetBool("Climbing", value: true);
		_animator.SetBool("JumpFalling", value: false);
		avatar.transform.localRotation = Quaternion.AngleAxis(180f, Vector3.up) * base.transform.localRotation;
		Vector3 position = base.transform.position;
		position.z = 0.45f;
		base.transform.position = position;
	}

	private void UpdateClimbing()
	{
		if (AreControlsLocked)
		{
			return;
		}
		normalizedVerticalSpeed = 0f;
		_animator.speed = 0f;
		if (SLAMInput.Provider.GetButton(SLAMInput.Button.UpOrAction))
		{
			normalizedVerticalSpeed = 1f;
			_animator.speed = 1f;
			_animator.SetFloat("ClimbDir", 0f);
			if (base.transform.position.y + standHeight > currentAction.DataContext.GetComponent<Collider>().bounds.max.y)
			{
				normalizedVerticalSpeed = 0f;
				_animator.speed = 0f;
			}
		}
		else if (SLAMInput.Provider.GetButton(SLAMInput.Button.Down))
		{
			normalizedVerticalSpeed = -1f;
			_animator.speed = 1f;
			_animator.SetFloat("ClimbDir", 1f);
			if (AreFeetTouchingGround())
			{
				normalizedVerticalSpeed = 0f;
				_animator.speed = 0f;
			}
		}
		ApplyHorizontalMovement(ref _velocity, crouch: false);
		ApplyClimbingMovement(ref _velocity);
	}

	private void ExitClimbing()
	{
		_animator.SetBool("Climbing", value: false);
		_animator.speed = 1f;
		avatar.transform.localRotation = Quaternion.AngleAxis(0f, Vector3.up);
		Vector3 position = base.transform.position;
		position.z = 0f;
		base.transform.position = position;
	}

	private void EnterCrouching()
	{
		_animator.SetBool("Crouch", value: true);
		Vector3 size = _controller.boxCollider.size;
		size.y = crouchHeight;
		_controller.ResizeCollider(size);
	}

	private void UpdateCrouching()
	{
		if (AreControlsLocked)
		{
			return;
		}
		if (SLAMInput.Provider.GetButton(SLAMInput.Button.Right))
		{
			normalizedHorizontalSpeed = 1f;
			_animator.SetBool("Running", value: true);
			if (base.transform.localEulerAngles.y > 269f)
			{
				base.transform.localRotation = Quaternion.AngleAxis(90f, Vector3.up);
			}
		}
		else if (SLAMInput.Provider.GetButton(SLAMInput.Button.Left))
		{
			normalizedHorizontalSpeed = -1f;
			_animator.SetBool("Running", value: true);
			if (base.transform.localEulerAngles.y < 91f)
			{
				base.transform.localRotation = Quaternion.AngleAxis(270f, Vector3.up);
			}
		}
		else
		{
			normalizedHorizontalSpeed = 0f;
			_animator.SetBool("Running", value: false);
		}
		ApplyHorizontalMovement(ref _velocity, crouch: true);
		ApplyGravity(ref _velocity);
	}

	private void ExitCrouching()
	{
		normalizedHorizontalSpeed = 0f;
		_animator.SetBool("Running", value: false);
		_animator.SetBool("Crouch", value: false);
		Vector3 size = _controller.boxCollider.size;
		size.y = standHeight;
		_controller.ResizeCollider(size);
	}

	private void EnterDrown()
	{
		_animator.SetBool("Drown", value: true);
		AreControlsLocked = true;
	}

	private void UpdateDrown()
	{
	}

	private void ExitDrown()
	{
		_animator.SetBool("Drown", value: false);
		AreControlsLocked = false;
	}

	private void EnterCollapse()
	{
		_controller.DestroyTriggerHelper();
		_animator.SetTrigger("Collapse");
		_velocity = Vector3.zero;
		AreControlsLocked = true;
	}

	private void UpdateCollapse()
	{
		_velocity.x = 0f;
		ApplyGravity(ref _velocity);
	}

	private void ExitCollapse()
	{
		AreControlsLocked = false;
		_controller.createTriggerHelper();
	}

	public void Jump(float height)
	{
		AddForce(Vector3.up * height);
	}

	public void AddForce(Vector3 force)
	{
		vImpulseJump = force;
		impulseJump = true;
	}

	public void Drown()
	{
		SwitchTo("Drown");
	}

	public void Collapse()
	{
		SwitchTo("Collapse");
	}

	public void Hit()
	{
		_animator.SetTrigger("Hit");
	}

	private void PressSwitch()
	{
		int num = -1;
		if (base.transform.position.x > currentAction.DataContext.transform.position.x)
		{
			num = 1;
			base.transform.position = currentAction.DataContext.transform.FindChild("CharacterPositionRight").position;
			base.transform.localRotation = Quaternion.AngleAxis(-90f, Vector3.up);
		}
		else
		{
			num = 0;
			base.transform.position = currentAction.DataContext.transform.FindChild("CharacterPositionLeft").position;
			base.transform.localRotation = Quaternion.AngleAxis(90f, Vector3.up);
		}
		_animator.SetInteger("AvatarDirection", num);
		_animator.SetInteger("HandleState", ((P_Toggle)currentAction.DataContext).IsToggled ? 1 : 0);
		_animator.SetTrigger("PressSwitch");
		((P_Toggle)currentAction.DataContext).PlayAnimation(num);
		StartCoroutine(LockControls(currentAction.Duration));
	}

	public void ResetToIdle()
	{
		SwitchTo("Idle");
	}

	public void ResetPlayer(Vector3 toPosition)
	{
		_controller.velocity = (_velocity = Vector3.zero);
		base.transform.position = toPosition;
		base.transform.localRotation = Quaternion.AngleAxis(90f, Vector3.up);
	}

	public void Pause()
	{
		AreControlsLocked = true;
		pausedVelocity = _velocity;
		_velocity = (_controller.velocity = Vector3.zero);
	}

	public void Resume()
	{
		AreControlsLocked = false;
		_velocity = (_controller.velocity = pausedVelocity);
	}

	private void ApplyClimbingMovement(ref Vector3 velocity)
	{
		velocity.y = Mathf.Lerp(velocity.y, normalizedVerticalSpeed * climbSpeed, Time.deltaTime * ladderDamping);
	}

	private void ApplyHorizontalMovement(ref Vector3 velocity, bool crouch)
	{
		float num = ((!crouch) ? runSpeed : crouchSpeed);
		float num2 = ((!_controller.isGrounded) ? inAirDamping : groundDamping);
		velocity.x = Mathf.Lerp(velocity.x, normalizedHorizontalSpeed * num, Time.deltaTime * num2);
	}

	private void ApplyGravity(ref Vector3 velocity)
	{
		velocity.y += gravity * Time.deltaTime;
		if (velocity.y < terminalVelocity)
		{
			velocity.y = Mathf.Clamp(velocity.y, terminalVelocity, velocity.y);
		}
	}

	private bool IsOnTooSteepSlope(out Vector3 slopeDown)
	{
		slopeDown = Vector3.zero;
		bool flag = base.transform.forward.x > 0.25f;
		Vector3 position = base.transform.position;
		position.x += _controller.boxCollider.size.x / 2f * (float)(flag ? 1 : (-1));
		position.x += _controller.skinWidth * (float)((!flag) ? 1 : (-1));
		position.y += _controller.boxCollider.size.y / 2f;
		Ray ray = new Ray(position, Vector3.down);
		if (Physics.Raycast(ray, out var hitInfo, slopeRaycastLength + _controller.boxCollider.size.y / 2f, _controller.platformMask))
		{
			float num = MathUtilities.AngleSigned(hitInfo.normal, Vector3.up, Vector3.forward);
			if (Mathf.Abs(num) > _controller.slopeLimit)
			{
				if (num < 0f)
				{
					slopeDown = Vector3.Cross(Vector3.forward, hitInfo.normal);
				}
				else
				{
					slopeDown = Vector3.Cross(hitInfo.normal, Vector3.forward);
				}
				return true;
			}
		}
		return false;
	}

	private bool IsOnTooSteepSlope()
	{
		Vector3 slopeDown;
		return IsOnTooSteepSlope(out slopeDown);
	}

	private bool HasSpaceToStand()
	{
		float num = standHeight / 2f;
		float num2 = _controller.boxCollider.size.x / 2f;
		float num3 = _controller.boxCollider.size.z / 2f;
		Ray[] array = new Ray[4]
		{
			new Ray(base.transform.position + new Vector3(0f - num2, num, num3), Vector3.up),
			new Ray(base.transform.position + new Vector3(num2, num, num3), Vector3.up),
			new Ray(base.transform.position + new Vector3(num2, num, 0f - num3), Vector3.up),
			new Ray(base.transform.position + new Vector3(0f - num2, num, 0f - num3), Vector3.up)
		};
		bool flag = false;
		for (int i = 0; i < array.Length; i++)
		{
			if (Physics.Raycast(array[i], out var _, num, _controller.platformMask))
			{
				flag = true;
				break;
			}
		}
		return !flag;
	}

	private bool ShouldPrepareForLanding()
	{
		BoxCollider boxCollider = (BoxCollider)GetComponent<Collider>();
		Vector3 vector = new Vector3(boxCollider.size.x * Mathf.Abs(base.transform.localScale.x), boxCollider.size.y * Mathf.Abs(base.transform.localScale.y), boxCollider.size.z * Mathf.Abs(base.transform.localScale.z)) / 2f;
		Vector3 vector2 = new Vector3(boxCollider.center.x * base.transform.localScale.x, boxCollider.center.y * base.transform.localScale.y, boxCollider.center.z * base.transform.localScale.z);
		Vector3 origin = base.transform.position + new Vector3(vector2.x, vector2.y - vector.y);
		origin.y += _controller.skinWidth;
		Vector3 origin2 = base.transform.position + new Vector3(vector2.x + vector.x, vector2.y - vector.y);
		origin2.x -= _controller.skinWidth;
		origin2.y += _controller.skinWidth;
		Vector3 origin3 = base.transform.position + new Vector3(vector2.x - vector.x, vector2.y - vector.y);
		origin3.x += _controller.skinWidth;
		origin3.y += _controller.skinWidth;
		Ray[] array = new Ray[3]
		{
			new Ray(origin3, Vector3.down),
			new Ray(origin, Vector3.down),
			new Ray(origin2, Vector3.down)
		};
		bool flag = false;
		for (int i = 0; i < array.Length; i++)
		{
			Debug.DrawLine(array[i].origin, array[i].origin + array[i].direction * (prepareForLandingHeight + _controller.skinWidth), Color.magenta);
			if (Physics.Raycast(array[i], prepareForLandingHeight + _controller.skinWidth, _controller.platformMask))
			{
				flag = true;
				break;
			}
		}
		return flag;
	}

	private bool AreFeetTouchingGround()
	{
		Ray ray = new Ray(base.transform.position + Vector3.up * 0.01f, Vector3.down);
		RaycastHit hitInfo;
		return Physics.Raycast(ray, out hitInfo, 0.02f, _controller.platformMask);
	}

	private bool CanWalkToLeft()
	{
		float num = 0.25f;
		float num2 = _controller.boxCollider.size.y - _controller.skinWidth * 2f;
		bool result = true;
		for (int i = 0; i < _controller.totalHorizontalRays; i++)
		{
			Vector3 vector = base.transform.position + Vector3.up * (_controller.skinWidth + num2 / (float)_controller.totalHorizontalRays * (float)i);
			Ray ray = new Ray(vector + Vector3.left * (_controller.boxCollider.bounds.extents.x + _controller.skinWidth), Vector3.left);
			if (Physics.Raycast(ray, out var hitInfo, _controller.skinWidth + num, _controller.platformMask))
			{
				float num3 = Vector3.Dot(hitInfo.normal, Vector3.left);
				if (!(num3 > -0.7f))
				{
					result = false;
					break;
				}
			}
		}
		return result;
	}

	private bool CanWalkToRight()
	{
		float num = 0.25f;
		float num2 = _controller.boxCollider.size.y - _controller.skinWidth * 2f;
		bool result = true;
		for (int i = 0; i < _controller.totalHorizontalRays; i++)
		{
			Vector3 vector = base.transform.position + Vector3.up * (_controller.skinWidth + num2 / (float)_controller.totalHorizontalRays * (float)i);
			Ray ray = new Ray(vector + Vector3.right * (_controller.boxCollider.bounds.extents.x + _controller.skinWidth), Vector3.right);
			if (Physics.Raycast(ray, out var hitInfo, _controller.skinWidth + num, _controller.platformMask))
			{
				float num3 = Vector3.Dot(hitInfo.normal, Vector3.right);
				if (!(num3 > -0.7f))
				{
					result = false;
					break;
				}
			}
		}
		return result;
	}

	private IEnumerator LockControls(float duration)
	{
		if (!Mathf.Approximately(0f, duration))
		{
			AreControlsLocked = true;
			yield return new WaitForSeconds(duration);
			AreControlsLocked = false;
		}
	}
}
