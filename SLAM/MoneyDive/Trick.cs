using System;

namespace SLAM.MoneyDive;

[Serializable]
public class Trick
{
	public string Id;

	public string Name;

	public float Duration;

	public TrickComplexity Complexity;
}
