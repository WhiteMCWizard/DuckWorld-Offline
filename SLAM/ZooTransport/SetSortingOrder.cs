using UnityEngine;

namespace SLAM.ZooTransport;

public class SetSortingOrder : MonoBehaviour
{
	[SerializeField]
	private int sortOrder;

	private void Start()
	{
		GetComponent<Renderer>().sortingOrder = sortOrder;
	}
}
