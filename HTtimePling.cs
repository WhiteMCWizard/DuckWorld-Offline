using System.Collections;
using UnityEngine;

public class HTtimePling : MonoBehaviour
{
	[SerializeField]
	private UILabel deltaTimeLabel;

	[SerializeField]
	private UITweener moveTween;

	[SerializeField]
	private UITweener alphaTween;

	public void DoIt(float deltaTime)
	{
		moveTween.PlayForward();
		alphaTween.PlayForward();
		bool flag = deltaTime > 0f;
		deltaTimeLabel.text = ((!flag) ? string.Empty : "+") + deltaTime;
		deltaTimeLabel.color = ((!flag) ? Color.red : Color.green);
		StartCoroutine(WaitAndDestroy(Mathf.Max(moveTween.duration + moveTween.delay, alphaTween.duration + alphaTween.delay)));
	}

	private IEnumerator WaitAndDestroy(float time)
	{
		yield return new WaitForSeconds(time);
		Object.Destroy(base.gameObject);
	}
}
