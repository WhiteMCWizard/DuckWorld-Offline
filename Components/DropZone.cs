using System.Collections.Generic;
using SLAM.CraneOperator;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DropZone : MonoBehaviour
{
	protected List<Crate> crates = new List<Crate>();

	public int CrateCount => crates.Count;

	public virtual void OnDrop(Crate crate)
	{
		crate.transform.parent = base.transform;
		Vector3 localPosition = crate.transform.localPosition;
		localPosition.x = Mathf.Round(localPosition.x);
		localPosition.y = Mathf.Round(localPosition.y);
		crate.transform.localPosition = localPosition;
		crates.Add(crate);
	}

	public virtual void OnPickup(Crate crate)
	{
		crates.Remove(crate);
	}
}
