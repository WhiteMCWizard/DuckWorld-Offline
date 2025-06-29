using SLAM.InputSystem;

public static class SLAMInput
{
	public enum Button
	{
		UpOrAction,
		Down,
		Left,
		Right,
		Action,
		Up
	}

	public static IInputProvider Provider { get; private set; }

	static SLAMInput()
	{
		Provider = new DesktopInputProvider();
	}
}
