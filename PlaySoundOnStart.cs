using UnityEngine;

public class PlaySoundOnStart : MonoBehaviour
{
	public string audioID;

	private void Start()
	{
		if (!string.IsNullOrEmpty(audioID))
		{
			AudioController.Play(audioID, base.transform);
		}
	}
}
