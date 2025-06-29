using System.Collections;
using SLAM.Platformer;
using UnityEngine;

namespace SLAM.BatCave;

public class BC_Visitation : MonoBehaviour
{
	[SerializeField]
	private BC_Visitor[] Visitors;

	[SerializeField]
	private Transform StartPosition;

	[SerializeField]
	private Transform EndPosition;

	[SerializeField]
	private float MinTimeBetweenVisitors;

	[SerializeField]
	private float MaxTimeBetweenVisitors;

	private Collider vistationCollider;

	private void Awake()
	{
		vistationCollider = GetComponent<Collider>();
		vistationCollider.isTrigger = true;
	}

	private void OnTriggerEnter(Collider other)
	{
		CC2DPlayer componentInParent = other.GetComponentInParent<CC2DPlayer>();
		if (componentInParent != null)
		{
			vistationCollider.enabled = false;
			StartCoroutine(startVisitation());
		}
	}

	private IEnumerator startVisitation()
	{
		BC_Visitor[] visitors = Visitors;
		foreach (BC_Visitor visitor in visitors)
		{
			visitor.WalkFromAToB(StartPosition.position, EndPosition.position);
			yield return new WaitForSeconds(Random.Range(MinTimeBetweenVisitors, MaxTimeBetweenVisitors));
		}
	}
}
