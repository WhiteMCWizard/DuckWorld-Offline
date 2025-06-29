using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SLAM.CraneOperator;

public class Crane : MonoBehaviour
{
	private const float maxPickupDistanceCrateAndClaw = 0.125f;

	private const float clawHeightOffset = 0.5f;

	[SerializeField]
	private Animator avatarAnim;

	[SerializeField]
	private Animator controlpanel;

	[SerializeField]
	private Transform cabinXRoot;

	[SerializeField]
	private Transform cabinZRoot;

	[SerializeField]
	private Transform clawRoot;

	[SerializeField]
	private BoxCollider clawCollider;

	[SerializeField]
	private LayerMask dropMask;

	[SerializeField]
	private float skinWidth = 0.2f;

	[SerializeField]
	private float minimumPickupDistance = 0.05f;

	[SerializeField]
	private float craneMovementSpeed = 4f;

	[SerializeField]
	private float clawMovementSpeed = 2f;

	[SerializeField]
	private float smoothTime = 0.2f;

	[SerializeField]
	private float snapAtGridUnits = 0.5f;

	[SerializeField]
	private float backZDepth = 5.5f;

	[SerializeField]
	private float frontZDepth;

	[SerializeField]
	private AnimationCurve depthCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

	[SerializeField]
	private float rightZoneStart;

	[SerializeField]
	private float rightZoneEnd;

	[SerializeField]
	private float leftZoneStart;

	[SerializeField]
	private float leftZoneEnd;

	[SerializeField]
	private float shipTopHeight = -8.5f;

	[SerializeField]
	private float shipBottomHeight = -11.5f;

	[SerializeField]
	private AnimationCurve shipHeightCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

	[SerializeField]
	private float leftCraneMovementLimit = -10f;

	[SerializeField]
	private float rightCraneMovementLimit = 10f;

	[SerializeField]
	private GameObject pickupParticles;

	[SerializeField]
	private GameObject dropParticles;

	[SerializeField]
	private DropZone[] boatDropZones;

	private Vector3 targetPosition;

	private Vector3 velocity;

	private Crate pickedUpCrate;

	public bool AreControlsLocked { get; set; }

	public bool IsCarryingCrate { get; private set; }

	public Transform ClawRoot => clawRoot;

	public Crate PickedUpCrate => pickedUpCrate;

	private void Start()
	{
		targetPosition = cabinXRoot.transform.localPosition;
	}

