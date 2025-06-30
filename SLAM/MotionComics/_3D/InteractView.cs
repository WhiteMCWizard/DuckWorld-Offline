using SLAM.Engine;

namespace SLAM.MotionComics._3D;

public class InteractView : View
{
	public void ContinueCutscene()
	{
		Controller<MotionComicPlayer>().ResumeCutscene();
	}
}
