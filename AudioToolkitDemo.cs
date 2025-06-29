using System.Collections.Generic;
using UnityEngine;

public class AudioToolkitDemo : MonoBehaviour
{
	public AudioClip customAudioClip;

	private float musicVolume = 1f;

	private bool musicPaused;

	private PoolableReference<AudioObject> introLoopOutroAudio;

	private bool wasClipAdded;

	private bool wasCategoryAdded;

	private List<bool> disableGUILevels = new List<bool>();

	private void OnGUI()
	{
		DrawGuiLeftSide();
		DrawGuiRightSide();
		DrawGuiBottom();
	}

	private void DrawGuiLeftSide()
	{
		GUIStyle gUIStyle = new GUIStyle(GUI.skin.label);
		gUIStyle.normal.textColor = new Color(1f, 1f, 0.5f);
		string text = $"ClockStone Audio Toolkit - Demo";
		GUI.Label(new Rect(22f, 10f, 500f, 20f), text, gUIStyle);
		int num = 10;
		int num2 = 35;
		int num3 = 200;
		num += 50;
		GUI.Label(new Rect(250f, num, num3, 30f), "Global Volume");
		AudioController.SetGlobalVolume(GUI.HorizontalSlider(new Rect(250f, num + 20, num3, 30f), AudioController.GetGlobalVolume(), 0f, 1f));
		if (GUI.Button(new Rect(20f, num, num3, 30f), "Cross-fade to music track 1"))
		{
			AudioController.PlayMusic("MusicTrack1");
		}
		num += num2;
		GUI.Label(new Rect(250f, num + 10, num3, 30f), "Music Volume");
		float num4 = GUI.HorizontalSlider(new Rect(250f, num + 30, num3, 30f), musicVolume, 0f, 1f);
		if (num4 != musicVolume)
		{
			musicVolume = num4;
			AudioController.SetCategoryVolume("Music", musicVolume);
		}
		if (GUI.Button(new Rect(20f, num, num3, 30f), "Cross-fade to music track 2"))
		{
			AudioController.PlayMusic("MusicTrack2");
		}
		num += num2;
		if (GUI.Button(new Rect(20f, num, num3, 30f), "Stop Music"))
		{
			AudioController.StopMusic(0.3f);
		}
		num += num2;
		bool flag = GUI.Toggle(new Rect(20f, num, num3, 30f), musicPaused, "Pause All Audio");
		if (flag != musicPaused)
		{
			musicPaused = flag;
			if (musicPaused)
			{
				AudioController.PauseAll(0.1f);
			}
			else
			{
				AudioController.UnpauseAll(0.1f);
			}
		}
		num += num2;
		num += num2;
		if (GUI.Button(new Rect(20f, num, num3, 30f), "Play Sound with random pitch"))
		{
			AudioController.Play("RandomPitchSound");
		}
		num += num2;
		if (GUI.Button(new Rect(20f, num, num3, 30f), "Play Sound with alternatives"))
		{
			AudioObject audioObject = AudioController.Play("AlternativeSound");
			if (audioObject != null)
			{
				audioObject.completelyPlayedDelegate = OnAudioCompleteleyPlayed;
			}
		}
		num += num2;
		if (GUI.Button(new Rect(20f, num, num3, 30f), "Play Both"))
		{
			AudioObject audioObject2 = AudioController.Play("RandomAndAlternativeSound");
			if (audioObject2 != null)
			{
				audioObject2.completelyPlayedDelegate = OnAudioCompleteleyPlayed;
			}
		}
		num += num2;
		num += num2;
		if (GUI.Button(new Rect(20f, num, num3, 30f), "Play Music Playlist"))
		{
			AudioController.PlayMusicPlaylist();
		}
		num += num2;
		if (AudioController.IsPlaylistPlaying() && GUI.Button(new Rect(20f, num, num3, 30f), "Next Track on Playlist"))
		{
			AudioController.PlayNextMusicOnPlaylist();
		}
		num += 32;
		if (AudioController.IsPlaylistPlaying() && GUI.Button(new Rect(20f, num, num3, 30f), "Previous Track on Playlist"))
		{
			AudioController.PlayPreviousMusicOnPlaylist();
		}
		num += num2;
		SingletonMonoBehaviour<AudioController>.Instance.loopPlaylist = GUI.Toggle(new Rect(20f, num, num3, 30f), SingletonMonoBehaviour<AudioController>.Instance.loopPlaylist, "Loop Playlist");
		num += num2;
		SingletonMonoBehaviour<AudioController>.Instance.shufflePlaylist = GUI.Toggle(new Rect(20f, num, num3, 30f), SingletonMonoBehaviour<AudioController>.Instance.shufflePlaylist, "Shuffle Playlist ");
		num += num2;
		SingletonMonoBehaviour<AudioController>.Instance.soundMuted = GUI.Toggle(new Rect(20f, num, num3, 30f), SingletonMonoBehaviour<AudioController>.Instance.soundMuted, "Sound Muted");
	}

