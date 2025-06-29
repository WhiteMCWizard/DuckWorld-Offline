using UnityEngine;

namespace CinemaDirector;

[TimelineTrack("Dialogue Track", TimelineTrackGenre.CharacterTrack, new CutsceneItemGenre[] { CutsceneItemGenre.AudioClipItem })]
public class DialogueTrack : AudioTrack, IActorTrack
{
	[SerializeField]
	private Transform anchor;

	public Transform Actor
	{
		get
		{
			ActorTrackGroup component = base.transform.parent.GetComponent<ActorTrackGroup>();
			if (component == null)
			{
				return null;
			}
			return component.Actor;
		}
	}

	public override void Initialize()
	{
		base.Initialize();
		setTransform();
	}

	public override void UpdateTrack(float time, float deltaTime)
	{
		setTransform();
		base.UpdateTrack(time, deltaTime);
	}

	private void setTransform()
	{
		if (anchor != null)
		{
			base.transform.position = anchor.position;
		}
		else if (Actor != null)
		{
			base.transform.position = Actor.transform.position;
		}
	}
}
