using System.Collections.Generic;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.Shops;

public abstract class ItemLibrary<T> : ScriptableObject where T : Item
{
	[SerializeField]
	protected List<T> items = new List<T>();

	public List<T> Items => items;

	public T GetItemByGUID(string guid)
	{
		return items.FirstOrDefault((T i) => i.GUID == guid);
	}
}
