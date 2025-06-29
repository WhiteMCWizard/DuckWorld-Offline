using UnityEngine;

namespace CinemaDirector;

[CutsceneItem("Animator", "Set Look At Weight", new CutsceneItemGenre[]
{
	CutsceneItemGenre.ActorItem,
	CutsceneItemGenre.MecanimItem
})]
public class SetLookAtWeightAnimatorEvent : CinemaActorEvent
{
	public float Weight;

	public float BodyWeight;

	public float HeadWeight = 1f;

	public float EyesWeight;

	public float ClampWeight = 0.5f;

	public override void Trigger(GameObject actor)
	{
		Animator component = actor.GetComponent<Animator>();
		if (!(component == null))
		{
			component.SetLookAtWeight(Weight, BodyWeight, HeadWeight, EyesWeight, ClampWeight);
		}
	}
}
