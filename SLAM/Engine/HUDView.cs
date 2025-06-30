namespace SLAM.Engine;

public class HUDView : View
{
	public virtual void OnPauseClicked()
	{
		Controller<GameController>().Pause();
	}
}