	private void Update()
	{
		Vector3 localPosition = cabinXRoot.transform.localPosition;
		Vector3 zero = Vector3.zero;
		RayOrigin[] movementRays = CraneOperatorGame.CalculateRaysFromBox(clawCollider, skinWidth);
		if (!AreControlsLocked)
		{
			if (SLAMInput.Provider.GetButton(SLAMInput.Button.Left))
			{
				zero += Vector3.left;
			}
			if (SLAMInput.Provider.GetButton(SLAMInput.Button.Right))
			{
				zero += Vector3.right;
			}
		}
		zero *= craneMovementSpeed * Time.deltaTime;
		targetPosition += zero;
		if (SLAMInput.Provider.GetButtonUp(SLAMInput.Button.Left))
		{
			snapHorizontallyToGrid(Direction.Left, localPosition, ref targetPosition, snapAtGridUnits);
			Vector3 deltaMovement = targetPosition - localPosition;
			HorizontalCollision(movementRays, ref deltaMovement);
			targetPosition = localPosition + deltaMovement;
		}
		if (SLAMInput.Provider.GetButtonUp(SLAMInput.Button.Right))
		{
			snapHorizontallyToGrid(Direction.Right, localPosition, ref targetPosition, snapAtGridUnits);
			Vector3 deltaMovement2 = targetPosition - localPosition;
			HorizontalCollision(movementRays, ref deltaMovement2);
			targetPosition = localPosition + deltaMovement2;
		}
		if (SLAMInput.Provider.GetButton(SLAMInput.Button.Right) || SLAMInput.Provider.GetButton(SLAMInput.Button.Left))
		{
			Vector3 deltaMovement3 = targetPosition - localPosition;
			HorizontalCollision(movementRays, ref deltaMovement3);
			targetPosition = localPosition + deltaMovement3;
		}
		targetPosition.x = Mathf.Clamp(targetPosition.x, leftCraneMovementLimit, rightCraneMovementLimit);
		localPosition = Vector3.SmoothDamp(localPosition, targetPosition, ref velocity, smoothTime);
		cabinXRoot.transform.localPosition = localPosition;
		float num = 0f;
		if (localPosition.x >= rightZoneStart)
		{
			num = (localPosition.x - rightZoneStart) / (rightZoneEnd - rightZoneStart);
		}
		else if (localPosition.x <= leftZoneStart)
		{
			num = (localPosition.x - leftZoneStart) / (leftZoneEnd - leftZoneStart);
		}
		float z = Mathf.Lerp(frontZDepth, backZDepth, depthCurve.Evaluate(num));
		Vector3 position = cabinZRoot.transform.position;
		position.z = z;
		cabinZRoot.transform.position = position;
		Vector3 deltaMovement4 = Vector3.zero;
		if (!AreControlsLocked)
		{
			if (SLAMInput.Provider.GetButton(SLAMInput.Button.Up))
			{
				deltaMovement4 += Vector3.up;
			}
			if (SLAMInput.Provider.GetButton(SLAMInput.Button.Down))
			{
				deltaMovement4 += Vector3.down;
			}
		}
		deltaMovement4 *= clawMovementSpeed * Time.deltaTime;
		MoveVertically(movementRays, ref deltaMovement4);
		clawRoot.localPosition += deltaMovement4;
		if (num > 0f && num < 1f)
		{
			Vector3 localPosition2 = clawRoot.localPosition;
			float num2 = ((!IsCarryingCrate) ? 0f : pickedUpCrate.Collider.bounds.size.y);
			localPosition2.y = Mathf.Max(Mathf.Lerp(shipBottomHeight - num2, shipTopHeight + num2, shipHeightCurve.Evaluate(num)), localPosition2.y);
			clawRoot.localPosition = localPosition2;
		}
		doPickupAndDrop();
		doAudio();
		doAnimation();
	}

	private void doPickupAndDrop()
	{
		if (!SLAMInput.Provider.GetButtonDown(SLAMInput.Button.Action))
		{
			return;
		}
		if (!IsCarryingCrate)
		{
			if (CanPickupCrate(out var crate))
			{
				StartCoroutine(PickUp(crate));
			}
		}
		else if (CanDropCrate(pickedUpCrate))
		{
			DropCrate();
		}
	}

	private void doAnimation()
	{
		avatarAnim.SetBool("Left", SLAMInput.Provider.GetButton(SLAMInput.Button.Left));
		avatarAnim.SetBool("Right", SLAMInput.Provider.GetButton(SLAMInput.Button.Right));
		avatarAnim.SetBool("Up", SLAMInput.Provider.GetButton(SLAMInput.Button.Up));
		avatarAnim.SetBool("Down", SLAMInput.Provider.GetButton(SLAMInput.Button.Down));
		controlpanel.SetBool("Left", SLAMInput.Provider.GetButton(SLAMInput.Button.Left));
		controlpanel.SetBool("Right", SLAMInput.Provider.GetButton(SLAMInput.Button.Right));
		controlpanel.SetBool("Up", SLAMInput.Provider.GetButton(SLAMInput.Button.Up));
		controlpanel.SetBool("Down", SLAMInput.Provider.GetButton(SLAMInput.Button.Down));
	}

	private static void doAudio()
	{
		if (SLAMInput.Provider.GetButtonDown(SLAMInput.Button.Up) || SLAMInput.Provider.GetButtonDown(SLAMInput.Button.Down))
		{
			AudioController.Play("CO_magnet_move_start");
			AudioController.Play("CO_magnet_move_loop");
		}
		if (SLAMInput.Provider.GetButtonUp(SLAMInput.Button.Up) || SLAMInput.Provider.GetButtonUp(SLAMInput.Button.Down))
		{
			AudioController.Stop("CO_magnet_move_loop");
			AudioController.Play("CO_magnet_move_end");
		}
		if (SLAMInput.Provider.GetButtonDown(SLAMInput.Button.Left) || SLAMInput.Provider.GetButtonDown(SLAMInput.Button.Right))
		{
			AudioController.Play("CO_crane_move_start");
			AudioController.Play("CO_crane_move_loop");
		}
		if ((SLAMInput.Provider.GetButtonUp(SLAMInput.Button.Left) && !SLAMInput.Provider.GetButton(SLAMInput.Button.Right)) || (SLAMInput.Provider.GetButtonUp(SLAMInput.Button.Right) && !SLAMInput.Provider.GetButton(SLAMInput.Button.Left)))
		{
			AudioController.Stop("CO_crane_move_loop");
			AudioController.Play("CO_crane_move_end");
		}
	}

