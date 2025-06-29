using UnityEngine;

public class PlayAudioAtStart : MonoBehaviour
{
	[SerializeField]
	private AudioClip audioClip;

	private void Start()
	{
		AudioController.Play(audioClip.name, base.transform);
	}
}
