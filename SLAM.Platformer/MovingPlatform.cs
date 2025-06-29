using System.Collections;
using UnityEngine;

namespace SLAM.Platformer;

public class MovingPlatform : MonoBehaviour
{
	[SerializeField]
	private Vector3 direction = Vector3.right;

	[SerializeField]
	private float distance = 1f;

	[SerializeField]
	private float movementSpeed = 2f;

	[SerializeField]
	private AnimationCurve movementCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

	[SerializeField]
	private bool loop = true;

	[SerializeField]
	private bool activated;

	[SerializeField]
	[Range(0f, 99f)]
	private float initialProgress;

	[SerializeField]
	private float delayAtEndPoint;

	[SerializeField]
	private string audioClipName = "CTB_platform_move_loop";

	private Vector3 startPosition;

	private Vector3 endPosition;

	private Vector3 currentDirection;

	private float duration;

	private float progress;

	private AudioObject audioItem;

	public float Distance => distance;

	public float MovementSpeed => movementSpeed;

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.magenta;
		Vector3 vector = ((!Application.isPlaying) ? base.transform.position : startPosition);
		Gizmos.DrawLine(vector, vector + direction * distance);
	}

	private void Start()
	{
		startPosition = base.transform.position;
		endPosition = base.transform.position + direction * distance;
		currentDirection = direction;
		duration = distance / movementSpeed;
		progress = duration * initialProgress * 0.01f;
		audioItem = AudioController.Play(audioClipName, base.transform);
		updatePlatformPosition();
	}

	private void Update()
	{
		if (activated)
		{
			updatePlatformPosition();
		}
	}

	private void updatePlatformPosition()
	{
		progress += Time.deltaTime * movementSpeed;
		float time = ((!loop) ? (Mathf.Lerp(0f, duration, progress / duration) / duration) : (Mathf.PingPong(progress, duration) / duration));
		Vector3 vector = Vector3.Lerp(startPosition, endPosition, movementCurve.Evaluate(time));
		Vector3 vector2 = vector - base.transform.position;
		if (loop && delayAtEndPoint > 0f && currentDirection != vector2.normalized)
		{
			StartCoroutine(doPauseAtEndPoint(delayAtEndPoint));
		}
		base.transform.position = vector;
		currentDirection = vector2.normalized;
		if (audioItem != null)
		{
			audioItem.volume = Mathf.Clamp01(vector2.magnitude / (movementSpeed * Time.deltaTime));
		}
	}

	private IEnumerator doPauseAtEndPoint(float delay)
	{
		activated = false;
		yield return new WaitForSeconds(delay);
		activated = true;
	}

	public void Toggle(bool status)
	{
		StopAllCoroutines();
		activated = status;
		if (audioItem != null)
		{
			audioItem.volume = 0f;
		}
	}
}
