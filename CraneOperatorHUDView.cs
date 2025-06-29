using System.Collections;
using SLAM;
using SLAM.Engine;
using UnityEngine;

public class CraneOperatorHUDView : HUDView
{
	[SerializeField]
	private UILabel lblTruckCount;

	[SerializeField]
	private GameObject truckPointPling;

	[SerializeField]
	private AnimationCurve plingXCurve;

	[SerializeField]
	private AnimationCurve plingYCurve;

	[SerializeField]
	private float plingLength;

	private int targetCompletedTrucks;

	private int currentCompletedTrucks;

	public void CreateTruckChecks(int count)
	{
		targetCompletedTrucks = count;
		updateTruckLabel();
	}

	public void UpdateTrucksFinished(int count, Vector3 truckPos)
	{
		currentCompletedTrucks = count;
		updateTruckLabel();
		StartCoroutine(doTruckPointPling(truckPos));
	}

	private IEnumerator doTruckPointPling(Vector3 truckPos)
	{
		GameObject go = spawnObjectAtWorldPos(truckPointPling, truckPos);
		Vector3 startPos = go.transform.position;
		Vector3 endPos = lblTruckCount.transform.position;
		Stopwatch sw = new Stopwatch(plingLength);
		while (!sw.Expired)
		{
			yield return null;
			Vector3 pos = new Vector3(MathUtilities.LerpUnclamped(startPos.x, endPos.x, plingXCurve.Evaluate(sw.Progress)), MathUtilities.LerpUnclamped(startPos.y, endPos.y, plingYCurve.Evaluate(sw.Progress)));
			go.transform.position = pos;
		}
		Object.Destroy(go);
	}

	private void updateTruckLabel()
	{
		lblTruckCount.text = $"{currentCompletedTrucks}/{targetCompletedTrucks}";
	}

	private GameObject spawnObjectAtWorldPos(GameObject prefab, Vector3 worldpos)
	{
		Vector3 position = UICamera.currentCamera.ScreenToWorldPoint(Camera.main.WorldToScreenPoint(worldpos));
		position.z = 0f;
		GameObject gameObject = Object.Instantiate(prefab);
		gameObject.transform.parent = base.transform;
		gameObject.transform.position = position;
		gameObject.transform.localScale = Vector3.one;
		return gameObject;
	}
}
