using System;

namespace CinemaDirector;

[AttributeUsage(AttributeTargets.Class)]
public class CutsceneItemAttribute : Attribute
{
	private string subCategory;

	private string label;

	private CutsceneItemGenre[] genres;

	private Type requiredObjectType;

	public string Category => subCategory;

	public string Label => label;

	public CutsceneItemGenre[] Genres => genres;

	public Type RequiredObjectType => requiredObjectType;

	public CutsceneItemAttribute(string category, string label, params CutsceneItemGenre[] genres)
	{
		subCategory = category;
		this.label = label;
		this.genres = genres;
	}

	public CutsceneItemAttribute(string category, string label, Type pairedObject, params CutsceneItemGenre[] genres)
	{
		subCategory = category;
		this.label = label;
		requiredObjectType = pairedObject;
		this.genres = genres;
	}
}
