using UnityEngine;

public class PlayAudioOnEnable : MonoBehaviour
{
	[SerializeField]
	private AudioClip audioClip;

	private void OnEnable()
	{
		if (audioClip == null)
		{
			Debug.LogWarning("I have a null audioClip!", this);
		}
		else
		{
			AudioController.Play(audioClip.name, base.transform);
		}
	}
}
