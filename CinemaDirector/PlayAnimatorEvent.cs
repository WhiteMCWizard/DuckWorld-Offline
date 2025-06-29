using UnityEngine;

namespace CinemaDirector;

[CutsceneItem("Animator", "Play Mecanim Animation", new CutsceneItemGenre[]
{
	CutsceneItemGenre.ActorItem,
	CutsceneItemGenre.MecanimItem
})]
public class PlayAnimatorEvent : CinemaActorEvent
{
	public string StateName;

	public int Layer = -1;

	private float Normalizedtime = float.NegativeInfinity;

	public override void Trigger(GameObject actor)
	{
		Animator component = actor.GetComponent<Animator>();
		if (!(component == null))
		{
			component.Play(StateName, Layer, Normalizedtime);
		}
	}
}
