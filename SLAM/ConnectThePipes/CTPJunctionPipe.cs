using System;
using System.Collections;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.ConnectThePipes;

public class CTPJunctionPipe : CTPPipe
{
	[SerializeField]
	private Vector3 outDirection2;

	[SerializeField]
	private GameObject leftWaterObject;

	[SerializeField]
	private GameObject rightWaterObject;

	[SerializeField]
	private GameObject topWaterObject;

	protected Vector3 waterDirection2;

	protected Vector3 extraPos;

	protected Vector3 extraDir;

	protected override void OnDrawGizmosSelected()
	{
		Vector3 vector = base.transform.TransformDirection(outDirection2);
		Gizmos.color = Color.green;
		GizmosUtils.DrawArrow(base.transform.position + vector * 0.5f, vector * 0.5f);
	}

	public override bool CanFlowWater(Vector3 waterInDir, out Vector3 waterOutDir)
	{
		Vector3 vector = base.transform.TransformDirection(inDirection);
		Vector3 vector2 = base.transform.TransformDirection(outDirection2);
		if (waterInTube)
		{
			waterOutDir = Vector3.one;
			return false;
		}
		if (Mathf.Approximately(Vector3.Dot(-vector2, waterInDir), 1f))
		{
			waterOutDir = -vector;
			Vector3 vector3 = base.transform.TransformDirection(outDirection);
			extraPos = base.transform.localPosition + vector3;
			extraDir = vector3;
			waterInTube = true;
			return true;
		}
		if (base.CanFlowWater(waterInDir, out waterOutDir))
		{
			extraPos = base.transform.localPosition + vector2;
			extraDir = vector2;
			return true;
		}
		waterOutDir = Vector3.zero;
		return false;
	}

	public override void StartDoFillWaterEffect(Vector3 inDir, GameObject waterFlowParticles, int startDelay)
	{
		UnityEngine.Object.FindObjectOfType<CTPWaterFlowManager>().StartWaterFlowFromPosition(extraPos, extraDir, startDelay + 1);
		GameObject startPipe;
		GameObject outPipe;
		GameObject outPipe2;
		if (MathUtilities.AreDirectionsSimilar(inDir, base.transform.right))
		{
			startPipe = rightWaterObject;
			outPipe = leftWaterObject;
			outPipe2 = topWaterObject;
		}
		else if (MathUtilities.AreDirectionsSimilar(inDir, -base.transform.right))
		{
			startPipe = leftWaterObject;
			outPipe = rightWaterObject;
			outPipe2 = topWaterObject;
		}
		else
		{
			if (!MathUtilities.AreDirectionsSimilar(inDir, -base.transform.forward))
			{
				throw new Exception("T junction doesnt know how to handle this direction? " + inDir);
			}
			startPipe = topWaterObject;
			outPipe = leftWaterObject;
			outPipe2 = rightWaterObject;
		}
		StartCoroutine(fillWaterEffect(startPipe, outPipe, outPipe2, startDelay));
	}

	private IEnumerator fillWaterEffect(GameObject startPipe, GameObject outPipe1, GameObject outPipe2, int waterFlowDelay)
	{
		startPipe.gameObject.SetActive(value: true);
		outPipe1.gameObject.SetActive(value: true);
		outPipe2.gameObject.SetActive(value: true);
		Material startMat = startPipe.GetComponent<Renderer>().materials.FirstOrDefault((Material m) => m.HasProperty("_Progress") && m.HasProperty("_Scale"));
		Material outMat1 = outPipe1.GetComponent<Renderer>().materials.FirstOrDefault((Material m) => m.HasProperty("_Progress") && m.HasProperty("_Scale"));
		Material outMat2 = outPipe2.GetComponent<Renderer>().materials.FirstOrDefault((Material m) => m.HasProperty("_Progress") && m.HasProperty("_Scale"));
		startMat.SetFloat("_Progress", -10f);
		outMat1.SetFloat("_Progress", -10f);
		outMat2.SetFloat("_Progress", -10f);
		startMat.SetFloat("_Scale", -1f);
		outMat1.SetFloat("_Scale", 1f);
		outMat2.SetFloat("_Scale", 1f);
		StartCoroutine(doOpenTube());
		yield return new WaitForSeconds((float)waterFlowDelay * 0.5f);
		Stopwatch sw = new Stopwatch(0.25f);
		while (!sw.Expired)
		{
			yield return null;
			startMat.SetFloat("_Progress", sw.Progress);
		}
		sw = new Stopwatch(0.25f);
		while (!sw.Expired)
		{
			yield return null;
			outMat1.SetFloat("_Progress", sw.Progress);
			outMat2.SetFloat("_Progress", sw.Progress);
		}
	}
}
