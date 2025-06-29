using System;
using System.Collections;
using SLAM.CameraSystem;
using UnityEngine;

namespace SLAM.MoneyDive;

public class MD_Cinematics : MonoBehaviour
{
	[SerializeField]
	private Animator avatarAnimator;

	[SerializeField]
	private Animator scroogeAnimator;

	[SerializeField]
	private Animator plankAnimator;

	[SerializeField]
	private Animator camera3Animator;

	[SerializeField]
	private MD_AvatarController avatarController;

	[SerializeField]
	private Transform avatarOutroAnimationLocation;

	[SerializeField]
	private MD_LevelSpawner levelSpawner;

	[SerializeField]
	private CameraManager cameraManager;

	[SerializeField]
	private MD_Controller gameController;

	[SerializeField]
	private CameraBehaviour intro1;

	[SerializeField]
	private CameraBehaviour intro2;

	[SerializeField]
	private CameraBehaviour intro3;

	[SerializeField]
	private CameraBehaviour outro1;

	[SerializeField]
	private float outro1Duration;

	[SerializeField]
	private float outro1CrossFade = 2f;

	[SerializeField]
	private AnimationCurve outro1Curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

	[SerializeField]
	private PrefabSpawner coinSplashParticleSpawner;

	[SerializeField]
	private Transform avatarBottomReachedLocation;

	private float intro1Duration = 1.25f;

	private float intro2Duration = 6.208f;

	private float intro3Duration = 2.917f;

	private float time;

	public bool IsDoingIntroduction { get; private set; }

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void PlayShortIntro(Action callback)
	{
		StartCoroutine(DoShortIntro(callback));
	}

	public void PlayIntro(Action callback)
	{
		StartCoroutine(DoIntro(callback));
	}

	public void SkipIntro(Action callback)
	{
		StopAllCoroutines();
		AudioController.StopCategory("SFX", 0f);
		AudioController.Play("MD_diving_wind_loop");
		gameController.FadeInAndOut(delegate
		{
			scroogeAnimator.transform.parent = avatarOutroAnimationLocation;
			scroogeAnimator.transform.localPosition = Vector3.zero;
			cameraManager.CrossFade(intro3, 0f);
			avatarAnimator.SetTrigger("Idle");
			avatarController.StartFalling();
			avatarController.AreControlsLocked = false;
			if (callback != null)
			{
				callback();
			}
		}, null);
	}

	public void PlayOutro(bool hasWon, Action callback)
	{
		StartCoroutine(DoOutro(hasWon, callback));
	}

	private IEnumerator DoShortIntro(Action callback)
	{
		avatarController.AreControlsLocked = true;
		AudioController.Play("MD_dive_anim", 1f, 0f, intro1Duration + intro2Duration);
		avatarAnimator.SetTrigger("Intro");
		scroogeAnimator.SetTrigger("Intro");
		plankAnimator.SetTrigger("Intro");
		gameController.FadeInAndOut(delegate
		{
			scroogeAnimator.transform.parent = avatarOutroAnimationLocation;
			scroogeAnimator.transform.localPosition = Vector3.zero;
			plankAnimator.SetTrigger("Intro");
			avatarAnimator.SetTrigger("Intro");
			camera3Animator.SetTrigger("Intro");
			cameraManager.CrossFade(intro3, 0f);
		}, null);
		yield return new WaitForSeconds(intro3Duration + 0.56f);
		AudioController.Play("MD_diving_wind_loop");
		avatarController.StartFalling();
		avatarController.AreControlsLocked = false;
		callback?.Invoke();
	}

	private IEnumerator DoIntro(Action callback)
	{
		IsDoingIntroduction = true;
		avatarController.AreControlsLocked = true;
		cameraManager.CrossFade(intro1, 0f);
		AudioController.Play("MD_dive_anim");
		avatarAnimator.SetTrigger("Intro");
		scroogeAnimator.SetTrigger("Intro");
		plankAnimator.SetTrigger("Intro");
		yield return new WaitForSeconds(intro1Duration);
		cameraManager.CrossFade(intro2, 0f);
		yield return new WaitForSeconds(intro2Duration);
		gameController.FadeInAndOut(delegate
		{
			scroogeAnimator.transform.parent = avatarOutroAnimationLocation;
			scroogeAnimator.transform.localPosition = Vector3.zero;
			plankAnimator.SetTrigger("Intro");
			avatarAnimator.SetTrigger("Intro");
			camera3Animator.SetTrigger("Intro");
			cameraManager.CrossFade(intro3, 0f);
		}, null);
		yield return new WaitForSeconds(intro3Duration + 0.56f);
		AudioController.Play("MD_diving_wind_loop");
		avatarController.StartFalling();
		avatarController.AreControlsLocked = false;
		IsDoingIntroduction = false;
		callback?.Invoke();
	}

	private IEnumerator DoOutro(bool victory, Action callback)
	{
		StartCoroutine(doStopwatch());
		avatarController.AreControlsLocked = true;
		levelSpawner.StopSpawning();
		cameraManager.CrossFade(outro1, outro1CrossFade, outro1Curve);
		scroogeAnimator.SetTrigger("Idle");
		if (victory)
		{
			AudioController.Play("MD_landing_succes");
		}
		else
		{
			AudioController.Play("MD_landing_fail");
		}
		while (avatarController.transform.position.y > avatarBottomReachedLocation.position.y)
		{
			yield return null;
		}
		avatarController.StopFalling();
		avatarController.transform.position = avatarOutroAnimationLocation.position;
		coinSplashParticleSpawner.Spawn();
		avatarAnimator.SetTrigger((!victory) ? "Failure" : "Succes");
		scroogeAnimator.SetTrigger((!victory) ? "Failure" : "Succes");
		AudioController.Stop("MD_diving_wind_loop");
		yield return new WaitForSeconds(outro1Duration);
		callback?.Invoke();
	}

	private IEnumerator doStopwatch()
	{
		time = 0f;
		bool run = true;
		while (run)
		{
			time += Time.deltaTime;
			yield return null;
		}
	}
}
