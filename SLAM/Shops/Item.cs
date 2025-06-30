using UnityEngine;

namespace SLAM.Shops;

[SerializeField]
public abstract class Item
{
	[HideInInspector]
	public string GUID;

	public abstract string CategoryName { get; }
}
