using UnityEngine;

namespace CinemaDirector;

[TrackGroup("Actor Track Group", new TimelineTrackGenre[] { TimelineTrackGenre.ActorTrack })]
public class ActorTrackGroup : TrackGroup
{
	[SerializeField]
	private Transform actor;

	public Transform Actor
	{
		get
		{
			return actor;
		}
		set
		{
			actor = value;
		}
	}
}
