#define DEBUG_CC2D_RAYS
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace SLAM.Platformer;

[RequireComponent(typeof(BoxCollider))]
public class CharacterControllerPath : MonoBehaviour
{
	private struct CharacterRaycastOrigins
	{
		public Vector3 topRight;

		public Vector3 topLeft;

		public Vector3 bottomRight;

		public Vector3 bottomLeft;
	}

	public class CharacterCollisionState2D
	{
		public bool right;

		public bool left;

		public bool above;

		public bool below;

		public bool becameGroundedThisFrame;

		public bool wasGroundedLastFrame;

		public bool movingDownSlope;

		public float slopeAngle;

		public bool hasCollision()
		{
			return below || right || left || above;
		}

		public void reset()
		{
			right = (left = (above = (below = (becameGroundedThisFrame = (movingDownSlope = false)))));
			slopeAngle = 0f;
		}

		public override string ToString()
		{
			return $"[CharacterCollisionState2D] r: {right}, l: {left}, a: {above}, b: {below}, movingDownSlope: {movingDownSlope}, angle: {slopeAngle}, wasGroundedLastFrame: {wasGroundedLastFrame}, becameGroundedThisFrame: {becameGroundedThisFrame}";
		}
	}

	private const float kSkinWidthFloatFudgeFactor = 0.001f;

	[Range(0.001f, 0.3f)]
	[SerializeField]
	private float _skinWidth = 0.02f;

	public LayerMask platformMask = 0;

	[SerializeField]
	private LayerMask oneWayPlatformMask = 0;

	[Range(0f, 90f)]
	public float slopeLimit = 30f;

	public AnimationCurve slopeSpeedMultiplier = new AnimationCurve(new Keyframe(-90f, 1.5f), new Keyframe(0f, 1f), new Keyframe(90f, 0f));

	[Range(2f, 20f)]
	public int totalHorizontalRays = 8;

	[Range(2f, 20f)]
	public int totalVerticalRays = 4;

	private float _slopeLimitTangent = Mathf.Tan(1.3089969f);

	public bool createTriggerHelperGameObject;

	[Range(0.8f, 0.999f)]
	public float triggerHelperBoxColliderScale = 0.95f;

	[NonSerialized]
	[HideInInspector]
	public new Transform transform;

	[NonSerialized]
	[HideInInspector]
	public BoxCollider boxCollider;

	[NonSerialized]
	[HideInInspector]
	public CharacterCollisionState2D collisionState = new CharacterCollisionState2D();

	[NonSerialized]
	[HideInInspector]
	public Vector3 velocity;

	private CharacterRaycastOrigins _raycastOrigins;

	private RaycastHit _raycastHit;

	private List<RaycastHit> _raycastHitsThisFrame = new List<RaycastHit>(2);

	private float _verticalDistanceBetweenRays;

	private float _horizontalDistanceBetweenRays;

	private bool _isGoingUpSlope;

	private GameObject triggerHelper;

	public float skinWidth
	{
		get
		{
			return _skinWidth;
		}
		set
		{
			_skinWidth = value;
			recalculateDistanceBetweenRays();
		}
	}

	public bool isGrounded => collisionState.below;

	public event Action<RaycastHit> onControllerCollidedEvent;

	public event Action<Collider> onTriggerEnterEvent;

	public event Action<Collider> onTriggerStayEvent;

	public event Action<Collider> onTriggerExitEvent;

	private void Awake()
	{
		platformMask = (int)platformMask | (int)oneWayPlatformMask;
		transform = GetComponent<Transform>();
		boxCollider = GetComponent<BoxCollider>();
		boxCollider.enabled = false;
		if (createTriggerHelperGameObject)
		{
			createTriggerHelper();
		}
		skinWidth = _skinWidth;
	}

	public void OnTriggerEnter(Collider col)
	{
		if (this.onTriggerEnterEvent != null)
		{
			this.onTriggerEnterEvent(col);
		}
	}

	public void OnTriggerStay(Collider col)
	{
		if (this.onTriggerStayEvent != null)
		{
			this.onTriggerStayEvent(col);
		}
	}

	public void OnTriggerExit(Collider col)
	{
		if (this.onTriggerExitEvent != null)
		{
			this.onTriggerExitEvent(col);
		}
	}

	[Conditional("DEBUG_CC2D_RAYS")]
	private void DrawRay(Vector3 start, Vector3 dir, Color color)
	{
		UnityEngine.Debug.DrawRay(start, dir, color);
	}

