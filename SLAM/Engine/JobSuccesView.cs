using System.Collections;
using UnityEngine;

namespace SLAM.Engine;

public class JobSuccesView : SuccesBaseView
{
	[SerializeField]
	private UILabel scoreLabel;

	private float scoreIncrementTime = 0.5f;

	private float scoreCountSoundInterval = 0.3f;

	protected override void updateStuff(bool isJob, bool isNextLevelAvailable, bool canChallengeFriend)
	{
		base.updateStuff(isJob, isNextLevelAvailable, canChallengeFriend);
		scoreLabel.text = "0";
		btnNextLevel.gameObject.SetActive(isNextLevelAvailable);
		StartCoroutine(countUp(Controller<GameController>().TotalScore));
	}

	private IEnumerator countUp(int score)
	{
		yield return new WaitForSeconds(0.7f);
		float time = 0f;
		float duration = scoreIncrementTime;
		float soundTime = duration;
		while (time < duration)
		{
			time += Time.deltaTime;
			if (soundTime > scoreCountSoundInterval)
			{
				PlayCounter();
				soundTime %= scoreCountSoundInterval;
			}
			scoreLabel.text = Mathf.RoundToInt(Mathf.Lerp(0f, score, time / duration)).ToString();
			yield return null;
		}
	}

	private void PlayCounter()
	{
		if ((bool)SingletonMonoBehaviour<AudioController>.DoesInstanceExist())
		{
			AudioController.Play("Avatar_pickup_coin");
		}
	}
}
