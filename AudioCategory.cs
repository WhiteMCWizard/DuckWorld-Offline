using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AudioCategory
{
	public string Name;

	private AudioCategory _parentCategory;

	[SerializeField]
	private string _parentCategoryName;

	public GameObject AudioObjectPrefab;

	public AudioItem[] AudioItems;

	[SerializeField]
	private float _volume = 1f;

	public float Volume
	{
		get
		{
			return _volume;
		}
		set
		{
			_volume = value;
			_ApplyVolumeChange();
		}
	}

	public float VolumeTotal
	{
		get
		{
			if (parentCategory != null)
			{
				return parentCategory.VolumeTotal * _volume;
			}
			return _volume;
		}
	}

	public AudioCategory parentCategory
	{
		get
		{
			if (string.IsNullOrEmpty(_parentCategoryName))
			{
				return null;
			}
			if (_parentCategory == null)
			{
				if (audioController != null)
				{
					_parentCategory = audioController._GetCategory(_parentCategoryName);
				}
				else
				{
					Debug.LogWarning("_audioController == null");
				}
			}
			return _parentCategory;
		}
		set
		{
			_parentCategory = value;
			if (value != null)
			{
				_parentCategoryName = _parentCategory.Name;
			}
			else
			{
				_parentCategoryName = null;
			}
		}
	}

	public AudioController audioController { get; set; }

	public AudioCategory(AudioController audioController)
	{
		this.audioController = audioController;
	}

	internal void _AnalyseAudioItems(Dictionary<string, AudioItem> audioItemsDict)
	{
		if (AudioItems == null)
		{
			return;
		}
		AudioItem[] audioItems = AudioItems;
		foreach (AudioItem audioItem in audioItems)
		{
			if (audioItem == null)
			{
				continue;
			}
			audioItem._Initialize(this);
			if (audioItemsDict != null)
			{
				try
				{
					audioItemsDict.Add(audioItem.Name, audioItem);
				}
				catch (ArgumentException)
				{
					Debug.LogWarning("Multiple audio items with name '" + audioItem.Name + "'");
				}
			}
		}
	}

	internal int _GetIndexOf(AudioItem audioItem)
	{
		if (AudioItems == null)
		{
			return -1;
		}
		for (int i = 0; i < AudioItems.Length; i++)
		{
			if (audioItem == AudioItems[i])
			{
				return i;
			}
		}
		return -1;
	}

	private void _ApplyVolumeChange()
	{
		List<AudioObject> playingAudioObjects = AudioController.GetPlayingAudioObjects();
		for (int i = 0; i < playingAudioObjects.Count; i++)
		{
			AudioObject audioObject = playingAudioObjects[i];
			if (_IsCategoryParentOf(audioObject.category, this))
			{
				audioObject._ApplyVolumeBoth();
			}
		}
	}

	private bool _IsCategoryParentOf(AudioCategory toTest, AudioCategory parent)
	{
		for (AudioCategory audioCategory = toTest; audioCategory != null; audioCategory = audioCategory.parentCategory)
		{
			if (audioCategory == parent)
			{
				return true;
			}
		}
		return false;
	}

	public void UnloadAllAudioClips()
	{
		for (int i = 0; i < AudioItems.Length; i++)
		{
			AudioItems[i].UnloadAudioClip();
		}
	}
}
