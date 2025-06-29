using SLAM.Slinq;
using UnityEngine;

public class DepthToSortingOrder : MonoBehaviour
{
	[SerializeField]
	private string objectPrefix = "Sprites_";

	private void Awake()
	{
		foreach (Transform item in from t in GetComponentsInChildren<Transform>(includeInactive: true)
			where t.name.StartsWith(objectPrefix)
			select t)
		{
			item.GetComponent<Renderer>().sortingOrder = (int)(0f - item.position.z);
		}
	}
}
