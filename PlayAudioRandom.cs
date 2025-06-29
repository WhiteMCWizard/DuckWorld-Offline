using System.Collections;
using UnityEngine;

public class PlayAudioRandom : MonoBehaviour
{
	[SerializeField]
	private AudioClip audio;

	[SerializeField]
	private float minIntervalTime;

	[SerializeField]
	private float maxIntervalTime;

	private void Start()
	{
		StartCoroutine(playAudioAfterRandomInterval());
	}

	private IEnumerator playAudioAfterRandomInterval()
	{
		yield return new WaitForSeconds(Random.value * (maxIntervalTime - minIntervalTime) + minIntervalTime);
		AudioObject ao = AudioController.Play(audio.name, base.transform);
		if (ao == null)
		{
			Debug.Log("Audio Object == null!", base.gameObject);
		}
		while (ao != null && ao.IsPlaying())
		{
			yield return null;
		}
		StartCoroutine(playAudioAfterRandomInterval());
	}
}
