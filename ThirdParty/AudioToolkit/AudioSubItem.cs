using System;
using UnityEngine;

[Serializable]
public class AudioSubItem
{
	public AudioSubItemType SubItemType;

	public float Probability = 1f;

	public bool DisableOtherSubitems;

	public string ItemModeAudioID;

	public AudioClip Clip;

	public float Volume = 1f;

	public float PitchShift;

	public float Pan2D;

	public float Delay;

	public float RandomPitch;

	public float RandomVolume;

	public float RandomDelay;

	public float ClipStopTime;

	public float ClipStartTime;

	public float FadeIn;

	public float FadeOut;

	public bool RandomStartPosition;

	private float _summedProbability = -1f;

	internal int _subItemID;

	[NonSerialized]
	private AudioItem _item;

	internal float _SummedProbability
	{
		get
		{
			return _summedProbability;
		}
		set
		{
			_summedProbability = value;
		}
	}

	public AudioItem item
	{
		get
		{
			return _item;
		}
		internal set
		{
			_item = value;
		}
	}

	public override string ToString()
	{
		if (SubItemType == AudioSubItemType.Clip)
		{
			return "CLIP: " + Clip.name;
		}
		return "ITEM: " + ItemModeAudioID;
	}
}
