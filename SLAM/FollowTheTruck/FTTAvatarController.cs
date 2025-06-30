using System.Collections;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.FollowTheTruck;

public class FTTAvatarController : MonoBehaviour
{
	[SerializeField]
	private float runningSpeed = 1f;

	[SerializeField]
	private float jumpingForce = 4f;

	[SerializeField]
	private float doubleJumpingForce = 4f;

	[SerializeField]
	private float laneWidth = 1f;

	[SerializeField]
	private float laneSwitchSpeed = 1f;

	[SerializeField]
	private float invulnerabilityTime = 2f;

	[SerializeField]
	private float blinkingSpeed = 0.5f;

	[SerializeField]
	private float hitWaterResetTime = 2f;

	[SerializeField]
	private float slideTime = 2f;

	[SerializeField]
	private float minimalSafeLaneDistance = 4f;

	[SerializeField]
	private Animator animator;

	[SerializeField]
	private Vector3 upForceModifier = Vector3.zero;

	[SerializeField]
	private Vector3 downForceModifier = Vector3.zero;

	[SerializeField]
	private Vector3 slidingDownForceModifier = Vector3.zero;

	private Vector3 velocity;

	private int currentLane;

	private int jumpCount;

	private bool invulnerable;

	private bool isDoingPlayerHitByObstacleRoutine;

	private bool jumpingAllowed = true;

	private float distanceToGround;

	private FTTWater water;

	private float originalCapsuleColliderHeight;

	private Vector3 originalCapsuleColliderCenter;

	private void Awake()
	{
		GameEvents.Subscribe<FTTGameStartedEvent>(onGameStart);
		GameEvents.Subscribe<FTTGameEndedEvent>(onGameEnd);
		base.enabled = false;
		GetComponent<Rigidbody>().isKinematic = true;
		originalCapsuleColliderHeight = ((CapsuleCollider)GetComponent<Collider>()).height;
		originalCapsuleColliderCenter = ((CapsuleCollider)GetComponent<Collider>()).center;
		water = Object.FindObjectOfType<FTTWater>();
	}

	private void OnDestroy()
	{
		GameEvents.Unsubscribe<FTTGameStartedEvent>(onGameStart);
		GameEvents.Unsubscribe<FTTGameEndedEvent>(onGameEnd);
	}

	private void Update()
	{
		velocity = GetComponent<Rigidbody>().velocity;
		velocity.z = runningSpeed;
		updateDistanceToGround();
		applyForceModifiers();
		if (shouldJump())
		{
			jump();
		}
		else if (isGrounded() && velocity.y <= 0.1f && jumpCount > 0)
		{
			resetJumpCount();
		}
		if (shouldSlide())
		{
			slide();
		}
		if (shouldDash())
		{
			dash();
		}
		animator.SetFloat("VelocityY", velocity.y);
		animator.SetFloat("VelocityZ", velocity.z);
		GetComponent<Rigidbody>().velocity = velocity;
	}

	private void OnCollisionEnter(Collision col)
	{
		if (isDoingPlayerHitByObstacleRoutine)
		{
			return;
		}
		if (base.transform.position.y < -0.6f)
		{
			OnWaterHit(null);
			return;
		}
		for (int i = 0; i < col.contacts.Length; i++)
		{
			Debug.DrawRay(col.contacts[i].point, col.contacts[i].normal, Color.green, 100f);
			if (col.collider.CompareTag("Lethal") || Vector3.Dot(base.transform.up, col.contacts[i].normal) < 0.5f)
			{
				Debug.Log("You hit a hole! y: " + base.transform.position.y + " - dot: " + Vector3.Dot(base.transform.up, col.contacts[i].normal));
				AudioController.Play((!col.collider.name.Contains("Water")) ? "CTT_avatar_hitByObject" : "CTT_avatar_hitByWater", col.contacts[i].point);
				loseLife();
				StopCoroutine("onPlayerHitByObstacle");
				if (base.enabled)
				{
					StartCoroutine("onPlayerHitByObstacle");
				}
				break;
			}
		}
	}

	private IEnumerator onPlayerHitByObstacle()
	{
		isDoingPlayerHitByObstacleRoutine = true;
		StopCoroutine("doDashRoutine");
		float distance = -0.5f;
		FTTAvatarController fTTAvatarController;
		float lookFwdAmount;
		int targetLane;
		do
		{
			fTTAvatarController = this;
			distance = (lookFwdAmount = distance + 0.5f);
		}
		while (!fTTAvatarController.findSafeLane(out targetLane, lookFwdAmount));
		yield return StartCoroutine("doDashRoutine", targetLane);
		isDoingPlayerHitByObstacleRoutine = false;
	}

