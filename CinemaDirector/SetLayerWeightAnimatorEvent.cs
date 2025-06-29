using UnityEngine;

namespace CinemaDirector;

[CutsceneItem("Animator", "Set Layer Weight", new CutsceneItemGenre[]
{
	CutsceneItemGenre.ActorItem,
	CutsceneItemGenre.MecanimItem
})]
public class SetLayerWeightAnimatorEvent : CinemaActorEvent
{
	public int LayerIndex;

	public float Weight;

	public override void Trigger(GameObject actor)
	{
		Animator component = actor.GetComponent<Animator>();
		if (!(component == null))
		{
			component.SetLayerWeight(LayerIndex, Weight);
		}
	}
}
