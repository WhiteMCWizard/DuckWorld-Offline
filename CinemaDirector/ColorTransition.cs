using UnityEngine;

namespace CinemaDirector;

[CutsceneItem("Transitions", "Color Transition", new CutsceneItemGenre[] { CutsceneItemGenre.GlobalItem })]
public class ColorTransition : CinemaGlobalAction
{
	public Color From = Color.black;

	public Color To = Color.clear;

	private void Awake()
	{
		if (base.gameObject.GetComponent<GUITexture>() == null)
		{
			base.gameObject.transform.position = Vector3.zero;
			base.gameObject.transform.localScale = new Vector3(100f, 100f, 100f);
			GUITexture gUITexture = base.gameObject.AddComponent<GUITexture>();
			gUITexture.enabled = false;
			gUITexture.texture = new Texture2D(1, 1);
			gUITexture.pixelInset = new Rect(0f, 0f, Screen.width, Screen.height);
			gUITexture.color = Color.clear;
		}
	}

	public override void Trigger()
	{
		GUITexture component = base.gameObject.GetComponent<GUITexture>();
		if (component != null)
		{
			component.enabled = true;
			component.pixelInset = new Rect(0f, 0f, Screen.width, Screen.height);
			component.color = From;
		}
	}

	public override void ReverseTrigger()
	{
		End();
	}

	public override void UpdateTime(float time, float deltaTime)
	{
		float transition = time / base.Duration;
		FadeToColor(From, To, transition);
	}

	public override void SetTime(float time, float deltaTime)
	{
		GUITexture component = base.gameObject.GetComponent<GUITexture>();
		if (component != null)
		{
			if (time >= 0f && time <= base.Duration)
			{
				component.enabled = true;
				UpdateTime(time, deltaTime);
			}
			else if (component.enabled)
			{
				component.enabled = false;
			}
		}
	}

	public override void End()
	{
		GUITexture component = base.gameObject.GetComponent<GUITexture>();
		if (component != null)
		{
			component.enabled = false;
		}
	}

	public override void ReverseEnd()
	{
		GUITexture component = base.gameObject.GetComponent<GUITexture>();
		if (component != null)
		{
			component.enabled = true;
			component.pixelInset = new Rect(0f, 0f, Screen.width, Screen.height);
			component.color = To;
		}
	}

	public override void Stop()
	{
		GUITexture component = base.gameObject.GetComponent<GUITexture>();
		if (component != null)
		{
			component.enabled = false;
		}
	}

	private void FadeToColor(Color from, Color to, float transition)
	{
		GUITexture component = base.gameObject.GetComponent<GUITexture>();
		if (component != null)
		{
			component.color = Color.Lerp(from, to, transition);
		}
	}
}
