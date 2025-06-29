using System.Collections;
using UnityEngine;

namespace SLAM.Fruityard;

public class FY_Grandma : MonoBehaviour
{
	[SerializeField]
	private Animator animator;

	private NavMeshAgent navMeshAgent;

	private Vector3[] areaPositions = new Vector3[2]
	{
		new Vector3(3.8f, 2.25f, 6.6f),
		new Vector3(10.4f, 2.25f, 8.1f)
	};

	private float[] areaRadiuses = new float[2] { 3.9f, 3.5f };

	private void Start()
	{
		navMeshAgent = GetComponent<NavMeshAgent>();
		StartCoroutine(walkToLocation(PickRandomPosition()));
	}

	private IEnumerator walkToLocation(Vector3 endPosition)
	{
		navMeshAgent.SetDestination(endPosition);
		while (navMeshAgent.pathPending)
		{
			yield return null;
		}
		while (navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance)
		{
			animator.SetFloat("Velocity", navMeshAgent.velocity.magnitude);
			if (Physics.Raycast(base.transform.position, Vector3.down, out var hit, 1f))
			{
				Vector3 angles = base.transform.eulerAngles;
				angles.x = Mathf.LerpAngle(angles.x, Vector3.Angle(Vector3.up, hit.normal), Time.deltaTime);
				base.transform.eulerAngles = angles;
			}
			yield return null;
		}
		animator.SetInteger("Idle", Random.Range(1, 6));
		while (!animator.GetCurrentAnimatorStateInfo(0).IsName("FY_Grandma_Idle_Pickup"))
		{
			yield return null;
		}
		animator.SetInteger("Idle", 0);
		Vector3 randomPos = PickRandomPosition();
		while (Vector3.Distance(randomPos, base.transform.position) < 4f)
		{
			randomPos = PickRandomPosition();
		}
		StartCoroutine(walkToLocation(randomPos));
	}

	private Vector3 PickRandomPosition()
	{
		int num = Random.Range(0, areaPositions.Length);
		Vector3 normalized = new Vector3(Random.value - 0.5f, 0f, Random.value - 0.5f).normalized;
		return areaPositions[num] + normalized * areaRadiuses[num] * Random.value;
	}
}
