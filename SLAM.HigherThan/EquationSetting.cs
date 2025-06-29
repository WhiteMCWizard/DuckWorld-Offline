using System;

namespace SLAM.HigherThan;

[Serializable]
public class EquationSetting
{
	public EquationType Type;

	public int MinNumber;

	public int MaxNumber;

	public bool RestrictedToTenths;

	public int[] ExcludedNumbers;
}
