using UnityEngine;

public class ThereCanBeOnlyOne : MonoBehaviour
{
	public GameObject Prefab;

	private void Awake()
	{
		if (Prefab != null)
		{
			if (!GameObject.Find(Prefab.name))
			{
				GameObject gameObject = Object.Instantiate(Prefab);
				gameObject.name = Prefab.name;
			}
		}
		else
		{
			Debug.LogError("Error prefab not set so There Can't Be Only One", this);
		}
	}
}
