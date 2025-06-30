using CinemaDirector;
using UnityEngine;

namespace SLAM.MotionComics._3D;

[CutsceneItem("SLAM", "Close Dialog", new CutsceneItemGenre[] { CutsceneItemGenre.GlobalItem })]
public class CloseDialogGlobal : CinemaGlobalEvent
{
	public override void Trigger()
	{
		Object.FindObjectOfType<MotionComicPlayer>().CloseDialog(base.TimelineTrack);
	}
}
