using System.Collections;
using UnityEngine;

namespace SLAM.Shops;

public class FishBowlAudio : MonoBehaviour
{
	[SerializeField]
	private float delay;

	[SerializeField]
	private AudioClip fishAudio;

	[SerializeField]
	private AnimationClip fishAnimation;

	private void Start()
	{
		StartCoroutine(playAudioAfterDelay());
	}

	private IEnumerator playAudioAfterDelay()
	{
		yield return new WaitForSeconds(delay);
		AudioController.Play(fishAudio.name, base.transform);
		yield return new WaitForSeconds(fishAudio.length);
		delay = fishAnimation.length - fishAudio.length;
		StartCoroutine(playAudioAfterDelay());
	}
}
