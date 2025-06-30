using UnityEngine;

namespace SLAM.Sokoban;

public class SKBSpawnPoint : MonoBehaviour
{
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawCube(base.transform.position, Vector3.one);
	}
}