	public bool CanPickupCrate(out Crate crate)
	{
		crate = null;
		bool result = false;
		Ray ray = getPickupRay().Ray;
		if (Physics.Raycast(ray, out var hitInfo, skinWidth + minimumPickupDistance, dropMask.value))
		{
			crate = hitInfo.transform.GetComponent<Crate>();
			if (crate != null)
			{
				Vector3 topCenter = crate.TopCenter;
				topCenter.y = ray.origin.y;
				Vector3 normalized = (topCenter - ray.origin).normalized;
				Vector3 origin = ray.origin;
				if (normalized.x < 0f)
				{
					origin.x += snapAtGridUnits - ray.origin.x % snapAtGridUnits;
				}
				else
				{
					origin.x -= snapAtGridUnits - ray.origin.x % snapAtGridUnits;
				}
				float maxDistance = Mathf.Max(0.01f, Mathf.Abs(origin.x - topCenter.x) + (snapAtGridUnits * (float)crate.UnitWidth - 1f) - ray.origin.x % snapAtGridUnits);
				Ray ray2 = new Ray(origin, normalized);
				if (!Physics.Raycast(ray2, out var _, maxDistance, dropMask.value))
				{
					result = true;
				}
			}
		}
		return result;
	}

	private RayOrigin getPickupRay()
	{
		Vector3 origin = new Vector3(clawCollider.bounds.center.x, clawCollider.bounds.min.y + skinWidth, clawCollider.bounds.center.z);
		Vector3 down = Vector3.down;
		return new RayOrigin(new Ray(origin, down), Direction.Down);
	}

	public bool CanDropCrate(Crate crate)
	{
		if (crate == null)
		{
			return false;
		}
		bool result = true;
		RayOrigin[] array = crate.Rays(Direction.Up, Direction.Left, Direction.Right, Direction.Forward, Direction.Back);
		for (int i = 0; i < array.Length; i++)
		{
			if (!Physics.Raycast(array[i].Ray, out var _, skinWidth + minimumPickupDistance, dropMask))
			{
				result = false;
				break;
			}
		}
		return result;
	}

	public IEnumerator PickUp(Crate crate)
	{
		AreControlsLocked = true;
		yield return StartCoroutine(centerClawOnCrate(crate));
		AreControlsLocked = false;
		Object.Instantiate(pickupParticles, crate.TopCenter, Quaternion.identity);
		AudioController.Play("CO_magnet_attach");
		if (crate.Type != Crate.CrateType.Giraffe)
		{
			AudioController.Play("Content_" + crate.Type);
		}
		crate.transform.parent = clawRoot.transform;
		crate.transform.localPosition = Vector3.zero;
		Vector3 deltaMove = crate.transform.position - crate.TopCenter;
		deltaMove.y -= 0.5f;
		crate.transform.localPosition += deltaMove;
		crate.PickUp();
		pickedUpCrate = crate;
		IsCarryingCrate = true;
		DropZone[] array = Object.FindObjectsOfType<DropZone>();
		foreach (DropZone dp in array)
		{
			if (dp.GetComponent<Collider>().bounds.Contains(pickedUpCrate.Collider.bounds.min) && dp.GetComponent<Collider>().bounds.Contains(pickedUpCrate.Collider.bounds.max))
			{
				dp.OnPickup(pickedUpCrate);
			}
		}
	}

	private IEnumerator centerClawOnCrate(Crate crate)
	{
		targetPosition.x = crate.TopCenter.x;
		float bailoutTimer = 0f;
		while (Mathf.Abs(targetPosition.x - cabinXRoot.position.x) > 0.01f)
		{
			yield return null;
			float num;
			bailoutTimer = (num = bailoutTimer + Time.deltaTime);
			if (num > 1f)
			{
				Debug.LogError("Bailed out!");
				break;
			}
		}
	}