	private void DrawGuiRightSide()
	{
		int num = 50;
		int num2 = 35;
		int num3 = 300;
		if (!wasCategoryAdded)
		{
			if (customAudioClip != null && GUI.Button(new Rect(Screen.width - (num3 + 20), num, num3, 30f), "Create new category with custom AudioClip"))
			{
				AudioCategory category = AudioController.NewCategory("Custom Category");
				AudioController.AddToCategory(category, customAudioClip, "CustomAudioItem");
				wasClipAdded = true;
				wasCategoryAdded = true;
			}
		}
		else
		{
			if (GUI.Button(new Rect(Screen.width - (num3 + 20), num, num3, 30f), "Play custom AudioClip"))
			{
				AudioController.Play("CustomAudioItem");
			}
			if (wasClipAdded)
			{
				num += num2;
				if (GUI.Button(new Rect(Screen.width - (num3 + 20), num, num3, 30f), "Remove custom AudioClip") && AudioController.RemoveAudioItem("CustomAudioItem"))
				{
					wasClipAdded = false;
				}
			}
		}
		num = 130;
		if (GUI.Button(new Rect(Screen.width - (num3 + 20), num, num3, 30f), "Play gapless audio loop [Unity v4.1]"))
		{
			AudioController.Play("GaplessLoopTest").Stop(1f, 4f);
		}
		num += num2;
		if (GUI.Button(new Rect(Screen.width - (num3 + 20), num, num3, 30f), "Play random loop sequence [Unity v4.1]"))
		{
			AudioController.Play("RandomLoopSequence");
		}
		num += num2;
		if (GUI.Button(new Rect(Screen.width - (num3 + 20), num, num3, 50f), "Play intro-loop-outro sequence\ngatling gun [Unity v4.1]"))
		{
			introLoopOutroAudio = new PoolableReference<AudioObject>(AudioController.Play("IntroLoopOutro_Gun"));
		}
		num += 20;
		num += num2;
		BeginDisabledGroup(introLoopOutroAudio == null || !(introLoopOutroAudio.Get() != null));
		if (GUI.Button(new Rect(Screen.width - (num3 + 20), num, num3, 30f), "Finish gatling gun sequence [Unity v4.1]"))
		{
			introLoopOutroAudio.Get().FinishSequence();
		}
		EndDisabledGroup();
	}

	private void DrawGuiBottom()
	{
		if (GUI.Button(new Rect(Screen.width / 2 - 150, Screen.height - 40, 300f, 30f), "Video tutorial & more info..."))
		{
			Application.OpenURL("http://unity.clockstone.com");
		}
	}

	private void OnAudioCompleteleyPlayed(AudioObject audioObj)
	{
		Debug.Log("Finished playing " + audioObj.audioID + " with clip " + audioObj.primaryAudioSource.clip.name);
	}

	private void BeginDisabledGroup(bool condition)
	{
		disableGUILevels.Add(condition);
		GUI.enabled = !IsGUIDisabled();
	}

	private void EndDisabledGroup()
	{
		int count = disableGUILevels.Count;
		if (count > 0)
		{
			disableGUILevels.RemoveAt(count - 1);
			GUI.enabled = !IsGUIDisabled();
		}
		else
		{
			Debug.LogWarning("misplaced EndDisabledGroup");
		}
	}

	private bool IsGUIDisabled()
	{
		foreach (bool disableGUILevel in disableGUILevels)
		{
			if (disableGUILevel)
			{
				return true;
			}
		}
		return false;
	}
}
