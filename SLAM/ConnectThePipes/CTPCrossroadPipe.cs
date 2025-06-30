using System.Collections;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.ConnectThePipes;

public class CTPCrossroadPipe : CTPPipe
{
	[SerializeField]
	protected Vector3 inDirection2;

	[SerializeField]
	protected Vector3 outDirection2;

	[SerializeField]
	private GameObject openObject2;

	[SerializeField]
	private GameObject closedObject2;

	protected bool hasWaterInTube2;

	public override bool HasWaterInTube()
	{
		return hasWaterInTube2 || base.HasWaterInTube();
	}

	public override void ResetWater()
	{
		base.ResetWater();
		if (hasWaterInTube2)
		{
			StartCoroutine(doCloseTube2());
		}
		hasWaterInTube2 = false;
	}

	protected override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		Vector3 vector = base.transform.TransformDirection(inDirection2);
		Vector3 vector2 = base.transform.TransformDirection(outDirection2);
		Gizmos.color = Color.green;
		GizmosUtils.DrawArrow(base.transform.position - vector * 0.5f, vector * 0.5f);
		Gizmos.color = Color.magenta;
		GizmosUtils.DrawArrow(base.transform.position + vector2 * 0.5f, vector2 * 0.5f);
	}

	public override bool CanFlowWater(Vector3 otherInDir, out Vector3 otherOutDir)
	{
		Vector3 vector = base.transform.TransformDirection(inDirection2);
		Vector3 vector2 = base.transform.TransformDirection(outDirection2);
		if (Mathf.Approximately(Vector3.Dot(vector, otherInDir), 1f))
		{
			if (hasWaterInTube2)
			{
				otherOutDir = Vector3.one;
				return false;
			}
			hasWaterInTube2 = true;
			otherOutDir = vector2;
			return true;
		}
		if (Mathf.Approximately(Vector3.Dot(-vector2, otherInDir), 1f))
		{
			if (hasWaterInTube2)
			{
				otherOutDir = Vector3.one;
				return false;
			}
			hasWaterInTube2 = true;
			otherOutDir = -vector;
			return true;
		}
		return base.CanFlowWater(otherInDir, out otherOutDir);
	}

	private new IEnumerator doFillWaterEffect(Vector3 otherInDir, GameObject particleEffect, int waterFlowDelay)
	{
		Vector3 worldIn = base.transform.TransformDirection(inDirection2);
		Vector3 worldOut = base.transform.TransformDirection(outDirection2);
		if (Mathf.Approximately(Vector3.Dot(-worldOut, otherInDir), 1f) || Mathf.Approximately(Vector3.Dot(worldIn, otherInDir), 1f))
		{
			Material mat = openObject2.GetComponent<Renderer>().materials.FirstOrDefault((Material m) => m.HasProperty("_Progress") && m.HasProperty("_Scale"));
			mat.SetFloat("_Progress", 0f);
			StartCoroutine(doOpenTube2());
			yield return new WaitForSeconds((float)waterFlowDelay * 0.5f);
			closedObject2.SetActive(value: false);
			openObject2.SetActive(value: true);
			float dot = Vector3.Dot(otherInDir, worldIn);
			float scale = 1f;
			if (dot > 0.1f)
			{
				scale = -1f;
			}
			mat.SetFloat("_Scale", scale);
			mat.SetFloat("_Progress", 0f);
			Stopwatch sw = new Stopwatch(0.5f);
			while (!sw.Expired)
			{
				yield return null;
				mat.SetFloat("_Progress", sw.Progress);
				float prog = ((!(dot > 0.01f)) ? (1f - sw.Progress) : sw.Progress);
				Vector3 newPos = getPosOnTube2(prog);
				particleEffect.transform.rotation = Quaternion.LookRotation(particleEffect.transform.position - newPos);
				particleEffect.transform.position = newPos;
			}
		}
		else
		{
			yield return StartCoroutine(base.doFillWaterEffect(otherInDir, particleEffect, waterFlowDelay));
		}
	}

	public override void StartDoFillWaterEffect(Vector3 inDir, GameObject waterFlowParticles, int p)
	{
		StartCoroutine(doFillWaterEffect(inDir, waterFlowParticles, p));
	}

	protected override Vector3 getPosOnTube(float prog)
	{
		if (openObject == null)
		{
			return Vector3.zero;
		}
		Renderer component = openObject.GetComponent<Renderer>();
		Vector3 vector = base.transform.TransformDirection(inDirection);
		Vector3 vector2 = base.transform.TransformDirection(outDirection);
		Vector3 vector3 = component.bounds.center / 2f + GetComponent<Collider>().bounds.center / 2f;
		vector3.y -= component.bounds.extents.y;
		if (prog < 0.5f)
		{
			return Vector3.Lerp(base.transform.position - vector / 2f, vector3, prog / 0.5f);
		}
		if (prog > 0.5f)
		{
			return Vector3.Lerp(vector3, base.transform.position + vector2 / 2f, (prog - 0.5f) / 0.5f);
		}
		return vector3;
	}

	protected Vector3 getPosOnTube2(float prog)
	{
		if (openObject == null)
		{
			return Vector3.zero;
		}
		Renderer component = openObject2.GetComponent<Renderer>();
		Vector3 vector = base.transform.TransformDirection(inDirection2);
		Vector3 vector2 = base.transform.TransformDirection(outDirection2);
		Vector3 vector3 = component.bounds.center / 2f + GetComponent<Collider>().bounds.center / 2f;
		vector3.y += component.bounds.extents.y;
		if (prog < 0.5f)
		{
			return Vector3.Lerp(base.transform.position - vector / 2f, vector3, prog / 0.5f);
		}
		if (prog > 0.5f)
		{
			return Vector3.Lerp(vector3, base.transform.position + vector2 / 2f, (prog - 0.5f) / 0.5f);
		}
		return vector3;
	}

	protected IEnumerator doOpenTube2()
	{
		Material mat = closedObject2.GetComponent<Renderer>().material;
		openObject2.SetActive(value: true);
		Stopwatch sw = new Stopwatch(1f);
		while ((bool)sw)
		{
			yield return null;
			mat.SetFloat("_AlphaMod", 1f - sw.Progress);
		}
	}

	protected IEnumerator doCloseTube2()
	{
		Material mat = closedObject2.GetComponent<Renderer>().material;
		closedObject2.SetActive(value: true);
		Stopwatch sw = new Stopwatch(1f);
		while ((bool)sw)
		{
			yield return null;
			mat.SetFloat("_AlphaMod", sw.Progress);
		}
		openObject2.SetActive(value: false);
	}
}