	public void DropCrate()
	{
		DropZone[] array = Object.FindObjectsOfType<DropZone>();
		foreach (DropZone dropZone in array)
		{
			if (dropZone.GetComponent<Collider>().bounds.Contains(pickedUpCrate.Collider.bounds.min) && dropZone.GetComponent<Collider>().bounds.Contains(pickedUpCrate.Collider.bounds.max))
			{
				if (pickedUpCrate.Type != Crate.CrateType.Giraffe)
				{
					AudioController.Stop("Content_" + pickedUpCrate.Type);
				}
				Object.Instantiate(dropParticles, pickedUpCrate.TopCenter, Quaternion.identity);
				dropZone.OnDrop(pickedUpCrate);
				pickedUpCrate.Release();
				pickedUpCrate = null;
				IsCarryingCrate = false;
				AudioController.Play("CO_magnet_release");
				AudioController.Play("CO_crate_impactHit");
				break;
			}
		}
	}

	public void HorizontalCollision(RayOrigin[] movementRays, ref Vector3 deltaMovement)
	{
		Direction direction = ((!(deltaMovement.x > 0f)) ? Direction.Left : Direction.Right);
		float num = Mathf.Abs(deltaMovement.x) + skinWidth;
		List<RayOrigin> list = new List<RayOrigin>();
		list.AddRange(movementRays);
		if (IsCarryingCrate)
		{
			list.AddRange(pickedUpCrate.Rays());
		}
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].Direction != direction)
			{
				continue;
			}
			Ray ray = list[i].Ray;
			if (Physics.Raycast(ray, out var hitInfo, num))
			{
				deltaMovement.x = hitInfo.point.x - ray.origin.x;
				switch (direction)
				{
				case Direction.Left:
					deltaMovement.x += skinWidth;
					break;
				case Direction.Right:
					deltaMovement.x -= skinWidth;
					break;
				}
				num = Mathf.Abs(deltaMovement.x);
				if (num < skinWidth + 0.001f)
				{
					break;
				}
			}
		}
	}

	public void MoveVertically(RayOrigin[] movementRays, ref Vector3 deltaMovement)
	{
		Direction direction = ((deltaMovement.y > 0f) ? Direction.Up : Direction.Down);
		float num = Mathf.Abs(deltaMovement.y) + skinWidth;
		List<RayOrigin> list = new List<RayOrigin>();
		if (IsCarryingCrate)
		{
			list.AddRange(pickedUpCrate.Rays(Direction.Up));
			for (int i = 0; i < movementRays.Length; i++)
			{
				if (movementRays[i].Direction == Direction.Up)
				{
					list.Add(movementRays[i]);
				}
			}
		}
		else
		{
			list.AddRange(movementRays);
		}
		for (int j = 0; j < list.Count; j++)
		{
			if (list[j].Direction != direction)
			{
				continue;
			}
			Ray ray = list[j].Ray;
			if (Physics.Raycast(ray, out var hitInfo, num))
			{
				deltaMovement.y = hitInfo.point.y - ray.origin.y;
				switch (direction)
				{
				case Direction.Up:
					deltaMovement.y -= skinWidth;
					break;
				case Direction.Down:
					deltaMovement.y += skinWidth;
					break;
				}
				num = Mathf.Abs(deltaMovement.y);
				if (num < skinWidth + 0.001f)
				{
					break;
				}
			}
		}
	}

	private void snapHorizontallyToGrid(Direction movingToDirection, Vector3 current, ref Vector3 target, float snapDistance)
	{
		switch (movingToDirection)
		{
		case Direction.Right:
			if (current.x > 0f)
			{
				target.x += snapDistance - target.x % snapDistance;
			}
			else
			{
				target.x += Mathf.Abs(target.x % snapDistance);
			}
			break;
		case Direction.Left:
			if (current.x > 0f)
			{
				target.x -= target.x % snapDistance;
			}
			else
			{
				target.x -= Mathf.Abs(target.x % snapDistance + snapDistance);
			}
			break;
		}
	}
}
