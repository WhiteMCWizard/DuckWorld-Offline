using System;

public static class SystemTime
{
	private static double _timeAtLaunch = time;

	public static double time
	{
		get
		{
			long ticks = DateTime.Now.Ticks;
			return (double)ticks * 1E-07;
		}
	}

	public static double timeSinceLaunch => time - _timeAtLaunch;
}