	private bool shouldJump()
	{
		return SLAMInput.Provider.GetButtonDown(SLAMInput.Button.UpOrAction) && (isGrounded() || jumpCount == 1);
	}

	private bool shouldSlide()
	{
		return SLAMInput.Provider.GetButtonDown(SLAMInput.Button.Down);
	}

	private bool shouldDash()
	{
		return SLAMInput.Provider.GetButtonDown(SLAMInput.Button.Right) || SLAMInput.Provider.GetButtonDown(SLAMInput.Button.Left);
	}

	private void dash()
	{
		Vector3 vector = ((!SLAMInput.Provider.GetButton(SLAMInput.Button.Right)) ? Vector3.left : Vector3.right);
		int num = Mathf.Clamp(currentLane + (int)vector.x, -1, 1);
		if (currentLane != num && canMoveInDirection(vector) && canMoveInDirection(vector + base.transform.forward))
		{
			StopCoroutine("doDashRoutine");
			StartCoroutine("doDashRoutine", num);
		}
	}

	private IEnumerator doDashRoutine(int targetLane)
	{
		animator.SetTrigger("DoDash");
		AudioController.Play("CTT_avatar_sidejump");
		float startX = GetComponent<Rigidbody>().position.x;
		float targetX = (float)targetLane * laneWidth;
		currentLane = targetLane;
		animator.SetFloat("VelocityX", laneSwitchSpeed * Mathf.Sign(targetX - startX));
		Stopwatch sw = new Stopwatch(Mathf.Abs(startX - targetX) / laneSwitchSpeed);
		while ((bool)sw)
		{
			yield return new WaitForFixedUpdate();
			GetComponent<Rigidbody>().MovePosition(new Vector3(Mathf.Lerp(startX, targetX, sw.Progress), GetComponent<Rigidbody>().position.y, GetComponent<Rigidbody>().position.z));
		}
		animator.SetFloat("VelocityX", 0f);
	}

	private void slide()
	{
		if (!animator.GetBool("IsSliding"))
		{
			StartCoroutine("doSlideRoutine");
		}
	}

	private IEnumerator doSlideRoutine()
	{
		AudioController.Play("CTT_avatar_slide");
		animator.SetBool("IsSliding", value: true);
		CapsuleCollider capsuleCollider = GetComponent<Collider>() as CapsuleCollider;
		float targetHeight = 0.5f;
		float heightDifference = originalCapsuleColliderHeight - targetHeight;
		capsuleCollider.height = targetHeight;
		capsuleCollider.center = new Vector3(0f, heightDifference * 0.5f);
		yield return new WaitForSeconds(slideTime);
		stopSliding();
	}

	private void stopSliding()
	{
		animator.SetBool("IsSliding", value: false);
		AudioController.Stop("CTT_avatar_slide");
		((CapsuleCollider)GetComponent<Collider>()).height = originalCapsuleColliderHeight;
		((CapsuleCollider)GetComponent<Collider>()).center = originalCapsuleColliderCenter;
	}

	public void SetJumpAllowed(bool allowJump)
	{
		jumpingAllowed = allowJump;
	}

	private void jump()
	{
		if (jumpingAllowed)
		{
			jumpCount++;
			AudioController.Play((jumpCount > 1) ? "Avatar_double_jump" : "CTT_avatar_jump");
			velocity.y = ((jumpCount <= 1) ? jumpingForce : doubleJumpingForce);
			animator.SetInteger("JumpCount", jumpCount);
			stopSliding();
		}
	}

	private void applyForceModifiers()
	{
		if (velocity.y > 0f)
		{
			velocity += upForceModifier * Time.deltaTime;
		}
		else if (velocity.y < 0f)
		{
			velocity += ((!animator.GetBool("IsSliding")) ? downForceModifier : slidingDownForceModifier) * Time.deltaTime;
		}
	}

	private void resetJumpCount()
	{
		jumpCount = 0;
		animator.SetInteger("JumpCount", jumpCount);
		AudioController.Play("CTT_avatar_land_wood");
	}

	private void loseLife()
	{
		if (!invulnerable)
		{
			GameEvents.Invoke(new FTTHeartLostEvent());
			StartCoroutine(doInvulnerableRoutine());
		}
	}

	private IEnumerator doInvulnerableRoutine()
	{
		invulnerable = true;
		SkinnedMeshRenderer[] smr = GetComponentsInChildren<SkinnedMeshRenderer>();
		Stopwatch sw = new Stopwatch(invulnerabilityTime);
		while ((bool)sw && base.enabled)
		{
			yield return new WaitForSeconds(blinkingSpeed);
			for (int i = 0; i < smr.Length; i++)
			{
				smr[i].enabled = !smr[i].enabled || sw.Expired || !base.enabled;
			}
		}
		invulnerable = false;
	}

