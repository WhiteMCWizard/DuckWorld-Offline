using System;
using System.Collections;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.FollowTheTruck;

public class FTTBeagleBoyController : MonoBehaviour
{
	[Serializable]
	public class CargoObject
	{
		public FTTCargoType Type;

		public GameObject Prefab;

		public AnimationCurve ThrowCurve;
	}

	private const float THROW_ANIMATION_LENGTH = 2.7f;

	[SerializeField]
	private CargoObject[] throwableObjects;

	[SerializeField]
	private Transform cargoItemNullPosition;

	[SerializeField]
	private float throwSpeed = 10f;

	private Animator animator;

	private bool animationFinished;

	private bool isThrowingCargo;

	private void Start()
	{
		animator = GetComponent<Animator>();
	}

	private void OnEnable()
	{
		GameEvents.Subscribe<FTTGameStartedEvent>(onGameStarted);
		GameEvents.Subscribe<FTTGameEndedEvent>(onGameEnded);
		GameEvents.Subscribe<FTTTrowCargoEvent>(onThrowCargo);
		GameEvents.Subscribe<FTTAvatarInWaterEvent>(onAvatarInWater);
		GameEvents.Subscribe<FTTHeartLostEvent>(onLostHeart);
	}

	private void OnDisable()
	{
		StopAllCoroutines();
		GameEvents.Unsubscribe<FTTGameStartedEvent>(onGameStarted);
		GameEvents.Unsubscribe<FTTGameEndedEvent>(onGameEnded);
		GameEvents.Unsubscribe<FTTTrowCargoEvent>(onThrowCargo);
		GameEvents.Unsubscribe<FTTAvatarInWaterEvent>(onAvatarInWater);
		GameEvents.Unsubscribe<FTTHeartLostEvent>(onLostHeart);
	}

	private IEnumerator throwCargoItem(CargoObject cargo, Vector3 to)
	{
		isThrowingCargo = true;
		GameObject cargoObject = UnityEngine.Object.Instantiate(cargo.Prefab, cargoItemNullPosition.position, cargoItemNullPosition.rotation) as GameObject;
		Collider cargoCollider = cargoObject.GetComponent<Collider>();
		Rigidbody cargoRigidBody = cargoObject.GetComponent<Rigidbody>();
		FTTCargoThrownEvent fTTCargoThrownEvent = new FTTCargoThrownEvent();
		fTTCargoThrownEvent.Type = cargo.Type;
		fTTCargoThrownEvent.Object = cargoObject;
		GameEvents.Invoke(fTTCargoThrownEvent);
		animator.SetTrigger("Throw");
		to += Vector3.up * cargoCollider.bounds.extents.y;
		cargoCollider.enabled = false;
		cargoRigidBody.useGravity = false;
		cargoRigidBody.isKinematic = true;
		cargoObject.transform.parent = cargoItemNullPosition;
		while (!animationFinished)
		{
			yield return null;
		}
		animationFinished = false;
		cargoCollider.enabled = true;
		cargoObject.transform.parent.DetachChildren();
		Quaternion fromRot = cargoObject.transform.rotation;
		Quaternion toRot = Quaternion.identity;
		Vector3 from = cargoObject.transform.position;
		float time = Vector3.Distance(from, to) / throwSpeed;
		Stopwatch sw = new Stopwatch(time);
		while (!sw.Expired)
		{
			yield return null;
			if (cargoObject != null)
			{
				cargoObject.transform.rotation = Quaternion.Lerp(fromRot, toRot, cargo.ThrowCurve.Evaluate(sw.Progress));
				cargoObject.transform.position = Vector3.Lerp(from, to, cargo.ThrowCurve.Evaluate(sw.Progress));
			}
		}
		if (cargoObject != null)
		{
			cargoRigidBody.useGravity = true;
			cargoRigidBody.isKinematic = false;
		}
		isThrowingCargo = false;
	}

	private void onThrowCargo(FTTTrowCargoEvent evt)
	{
		if (!isThrowingCargo)
		{
			CargoObject cargoObject = throwableObjects.FirstOrDefault((CargoObject to) => to.Type == evt.Type);
			if (cargoObject == null)
			{
				Debug.LogWarning(string.Concat("Hey Buddy, the cargo type '", evt.Type, "' is unknown to me."), this);
			}
			else
			{
				StartCoroutine(throwCargoItem(cargoObject, evt.Target.position));
			}
		}
	}

	private void onGameStarted(FTTGameStartedEvent evt)
	{
		FTTCargoThrowTrigger[] array = UnityEngine.Object.FindObjectsOfType<FTTCargoThrowTrigger>();
		Vector3 vector = Vector3.back * evt.Settings.AvatarRunSpeed * 2.7f;
		FTTCargoThrowTrigger[] array2 = array;
		foreach (FTTCargoThrowTrigger fTTCargoThrowTrigger in array2)
		{
			fTTCargoThrowTrigger.transform.position = fTTCargoThrowTrigger.transform.position + vector;
			foreach (Transform item in fTTCargoThrowTrigger.transform)
			{
				item.position -= vector;
			}
		}
	}

	private void onAvatarInWater(FTTAvatarInWaterEvent evt)
	{
		if (isThrowingCargo && !animationFinished)
		{
			animator.speed = 0f;
		}
		else
		{
			animator.SetTrigger("Hit");
		}
	}

	private void onLostHeart(FTTHeartLostEvent evt)
	{
		animator.speed = 1f;
	}

	private void onGameEnded(FTTGameEndedEvent evt)
	{
		if (!evt.Success)
		{
			animator.SetBool("PlayerLost", value: true);
		}
	}

	private void ReleaseObject()
	{
		animationFinished = true;
	}
}
