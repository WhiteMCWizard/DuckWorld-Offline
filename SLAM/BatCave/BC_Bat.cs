using UnityEngine;

namespace SLAM.BatCave;

public class BC_Bat : BC_MovingEnemy
{
	protected override float timeBetweenAttackAnimAndHitEvent => 0.15f;

	public override void Move()
	{
		base.Move();
		animator.SetBool("IsFlying", value: true);
	}

	public override void TurnAround()
	{
		base.TurnAround();
		if (base.MovementSpeed > 0f)
		{
			animator.transform.rotation = Quaternion.AngleAxis(35f, Vector3.up);
		}
		else
		{
			animator.transform.rotation = Quaternion.AngleAxis(215f, Vector3.up);
		}
	}
}
