using UnityEngine;

namespace SLAM.AssemblyLine;

public class ALDragAndDropManager : MonoBehaviour
{
	[SerializeField]
	private LayerMask layerMask;

	[SerializeField]
	private ALDropZone[] dropZones;

	[SerializeField]
	private ALConveyorBelt conveyorBelt;

	private ALRobotPart draggingPart;

	private Vector3 draggingOffset;

	private Plane hitPlane = new Plane(Vector3.back, Vector3.zero);

	private Collider draggableArea;

	private bool isDragging => draggingPart != null;

	private void Start()
	{
		draggableArea = GetComponent<Collider>();
		hitPlane = new Plane(Vector3.back, conveyorBelt.BeltStartPosition);
	}

	private void Update()
	{
		if (isDragging)
		{
			updateDragging();
		}
		else
		{
			updateInput();
		}
	}

	private void updateDragging()
	{
		if (!Input.GetMouseButton(0))
		{
			stopDragging();
			return;
		}
		Vector3 worldMousePos = getWorldMousePos();
		Vector3 position = worldMousePos + draggingOffset;
		draggingPart.transform.position = position;
		draggingPart.transform.position = containWithinBounds(draggingPart.transform.position, draggingPart.GetComponent<Collider>().bounds, draggableArea.bounds);
		for (int i = 0; i < dropZones.Length; i++)
		{
			dropZones[i].CanDropPart(draggingPart, worldMousePos);
		}
	}

	private Vector3 containWithinBounds(Vector3 position, Bounds myBounds, Bounds otherBounds)
	{
		if (myBounds.max.y > otherBounds.max.y)
		{
			position.y = otherBounds.max.y + (position.y - myBounds.max.y);
		}
		if (myBounds.min.y < otherBounds.min.y)
		{
			position.y = otherBounds.min.y + (position.y - myBounds.min.y);
		}
		if (myBounds.max.x > otherBounds.max.x)
		{
			position.x = otherBounds.max.x + (position.x - myBounds.max.x);
		}
		if (myBounds.min.x < otherBounds.min.x)
		{
			position.x = otherBounds.min.x + (position.x - myBounds.min.x);
		}
		return position;
	}

	private void updateInput()
	{
		if (Input.GetMouseButtonDown(0) && Camera.main != null && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hitInfo, float.PositiveInfinity, layerMask.value))
		{
			ALRobotPart component = hitInfo.transform.GetComponent<ALRobotPart>();
			if (component.CanDrag && draggableArea.bounds.Contains(component.GetComponent<Collider>().bounds.min))
			{
				draggingPart = component;
				draggingPart.IsDragging = true;
				draggingOffset = component.transform.position - getWorldMousePos();
				AssemblyLineGame.PartPickedUpEvent partPickedUpEvent = new AssemblyLineGame.PartPickedUpEvent();
				partPickedUpEvent.Part = draggingPart;
				GameEvents.Invoke(partPickedUpEvent);
			}
		}
	}

	private void stopDragging()
	{
		for (int i = 0; i < dropZones.Length; i++)
		{
			if (dropZones[i].CanDropPart(draggingPart, getWorldMousePos()))
			{
				dropZones[i].DropPart(draggingPart);
				draggingPart = null;
				return;
			}
		}
		AssemblyLineGame.PartReleasedEvent partReleasedEvent = new AssemblyLineGame.PartReleasedEvent();
		partReleasedEvent.Part = draggingPart;
		GameEvents.Invoke(partReleasedEvent);
		StartCoroutine(conveyorBelt.MovePartBackToBelt(draggingPart));
		draggingPart = null;
	}

	private Vector3 getWorldMousePos()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		hitPlane.Raycast(ray, out var enter);
		return ray.GetPoint(enter);
	}
}
