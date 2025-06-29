using System.Collections.Generic;
using UnityEngine;

namespace CinemaDirector;

[TrackGroup("MultiActor Track Group", new TimelineTrackGenre[] { TimelineTrackGenre.MultiActorTrack })]
public class MultiActorTrackGroup : TrackGroup
{
	[SerializeField]
	private List<Transform> actors = new List<Transform>();

	public List<Transform> Actors
	{
		get
		{
			return actors;
		}
		set
		{
			actors = value;
		}
	}
}
