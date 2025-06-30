using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SLAM.AssemblyLine;

public class ALConveyorBelt : MonoBehaviour
{
	[Serializable]
	public struct ALGear
	{
		public ALRotate rotator;

		public bool flip;
	}

	private const float rayWidth = 0.1f;

	[SerializeField]
	private ALGear[] gears;

	[SerializeField]
	private Vector3 beltStartPosition;

	[SerializeField]
	private Vector3 beltEndPosition;

	[SerializeField]
	private ALRobotPartGenerator partGenerator;

	[SerializeField]
	private float partDropMovementSpeed;

	[SerializeField]
	private AnimationCurve partDropAnimCurve;

	[SerializeField]
	private GameObject movingBelt;

	[SerializeField]
	private Vector3 destroyEndPosition;

	[SerializeField]
	private float destroyTime;

	[SerializeField]
	private AnimationCurve destroyCurve;

	[SerializeField]
	private AnimationCurve destroyRotationCurve;

	[SerializeField]
	private Animator doorAnim;

	[SerializeField]
	private ParticleSystem destroyParticles;

	private bool paused;

	private float duration;

	private float spawnInterval;

	private List<ALRobotPart> activeParts = new List<ALRobotPart>();

	public Vector3 BeltStartPosition => beltStartPosition;

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawLine(beltStartPosition, beltEndPosition);
		Gizmos.color = Color.red;
		Vector3 vector = beltEndPosition;
		Vector3 a = beltEndPosition;
		for (float num = 0f; num <= 1f; num += 0.05f)
		{
			Vector3 vector2 = lerpUnclamped(a, destroyEndPosition, destroyCurve.Evaluate(num));
			vector2.x -= num;
			Gizmos.color = Color.Lerp(Color.green, Color.red, num);
			Gizmos.DrawLine(vector, vector2);
			vector = vector2;
		}
	}

	private void OnEnable()
	{
		InvokeRepeating("spawnPart", spawnInterval, spawnInterval);
		doorAnim.SetBool("Open", value: true);
		AudioController.Play("AL_conveyor_loop");
		AudioController.Play("AL_shutter_open");
	}

	private void OnDisable()
	{
		CancelInvoke("spawnPart");
		if (doorAnim != null)
		{
			doorAnim.SetBool("Open", value: false);
		}
		if (movingBelt != null)
		{
			movingBelt.GetComponent<Renderer>().material.SetFloat("_Speed", 0f);
		}
	}

	private void Update()
	{
		for (int i = 0; i < activeParts.Count; i++)
		{
			ALRobotPart aLRobotPart = activeParts[i];
			if (!aLRobotPart.IsDragging)
			{
				if (aLRobotPart.Timer.Expired)
				{
					AssemblyLineGame.LifeLostEvent lifeLostEvent = new AssemblyLineGame.LifeLostEvent();
					lifeLostEvent.Part = aLRobotPart;
					GameEvents.Invoke(lifeLostEvent);
					StartCoroutine(doDestroyPartSequence(aLRobotPart));
					activeParts.RemoveAt(i);
					i--;
				}
				else
				{
					Vector3 positionOnBelt = getPositionOnBelt(aLRobotPart.Timer.Progress);
					Bounds bounds = aLRobotPart.GetComponentInChildren<Collider>().bounds;
					positionOnBelt.x += aLRobotPart.transform.position.x - bounds.center.x;
					positionOnBelt.y += aLRobotPart.transform.position.y - bounds.min.y;
					aLRobotPart.transform.position = positionOnBelt;
				}
			}
		}
	}

	private void spawnPart()
	{
		if (!paused)
		{
			ALRobotPart aLRobotPart = UnityEngine.Object.Instantiate(partGenerator.GeneratePart());
			Vector3 position = beltStartPosition;
			Bounds bounds = aLRobotPart.GetComponentInChildren<Collider>().bounds;
			position.x += aLRobotPart.transform.position.x - bounds.center.x;
			position.y += aLRobotPart.transform.position.y - bounds.min.y;
			aLRobotPart.transform.position = position;
			aLRobotPart.Timer = new Stopwatch(duration);
			activeParts.Add(aLRobotPart);
			AssemblyLineGame.PartSpawnedEvent partSpawnedEvent = new AssemblyLineGame.PartSpawnedEvent();
			partSpawnedEvent.Part = aLRobotPart;
			GameEvents.Invoke(partSpawnedEvent);
		}
	}

	private float getTimeOnBelt(Vector3 targetPos)
	{
		return (beltStartPosition.x - targetPos.x) / (beltStartPosition.x - beltEndPosition.x);
	}

	private Vector3 getPositionOnBelt(float t)
	{
		return Vector3.Lerp(beltStartPosition, beltEndPosition, t);
	}

	private Vector3 getValidPosition(ALRobotPart part)
	{
		if (isAreaUnderPartClear(part, out var hitPart))
		{
			return part.transform.position;
		}
		Collider component = part.GetComponent<Collider>();
		Collider component2 = hitPart.GetComponent<Collider>();
		if (hasFreeSpot(component.bounds.size.x, component2.bounds, Vector3.right, out var freeSpot))
		{
			return freeSpot;
		}
		if (hasFreeSpot(component.bounds.size.x, component2.bounds, Vector3.left, out freeSpot))
		{
			return freeSpot;
		}
		return beltEndPosition;
	}

	private bool hasFreeSpot(float width, Bounds startingBounds, Vector3 dir, out Vector3 freeSpot)
	{
		Vector3 vector = new Vector3((!(dir.x > 0f)) ? startingBounds.min.x : startingBounds.max.x, startingBounds.center.y, startingBounds.center.z);
		Ray ray = new Ray(vector, dir);
		if (Physics.Raycast(ray, out var hitInfo, width))
		{
			return hasFreeSpot(width, hitInfo.collider.bounds, dir, out freeSpot);
		}
		freeSpot = vector + dir * width / 2f;
		return freeSpot.x < beltStartPosition.x;
	}

	private bool isAreaUnderPartClear(ALRobotPart part, out ALRobotPart hitPart)
	{
		hitPart = null;
		Collider component = part.GetComponent<Collider>();
		float num = component.bounds.min.x;
		while (num < component.bounds.max.x)
		{
			num += 0.1f;
			Ray ray = new Ray(new Vector3(Mathf.Min(num, component.bounds.max.x), component.bounds.min.y, component.bounds.center.z), Vector3.down);
			RaycastHit[] array = Physics.RaycastAll(ray, 100f);
			RaycastHit[] array2 = array;
			foreach (RaycastHit raycastHit in array2)
			{
				hitPart = raycastHit.transform.GetComponent<ALRobotPart>();
				if (hitPart != null && hitPart.CanDrag)
				{
					return false;
				}
			}
		}
		return true;
	}

	private IEnumerator doDestroyPartSequence(ALRobotPart part)
	{
		Vector3 offset = part.transform.position - part.GetComponentInChildren<Collider>().bounds.center;
		GameObject offsetFixerGo = new GameObject("Deathoffsetfixer");
		offsetFixerGo.transform.position = part.transform.position - offset;
		Stopwatch sw = new Stopwatch(destroyTime);
		Vector3 startPos = offsetFixerGo.transform.position;
		part.transform.parent = offsetFixerGo.transform;
		part.transform.localPosition = offset;
		part.CanDrag = false;
		while (!sw.Expired)
		{
			yield return null;
			float r = destroyRotationCurve.Evaluate(sw.Progress) * 360f;
			offsetFixerGo.transform.rotation = Quaternion.AngleAxis(r, Vector3.up);
			offsetFixerGo.transform.position = lerpUnclamped(startPos, destroyEndPosition, destroyCurve.Evaluate(sw.Progress));
		}
		destroyParticles.Play();
		UnityEngine.Object.Destroy(offsetFixerGo);
	}

	private float lerpUnclamped(float a, float b, float t)
	{
		return a + t * (b - a);
	}

	private Vector3 lerpUnclamped(Vector3 a, Vector3 b, float t)
	{
		return new Vector3(lerpUnclamped(a.x, b.x, t), lerpUnclamped(a.y, b.y, t), lerpUnclamped(a.z, b.z, t));
	}

	public IEnumerator MovePartBackToBelt(ALRobotPart draggingPart)
	{
		float t = getTimeOnBelt(getValidPosition(draggingPart));
		Collider draggingPartCollider = draggingPart.GetComponent<Collider>();
		draggingPart.Timer = new Stopwatch(duration, t * duration);
		Vector3 startPosition = draggingPart.transform.position;
		Vector3 endPosition = getPositionOnBelt(draggingPart.Timer.Progress);
		Vector3 offset = new Vector3(draggingPart.transform.position.x - draggingPartCollider.bounds.center.x, draggingPart.transform.position.y - draggingPartCollider.bounds.min.y);
		GameObject dummyCollider = new GameObject("DummyCollider", typeof(BoxCollider));
		dummyCollider.transform.position = endPosition;
		dummyCollider.GetComponent<Collider>().bounds.Encapsulate(draggingPartCollider.bounds);
		Stopwatch sw = new Stopwatch(Vector3.Distance(startPosition, endPosition) / partDropMovementSpeed);
		while (!sw.Expired)
		{
			yield return null;
			endPosition = getPositionOnBelt(draggingPart.Timer.Progress) + offset;
			draggingPart.transform.position = Vector3.Lerp(startPosition, endPosition, partDropAnimCurve.Evaluate(sw.Progress));
		}
		UnityEngine.Object.Destroy(dummyCollider);
		draggingPart.IsDragging = false;
	}

	public void SetDifficulty(AssemblyLineGame.AssemblyLineGameDifficulty diff)
	{
		duration = diff.Duration;
		spawnInterval = diff.SpawnInterval;
		movingBelt.GetComponent<Renderer>().material.SetFloat("_Speed", diff.BeltMaterialSpeed);
		partGenerator.SetDifficulty(diff);
	}

	public void PauseSpawningParts()
	{
		paused = true;
	}

	public void ResumeSpawningParts()
	{
		paused = false;
	}
}
