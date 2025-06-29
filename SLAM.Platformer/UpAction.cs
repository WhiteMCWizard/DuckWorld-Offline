namespace SLAM.Platformer;

public class UpAction
{
	public Action Type;

	public float Duration;

	public P_PressUpTrigger DataContext;

	public static UpAction NoAction => new UpAction();

	public UpAction()
	{
	}

	public UpAction(Action type, float duration, P_PressUpTrigger context)
	{
		Type = type;
		Duration = duration;
		DataContext = context;
	}
}
