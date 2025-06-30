using UnityEngine;

namespace SLAM.BatCave;

public class BC_Scorpion : BC_MovingEnemy
{
	protected override float timeBetweenAttackAnimAndHitEvent => 0.2f;

	public override void Move()
	{
		base.Move();
		animator.SetBool("IsWalking", value: true);
	}

	public override void TurnAround()
	{
		base.TurnAround();
		if (base.MovementSpeed > 0f)
		{
			base.transform.rotation = Quaternion.AngleAxis(90f, Vector3.up);
		}
		else
		{
			base.transform.rotation = Quaternion.AngleAxis(-90f, Vector3.up);
		}
	}
}
