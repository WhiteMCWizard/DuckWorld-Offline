using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace CinemaDirector;

public static class DirectorRuntimeHelper
{
	public static List<Type> GetAllowedTrackTypes(TrackGroup trackGroup)
	{
		TimelineTrackGenre[] array = new TimelineTrackGenre[0];
		MemberInfo type = trackGroup.GetType();
		object[] customAttributes = type.GetCustomAttributes(typeof(TrackGroupAttribute), inherit: true);
		for (int i = 0; i < customAttributes.Length; i++)
		{
			TrackGroupAttribute trackGroupAttribute = (TrackGroupAttribute)customAttributes[i];
			if (trackGroupAttribute != null)
			{
				array = trackGroupAttribute.AllowedTrackGenres;
				break;
			}
		}
		List<Type> list = new List<Type>();
		Type[] allSubTypes = GetAllSubTypes(typeof(TimelineTrack));
		foreach (Type type2 in allSubTypes)
		{
			object[] customAttributes2 = type2.GetCustomAttributes(typeof(TimelineTrackAttribute), inherit: true);
			for (int k = 0; k < customAttributes2.Length; k++)
			{
				TimelineTrackAttribute timelineTrackAttribute = (TimelineTrackAttribute)customAttributes2[k];
				if (timelineTrackAttribute == null)
				{
					continue;
				}
				TimelineTrackGenre[] trackGenres = timelineTrackAttribute.TrackGenres;
				foreach (TimelineTrackGenre timelineTrackGenre in trackGenres)
				{
					TimelineTrackGenre[] array2 = array;
					foreach (TimelineTrackGenre timelineTrackGenre2 in array2)
					{
						if (timelineTrackGenre == timelineTrackGenre2)
						{
							list.Add(type2);
							break;
						}
					}
				}
				break;
			}
		}
		return list;
	}

	public static List<Type> GetAllowedItemTypes(TimelineTrack timelineTrack)
	{
		CutsceneItemGenre[] array = new CutsceneItemGenre[0];
		MemberInfo type = timelineTrack.GetType();
		object[] customAttributes = type.GetCustomAttributes(typeof(TimelineTrackAttribute), inherit: true);
		for (int i = 0; i < customAttributes.Length; i++)
		{
			TimelineTrackAttribute timelineTrackAttribute = (TimelineTrackAttribute)customAttributes[i];
			if (timelineTrackAttribute != null)
			{
				array = timelineTrackAttribute.AllowedItemGenres;
				break;
			}
		}
		List<Type> list = new List<Type>();
		Type[] allSubTypes = GetAllSubTypes(typeof(TimelineItem));
		foreach (Type type2 in allSubTypes)
		{
			object[] customAttributes2 = type2.GetCustomAttributes(typeof(CutsceneItemAttribute), inherit: true);
			for (int k = 0; k < customAttributes2.Length; k++)
			{
				CutsceneItemAttribute cutsceneItemAttribute = (CutsceneItemAttribute)customAttributes2[k];
				if (cutsceneItemAttribute == null)
				{
					continue;
				}
				CutsceneItemGenre[] genres = cutsceneItemAttribute.Genres;
				foreach (CutsceneItemGenre cutsceneItemGenre in genres)
				{
					CutsceneItemGenre[] array2 = array;
					foreach (CutsceneItemGenre cutsceneItemGenre2 in array2)
					{
						if (cutsceneItemGenre == cutsceneItemGenre2)
						{
							list.Add(type2);
							break;
						}
					}
				}
				break;
			}
		}
		return list;
	}

	private static Type[] GetAllSubTypes(Type ParentType)
	{
		List<Type> list = new List<Type>();
		Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
		foreach (Assembly assembly in assemblies)
		{
			Type[] types = assembly.GetTypes();
			foreach (Type type in types)
			{
				if (type.IsSubclassOf(ParentType))
				{
					list.Add(type);
				}
			}
		}
		return list.ToArray();
	}

	public static List<Transform> GetAllTransformsInHierarchy(Transform parent)
	{
		List<Transform> list = new List<Transform>();
		foreach (Transform item in parent)
		{
			list.AddRange(GetAllTransformsInHierarchy(item));
			list.Add(item);
		}
		return list;
	}
}