	public void move(Vector3 deltaMovement)
	{
		collisionState.wasGroundedLastFrame = collisionState.below;
		collisionState.reset();
		_raycastHitsThisFrame.Clear();
		_isGoingUpSlope = false;
		primeRaycastOrigins(deltaMovement);
		if (deltaMovement.y < 0f && collisionState.wasGroundedLastFrame)
		{
			handleVerticalSlope(ref deltaMovement);
		}
		if (Mathf.Abs(deltaMovement.x) > 0.001f || Mathf.Abs(deltaMovement.z) > 0.001f)
		{
			moveHorizontally(ref deltaMovement);
		}
		if (Mathf.Abs(deltaMovement.y) > 0.001f)
		{
			moveVertically(ref deltaMovement);
		}
		transform.Translate(deltaMovement, Space.Self);
		if (Time.deltaTime > 0f)
		{
			velocity = deltaMovement / Time.deltaTime;
		}
		if (!collisionState.wasGroundedLastFrame && collisionState.below)
		{
			collisionState.becameGroundedThisFrame = true;
		}
		if (_isGoingUpSlope)
		{
			velocity.y = 0f;
		}
		if (this.onControllerCollidedEvent != null)
		{
			for (int i = 0; i < _raycastHitsThisFrame.Count; i++)
			{
				this.onControllerCollidedEvent(_raycastHitsThisFrame[i]);
			}
		}
	}

	public void warpToGrounded()
	{
		do
		{
			move(new Vector3(0f, -1f, 0f));
		}
		while (!isGrounded);
	}

	public void recalculateDistanceBetweenRays()
	{
		float num = boxCollider.size.y * Mathf.Abs(transform.localScale.y);
		_verticalDistanceBetweenRays = num / (float)(totalHorizontalRays - 1);
		float num2 = boxCollider.size.z * Mathf.Abs(transform.localScale.z);
		_horizontalDistanceBetweenRays = num2 / (float)(totalVerticalRays - 1);
	}

	public void createTriggerHelper()
	{
		if (triggerHelper != null)
		{
			UnityEngine.Object.Destroy(triggerHelper);
		}
		triggerHelper = new GameObject("PlayerTriggerHelper");
		triggerHelper.layer = base.gameObject.layer;
		triggerHelper.tag = base.gameObject.tag;
		triggerHelper.transform.parent = transform;
		triggerHelper.transform.localPosition = Vector3.zero;
		triggerHelper.transform.localRotation = Quaternion.identity;
		Rigidbody rigidbody = triggerHelper.AddComponent<Rigidbody>();
		rigidbody.mass = 1f;
		rigidbody.useGravity = false;
		BoxCollider boxCollider = triggerHelper.AddComponent<BoxCollider>();
		boxCollider.center = this.boxCollider.center;
		boxCollider.size = this.boxCollider.size * triggerHelperBoxColliderScale;
		boxCollider.isTrigger = true;
		FixedJoint fixedJoint = triggerHelper.AddComponent<FixedJoint>();
		fixedJoint.connectedBody = GetComponent<Rigidbody>();
	}

	public void ResizeCollider(Vector3 newSize)
	{
		Vector3 size = boxCollider.size;
		Vector3 vector = newSize - size;
		Vector3 center = boxCollider.center;
		Vector3 center2 = center + vector / 2f;
		boxCollider.center = center2;
		boxCollider.size = newSize;
		recalculateDistanceBetweenRays();
		createTriggerHelper();
	}

	private void primeRaycastOrigins(Vector3 deltaMovement)
	{
		Vector3 vector = new Vector3(boxCollider.size.x * Mathf.Abs(transform.localScale.x), boxCollider.size.y * Mathf.Abs(transform.localScale.y), boxCollider.size.z * Mathf.Abs(transform.localScale.z)) / 2f;
		Vector3 vector2 = transform.position + new Vector3(boxCollider.center.x * transform.localScale.x, boxCollider.center.y * transform.localScale.y, boxCollider.center.z * transform.localScale.z);
		_raycastOrigins.topRight = vector2 + transform.forward * vector.z + transform.up * vector.y;
		_raycastOrigins.topLeft = vector2 + -transform.forward * vector.z + transform.up * vector.y;
		_raycastOrigins.bottomRight = vector2 + transform.forward * vector.z + -transform.up * vector.y;
		_raycastOrigins.bottomLeft = vector2 + -transform.forward * vector.z + -transform.up * vector.y;
	}

