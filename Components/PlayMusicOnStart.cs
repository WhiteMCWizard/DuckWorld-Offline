using UnityEngine;

public class PlayMusicOnStart : MonoBehaviour
{
	public string audioID;

	private void Start()
	{
		if (!string.IsNullOrEmpty(audioID))
		{
			AudioController.PlayMusic(audioID);
		}
	}
}
