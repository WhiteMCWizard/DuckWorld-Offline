using System.Collections;
using SLAM.Engine;
using UnityEngine;

namespace SLAM.BeatTheBeagleBoys;

[RequireComponent(typeof(FiniteStateMachine))]
public class BTBCamera : MonoBehaviour
{
	private const string STATE_IDLE = "Idle";

	private const string STATE_ALERTED = "Alerted";

	[SerializeField]
	[Header("Camera rotation settings")]
	private float maxAngle = 45f;

	[SerializeField]
	private float rotationDuration = 1f;

	[SerializeField]
	private float rotationWaitTime = 0.5f;

	[SerializeField]
	private float trackingSpeed = 2f;

	[SerializeField]
	private float characterDetectionAngle = 0.3f;

	[SerializeField]
	private AnimationCurve rotationCurve;

	protected FiniteStateMachine stateMachine;

	private BTBArea area;

	private Camera cameraComp;

	private Quaternion originalRotation;

	private BTBCharacter targetCharacter;

	private Vector3 originalForward;

	public int ThiefsEncountered { get; set; }

	public Camera CameraComp => cameraComp;

	private void OnDrawGizmos()
	{
		Quaternion quaternion = ((!Application.isPlaying) ? base.transform.rotation : originalRotation);
		Vector3 to = quaternion * Quaternion.Euler(0f, 0f - maxAngle, 0f) * Vector3.forward + base.transform.position;
		Vector3 to2 = quaternion * Quaternion.Euler(0f, maxAngle, 0f) * Vector3.forward + base.transform.position;
		Gizmos.color = Color.red;
		Gizmos.DrawLine(base.transform.position, to);
		Gizmos.DrawLine(base.transform.position, to2);
		if (base.enabled && Application.isPlaying)
		{
			Gizmos.DrawSphere(base.transform.position, 0.1f);
		}
		Gizmos.color = Color.blue;
		GizmosUtils.DrawArrow(base.transform.position, base.transform.forward);
	}

	private void OnEnable()
	{
		if (area != null && area.HasThief)
		{
			base.transform.rotation = Quaternion.LookRotation((area.CurrentThief.transform.position - base.transform.position).normalized);
		}
	}

	private void Awake()
	{
		area = base.transform.GetComponentInParent<BTBArea>();
		cameraComp = GetComponent<Camera>();
		if (!(area == null))
		{
			originalRotation = base.transform.rotation;
			originalForward = base.transform.forward;
			stateMachine = GetComponent<FiniteStateMachine>();
			stateMachine.AddState("Idle", OnEnterStateIdle, WhileStateIdle, OnExitStateIdle);
			stateMachine.AddState("Alerted", null, WhileStateAlerted, null);
			stateMachine.SwitchTo("Idle");
		}
	}

	private void updateCameraUI()
	{
		if (!area.IsMonitored)
		{
			cameraComp.enabled = false;
		}
	}

	public RenderTexture GetRenderTexture(int width, int height)
	{
		if (cameraComp.targetTexture == null)
		{
			cameraComp.targetTexture = new RenderTexture(width, height, 24);
		}
		return cameraComp.targetTexture;
	}

	private void OnEnterStateIdle()
	{
		StartCoroutine(doCameraRotation());
	}

	private IEnumerator doCameraRotation()
	{
		while (true)
		{
			yield return StartCoroutine(rotateTo(originalRotation * Quaternion.Euler(0f, 0f - maxAngle, 0f)));
			yield return new WaitForSeconds(rotationWaitTime);
			yield return StartCoroutine(rotateTo(originalRotation * Quaternion.Euler(0f, maxAngle, 0f)));
			yield return new WaitForSeconds(rotationWaitTime);
		}
	}

	private IEnumerator rotateTo(Quaternion endRotation)
	{
		Quaternion startRotationX = base.transform.rotation;
		Quaternion startRotationY = base.transform.parent.rotation;
		Stopwatch sw = new Stopwatch(rotationDuration);
		Quaternion rotationX = default(Quaternion);
		Quaternion rotationY = default(Quaternion);
		while ((bool)sw)
		{
			yield return null;
			rotationX = Quaternion.Slerp(startRotationX, endRotation, rotationCurve.Evaluate(sw.Progress));
			rotationY = Quaternion.Slerp(startRotationY, endRotation, rotationCurve.Evaluate(sw.Progress));
			base.transform.localRotation = Quaternion.Euler(rotationX.eulerAngles.x, 0f, 0f);
			base.transform.parent.rotation = Quaternion.Euler(0f, rotationY.eulerAngles.y, 0f);
		}
	}

	private void WhileStateIdle()
	{
		updateCameraUI();
		if (area.HasThief)
		{
			targetCharacter = area.CurrentThief;
		}
		if (targetCharacter != null)
		{
			stateMachine.SwitchTo("Alerted");
		}
	}

	private bool isLookingAtCharacter(BTBCharacter character)
	{
		float num = Vector3.Dot(base.transform.forward, (character.transform.position - base.transform.position).normalized);
		return num > 1f - characterDetectionAngle;
	}

	private void OnExitStateIdle()
	{
		StopAllCoroutines();
	}

	private void WhileStateAlerted()
	{
		updateCameraUI();
		if (targetCharacter == null)
		{
			stateMachine.SwitchTo("Idle");
		}
		else
		{
			LookAtTarget(targetCharacter.transform.position + Vector3.up * 1.5f);
		}
	}

	public void LookAtTarget(Vector3 target)
	{
		Quaternion b = Quaternion.LookRotation((target - base.transform.position).normalized);
		Quaternion quaternion = Quaternion.Slerp(base.transform.rotation, b, Time.deltaTime * trackingSpeed);
		Quaternion quaternion2 = Quaternion.Slerp(base.transform.parent.rotation, b, Time.deltaTime * trackingSpeed);
		float num = quaternion.eulerAngles.x;
		float num2 = quaternion2.eulerAngles.y;
		if (Vector3.Angle(originalForward, target) > maxAngle)
		{
			num = Mathf.Clamp(num, originalRotation.eulerAngles.x - maxAngle, originalRotation.eulerAngles.x + maxAngle);
			num2 = Mathf.Clamp(num2, originalRotation.eulerAngles.y - maxAngle, originalRotation.eulerAngles.y + maxAngle);
		}
		base.transform.localRotation = Quaternion.Euler(num, 0f, 0f);
		base.transform.parent.rotation = Quaternion.Euler(0f, num2, 0f);
	}
}
