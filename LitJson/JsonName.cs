using System;

namespace LitJson;

public class JsonName : Attribute
{
	public string Name { get; protected set; }

	public JsonName(string name)
	{
		Name = name;
	}
}
