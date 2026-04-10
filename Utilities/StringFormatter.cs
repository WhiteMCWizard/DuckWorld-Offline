using System;
using UnityEngine;

public static class StringFormatter
{
	public static string GetLocalizationFormatted(string key, params object[] args)
	{
		string result = key;
		try
		{
			result = string.Format(Localization.Get(key), args);
		}
		catch
		{
			Debug.LogError("LocalisationKey: " + key + " required more then the supllied " + args.Length + " args!");
		}
		return result;
	}

	public static string GetFormattedDate(DateTime date)
	{
		return string.Format("{0:" + Localization.Get("UI_DATE_FORMAT") + "}", date);
	}

	public static string GetFormattedTime(float elapsedSeconds, bool miliseconds)
	{
		if (miliseconds)
		{
			return $"{(int)elapsedSeconds / 60:00}:{(int)elapsedSeconds % 60:00}:{(elapsedSeconds - (float)(int)elapsedSeconds) * 100f:00}";
		}
		return $"{(int)elapsedSeconds / 60:00}:{(int)elapsedSeconds % 60:00}";
	}

	public static string GetFormattedTime(float elapsedSeconds)
	{
		return GetFormattedTime(elapsedSeconds, miliseconds: false);
	}

	public static string GetFormattedTimeFromMiliseconds(float elapsedMiliseconds)
	{
		return GetFormattedTime(elapsedMiliseconds / 1000f, miliseconds: true);
	}
}
