using UnityEngine;

namespace CinemaDirector;

[CutsceneItem("Animator", "Look At Target", new CutsceneItemGenre[]
{
	CutsceneItemGenre.ActorItem,
	CutsceneItemGenre.MecanimItem
})]
public class LookAtTarget : CinemaActorAction
{
	public Transform LookAtPosition;

	public float Weight;

	public float BodyWeight;

	public float HeadWeight = 1f;

	public float EyesWeight;

	public float ClampWeight = 0.5f;

	public override void Trigger(GameObject Actor)
	{
		Animator component = Actor.GetComponent<Animator>();
		if (!(component == null))
		{
			component.SetLookAtPosition(LookAtPosition.position);
		}
	}

	public override void UpdateTime(GameObject Actor, float runningTime, float deltaTime)
	{
		Animator component = Actor.GetComponent<Animator>();
		if (!(component == null))
		{
			component.SetLookAtPosition(LookAtPosition.position);
			component.SetLookAtWeight(Weight, BodyWeight, HeadWeight, EyesWeight, ClampWeight);
		}
	}

	public override void End(GameObject Actor)
	{
		Animator component = Actor.GetComponent<Animator>();
		if ((bool)component)
		{
			component.SetLookAtWeight(0f);
		}
	}
}
