using System.Collections;
using UnityEngine;

namespace SLAM.MoneyDive;

public class MD_PointsPling : MonoBehaviour
{
	[SerializeField]
	private UILabel scoreLabel;

	[SerializeField]
	private UITweener[] showAnimations;

	[SerializeField]
	private TweenPosition moveToScoreAnimation;

	[SerializeField]
	private UITweener[] hideAnimations;

	private int points;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void Pling(int points, Transform scoreLabel, MD_HUDView hud)
	{
		this.points = points;
		this.scoreLabel.text = "+" + this.points;
		StartCoroutine(DoPling(scoreLabel, hud));
	}

	private IEnumerator DoPling(Transform scoreLabel, MD_HUDView hud)
	{
		float longestAnimation = UIExtentions.GetLongestAnimationTime(showAnimations);
		for (int i = 0; i < showAnimations.Length; i++)
		{
			showAnimations[i].PlayForward();
		}
		yield return new WaitForSeconds(longestAnimation);
		moveToScoreAnimation.from = base.transform.localPosition;
		moveToScoreAnimation.to = base.transform.parent.InverseTransformPoint(scoreLabel.position);
		moveToScoreAnimation.PlayForward();
		for (int j = 0; j < hideAnimations.Length; j++)
		{
			hideAnimations[j].PlayForward();
		}
		yield return new WaitForSeconds(UIExtentions.GetAnimationDuration(moveToScoreAnimation));
		hud.AddPoints(points);
		Object.Destroy(base.gameObject);
	}
}
