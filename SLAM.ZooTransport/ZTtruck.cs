using System.Collections;
using UnityEngine;

namespace SLAM.ZooTransport;

public class ZTtruck : MonoBehaviour
{
	[SerializeField]
	private float accelForce;

	[SerializeField]
	private float brakeForce;

	[SerializeField]
	private float reverseForce;

	[SerializeField]
	private Transform centerOfMass;

	[SerializeField]
	private ZTwheelScript[] wheels;

	[SerializeField]
	private AudioClip engineSound;

	[SerializeField]
	[Range(0.5f, 1.5f)]
	private float engineIdlePitch = 0.8f;

	[Range(0.5f, 1.5f)]
	[SerializeField]
	private float engineFullPitch = 1.2f;

	[SerializeField]
	private float engineFullThrottleForce = 13f;

	private float minVelocityBrakeSound = 7f;

	private float input;

	private int whereWheelsOnGroundLastFrame = 4;

	private AudioObject engineSoundObj;

	private bool hasReachEnd;

	private Rigidbody rigidBody;

	public static AudioClip engSound;

	public bool HasReachedEnd => hasReachEnd;

	private void Start()
	{
		if (engineSound == null)
		{
			Debug.LogError("No engine sound :(");
			Debug.Log(engSound);
		}
		else
		{
			engineSoundObj = AudioController.Play(engineSound.name);
			engSound = engineSound;
		}
		rigidBody = GetComponent<Rigidbody>();
		WheelCollider[] array = Object.FindObjectsOfType<WheelCollider>();
		WheelCollider[] array2 = array;
		foreach (WheelCollider wheelCollider in array2)
		{
			wheelCollider.motorTorque = 0.0001f;
		}
	}

	private void OnEnable()
	{
		GameEvents.Subscribe<ZooTransportGame.ZTTruckReachedEndEevent>(onTruckReachedEnd);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<ZooTransportGame.ZTTruckReachedEndEevent>(onTruckReachedEnd);
		if (engineSoundObj != null)
		{
			engineSoundObj.pitch = 1f;
		}
	}

	private void onTruckReachedEnd(ZooTransportGame.ZTTruckReachedEndEevent evt)
	{
		hasReachEnd = true;
		StartCoroutine(doParkTruck(evt.TruckEndPosition));
	}

	public void FixedUpdate()
	{
		Vector3 velocity = rigidBody.velocity;
		bool flag = velocity.x < 0f;
		int num = 0;
		float num2 = 0f;
		ZTwheelScript[] array = wheels;
		foreach (ZTwheelScript zTwheelScript in array)
		{
			zTwheelScript.ManualFixedUpdate(velocity.x);
			if (zTwheelScript.IsOnGround())
			{
				num2 += 0.5f;
				num++;
			}
		}
		if (input < 0f && num > 0)
		{
			if (flag)
			{
				AudioController.Play("Truck_reverse_beep_loop");
			}
			else if (velocity.magnitude > minVelocityBrakeSound)
			{
				AudioController.Play("Truck_brake_random");
			}
		}
		else if (AudioController.IsPlaying("Truck_brake_random"))
		{
			AudioController.Stop("Truck_brake_random");
		}
		float num3 = input * ((input > 0f) ? accelForce : ((!flag) ? brakeForce : reverseForce));
		rigidBody.AddForce(base.transform.forward * num3 * Time.fixedDeltaTime * num2, ForceMode.Acceleration);
		input = 0f;
		if (num != whereWheelsOnGroundLastFrame)
		{
			AudioController.Play("Truck_suspension");
		}
		if (engineSoundObj != null)
		{
			engineSoundObj.pitch = Mathf.Lerp(engineIdlePitch, engineFullPitch, Mathf.Abs(rigidBody.velocity.x / engineFullThrottleForce));
		}
		whereWheelsOnGroundLastFrame = num;
	}

	public void ManualUpdate()
	{
		if (base.enabled)
		{
			input = 0f;
			if (SLAMInput.Provider.GetButton(SLAMInput.Button.Left))
			{
				input += -1f;
			}
			if (SLAMInput.Provider.GetButton(SLAMInput.Button.Right))
			{
				input += 1f;
			}
		}
	}

	public IEnumerator DoStopTruck()
	{
		base.enabled = false;
		if (rigidBody.velocity.magnitude > minVelocityBrakeSound)
		{
			AudioController.Play("Truck_brake_random");
		}
		Stopwatch sw = new Stopwatch(1f);
		float startVelX = rigidBody.velocity.x;
		while (!sw.Expired)
		{
			yield return null;
			if (rigidBody.isKinematic)
			{
				rigidBody.isKinematic = false;
			}
			rigidBody.velocity = Vector3.Lerp(new Vector3(startVelX, rigidBody.velocity.y, rigidBody.velocity.z), new Vector3(0f, rigidBody.velocity.y, rigidBody.velocity.z), sw.Progress);
			if (engineSoundObj != null)
			{
				engineSoundObj.pitch = Mathf.Lerp(engineIdlePitch, engineFullPitch, rigidBody.velocity.x / engineFullThrottleForce);
			}
		}
		rigidBody.velocity = Vector3.zero;
		rigidBody.isKinematic = true;
	}

	private IEnumerator doParkTruck(Vector3 endPosition)
	{
		base.enabled = false;
		if (rigidBody.velocity.magnitude > minVelocityBrakeSound)
		{
			AudioController.Play("Truck_brake_random");
		}
		float startVelX = rigidBody.velocity.x;
		float targetVelX = 10f;
		float distanceTotal = Vector3.Distance(base.transform.position, endPosition);
		float currentDistance = distanceTotal;
		if (rigidBody.isKinematic)
		{
			rigidBody.isKinematic = false;
		}
		while (currentDistance > 0.5f)
		{
			yield return null;
			if (currentDistance < distanceTotal / 2f && targetVelX > 0f)
			{
				distanceTotal = Vector3.Distance(base.transform.position, endPosition);
				startVelX = rigidBody.velocity.x;
				targetVelX = 0f;
			}
			currentDistance = Vector3.Distance(base.transform.position, endPosition);
			float progress = 1f - currentDistance / distanceTotal;
			rigidBody.velocity = Vector3.Lerp(new Vector3(startVelX, rigidBody.velocity.y, rigidBody.velocity.z), new Vector3(targetVelX, rigidBody.velocity.y, rigidBody.velocity.z), progress);
			engineSoundObj.pitch = Mathf.Lerp(engineIdlePitch, engineFullPitch, rigidBody.velocity.x / engineFullThrottleForce);
		}
		rigidBody.velocity = Vector3.zero;
		rigidBody.isKinematic = true;
		GameEvents.Invoke(new ZooTransportGame.ZTGameEndEvent());
	}
}
