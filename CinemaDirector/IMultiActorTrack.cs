using System.Collections.Generic;
using UnityEngine;

namespace CinemaDirector;

public interface IMultiActorTrack
{
	List<Transform> Actors { get; }
}
