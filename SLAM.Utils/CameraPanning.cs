using UnityEngine;

namespace SLAM.Utils;

public class CameraPanning : MonoBehaviour
{
	[SerializeField]
	private Vector2 maxOffset;

	private Vector3 offset;

	[SerializeField]
	private float speed;

	private Vector3 originalPosition;

	private void Start()
	{
		originalPosition = base.transform.position;
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawWireCube((!Application.isPlaying) ? base.transform.position : originalPosition, maxOffset * 2f);
	}

	private void Update()
	{
		Vector3 vector = Camera.main.ScreenToViewportPoint(Input.mousePosition);
		offset.x = Mathf.Lerp(maxOffset.x, 0f - maxOffset.x, vector.x);
		offset.y = Mathf.Lerp(maxOffset.y, 0f - maxOffset.y, vector.y);
		base.transform.position = Vector3.Lerp(base.transform.position, originalPosition - offset, speed * Time.deltaTime);
	}
}
