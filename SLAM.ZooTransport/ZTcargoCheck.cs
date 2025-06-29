using System.Collections.Generic;
using UnityEngine;

namespace SLAM.ZooTransport;

public class ZTcargoCheck : MonoBehaviour
{
	private int _cratesInLevel;

	private List<ZTcargo> cratesInTruck;

	private ZTcargo[] cargoArray;

	public int crateAmount => cratesInTruck.Count;

	public int cratesInLevel => _cratesInLevel;

	public void SetCargo(Object cargoSet)
	{
		foreach (Transform item in base.transform)
		{
			Object.Destroy(item.gameObject);
		}
		cratesInTruck = new List<ZTcargo>();
		GameObject gameObject = Object.Instantiate(cargoSet) as GameObject;
		gameObject.transform.parent = base.transform;
		gameObject.transform.localPosition = new Vector3(0.01f, 0.01f, 0f);
		cargoArray = gameObject.GetComponentsInChildren<ZTcargo>();
		_cratesInLevel = 0;
	}

	private Vector3 getDesiredCargoSpawnPosition(GameObject cargoSetObject)
	{
		Bounds bounds = GetComponent<Collider>().bounds;
		Bounds bounds2 = default(Bounds);
		Collider[] componentsInChildren = cargoSetObject.GetComponentsInChildren<Collider>();
		foreach (Collider collider in componentsInChildren)
		{
			bounds2.Encapsulate(collider.bounds);
		}
		Vector3 min = bounds.min;
		min.z = bounds.center.z;
		min.x = bounds.min.x + bounds2.size.x;
		min.y = bounds.min.y + bounds2.size.y;
		return min;
	}

	private void drawDebugBounds(Bounds b, Color c, float d)
	{
		Debug.DrawLine(b.min, b.max, c, d);
	}

	public void ManualUpdate()
	{
		ZTcargo[] array = cargoArray;
		foreach (ZTcargo zTcargo in array)
		{
			zTcargo.ManualUpdate();
		}
	}

	public int CheckCargo()
	{
		int num = _cratesInLevel - crateAmount;
		return (num <= 0) ? crateAmount : (-num);
	}

	private void OnTriggerEnter(Collider col)
	{
		ZTcargo component = col.GetComponent<ZTcargo>();
		if (!(component == null))
		{
			if (!cratesInTruck.Contains(component))
			{
				cratesInTruck.Add(component);
				component.SetInTruck(inTruck: true);
			}
			if (crateAmount > _cratesInLevel)
			{
				_cratesInLevel = crateAmount;
			}
		}
	}

	private void OnTriggerExit(Collider col)
	{
		ZTcargo component = col.GetComponent<ZTcargo>();
		if (!(component == null) && cratesInTruck.Contains(component))
		{
			cratesInTruck.Remove(component);
			component.SetInTruck(inTruck: false);
		}
	}
}
