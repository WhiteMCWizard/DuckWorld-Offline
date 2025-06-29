using System.Collections;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.ConnectThePipes;

[SelectionBase]
public class CTPPipe : MonoBehaviour
{
	public const float WATER_FLOW_SPEED = 0.5f;

	[SerializeField]
	protected Vector3 inDirection;

	[SerializeField]
	protected Vector3 outDirection;

	[SerializeField]
	protected bool canRotate = true;

	[SerializeField]
	private float rotationSpeed;

	[SerializeField]
	private AnimationCurve rotationCurve;

	[SerializeField]
	protected GameObject openObject;

	[SerializeField]
	protected GameObject closedObject;

	protected bool waterInTube;

	protected virtual void OnDrawGizmosSelected()
	{
		Vector3 vector = base.transform.TransformDirection(inDirection);
		Vector3 vector2 = base.transform.TransformDirection(outDirection);
		Gizmos.color = Color.red;
		GizmosUtils.DrawArrow(base.transform.position - vector * 0.5f, vector * 0.5f);
		Gizmos.color = Color.blue;
		GizmosUtils.DrawArrow(base.transform.position + vector2 * 0.5f, vector2 * 0.5f);
	}

	public virtual bool CanFlowWater(Vector3 waterInDir, out Vector3 waterOutDir)
	{
		Vector3 vector = base.transform.TransformDirection(inDirection);
		Vector3 vector2 = base.transform.TransformDirection(outDirection);
		waterOutDir = Vector3.zero;
		if (waterInTube)
		{
			if (Mathf.Approximately(Vector3.Dot(vector, waterInDir), 1f) || Mathf.Approximately(Vector3.Dot(-vector2, waterInDir), 1f))
			{
				waterOutDir = Vector3.one;
			}
			return false;
		}
		if (Mathf.Approximately(Vector3.Dot(vector, waterInDir), 1f))
		{
			waterOutDir = vector2;
			waterInTube = true;
			return true;
		}
		if (Mathf.Approximately(Vector3.Dot(-vector2, waterInDir), 1f))
		{
			waterOutDir = -vector;
			waterInTube = true;
			return true;
		}
		return false;
	}

	protected IEnumerator doFillWaterEffect(Vector3 waterInDirection, GameObject particleEffect, int waterFlowDelay)
	{
		Material mat = openObject.GetComponent<Renderer>().materials.FirstOrDefault((Material m) => m.HasProperty("_Progress") && m.HasProperty("_Scale"));
		mat.SetFloat("_Progress", -10f);
		StartCoroutine(doOpenTube());
		yield return new WaitForSeconds((float)waterFlowDelay * 0.5f);
		if (!(closedObject == null) && !(openObject == null))
		{
			Vector3 worldInDir = base.transform.TransformDirection(inDirection);
			float dot = Vector3.Dot(waterInDirection, worldInDir);
			float scale = ((!(dot > 0.1f)) ? 1 : (-1));
			mat.SetFloat("_Scale", scale);
			Stopwatch sw = new Stopwatch(0.5f);
			while (!sw.Expired)
			{
				yield return null;
				mat.SetFloat("_Progress", sw.Progress);
				float prog = ((!(dot > 0.01f)) ? (1f - sw.Progress) : sw.Progress);
				Vector3 newPos = getPosOnTube(prog);
				particleEffect.transform.rotation = Quaternion.LookRotation(newPos - particleEffect.transform.position);
				particleEffect.transform.position = newPos;
			}
		}
	}

	protected IEnumerator doOpenTube()
	{
		Material mat = closedObject.GetComponent<Renderer>().material;
		openObject.SetActive(value: true);
		Stopwatch sw = new Stopwatch(1f);
		while ((bool)sw)
		{
			yield return null;
			mat.SetFloat("_AlphaMod", 1f - sw.Progress);
		}
	}

	protected IEnumerator doCloseTube()
	{
		Material mat = closedObject.GetComponent<Renderer>().material;
		Stopwatch sw = new Stopwatch(1f);
		while ((bool)sw)
		{
			yield return null;
			mat.SetFloat("_AlphaMod", sw.Progress);
		}
		openObject.SetActive(value: false);
	}

	public virtual void ResetWater()
	{
		waterInTube = false;
		StartCoroutine(doCloseTube());
	}

	public virtual bool HasWaterInTube()
	{
		return waterInTube;
	}

	protected virtual Vector3 getPosOnTube(float prog)
	{
		if (openObject == null)
		{
			return Vector3.zero;
		}
		Renderer component = openObject.GetComponent<Renderer>();
		Vector3 vector = base.transform.TransformDirection(inDirection);
		Vector3 vector2 = base.transform.TransformDirection(outDirection);
		Vector3 vector3 = component.bounds.center / 2f + GetComponent<Collider>().bounds.center / 2f;
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

	public virtual void OnClick(int direction = 1)
	{
		if (canRotate)
		{
			ConnectThePipesGame.PipeClickedEvent pipeClickedEvent = new ConnectThePipesGame.PipeClickedEvent();
			pipeClickedEvent.pipe = this;
			GameEvents.Invoke(pipeClickedEvent);
			AudioController.Play("pipe_rotate_01", base.transform);
			StartCoroutine(rotatePipe(direction));
		}
	}

	protected IEnumerator rotatePipe(int direction = 1)
	{
		canRotate = false;
		Stopwatch sw = new Stopwatch(rotationSpeed);
		Quaternion startRot = base.transform.rotation;
		Quaternion endRot = base.transform.rotation * Quaternion.AngleAxis(90f * (float)direction, Vector3.up);
		while (!sw.Expired)
		{
			yield return null;
			base.transform.rotation = Quaternion.Lerp(startRot, endRot, rotationCurve.Evaluate(sw.Progress));
		}
		canRotate = true;
	}

	public virtual void StartDoFillWaterEffect(Vector3 inDir, GameObject waterFlowParticles, int startDelay)
	{
		StartCoroutine(doFillWaterEffect(inDir, waterFlowParticles, startDelay));
	}
}
