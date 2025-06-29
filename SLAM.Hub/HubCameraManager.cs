using System;
using System.Collections;
using UnityEngine;

namespace SLAM.Hub;

public class HubCameraManager : MonoBehaviour
{
	[SerializeField]
	private Transform overviewLocator;

	[SerializeField]
	private AnimationClip introAnimationClip;

	[SerializeField]
	private AnimationCurve zoomCurve;

	[SerializeField]
	private float zoomTime;

	private Action onIntroDone;

	[SerializeField]
	private float panSpeed;

	[SerializeField]
	private Vector3 maxOffset;

	[Tooltip("How long to wait before playing idle")]
	[SerializeField]
	private float idleAnimationDelay;

	[SerializeField]
	private AnimationClip idleAnimationClip;

	[SerializeField]
	private HubController hubController;

	private Vector3 offset;

	private float idleCounter;

	private Vector3 panVelocity;

	private Vector3 panAmount;

	private Animation anim;

	public bool PanningAndIdleAllowed { get; set; }

	public bool IsAnimating { get; protected set; }

	public bool IsPlayingIntroAnimation { get; protected set; }

	public bool IsZoomedIn { get; protected set; }

	private void OnDrawGizmos()
	{
		Gizmos.DrawWireCube(overviewLocator.position, maxOffset * 2f);
	}

	private void Awake()
	{
		anim = GetComponent<Animation>();
	}

	private void Update()
	{
		if (IsAnimating || IsPlayingIntroAnimation || IsZoomedIn || hubController.IsSmartphoneVisible)
		{
			return;
		}
		Vector3 vector = Camera.main.ScreenToViewportPoint(Input.mousePosition);
		offset.x = Mathf.Lerp(0f - maxOffset.x, maxOffset.x, vector.x);
		offset.y = Mathf.Lerp(maxOffset.y, 0f - maxOffset.y, vector.y);
		base.transform.position = Vector3.Lerp(base.transform.position, overviewLocator.position - offset, Time.deltaTime * panSpeed);
		if (Input.GetAxis("Mouse X") > 0.2f || Input.GetAxis("Mouse Y") > 0.2f || Input.anyKey || Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2) || hubController.IsFirstPlayViewOpen)
		{
			if (anim.IsPlaying(idleAnimationClip.name))
			{
				anim.Stop(idleAnimationClip.name);
				hubController.FadeToOverview();
				AnimateToOverview(hubController.OnCameraIdleStop);
			}
			idleCounter = 0f;
		}
		else
		{
			idleCounter += Time.deltaTime;
			if (idleCounter > idleAnimationDelay && !anim.IsPlaying(idleAnimationClip.name))
			{
				anim.Play(idleAnimationClip.name);
				hubController.OnCameraIdleStart();
			}
		}
	}

	public void AnimateToOverview(Action onDoneCallback)
	{
		IsZoomedIn = false;
		StartCoroutine(lerpToLocation(overviewLocator, onDoneCallback));
	}

	public float AnimateToLocation(HubLocationProvider location, Action onDoneCallback)
	{
		IsZoomedIn = true;
		if (location.FlyToAnimation != null)
		{
			StartCoroutine(animationToLocation(location, onDoneCallback));
		}
		else
		{
			StartCoroutine(lerpToLocation(location.ZoomInLocation.transform, onDoneCallback));
		}
		return zoomTime;
	}

	public IEnumerator lerpToLocation(Transform targetTransform, Action onDoneCallback)
	{
		IsAnimating = true;
		Stopwatch sw = new Stopwatch(zoomTime);
		Vector3 startPos = base.transform.position;
		Vector3 endPos = targetTransform.position;
		Quaternion startRot = base.transform.rotation;
		Quaternion endRot = targetTransform.rotation;
		while (!sw.Expired)
		{
			yield return null;
			base.transform.position = Vector3.Lerp(startPos, endPos, zoomCurve.Evaluate(sw.Progress));
			base.transform.rotation = Quaternion.Lerp(startRot, endRot, zoomCurve.Evaluate(sw.Progress));
		}
		IsAnimating = false;
		onDoneCallback?.Invoke();
	}

	public void PlayIntroAnimation()
	{
		StartCoroutine(playIntroAnimation());
	}

	public void WarpToLocation(HubLocationProvider location)
	{
		IsZoomedIn = true;
		if (location.FlyToAnimation != null)
		{
			AnimationState animationState = anim[location.FlyToAnimation.name];
			if (anim.GetClip(animationState.name) == null)
			{
				anim.AddClip(animationState.clip, animationState.name);
			}
			animationState.normalizedTime = 1f;
			animationState.enabled = true;
			anim.Sample();
			animationState.enabled = false;
		}
		else
		{
			base.transform.position = location.ZoomInLocation.transform.position;
			base.transform.rotation = location.ZoomInLocation.transform.rotation;
		}
	}

	private IEnumerator playIntroAnimation()
	{
		IsPlayingIntroAnimation = true;
		anim.Play(introAnimationClip.name);
		while (anim.IsPlaying(introAnimationClip.name))
		{
			if (Input.anyKey || Input.GetMouseButton(0))
			{
				anim.Stop(introAnimationClip.name);
				break;
			}
			yield return null;
		}
		anim[introAnimationClip.name].normalizedTime = 1f;
		anim[introAnimationClip.name].enabled = true;
		anim.Sample();
		IsPlayingIntroAnimation = false;
		yield return null;
		GetComponentInChildren<SpriteRenderer>().enabled = false;
	}

	private IEnumerator animationToLocation(HubLocationProvider location, Action onDoneCallback)
	{
		IsAnimating = true;
		if (anim.GetClip(location.FlyToAnimation.name) == null)
		{
			anim.AddClip(location.FlyToAnimation, location.FlyToAnimation.name);
		}
		anim.Play(location.FlyToAnimation.name);
		yield return new WaitForSeconds(location.FlyToAnimation.length);
		IsAnimating = false;
	}
}
