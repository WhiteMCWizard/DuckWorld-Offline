using System.Collections;
using System.Collections.Generic;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.AssemblyLine;

public class ALDropZone : MonoBehaviour
{
	[SerializeField]
	private float partMovementSpeed;

	[SerializeField]
	private AnimationCurve partAnimationCurve;

	[SerializeField]
	private MeshRenderer beam;

	[SerializeField]
	private AnimationCurve robotCompleteAnimCurve;

	[SerializeField]
	private Vector3 robotCompletePosition;

	[SerializeField]
	private float robotCompleteAnimLength;

	[SerializeField]
	private GameObject[] lightOffObject;

	public List<ALRobotPart> DroppedParts { get; protected set; }

	public int DesignatedKind { get; private set; }

	public ALRobotPart.RobotPartType PlacedTypes { get; private set; }

	private void Start()
	{
		DroppedParts = new List<ALRobotPart>();
		DesignatedKind = -1;
		beam.material.SetTextureOffset("_MainTex", Vector2.one * Random.value);
		beam.material.SetTextureOffset("_SecondTex", Vector2.one * Random.value);
		beam.material.SetTextureOffset("_ThirdTex", Vector2.one * Random.value);
		beam.material.SetFloat("_MainTexSpeed", Random.value);
		beam.material.SetFloat("_SecondTexSpeed", Random.value);
		beam.material.SetFloat("_ThirdTexSpeed", Random.value);
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.DrawLine(base.transform.position, base.transform.position + robotCompletePosition);
	}

	private void OnEnable()
	{
		if (beam != null)
		{
			beam.gameObject.SetActive(value: true);
		}
		for (int i = 0; i < lightOffObject.Length; i++)
		{
			if (lightOffObject[i] != null)
			{
				lightOffObject[i].SetActive(value: false);
			}
		}
	}

	private void OnDisable()
	{
		if (beam != null)
		{
			beam.gameObject.SetActive(value: false);
		}
		for (int i = 0; i < lightOffObject.Length; i++)
		{
			if (lightOffObject[i] != null)
			{
				lightOffObject[i].SetActive(value: true);
			}
		}
	}

	public bool CanDropPart(ALRobotPart robotPart, Vector3 worldMousePos)
	{
		Collider component = GetComponent<Collider>();
		worldMousePos.z = component.bounds.center.z;
		bool flag = component.bounds.Contains(worldMousePos) && (DesignatedKind == -1 || DesignatedKind == robotPart.Kind) && (PlacedTypes & robotPart.Type) != robotPart.Type;
		float num = ((!flag) ? 0.75f : 1f);
		if (beam.material.GetFloat("_AlphaMod") != num)
		{
			beam.material.SetFloat("_AlphaMod", num);
			if (flag)
			{
				GameEvents.Invoke(new AssemblyLineGame.PartHoverEvent());
			}
		}
		return flag;
	}

	public void DropPart(ALRobotPart robotPart)
	{
		DesignatedKind = robotPart.Kind;
		PlacedTypes |= robotPart.Type;
		DroppedParts.Add(robotPart);
		robotPart.CanDrag = false;
		robotPart.transform.parent = base.transform;
		StartCoroutine(animatePartToTargetPosition(robotPart, base.transform.position));
		beam.material.SetColor("_Color", robotPart.BeamColor);
		beam.material.SetFloat("_AlphaMod", 0.75f);
	}

	private IEnumerator animatePartToTargetPosition(ALRobotPart robotPart, Vector3 endPosition)
	{
		Vector3 startPosition = robotPart.transform.position;
		Stopwatch sw = new Stopwatch(Vector3.Distance(startPosition, endPosition) / partMovementSpeed);
		blendToAnim(robotPart.GetComponentInChildren<Animation>(), "Idle", sw.Duration, syncTime: true);
		AssemblyLineGame.PartDroppedEvent partDroppedEvent = new AssemblyLineGame.PartDroppedEvent();
		partDroppedEvent.Part = robotPart;
		partDroppedEvent.DropZone = this;
		GameEvents.Invoke(partDroppedEvent);
		while (!sw.Expired)
		{
			yield return null;
			if (robotPart == null)
			{
				yield break;
			}
			robotPart.transform.position = Vector3.Lerp(startPosition, endPosition, partAnimationCurve.Evaluate(sw.Progress));
		}
		if (PlacedTypes == (ALRobotPart.RobotPartType.Head | ALRobotPart.RobotPartType.ArmRight | ALRobotPart.RobotPartType.ArmLeft | ALRobotPart.RobotPartType.Body | ALRobotPart.RobotPartType.Feet))
		{
			StartCoroutine(robotCompleted());
		}
	}

	private IEnumerator robotCompleted()
	{
		GameObject dropRoot = new GameObject("DropAnimRoot " + base.name);
		AssemblyLineGame.RobotCompletedEvent robotCompletedEvent = new AssemblyLineGame.RobotCompletedEvent();
		robotCompletedEvent.DropZone = this;
		robotCompletedEvent.Robot = dropRoot;
		GameEvents.Invoke(robotCompletedEvent);
		yield return new WaitForSeconds(0.5f);
		dropRoot.transform.position = base.transform.position;
		DroppedParts.ForEach(delegate(ALRobotPart d)
		{
			d.transform.parent = dropRoot.transform;
			blendToAnim(d.GetComponentInChildren<Animation>(), "Outro");
		});
		DroppedParts.Clear();
		PlacedTypes = (ALRobotPart.RobotPartType)0;
		DesignatedKind = -1;
		Stopwatch sw = new Stopwatch(robotCompleteAnimLength);
		while (!sw.Expired)
		{
			yield return null;
			dropRoot.transform.position = Vector3.Lerp(base.transform.position, base.transform.position + robotCompletePosition, robotCompleteAnimCurve.Evaluate(sw.Progress));
		}
		if (DroppedParts.Count <= 0)
		{
			beam.material.SetColor("_Color", Color.white);
		}
		Object.Destroy(dropRoot);
	}

	private void blendToAnim(Animation anim, string animName, float fadeLength = 0.3f, bool syncTime = false)
	{
		foreach (AnimationState item in anim)
		{
			if (item.name.EndsWith(animName))
			{
				anim.clip = item.clip;
				if (syncTime)
				{
					Animation componentInChildren = DroppedParts.First().GetComponentInChildren<Animation>();
					float normalizedTime = componentInChildren[componentInChildren.clip.name].normalizedTime;
					item.normalizedTime = normalizedTime;
				}
				anim.CrossFade(item.clip.name, fadeLength);
				return;
			}
		}
		Debug.Log("Couldnt find anim that ends with " + animName + " in " + anim.gameObject.name, anim.gameObject);
	}
}
