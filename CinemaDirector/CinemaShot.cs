using UnityEngine;

namespace CinemaDirector;

[CutsceneItem("Shots", "Shot", new CutsceneItemGenre[] { CutsceneItemGenre.CameraShot })]
public class CinemaShot : CinemaGlobalAction
{
	public Camera shotCamera;

	private bool cachedState;

	public float CutTime
	{
		get
		{
			return base.Firetime;
		}
		set
		{
			base.Firetime = value;
		}
	}

	public float ShotLength
	{
		get
		{
			return base.Duration;
		}
		set
		{
			base.Duration = value;
		}
	}

	public override void Initialize()
	{
		if (shotCamera != null)
		{
			cachedState = shotCamera.gameObject.activeInHierarchy;
		}
	}

	public override void Trigger()
	{
		if (shotCamera != null)
		{
			shotCamera.gameObject.SetActive(value: true);
		}
	}

	public override void End()
	{
		if (shotCamera != null)
		{
			shotCamera.gameObject.SetActive(value: false);
		}
	}

	public override void Stop()
	{
		if (shotCamera != null)
		{
			shotCamera.gameObject.SetActive(cachedState);
		}
	}
}
