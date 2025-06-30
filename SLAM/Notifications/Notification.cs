using System.Collections;
using UnityEngine;

namespace SLAM.Notifications;

public class Notification : MonoBehaviour
{
	[SerializeField]
	private float showDuration = 5f;

	[SerializeField]
	private UITweener tween;

	[SerializeField]
	private UILabel titleLabel;

	[SerializeField]
	private UILabel bodyLabel;

	[SerializeField]
	private UISprite achievementIcon;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void Show(NotificationEvent notification)
	{
		titleLabel.text = notification.Title;
		bodyLabel.text = notification.Body;
		achievementIcon.spriteName = notification.IconSpriteName;
		StartCoroutine(doShowHideRoutine());
	}

	private IEnumerator doShowHideRoutine()
	{
		AudioController.Play("Duckstad - score positive");
		float tweenDuration = tween.delay + tween.duration;
		tween.PlayForward();
		yield return CoroutineUtils.WaitForUnscaledSeconds(tweenDuration);
		yield return CoroutineUtils.WaitForUnscaledSeconds(showDuration);
		tween.PlayReverse();
		yield return CoroutineUtils.WaitForUnscaledSeconds(tweenDuration);
		Object.Destroy(base.gameObject);
	}
}
