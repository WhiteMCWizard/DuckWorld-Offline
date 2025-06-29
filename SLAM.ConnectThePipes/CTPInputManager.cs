using UnityEngine;

namespace SLAM.ConnectThePipes;

public class CTPInputManager : MonoBehaviour
{
	public bool AreControlsLocked { get; set; }

	private void Update()
	{
		if (AreControlsLocked || (!Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1)))
		{
			return;
		}
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out var hitInfo))
		{
			int direction = (Input.GetMouseButtonDown(0) ? 1 : (-1));
			if (hitInfo.collider.HasComponent<CTPPipe>())
			{
				hitInfo.collider.GetComponent<CTPPipe>().OnClick(direction);
			}
		}
	}
}
