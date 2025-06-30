using CinemaDirector;
using UnityEngine;

namespace SLAM.MotionComics._3D;

[CutsceneItem("SLAM", "Pause until interact", new CutsceneItemGenre[] { CutsceneItemGenre.GlobalItem })]
public class WaitForInteractGlobal : CinemaGlobalEvent
{
	[SerializeField]
	private AudioClip clipToPlayAtInteract;

	public override void Trigger()
	{
		Object.FindObjectOfType<MotionComicPlayer>().ShowInteractButton(clipToPlayAtInteract);
	}
}