	private void OnDrawGizmos()
	{
		if (Application.isPlaying)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawLine(_raycastOrigins.bottomRight, _raycastOrigins.bottomLeft);
			Gizmos.color = Color.green;
			Gizmos.DrawLine(_raycastOrigins.bottomLeft, _raycastOrigins.topLeft);
			Gizmos.color = Color.blue;
			Gizmos.DrawLine(_raycastOrigins.topLeft, _raycastOrigins.topRight);
			Gizmos.color = Color.yellow;
			Gizmos.DrawLine(_raycastOrigins.topRight, _raycastOrigins.bottomRight);
		}
	}

	private void moveHorizontally(ref Vector3 deltaMovement)
	{
		Vector3 vector = new Vector3(deltaMovement.x, 0f, deltaMovement.z);
		float num = Vector3.Dot(transform.TransformDirection(vector.normalized), transform.forward);
		UnityEngine.Debug.DrawRay(transform.position, transform.TransformDirection(vector.normalized));
		bool flag = num > 0f;
		float num2 = vector.magnitude + _skinWidth;
		Vector3 vector2 = ((!flag) ? (-transform.forward) : transform.forward);
		Vector3 vector3 = ((!flag) ? _raycastOrigins.bottomLeft : _raycastOrigins.bottomRight);
		for (int i = -1; i < totalHorizontalRays; i++)
		{
			Vector3 vector4 = new Vector3(vector3.x, vector3.y + (float)i * _verticalDistanceBetweenRays, vector3.z);
			DrawRay(vector4, vector2 * num2, Color.red);
			bool flag2 = false;
			if ((i != 0 || !collisionState.wasGroundedLastFrame) ? Physics.Raycast(vector4, vector2, out _raycastHit, num2, (int)platformMask & ~(int)oneWayPlatformMask) : Physics.Raycast(vector4, vector2, out _raycastHit, num2, platformMask))
			{
				Vector3 vector5 = transform.InverseTransformDirection(_raycastHit.point - vector4);
				deltaMovement.x = vector5.x - vector5.normalized.x * skinWidth;
				deltaMovement.z = vector5.z - vector5.normalized.z * skinWidth;
				_raycastHitsThisFrame.Add(_raycastHit);
			}
		}
	}

	private bool handleHorizontalSlope(ref Vector3 deltaMovement, float angle)
	{
		if (Mathf.RoundToInt(angle) == 90)
		{
			return false;
		}
		if (angle < slopeLimit)
		{
			if (deltaMovement.y < 0.07f)
			{
				float num = slopeSpeedMultiplier.Evaluate(angle);
				deltaMovement.x *= num;
				deltaMovement.y = Mathf.Abs(Mathf.Tan(angle * ((float)Math.PI / 180f)) * deltaMovement.x);
				_isGoingUpSlope = true;
				collisionState.below = true;
			}
		}
		else
		{
			deltaMovement.x = 0f;
		}
		return true;
	}

	private void moveVertically(ref Vector3 deltaMovement)
	{
		bool flag = deltaMovement.y > 0f;
		float num = Mathf.Abs(deltaMovement.y) + _skinWidth;
		Vector3 vector = ((!flag) ? (-transform.up) : transform.up);
		Vector3 vector2 = ((!flag) ? _raycastOrigins.bottomLeft : _raycastOrigins.topLeft);
		vector2.x += deltaMovement.x;
		LayerMask layerMask = platformMask;
		if (flag && !collisionState.wasGroundedLastFrame)
		{
			layerMask = (int)layerMask & ~(int)oneWayPlatformMask;
		}
		for (int i = 0; i < totalVerticalRays + 1; i++)
		{
			Vector3 vector3 = vector2 + transform.forward * i * _horizontalDistanceBetweenRays;
			DrawRay(vector3, vector * num, Color.red);
			if (Physics.Raycast(vector3, vector, out _raycastHit, num, layerMask))
			{
				deltaMovement.y = _raycastHit.point.y - vector3.y;
				num = Mathf.Abs(deltaMovement.y);
				if (flag)
				{
					deltaMovement.y -= _skinWidth;
					collisionState.above = true;
				}
				else
				{
					deltaMovement.y += _skinWidth;
					collisionState.below = true;
				}
				_raycastHitsThisFrame.Add(_raycastHit);
				if (!flag && deltaMovement.y > 1E-05f)
				{
					_isGoingUpSlope = true;
				}
				if (num < _skinWidth + 0.001f)
				{
					break;
				}
			}
		}
	}

	private void handleVerticalSlope(ref Vector3 deltaMovement)
	{
		float num = (_raycastOrigins.bottomLeft.x + _raycastOrigins.bottomRight.x) * 0.5f;
		Vector3 vector = -Vector3.up;
		float num2 = _slopeLimitTangent * (_raycastOrigins.bottomRight.x - num);
		if (num2 <= 0f)
		{
			return;
		}
		Vector3 vector2 = new Vector3(num, _raycastOrigins.bottomLeft.y);
		DrawRay(vector2, vector * num2, Color.yellow);
		if (Physics.Raycast(vector2, vector, out _raycastHit, num2, platformMask))
		{
			float num3 = Vector3.Angle(_raycastHit.normal, Vector3.up);
			if (num3 != 0f && Mathf.Sign(_raycastHit.normal.x) == Mathf.Sign(deltaMovement.x))
			{
				float num4 = slopeSpeedMultiplier.Evaluate(0f - num3);
				deltaMovement.y = _raycastHit.point.y - vector2.y - skinWidth;
				deltaMovement.x *= num4;
				collisionState.movingDownSlope = true;
				collisionState.slopeAngle = num3;
			}
		}
	}
}
