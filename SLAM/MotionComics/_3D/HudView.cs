using SLAM.Engine;

namespace SLAM.MotionComics._3D;

public class HudView : View
{
	public void OnSkipPressed()
	{
		Controller<MotionComicPlayer>().SkipCutscene();
	}
}
