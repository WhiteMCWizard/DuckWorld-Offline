using System.Collections;
using UnityEngine;

namespace SLAM.DuckQuiz;

public class DQScoreCounter : MonoBehaviour
{
	[SerializeField]
	private UILabel lblScoreCount;

	[SerializeField]
	private float scorePerSecond;

	private int scoreVisibleOnLabel;

	private void OnEnable()
	{
		int.TryParse(lblScoreCount.text, out scoreVisibleOnLabel);
		GameEvents.Subscribe<DQGameController.ScoreGainedEvent>(onScoreGained);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<DQGameController.ScoreGainedEvent>(onScoreGained);
	}

	private void onScoreGained(DQGameController.ScoreGainedEvent evt)
	{
		StopAllCoroutines();
		StartCoroutine(doScoreCounter(evt.TotalScore));
	}

	private IEnumerator doScoreCounter(int targetScore)
	{
		Stopwatch sw = new Stopwatch((float)Mathf.Abs(scoreVisibleOnLabel - targetScore) * scorePerSecond);
		int startScore = scoreVisibleOnLabel;
		while ((bool)sw)
		{
			yield return null;
			scoreVisibleOnLabel = (int)Mathf.Lerp(startScore, targetScore, sw.Progress);
			lblScoreCount.text = scoreVisibleOnLabel.ToString("0");
		}
		GameEvents.Invoke(new DQGameController.ScoreCounterFinishedEvent());
	}
}
