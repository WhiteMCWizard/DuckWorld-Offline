using System.Collections;
using UnityEngine;

namespace SLAM.Engine;

public class ScoreRow : MonoBehaviour
{
	[SerializeField]
	private UILabel lblDescription;

	[SerializeField]
	private UILabel lblScore;

	[SerializeField]
	private AnimationCurve alphaFadeInCurve;

	[SerializeField]
	private float fadeInTime;

	[SerializeField]
	private float minScoreIncrementTime = 1f;

	[SerializeField]
	private float maxScoreIncrementTime = 2f;

	[SerializeField]
	private float incrementTimePerScore = 0.1f;

	[SerializeField]
	private float scoreCountSoundInterval = 0.3f;

	private void Awake()
	{
		UILabel uILabel = lblDescription;
		float alpha = 0f;
		lblScore.alpha = alpha;
		uILabel.alpha = alpha;
	}

	public IEnumerator AnimateScoreRow(string description, int score, UILabel totalScoresLabel)
	{
		int beginTotalScores = int.Parse(totalScoresLabel.text);
		int endTotalScores = Mathf.Max(0, beginTotalScores + score);
		lblDescription.text = description;
		if (lblScore != null)
		{
			lblScore.text = "0";
		}
		Stopwatch fadeInStopwatch = new Stopwatch(fadeInTime);
		while (!fadeInStopwatch.Expired)
		{
			yield return null;
			lblDescription.alpha = alphaFadeInCurve.Evaluate(fadeInStopwatch.Progress);
			if (lblScore != null)
			{
				lblScore.alpha = alphaFadeInCurve.Evaluate(fadeInStopwatch.Progress);
			}
		}
		Stopwatch scoreIncrementStopwatch = new Stopwatch(Mathf.Clamp(Mathf.Abs((float)score * incrementTimePerScore), minScoreIncrementTime, maxScoreIncrementTime));
		float time = 0f;
		while (!scoreIncrementStopwatch.Expired)
		{
			time += Time.deltaTime;
			if (time > scoreCountSoundInterval)
			{
				PlayCounter();
				time %= scoreCountSoundInterval;
			}
			yield return null;
			lblScore.text = Mathf.Lerp(0f, score, scoreIncrementStopwatch.Progress).ToString("+#;-#;0");
			totalScoresLabel.text = Mathf.Lerp(beginTotalScores, endTotalScores, scoreIncrementStopwatch.Progress).ToString("#;-#;0");
		}
		AudioController.Play("Interface_score_counted");
	}

	private void PlayCounter()
	{
		if ((bool)SingletonMonoBehaviour<AudioController>.DoesInstanceExist())
		{
			AudioController.Play("Avatar_pickup_coin");
		}
	}
}
