using System;
using UnityEngine;

namespace SLAM.InputSystem;

public class JoystickKeyControl : JoystickControl
{
	[Serializable]
	private struct AxisKeyMapping
	{
		[SerializeField]
		private Vector3 threshold;

		[SerializeField]
		private KeyCode keyCode;

		[SerializeField]
		private float angleThreshold;

		public KeyCode KeyCode => keyCode;

		public AxisKeyMapping(Vector3 threshold, KeyCode keyCode, float angleThreshold)
		{
			this.threshold = threshold;
			this.keyCode = keyCode;
			this.angleThreshold = angleThreshold;
		}

		public bool OverThreshold(Vector3 input)
		{
			if (input.sqrMagnitude < threshold.sqrMagnitude)
			{
				return false;
			}
			float num = Vector3.Dot(input.normalized, threshold.normalized);
			return num >= 1f - angleThreshold && num <= 1f + angleThreshold;
		}
	}

	[SerializeField]
	private AxisKeyMapping[] keys;

	protected Vector3 prevInputAxis;

	protected override void Reset()
	{
		base.Reset();
		inputDecay = 0f;
		inputCurve = new AnimationCurve(new Keyframe(0f, 0f, 1f, 1f), new Keyframe(0.5f, 1f, 1f, 1f));
		keys = new AxisKeyMapping[4]
		{
			new AxisKeyMapping(new Vector3(0f, 0.5f), KeyCode.UpArrow, 0.35f),
			new AxisKeyMapping(new Vector3(0f, -0.5f), KeyCode.DownArrow, 0.35f),
			new AxisKeyMapping(new Vector3(0.5f, 0f), KeyCode.RightArrow, 0.35f),
			new AxisKeyMapping(new Vector3(-0.5f, 0f), KeyCode.LeftArrow, 0.35f)
		};
	}

	protected override void Update()
	{
		base.Update();
		for (int i = 0; i < keys.Length; i++)
		{
			if (keys[i].OverThreshold(inputAxis) && !keys[i].OverThreshold(prevInputAxis))
			{
				(SLAMInput.Provider as MobileInputProvider).SetKeyDown(keys[i].KeyCode);
			}
			if (!keys[i].OverThreshold(inputAxis) && keys[i].OverThreshold(prevInputAxis))
			{
				(SLAMInput.Provider as MobileInputProvider).SetKeyUp(keys[i].KeyCode);
			}
		}
		prevInputAxis = inputAxis;
	}
}