	public void OnWaterHit(FTTWater water)
	{
		if (runningSpeed > 0f)
		{
			AudioController.Play("CTT_avatar_fall_water", base.transform.position);
			GameEvents.Invoke(new FTTAvatarInWaterEvent());
			StartCoroutine(doWaterHitRoutine());
		}
	}

	private IEnumerator doWaterHitRoutine()
	{
		animator.SetBool("IsSinking", value: true);
		float oldRunSpeed = runningSpeed;
		runningSpeed = 0f;
		Vector3 startPos = base.transform.position;
		Vector3 endPos = base.transform.position + Vector3.down * 3f;
		Stopwatch sw = new Stopwatch(hitWaterResetTime);
		while ((bool)sw)
		{
			yield return null;
			base.transform.position = Vector3.Lerp(startPos, endPos, sw.Progress);
		}
		animator.SetBool("IsSinking", value: false);
		loseLife();
		if (base.enabled)
		{
			base.transform.position = getNearestSafePosition();
			currentLane = Mathf.FloorToInt(base.transform.position.x / laneWidth);
			runningSpeed = oldRunSpeed;
		}
	}

	private Vector3 getNearestSafePosition()
	{
		float num = 0f;
		int safestLane;
		while (!findSafeLane(out safestLane, num += 1f))
		{
		}
		return new Vector3((float)safestLane * laneWidth, 0f, base.transform.position.z + num);
	}

	private void OnFootstep()
	{
		if (base.enabled && isGrounded() && !animator.GetBool("IsSliding"))
		{
			AudioController.Play("CTT_avatar_footstep");
		}
	}

	private void OnDoubleJump()
	{
	}

	private void updateDistanceToGround()
	{
		Ray ray = new Ray(base.transform.position + Vector3.up * 0.1f, Vector3.down);
		RaycastHit[] array = Physics.RaycastAll(ray, 5f);
		distanceToGround = float.MaxValue;
		for (int i = 0; i < array.Length; i++)
		{
			if (!array[i].collider.isTrigger)
			{
				distanceToGround = ((!(array[i].distance - 0.1f < distanceToGround)) ? distanceToGround : (array[i].distance - 0.1f));
			}
		}
		animator.SetFloat("DistanceToGround", distanceToGround);
	}

	private bool isGrounded()
	{
		return distanceToGround < 0.05f;
	}

	private bool canMoveInDirection(Vector3 dir)
	{
		RaycastHit[] collection = Physics.RaycastAll(base.transform.position, dir, laneWidth);
		return !collection.Any((RaycastHit h) => !h.collider.isTrigger);
	}

	private bool findSafeLane(out int safestLane, float lookFwdAmount = 0f)
	{
		safestLane = 2147483646;
		for (int i = -1; i <= 1; i++)
		{
			Vector3 vector = new Vector3((float)i * laneWidth, 0.5f, base.transform.position.z + lookFwdAmount);
			RaycastHit[] collection = Physics.RaycastAll(vector, Vector3.forward, 5f);
			bool flag = collection.Any((RaycastHit info) => !info.collider.isTrigger && !info.collider.CompareTag("Player"));
			bool flag2 = true;
			float num = 100f;
			int num2 = 0;
			while (flag2 && (float)num2 < minimalSafeLaneDistance)
			{
				Ray ray = new Ray(vector + Vector3.forward * num2 + Vector3.up * num, Vector3.down);
				RaycastHit[] collection2 = Physics.RaycastAll(ray);
				flag2 = collection2.Any((RaycastHit info) => !info.collider.isTrigger && !info.collider.CompareTag("Player") && info.point.y > water.transform.position.y) && !collection2.Any((RaycastHit info) => info.collider.name.Contains("Crate"));
				DebugUtils.DrawArrow(ray.origin, ray.direction * num, (!flag2) ? Color.red : Color.green);
				num2++;
			}
			DebugUtils.DrawArrow(vector, Vector3.forward * minimalSafeLaneDistance, flag ? Color.red : Color.green);
			if (!flag && flag2 && Mathf.Abs(safestLane - currentLane) > Mathf.Abs(i - currentLane))
			{
				safestLane = i;
			}
		}
		return safestLane != 2147483646;
	}

	private void onGameStart(FTTGameStartedEvent evt)
	{
		base.enabled = true;
		GetComponent<Rigidbody>().isKinematic = false;
		runningSpeed = evt.Settings.AvatarRunSpeed;
	}

	private void onGameEnd(FTTGameEndedEvent evt)
	{
		base.enabled = false;
		animator.SetBool("GameOver", !evt.Success);
	}

	public void OnSewerPipeHit(FTTSewer fTTSewer)
	{
		loseLife();
	}
}
