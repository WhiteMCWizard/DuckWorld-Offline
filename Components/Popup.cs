using UnityEngine;

public class Popup : PropertyAttribute
{
	public string[] Options;

	public Popup(params string[] opts)
	{
		Options = opts;
	}
}
