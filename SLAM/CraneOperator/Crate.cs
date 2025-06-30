using System.Collections.Generic;
using UnityEngine;

namespace SLAM.CraneOperator;

public class Crate : MonoBehaviour
{
	public enum CrateType
	{
		Crocodile = 1,
		Elephant,
		Fish,
		Flamingo,
		Giraffe,
		Monkey
	}

	public CrateType Type;

	[SerializeField]
	private float skinWidth = 0.2f;

	[SerializeField]
	private int unitWidth = 1;

	[SerializeField]
	private int unitHeight = 1;

	private BoxCollider box;

	public Vector3 TopCenter
	{
		get
		{
			Vector3 center = box.bounds.center;
			center.y += box.bounds.size.y / 2f;
			return center;
		}
	}

	public bool IsBeingCarried { get; private set; }

	public int UnitWidth => unitWidth;

	public int UnitHeight => unitHeight;

	public BoxCollider Collider => box;

	private void Start()
	{
		box = GetComponent<Collider>() as BoxCollider;
		if (box == null)
		{
			Debug.LogError("Error: BoxCollider not found on this crate but is needed.", this);
		}
	}

	public void PickUp()
	{
		IsBeingCarried = true;
		box.size = new Vector3(box.size.x, box.size.y, 0.75f);
	}

	public void Release()
	{
		IsBeingCarried = false;
		box.size = new Vector3(box.size.x, box.size.y, 3f);
	}

	public RayOrigin[] Rays(params Direction[] excluded)
	{
		List<RayOrigin> list = new List<RayOrigin>();
		RayOrigin[] array = Rays();
		for (int i = 0; i < array.Length; i++)
		{
			bool flag = false;
			for (int j = 0; j < excluded.Length; j++)
			{
				if (array[i].Direction == excluded[j])
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				list.Add(array[i]);
			}
		}
		return list.ToArray();
	}

	public RayOrigin[] Rays()
	{
		return CraneOperatorGame.CalculateRaysFromBox(box, skinWidth);
	}
}
