using System.Collections;
using System.Collections.Generic;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.CameraSystem;

public class CameraManager : MonoBehaviour
{
	private struct CameraState
	{
		public float Weight;

		public Vector3 Position;

		public Quaternion Rotation;
	}

	private Dictionary<int, List<CameraBehaviour>> behaviourGroups = new Dictionary<int, List<CameraBehaviour>>();

	private void LateUpdate()
	{
		foreach (KeyValuePair<int, List<CameraBehaviour>> behaviourGroup in behaviourGroups)
		{
			doBehaviourGroup(behaviourGroup.Value);
		}
	}

	private void doBehaviourGroup(List<CameraBehaviour> behaviourGroup)
	{
		float num = 0f;
		List<CameraState> list = new List<CameraState>();
		foreach (CameraBehaviour item in behaviourGroup)
		{
			if (!(item.Weight <= 0f))
			{
				item.GetPositionAndRotation(out var position, out var rotation);
				list.Add(new CameraState
				{
					Weight = item.Weight,
					Position = position,
					Rotation = rotation
				});
				num += item.Weight;
			}
		}
		if (num <= 0f)
		{
			return;
		}
		Vector3 zero = Vector3.zero;
		Vector3 zero2 = Vector3.zero;
		Vector3 zero3 = Vector3.zero;
		foreach (CameraState item2 in list)
		{
			float num2 = item2.Weight / num;
			zero += item2.Position * num2;
			zero2 += item2.Rotation * Vector3.forward * num2;
			zero3 += item2.Rotation * Vector3.up * num2;
		}
		base.transform.position = zero;
		base.transform.rotation = Quaternion.LookRotation(zero2, zero3);
	}

	public Coroutine CrossFade(CameraBehaviour behaviour, float duration)
	{
		return CrossFade(behaviour, duration, AnimationCurve.Linear(0f, 0f, 1f, 1f));
	}

	public Coroutine CrossFade(CameraBehaviour behaviour, float duration, AnimationCurve blendCurve)
	{
		if (duration <= 0f)
		{
			List<CameraBehaviour> list = behaviourGroups[behaviour.Layer];
			for (int i = 0; i < list.Count; i++)
			{
				float weight = ((!(list[i] == behaviour)) ? 0f : 1f);
				list[i].Weight = weight;
			}
		}
		StopCoroutine("doCrossFade");
		return StartCoroutine("doCrossFade", new object[3] { behaviour, duration, blendCurve });
	}

	private IEnumerator doCrossFade(object[] args)
	{
		CameraBehaviour behaviour = (CameraBehaviour)args[0];
		float duration = (float)args[1];
		AnimationCurve blendCurve = (AnimationCurve)args[2];
		List<CameraBehaviour> group = behaviourGroups[behaviour.Layer];
		Stopwatch sw = new Stopwatch(duration);
		float[] origWeights = group.Select((CameraBehaviour cb) => cb.Weight).ToArray();
		do
		{
			yield return new WaitForEndOfFrame();
			for (int i = 0; i < group.Count; i++)
			{
				float target = ((!(group[i] == behaviour)) ? 0f : 1f);
				group[i].Weight = Mathf.Lerp(origWeights[i], target, blendCurve.Evaluate(sw.Progress));
			}
		}
		while (!sw.Expired);
	}

	public void AddBehaviour(CameraBehaviour behaviour)
	{
		if (!behaviourGroups.ContainsKey(behaviour.Layer))
		{
			behaviourGroups.Add(behaviour.Layer, new List<CameraBehaviour> { behaviour });
		}
		else
		{
			behaviourGroups[behaviour.Layer].Add(behaviour);
		}
	}

	public void RemoveBehaviour(CameraBehaviour behaviour)
	{
		behaviourGroups[behaviour.Layer].Remove(behaviour);
	}

	public CameraBehaviour CurrentBehaviour(int layer)
	{
		return behaviourGroups[layer].OrderByDescending((CameraBehaviour c) => c.Weight).First();
	}
}
