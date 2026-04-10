using System;
using System.Collections.Generic;
using SLAM.Slinq;
using UnityEngine;

public static class TransformExtensions
{
	public static bool HasComponent<T>(this Component comp) where T : Component
	{
		return comp.GetComponent<T>() != null;
	}

	public static T GetComponentInChildren<T>(this GameObject go, bool includeInactive) where T : Component
	{
		return go.GetComponentsInChildren<T>(includeInactive).FirstOrDefault();
	}

	public static void SetLayerRecursively(this GameObject go, int layerNumber)
	{
		if (!(go == null))
		{
			Transform[] componentsInChildren = go.GetComponentsInChildren<Transform>(includeInactive: true);
			foreach (Transform transform in componentsInChildren)
			{
				transform.gameObject.layer = layerNumber;
			}
		}
	}

	public static Transform FindChildRecursively(this Transform parent, string childname)
	{
		foreach (Transform item in parent)
		{
			if (item.name.Equals(childname, StringComparison.InvariantCultureIgnoreCase))
			{
				return item;
			}
			Transform transform2 = item.FindChildRecursively(childname);
			if (transform2 != null)
			{
				return transform2;
			}
		}
		return null;
	}

	public static Transform[] FindChildrenRecursively(this Transform parent, string childname)
	{
		return parent.findChildrenRecursively(childname).ToArray();
	}

	private static List<Transform> findChildrenRecursively(this Transform parent, string childname)
	{
		List<Transform> list = new List<Transform>();
		foreach (Transform item in parent)
		{
			if (item.name.Equals(childname, StringComparison.InvariantCultureIgnoreCase))
			{
				list.Add(item);
			}
			list.AddRange(item.findChildrenRecursively(childname));
		}
		return list;
	}

	public static void DestroyChildren(this Transform parent)
	{
		foreach (Transform item in parent.transform)
		{
			UnityEngine.Object.Destroy(item.gameObject);
		}
	}
}
