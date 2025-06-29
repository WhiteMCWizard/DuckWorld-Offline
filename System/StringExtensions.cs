namespace System;

public static class StringExtensions
{
	public static string PadBoth(this string str, int length)
	{
		int num = length - str.Length;
		int totalWidth = num / 2 + str.Length;
		return str.PadLeft(totalWidth).PadRight(length);
	}
}
