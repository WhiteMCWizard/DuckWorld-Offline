using System;
using UnityEngine;

[Serializable]
public class AudioItem
{
	[Serializable]
	public enum LoopMode
	{
		DoNotLoop = 0,
		LoopSubitem = 1,
		LoopSequence = 2,
		PlaySequenceAndLoopLast = 4,
		IntroLoopOutroSequence = 5
	}

	public string Name;

	public LoopMode Loop;

	public int loopSequenceCount;

	public float loopSequenceOverlap;

	public float loopSequenceRandomDelay;

	public float loopSequenceRandomPitch;

	public float loopSequenceRandomVolume;

	public bool DestroyOnLoad = true;

	public float Volume = 1f;

	public AudioPickSubItemMode SubItemPickMode = AudioPickSubItemMode.RandomNotSameTwice;

	public float MinTimeBetweenPlayCalls = 0.1f;

	public int MaxInstanceCount;

	public float Delay;

	public bool overrideAudioSourceSettings;

	public float audioSource_MinDistance = 1f;

	public float audioSource_MaxDistance = 500f;

	public float spatialBlend;

	public AudioSubItem[] subItems;

	internal int _lastChosen = -1;

	internal double _lastPlayedTime = -1.0;

	[NonSerialized]
	private AudioCategory _category;

	public AudioCategory category
	{
		get
		{
			return _category;
		}
		private set
		{
			_category = value;
		}
	}

	private void Awake()
	{
		if (Loop == (LoopMode)3)
		{
			Loop = LoopMode.LoopSequence;
		}
		_lastChosen = -1;
	}

	internal void _Initialize(AudioCategory categ)
	{
		category = categ;
		_NormalizeSubItems();
	}

	private void _NormalizeSubItems()
	{
		float num = 0f;
		int num2 = 0;
		bool flag = false;
		AudioSubItem[] array = subItems;
		foreach (AudioSubItem audioSubItem in array)
		{
			if (_IsValidSubItem(audioSubItem) && audioSubItem.DisableOtherSubitems)
			{
				flag = true;
				break;
			}
		}
		AudioSubItem[] array2 = subItems;
		foreach (AudioSubItem audioSubItem2 in array2)
		{
			audioSubItem2.item = this;
			if (_IsValidSubItem(audioSubItem2) && (audioSubItem2.DisableOtherSubitems || !flag))
			{
				num += audioSubItem2.Probability;
			}
			audioSubItem2._subItemID = num2;
			num2++;
		}
		if (num <= 0f)
		{
			return;
		}
		float num3 = 0f;
		AudioSubItem[] array3 = subItems;
		foreach (AudioSubItem audioSubItem3 in array3)
		{
			if (_IsValidSubItem(audioSubItem3))
			{
				if (audioSubItem3.DisableOtherSubitems || !flag)
				{
					num3 += audioSubItem3.Probability / num;
				}
				audioSubItem3._SummedProbability = num3;
			}
		}
	}

	private static bool _IsValidSubItem(AudioSubItem item)
	{
		return item.SubItemType switch
		{
			AudioSubItemType.Clip => item.Clip != null, 
			AudioSubItemType.Item => item.ItemModeAudioID != null && item.ItemModeAudioID.Length > 0, 
			_ => false, 
		};
	}

	public void UnloadAudioClip()
	{
		AudioSubItem[] array = subItems;
		foreach (AudioSubItem audioSubItem in array)
		{
			if ((bool)audioSubItem.Clip)
			{
				Resources.UnloadAsset(audioSubItem.Clip);
			}
		}
	}
}
