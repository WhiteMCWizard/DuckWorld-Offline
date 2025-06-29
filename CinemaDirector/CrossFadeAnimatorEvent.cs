using UnityEngine;

namespace CinemaDirector;

[CutsceneItem("Animator", "Cross Fade Animator", new CutsceneItemGenre[]
{
	CutsceneItemGenre.ActorItem,
	CutsceneItemGenre.MecanimItem
})]
public class CrossFadeAnimatorEvent : CinemaActorEvent
{
	public string AnimationStateName = string.Empty;

	public float TransitionDuration = 1f;

	public int Layer = -1;

	private float NormalizedTime = float.NegativeInfinity;

	public override void Trigger(GameObject actor)
	{
		Animator component = actor.GetComponent<Animator>();
		if (!(component == null))
		{
			component.CrossFade(AnimationStateName, TransitionDuration, Layer, NormalizedTime);
		}
	}
}
