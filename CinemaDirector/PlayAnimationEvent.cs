using UnityEngine;

namespace CinemaDirector;

[CutsceneItem("Animation", "Play Animation", new CutsceneItemGenre[] { CutsceneItemGenre.ActorItem })]
public class PlayAnimationEvent : CinemaActorAction
{
	public AnimationClip animationClip;

	public WrapMode wrapMode;

	public void Update()
	{
		if (wrapMode != WrapMode.Loop && (bool)animationClip)
		{
			base.Duration = animationClip.length;
		}
	}

	public override void Trigger(GameObject Actor)
	{
		Animation component = Actor.GetComponent<Animation>();
		if ((bool)animationClip && (bool)component)
		{
			component.wrapMode = wrapMode;
		}
	}

	public override void UpdateTime(GameObject Actor, float runningTime, float deltaTime)
	{
		Animation component = Actor.GetComponent<Animation>();
		if ((bool)component && !(animationClip == null))
		{
			if (component[animationClip.name] == null)
			{
				component.AddClip(animationClip, animationClip.name);
			}
			AnimationState animationState = component[animationClip.name];
			if (!component.IsPlaying(animationClip.name))
			{
				component.wrapMode = wrapMode;
				component.Play(animationClip.name);
			}
			animationState.time = runningTime;
			animationState.enabled = true;
			component.Sample();
			animationState.enabled = false;
		}
	}

	public override void End(GameObject Actor)
	{
		Animation component = Actor.GetComponent<Animation>();
		if ((bool)component)
		{
			component.Stop();
		}
	}
}
