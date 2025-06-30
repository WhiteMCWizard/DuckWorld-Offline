using UnityEngine;

namespace SLAM.Platformer.MonkeyBattle;

public class MB_DestructibleObstacle : MonoBehaviour
{
	protected virtual void HitByBanana(MB_Banana banana)
	{
		banana.SetVelocityDirection(banana.transform.forward);
		Object.Destroy(base.gameObject);
	}
}
