using System.Collections;
using SLAM.Engine;
using SLAM.Slinq;
using SLAM.ToolTips;
using UnityEngine;

namespace SLAM.ZooTransport;

public class ZTtutorialView : TutorialView
{
	[SerializeField]
	private ToolTip driveToolTip;

	[SerializeField]
	private ToolTip brakeToolTip;

	private ZTtruck truck;

	private Collider[] driveColliders;

	private Collider[] brakeColliders;

	protected override void Start()
	{
		base.Start();
		truck = Object.FindObjectOfType<ZTtruck>();
		driveColliders = driveToolTip.GetComponentsInChildren<Collider>();
		brakeColliders = brakeToolTip.GetComponentsInChildren<Collider>();
		for (int i = 0; i < driveColliders.Length; i++)
		{
			driveColliders[i].enabled = true;
		}
		for (int j = 0; j < brakeColliders.Length; j++)
		{
			brakeColliders[j].enabled = true;
		}
		StartCoroutine(step1());
		StartCoroutine(step2());
	}

	private IEnumerator step1()
	{
		while (driveColliders.Count((Collider c) => c.GetComponent<Collider>().bounds.Contains(truck.transform.position)) == 0)
		{
			yield return null;
		}
		driveToolTip.Show();
		while (driveColliders.Count((Collider c) => c.GetComponent<Collider>().bounds.Contains(truck.transform.position)) != 0)
		{
			yield return null;
		}
		driveToolTip.Hide();
		StartCoroutine(step1());
	}

	private IEnumerator step2()
	{
		while (brakeColliders.Count((Collider c) => c.GetComponent<Collider>().bounds.Contains(truck.transform.position)) == 0)
		{
			yield return null;
		}
		brakeToolTip.Show();
		while (brakeColliders.Count((Collider c) => c.GetComponent<Collider>().bounds.Contains(truck.transform.position)) != 0)
		{
			yield return null;
		}
		brakeToolTip.Hide();
		StartCoroutine(step2());
	}
}
