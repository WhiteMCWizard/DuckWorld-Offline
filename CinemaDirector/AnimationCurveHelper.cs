using UnityEngine;

namespace CinemaDirector;

public static class AnimationCurveHelper
{
	public static int AddKey(AnimationCurve curve, Keyframe keyframe)
	{
		if (curve.length == 0)
		{
			return curve.AddKey(keyframe);
		}
		if (curve.length == 1)
		{
			Keyframe key = curve[0];
			int num = curve.AddKey(keyframe);
			switch (num)
			{
			case -1:
				return 0;
			case 0:
				curve.MoveKey(1, key);
				break;
			default:
				curve.MoveKey(0, key);
				break;
			}
			return num;
		}
		Keyframe key2 = default(Keyframe);
		Keyframe key3 = default(Keyframe);
		for (int i = 0; i < curve.length - 1; i++)
		{
			Keyframe keyframe2 = curve[i];
			Keyframe keyframe3 = curve[i + 1];
			if (keyframe2.time < keyframe.time && keyframe.time < keyframe3.time)
			{
				key2 = keyframe2;
				key3 = keyframe3;
			}
		}
		int num2 = curve.AddKey(keyframe);
		if (num2 > 0)
		{
			curve.MoveKey(num2 - 1, key2);
			int tangentMode = curve[num2 - 1].tangentMode;
			if (IsAuto(tangentMode))
			{
				curve.SmoothTangents(num2 - 1, 0f);
			}
			if (IsBroken(tangentMode) && IsRightLinear(tangentMode))
			{
				SetKeyRightLinear(curve, num2 - 1);
			}
		}
		if (num2 < curve.length - 1)
		{
			curve.MoveKey(num2 + 1, key3);
			int tangentMode2 = curve[num2 + 1].tangentMode;
			if (IsAuto(tangentMode2))
			{
				curve.SmoothTangents(num2 + 1, 0f);
			}
			if (IsBroken(tangentMode2) && IsLeftLinear(tangentMode2))
			{
				SetKeyLeftLinear(curve, num2 + 1);
			}
		}
		return num2;
	}

	public static int MoveKey(AnimationCurve curve, int index, Keyframe keyframe)
	{
		keyframe.tangentMode = curve[index].tangentMode;
		int num = curve.MoveKey(index, keyframe);
		if (IsAuto(keyframe.tangentMode))
		{
			curve.SmoothTangents(num, 0f);
		}
		else if (IsBroken(keyframe.tangentMode))
		{
			if (IsLeftLinear(keyframe.tangentMode))
			{
				SetKeyLeftLinear(curve, num);
			}
			if (IsRightLinear(keyframe.tangentMode))
			{
				SetKeyRightLinear(curve, num);
			}
		}
		if (num > 0)
		{
			int tangentMode = curve[num - 1].tangentMode;
			if (IsAuto(tangentMode))
			{
				curve.SmoothTangents(num - 1, 0f);
			}
			if (IsBroken(tangentMode) && IsRightLinear(tangentMode))
			{
				SetKeyRightLinear(curve, num - 1);
			}
		}
		if (num < curve.length - 1)
		{
			int tangentMode2 = curve[num + 1].tangentMode;
			if (IsAuto(tangentMode2))
			{
				curve.SmoothTangents(num + 1, 0f);
			}
			if (IsBroken(tangentMode2) && IsLeftLinear(tangentMode2))
			{
				SetKeyLeftLinear(curve, num + 1);
			}
		}
		return num;
	}

	public static void RemoveKey(AnimationCurve curve, int index)
	{
		curve.RemoveKey(index);
		if (index > 0)
		{
			int tangentMode = curve[index - 1].tangentMode;
			if (IsAuto(tangentMode))
			{
				curve.SmoothTangents(index - 1, 0f);
			}
			if (IsBroken(tangentMode) && IsRightLinear(tangentMode))
			{
				SetKeyRightLinear(curve, index - 1);
			}
		}
		if (index < curve.length)
		{
			int tangentMode2 = curve[index].tangentMode;
			if (IsAuto(tangentMode2))
			{
				curve.SmoothTangents(index, 0f);
			}
			if (IsBroken(tangentMode2) && IsLeftLinear(tangentMode2))
			{
				SetKeyLeftLinear(curve, index);
			}
		}
	}

	public static void SetKeyRightLinear(AnimationCurve curve, int index)
	{
		Keyframe keyframe = curve[index];
		float outTangent = keyframe.outTangent;
		if (index < curve.length - 1)
		{
			Keyframe keyframe2 = curve[index + 1];
			outTangent = (keyframe2.value - keyframe.value) / (keyframe2.time - keyframe.time);
		}
		Keyframe key = new Keyframe(keyframe.time, keyframe.value, keyframe.inTangent, outTangent);
		int num = ((!IsAuto(keyframe.tangentMode) && keyframe.tangentMode != 0) ? (keyframe.tangentMode % 8 - 1) : 0);
		key.tangentMode = num + 16 + 1;
		curve.MoveKey(index, key);
	}

	public static void SetKeyLeftLinear(AnimationCurve curve, int index)
	{
		Keyframe keyframe = curve[index];
		float inTangent = keyframe.inTangent;
		if (index > 0)
		{
			Keyframe keyframe2 = curve[index - 1];
			inTangent = (keyframe.value - keyframe2.value) / (keyframe.time - keyframe2.time);
		}
		Keyframe key = new Keyframe(keyframe.time, keyframe.value, inTangent, keyframe.outTangent);
		int num = ((keyframe.tangentMode > 16) ? (keyframe.tangentMode / 8 * 8) : 0);
		key.tangentMode = num + 1 + 4;
		curve.MoveKey(index, key);
	}

	public static bool IsAuto(int tangentMode)
	{
		return tangentMode == 10;
	}

	public static bool IsBroken(int tangentMode)
	{
		return tangentMode % 2 == 1;
	}

	public static bool IsRightLinear(int tangentMode)
	{
		return tangentMode / 8 == 2;
	}

	public static bool IsLeftLinear(int tangentMode)
	{
		return IsBroken(tangentMode) && tangentMode % 8 == 5;
	}
}
