using System.Collections;
using UnityEngine;

namespace SLAM.MoneyDive;

public class MD_BonusPling : MonoBehaviour
{
	[SerializeField]
	private UILabel scoreLabel;

	[SerializeField]
	private UITweener[] bonusReceivedAnimations;

	[SerializeField]
	private TweenPosition moveToScoreAnimation;

	[SerializeField]
	private UITweener[] hideAnimations;

	[SerializeField]
	private UITweener[] bonusLostAnimations;

	[SerializeField]
	private float plingBeginDelay = 0.3f;

	private int points;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void BonusReceivedPling(int points, Transform totalScoreLabel, MD_HUDView hud)
	{
		this.points = points;
		scoreLabel.text = "+" + points;
		StartCoroutine(DoPling(totalScoreLabel, hud));
	}

	public void BonusLostPling(int score)
	{
		scoreLabel.text = score.ToString();
		float longestAnimationTime = UIExtentions.GetLongestAnimationTime(bonusLostAnimations);
		StartCoroutine(DestroyIn(longestAnimationTime));
	}

	private IEnumerator DoPling(Transform totalScoreLabel, MD_HUDView hud)
	{
		yield return new WaitForSeconds(plingBeginDelay);
		float longestAnimation = UIExtentions.GetLongestAnimationTime(bonusReceivedAnimations);
		for (int i = 0; i < bonusReceivedAnimations.Length; i++)
		{
			bonusReceivedAnimations[i].PlayForward();
		}
		yield return new WaitForSeconds(longestAnimation);
		moveToScoreAnimation.from = base.transform.localPosition;
		moveToScoreAnimation.to = base.transform.parent.InverseTransformPoint(totalScoreLabel.position);
		moveToScoreAnimation.PlayForward();
		for (int j = 0; j < hideAnimations.Length; j++)
		{
			hideAnimations[j].PlayForward();
		}
		yield return new WaitForSeconds(moveToScoreAnimation.delay + moveToScoreAnimation.duration);
		hud.AddPoints(points);
		Object.Destroy(base.gameObject);
	}

	private IEnumerator DestroyIn(float seconds)
	{
		yield return new WaitForSeconds(seconds);
		Object.Destroy(base.gameObject);
	}
}
