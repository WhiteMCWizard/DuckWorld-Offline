using System;
using System.Collections;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.AssemblyLine;

public class ALRobotPart : MonoBehaviour
{
	[Flags]
	public enum RobotPartType
	{
		Head = 1,
		ArmRight = 2,
		ArmLeft = 4,
		Body = 8,
		Feet = 0x10
	}

	[SerializeField]
	private RobotPartType type;

	[SerializeField]
	private int kind;

	[SerializeField]
	private Color beamColor;

	private bool _isDragging;

	private bool _canDrag = true;

	private float defaultAmbientThreshold = 0.4f;

	private float mouseoverAmbientThreshold = 1f;

	private float fadeTime = 0.2f;

	public Stopwatch Timer { get; set; }

	public bool IsDragging
	{
		get
		{
			return _isDragging;
		}
		set
		{
			if (!value)
			{
				StopAllCoroutines();
				StartCoroutine(setAmbientThreshold(defaultAmbientThreshold));
			}
			_isDragging = value;
		}
	}

	public bool CanDrag
	{
		get
		{
			return _canDrag;
		}
		set
		{
			StopAllCoroutines();
			StartCoroutine(setAmbientThreshold(defaultAmbientThreshold));
			_canDrag = value;
		}
	}

	public int Kind => kind;

	public RobotPartType Type => type;

	public Color BeamColor => beamColor;

	private void OnMouseEnter()
	{
		setAmbientThresholdDirect(mouseoverAmbientThreshold);
	}

	private void OnMouseExit()
	{
		if (!IsDragging)
		{
			setAmbientThresholdDirect(defaultAmbientThreshold);
		}
	}

	private void setAmbientThresholdDirect(float threshold)
	{
		Material[] materials = GetComponentInChildren<Renderer>().materials;
		Material[] array = materials;
		foreach (Material material in array)
		{
			material.SetFloat("_AmbientThreshold", threshold);
		}
	}

	private IEnumerator setAmbientThreshold(float threshold)
	{
		Material[] mats = GetComponentInChildren<Renderer>().materials;
		float startThreshold = mats.FirstOrDefault((Material m) => m.HasProperty("_AmbientThreshold")).GetFloat("_AmbientThreshold");
		Stopwatch sw = new Stopwatch(fadeTime);
		while (!sw.Expired)
		{
			yield return null;
			Material[] array = mats;
			foreach (Material mat in array)
			{
				mat.SetFloat("_AmbientThreshold", Mathf.Lerp(startThreshold, threshold, sw.Progress));
			}
		}
	}

	private IEnumerator waitForPartToBeVisible()
	{
		Renderer renderer = GetComponentInChildren<Renderer>();
		ALDragAndDropManager dragdropmanager = UnityEngine.Object.FindObjectOfType<ALDragAndDropManager>();
		Collider draggableArea = dragdropmanager.GetComponent<Collider>();
		while (!draggableArea.bounds.Contains(renderer.bounds.center))
		{
			yield return null;
		}
	}

	public Coroutine WaitForPartToBeVisible()
	{
		return StartCoroutine(waitForPartToBeVisible());
	}
}
