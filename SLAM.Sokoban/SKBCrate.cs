using UnityEngine;

namespace SLAM.Sokoban;

public class SKBCrate : MonoBehaviour
{
	[SerializeField]
	private int markerType = 1;

	public int MarkerType => markerType;

	public bool Completed { get; protected set; }

	public void SetCompleted(bool complete)
	{
		Completed = complete;
	}
}
