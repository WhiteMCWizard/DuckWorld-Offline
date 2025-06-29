using UnityEngine;

namespace SLAM.InputSystem;

public class JoystickAxisControl : JoystickControl
{
	[SerializeField]
	private string xAxisName = "Horizontal";

	[SerializeField]
	private string yAxisName = "Vertical";

	protected override void Reset()
	{
		base.Reset();
		inputCurve = new AnimationCurve(new Keyframe(0.01f, 0f, 1f, 1f), new Keyframe(1f, 1f, 1f, 1f));
	}

	protected override void Update()
	{
		base.Update();
		(SLAMInput.Provider as MobileInputProvider).SetAxis(xAxisName, inputAxis.x);
		(SLAMInput.Provider as MobileInputProvider).SetAxis(yAxisName, inputAxis.y);
	}
}
