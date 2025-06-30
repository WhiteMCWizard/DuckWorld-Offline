using UnityEngine;

namespace SLAM.Platformer.MonkeyBattle;

public class MB_Cannon : MonoBehaviour
{
	[SerializeField]
	private float shootingForce;

	[SerializeField]
	private Object bananaPrefab;

	public void Fire(Vector3 dir)
	{
		GameObject gameObject = Object.Instantiate(bananaPrefab, base.transform.position, base.transform.rotation) as GameObject;
		gameObject.GetComponent<MB_Banana>().ShootInDirection(dir, shootingForce);
	}
}
