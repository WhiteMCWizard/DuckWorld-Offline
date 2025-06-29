using System;
using System.Collections;
using UnityEngine;

[AddComponentMenu("ClockStone/Audio/AudioObject")]
[RequireComponent(typeof(AudioSource))]
public class AudioObject : RegisteredComponent
{
	public delegate void AudioEventDelegate(AudioObject audioObject);

	private const float VOLUME_TRANSFORM_POWER = 1.6f;

	[NonSerialized]
	private AudioCategory _category;

	private AudioSubItem _subItemPrimary;

	private AudioSubItem _subItemSecondary;

	private AudioEventDelegate _completelyPlayedDelegate;

	private int _pauseCoroutineCounter;

	internal float _volumeExcludingCategory = 1f;

	private float _volumeFromPrimaryFade = 1f;

	private float _volumeFromSecondaryFade = 1f;

	internal float _volumeFromScriptCall = 1f;

	private bool _paused;

	private bool _applicationPaused;

	private AudioFader _primaryFader;

	private AudioFader _secondaryFader;

	private double _playTime = -1.0;

	private double _playStartTimeLocal = -1.0;

	private double _playStartTimeSystem = -1.0;

	private double _playScheduledTimeDsp = -1.0;

	private double _audioObjectTime;

	private bool _IsInactive = true;

	private bool _stopRequested;

	private bool _finishSequence;

	private int _loopSequenceCount;

	private bool _stopAfterFadeoutUserSetting;

	private bool _pauseWithFadeOutRequested;

	private double _dspTimeRemainingAtPause;

	private AudioController _audioController;

	internal bool _isCurrentPlaylistTrack;

	internal float _audioSource_MinDistance_Saved = 1f;

	internal float _audioSource_MaxDistance_Saved = 500f;

	internal int _lastChosenSubItemIndex = -1;

	private AudioSource _audioSource1;

	private AudioSource _audioSource2;

	private bool _primaryAudioSourcePaused;

	private bool _secondaryAudioSourcePaused;

	public string audioID { get; internal set; }

	public AudioCategory category
	{
		get
		{
			return _category;
		}
		internal set
		{
			_category = value;
		}
	}

	public AudioSubItem subItem
	{
		get
		{
			return _subItemPrimary;
		}
		internal set
		{
			_subItemPrimary = value;
		}
	}

	public bool isPlayedAsMusic { get; internal set; }

	public AudioItem audioItem
	{
		get
		{
			if (subItem != null)
			{
				return subItem.item;
			}
			return null;
		}
	}

	public AudioEventDelegate completelyPlayedDelegate
	{
		get
		{
			return _completelyPlayedDelegate;
		}
		set
		{
			_completelyPlayedDelegate = value;
		}
	}

	public float volume
	{
		get
		{
			return _volumeWithCategory;
		}
		set
		{
			float volumeFromCategory = _volumeFromCategory;
			if (volumeFromCategory > 0f)
			{
				_volumeExcludingCategory = value / volumeFromCategory;
			}
			else
			{
				_volumeExcludingCategory = value;
			}
			_ApplyVolumeBoth();
		}
	}

	public float volumeItem
	{
		get
		{
			if (_volumeFromScriptCall > 0f)
			{
				return _volumeExcludingCategory / _volumeFromScriptCall;
			}
			return _volumeExcludingCategory;
		}
		set
		{
			_volumeExcludingCategory = value * _volumeFromScriptCall;
			_ApplyVolumeBoth();
		}
	}

	public float volumeTotal => volumeTotalWithoutFade * _volumeFromPrimaryFade;

	public float volumeTotalWithoutFade
	{
		get
		{
			float num = _volumeWithCategory;
			AudioController audioController = null;
			audioController = ((category == null) ? _audioController : category.audioController);
			if (audioController != null)
			{
				num *= audioController.Volume;
				if (audioController.soundMuted && !isPlayedAsMusic)
				{
					num = 0f;
				}
			}
			return num;
		}
	}

	public double playCalledAtTime => _playTime;

	public double startedPlayingAtTime => _playStartTimeSystem;

	public float timeUntilEnd => clipLength - audioTime;

	public double scheduledPlayingAtDspTime
	{
		get
		{
			return _playScheduledTimeDsp;
		}
		set
		{
			_playScheduledTimeDsp = value;
			primaryAudioSource.SetScheduledStartTime(_playScheduledTimeDsp);
		}
	}

	public float clipLength
	{
		get
		{
			if (_stopClipAtTime > 0f)
			{
				return _stopClipAtTime - _startClipAtTime;
			}
			if (primaryAudioSource.clip != null)
			{
				return primaryAudioSource.clip.length - _startClipAtTime;
			}
			return 0f;
		}
	}

	public float audioTime
	{
		get
		{
			return primaryAudioSource.time - _startClipAtTime;
		}
		set
		{
			primaryAudioSource.time = value + _startClipAtTime;
		}
	}

	public bool isFadingOut => _primaryFader.isFadingOut;

	public bool isFadeOutComplete => _primaryFader.isFadingOutComplete;

	public bool isFadingOutOrScheduled => _primaryFader.isFadingOutOrScheduled;

	public bool isFadingIn => _primaryFader.isFadingIn;

	public float pitch
	{
		get
		{
			return primaryAudioSource.pitch;
		}
		set
		{
			primaryAudioSource.pitch = value;
		}
	}

	public float pan
	{
		get
		{
			return primaryAudioSource.panStereo;
		}
		set
		{
			primaryAudioSource.panStereo = value;
		}
	}

	public double audioObjectTime => _audioObjectTime;

	public bool stopAfterFadeOut
	{
		get
		{
			return _stopAfterFadeoutUserSetting;
		}
		set
		{
			_stopAfterFadeoutUserSetting = value;
		}
	}

	public AudioSource primaryAudioSource => _audioSource1;

	public AudioSource secondaryAudioSource => _audioSource2;

	internal float _volumeFromCategory
	{
		get
		{
			if (category != null)
			{
				return category.VolumeTotal;
			}
			return 1f;
		}
	}

	internal float _volumeWithCategory => _volumeFromCategory * _volumeExcludingCategory;

	private float _stopClipAtTime => (subItem == null) ? 0f : subItem.ClipStopTime;

	private float _startClipAtTime => (subItem == null) ? 0f : subItem.ClipStartTime;

	private bool _shouldStopIfPrimaryFadedOut => _stopAfterFadeoutUserSetting && !_pauseWithFadeOutRequested;

	public void FadeIn(float fadeInTime)
	{
		if (_playStartTimeLocal > 0.0)
		{
			double num = _playStartTimeLocal - audioObjectTime;
			if (num > 0.0)
			{
				_primaryFader.FadeIn(fadeInTime, _playStartTimeLocal);
				_UpdateFadeVolume();
				return;
			}
		}
		_primaryFader.FadeIn(fadeInTime, audioObjectTime, !_shouldStopIfPrimaryFadedOut);
		_UpdateFadeVolume();
	}

	public void PlayScheduled(double dspTime)
	{
		_PlayScheduled(dspTime);
	}

	public void PlayAfter(string audioID, double deltaDspTime = 0.0, float volume = 1f, float startTime = 0f)
	{
		AudioController.PlayAfter(audioID, this, deltaDspTime, volume, startTime);
	}

	public void PlayNow(string audioID, float delay = 0f, float volume = 1f, float startTime = 0f)
	{
		AudioItem audioItem = AudioController.GetAudioItem(audioID);
		if (audioItem == null)
		{
			Debug.LogWarning("Audio item with name '" + audioID + "' does not exist");
		}
		else
		{
			_audioController.PlayAudioItem(audioItem, volume, base.transform.position, base.transform.parent, delay, startTime, playWithoutAudioObject: false, this, 0.0);
		}
	}

	public void Play(float delay = 0f)
	{
		_PlayDelayed(delay);
	}

	public void Stop()
	{
		Stop(-1f);
	}

	public void Stop(float fadeOutLength)
	{
		Stop(fadeOutLength, 0f);
	}

	public void Stop(float fadeOutLength, float startToFadeTime)
	{
		if (IsPaused(returnTrueIfStillFadingOut: false))
		{
			fadeOutLength = 0f;
			startToFadeTime = 0f;
		}
		if (startToFadeTime > 0f)
		{
			StartCoroutine(_WaitForSecondsThenStop(startToFadeTime, fadeOutLength));
			return;
		}
		_stopRequested = true;
		if (fadeOutLength < 0f)
		{
			fadeOutLength = ((subItem == null) ? 0f : subItem.FadeOut);
		}
		if (fadeOutLength == 0f && startToFadeTime == 0f)
		{
			_Stop();
			return;
		}
		FadeOut(fadeOutLength, startToFadeTime);
		if (IsSecondaryPlaying())
		{
			SwitchAudioSources();
			FadeOut(fadeOutLength, startToFadeTime);
			SwitchAudioSources();
		}
	}

	public void FinishSequence()
	{
		if (_finishSequence)
		{
			return;
		}
		AudioItem audioItem = this.audioItem;
		if (audioItem != null)
		{
			switch (audioItem.Loop)
			{
			case AudioItem.LoopMode.LoopSequence:
			case (AudioItem.LoopMode)3:
				_finishSequence = true;
				break;
			case AudioItem.LoopMode.PlaySequenceAndLoopLast:
			case AudioItem.LoopMode.IntroLoopOutroSequence:
				primaryAudioSource.loop = false;
				_finishSequence = true;
				break;
			}
		}
	}

	private IEnumerator _WaitForSecondsThenStop(float startToFadeTime, float fadeOutLength)
	{
		yield return new WaitForSeconds(startToFadeTime);
		if (!_IsInactive)
		{
			Stop(fadeOutLength);
		}
	}

	public void FadeOut(float fadeOutLength)
	{
		FadeOut(fadeOutLength, 0f);
	}

	public void FadeOut(float fadeOutLength, float startToFadeTime)
	{
		if (fadeOutLength < 0f)
		{
			fadeOutLength = ((subItem == null) ? 0f : subItem.FadeOut);
		}
		if (fadeOutLength > 0f || startToFadeTime > 0f)
		{
			_primaryFader.FadeOut(fadeOutLength, startToFadeTime);
		}
		else if (fadeOutLength == 0f)
		{
			if (_shouldStopIfPrimaryFadedOut)
			{
				_Stop();
			}
			else
			{
				_primaryFader.FadeOut(0f, startToFadeTime);
			}
		}
	}

	public void Pause()
	{
		Pause(0f);
	}

	public void Pause(float fadeOutTime)
	{
		if (!_paused)
		{
			_paused = true;
			if (fadeOutTime > 0f)
			{
				_pauseWithFadeOutRequested = true;
				FadeOut(fadeOutTime);
				StartCoroutine(_WaitThenPause(fadeOutTime, ++_pauseCoroutineCounter));
			}
			else
			{
				_PauseNow();
			}
		}
	}

	private void _PauseNow()
	{
		if (_playScheduledTimeDsp > 0.0)
		{
			_dspTimeRemainingAtPause = _playScheduledTimeDsp - AudioSettings.dspTime;
			scheduledPlayingAtDspTime = 9000000000.0;
		}
		_PauseAudioSources();
		if (_pauseWithFadeOutRequested)
		{
			_pauseWithFadeOutRequested = false;
			_primaryFader.Set0();
		}
	}

	public void Unpause()
	{
		Unpause(0f);
	}

	public void Unpause(float fadeInTime)
	{
		if (_paused)
		{
			_UnpauseNow();
			if (fadeInTime > 0f)
			{
				FadeIn(fadeInTime);
			}
			_pauseWithFadeOutRequested = false;
		}
	}

	private void _UnpauseNow()
	{
		_paused = false;
		if ((bool)secondaryAudioSource && _secondaryAudioSourcePaused)
		{
			secondaryAudioSource.Play();
		}
		if (_dspTimeRemainingAtPause > 0.0 && _primaryAudioSourcePaused)
		{
			double time = AudioSettings.dspTime + _dspTimeRemainingAtPause;
			_playStartTimeSystem = AudioController.systemTime + _dspTimeRemainingAtPause;
			primaryAudioSource.PlayScheduled(time);
			scheduledPlayingAtDspTime = time;
			_dspTimeRemainingAtPause = -1.0;
		}
		else if (_primaryAudioSourcePaused)
		{
			primaryAudioSource.Play();
		}
	}

	private IEnumerator _WaitThenPause(float waitTime, int counter)
	{
		yield return new WaitForSeconds(waitTime);
		if (_pauseWithFadeOutRequested && counter == _pauseCoroutineCounter)
		{
			_PauseNow();
		}
	}

	private void _PauseAudioSources()
	{
		if (primaryAudioSource.isPlaying)
		{
			_primaryAudioSourcePaused = true;
			primaryAudioSource.Pause();
		}
		else
		{
			_primaryAudioSourcePaused = false;
		}
		if ((bool)secondaryAudioSource && secondaryAudioSource.isPlaying)
		{
			_secondaryAudioSourcePaused = true;
			secondaryAudioSource.Pause();
		}
		else
		{
			_secondaryAudioSourcePaused = false;
		}
	}

	public bool IsPaused(bool returnTrueIfStillFadingOut = true)
	{
		if (!returnTrueIfStillFadingOut)
		{
			return !_pauseWithFadeOutRequested && _paused;
		}
		return _paused;
	}

	public bool IsPlaying()
	{
		return IsPrimaryPlaying() || IsSecondaryPlaying();
	}

	public bool IsPrimaryPlaying()
	{
		return primaryAudioSource.isPlaying;
	}

	public bool IsSecondaryPlaying()
	{
		return secondaryAudioSource != null && secondaryAudioSource.isPlaying;
	}

	public void SwitchAudioSources()
	{
		if (_audioSource2 == null)
		{
			_CreateSecondAudioSource();
		}
		_SwitchValues(ref _audioSource1, ref _audioSource2);
		_SwitchValues(ref _primaryFader, ref _secondaryFader);
		_SwitchValues(ref _subItemPrimary, ref _subItemSecondary);
		_SwitchValues(ref _volumeFromPrimaryFade, ref _volumeFromSecondaryFade);
	}

	private void _SwitchValues<T>(ref T v1, ref T v2)
	{
		T val = v1;
		v1 = v2;
		v2 = val;
	}

	protected override void Awake()
	{
		base.Awake();
		if (_primaryFader == null)
		{
			_primaryFader = new AudioFader();
		}
		else
		{
			_primaryFader.Set0();
		}
		if (_secondaryFader == null)
		{
			_secondaryFader = new AudioFader();
		}
		else
		{
			_secondaryFader.Set0();
		}
		if (_audioSource1 == null)
		{
			_audioSource1 = GetComponent<AudioSource>();
		}
		else if ((bool)_audioSource2 && _audioSource1 != GetComponent<AudioSource>())
		{
			SwitchAudioSources();
		}
		_Set0();
		_audioController = SingletonMonoBehaviour<AudioController>.Instance;
	}

	private void _CreateSecondAudioSource()
	{
		_audioSource2 = base.gameObject.AddComponent<AudioSource>();
		_audioSource2.rolloffMode = _audioSource1.rolloffMode;
		_audioSource2.minDistance = _audioSource1.minDistance;
		_audioSource2.maxDistance = _audioSource1.maxDistance;
		_audioSource2.dopplerLevel = _audioSource1.dopplerLevel;
		_audioSource2.spread = _audioSource1.spread;
		_audioSource2.spatialBlend = _audioSource1.spatialBlend;
		_audioSource2.velocityUpdateMode = _audioSource1.velocityUpdateMode;
		_audioSource2.ignoreListenerVolume = _audioSource1.ignoreListenerVolume;
		_audioSource2.playOnAwake = false;
		_audioSource2.priority = _audioSource1.priority;
		_audioSource2.bypassEffects = _audioSource1.bypassEffects;
		_audioSource2.ignoreListenerPause = _audioSource1.ignoreListenerPause;
	}

	private void _Set0()
	{
		_SetReferences0();
		_audioObjectTime = 0.0;
		primaryAudioSource.playOnAwake = false;
		if ((bool)secondaryAudioSource)
		{
			secondaryAudioSource.playOnAwake = false;
		}
		_lastChosenSubItemIndex = -1;
		_primaryFader.Set0();
		_secondaryFader.Set0();
		_playTime = -1.0;
		_playStartTimeLocal = -1.0;
		_playStartTimeSystem = -1.0;
		_playScheduledTimeDsp = -1.0;
		_volumeFromPrimaryFade = 1f;
		_volumeFromSecondaryFade = 1f;
		_volumeFromScriptCall = 1f;
		_IsInactive = true;
		_stopRequested = false;
		_finishSequence = false;
		_volumeExcludingCategory = 1f;
		_paused = false;
		_applicationPaused = false;
		_isCurrentPlaylistTrack = false;
		_loopSequenceCount = 0;
		_stopAfterFadeoutUserSetting = true;
		_pauseWithFadeOutRequested = false;
		_dspTimeRemainingAtPause = -1.0;
		_primaryAudioSourcePaused = false;
		_secondaryAudioSourcePaused = false;
	}

	private void _SetReferences0()
	{
		_audioController = null;
		primaryAudioSource.clip = null;
		if (secondaryAudioSource != null)
		{
			secondaryAudioSource.playOnAwake = false;
			secondaryAudioSource.clip = null;
		}
		subItem = null;
		category = null;
		_completelyPlayedDelegate = null;
	}

	private void _PlayScheduled(double dspTime)
	{
		if (!primaryAudioSource.clip)
		{
			Debug.LogError("audio.clip == null in " + base.gameObject.name);
			return;
		}
		_playScheduledTimeDsp = dspTime;
		double num = dspTime - AudioSettings.dspTime;
		_playStartTimeLocal = num + audioObjectTime;
		_playStartTimeSystem = num + AudioController.systemTime;
		primaryAudioSource.PlayScheduled(dspTime);
		_OnPlay();
	}

	private void _PlayDelayed(float delay)
	{
		if (!primaryAudioSource.clip)
		{
			Debug.LogError("audio.clip == null in " + base.gameObject.name);
			return;
		}
		primaryAudioSource.PlayDelayed(delay);
		_playScheduledTimeDsp = -1.0;
		_playStartTimeLocal = audioObjectTime + (double)delay;
		_playStartTimeSystem = AudioController.systemTime + (double)delay;
		_OnPlay();
	}

	private void _OnPlay()
	{
		_IsInactive = false;
		_playTime = audioObjectTime;
		_paused = false;
		_primaryAudioSourcePaused = false;
		_secondaryAudioSourcePaused = false;
		_primaryFader.Set0();
	}

	private void _Stop()
	{
		_primaryFader.Set0();
		_secondaryFader.Set0();
		primaryAudioSource.Stop();
		if ((bool)secondaryAudioSource)
		{
			secondaryAudioSource.Stop();
		}
		_paused = false;
		_primaryAudioSourcePaused = false;
		_secondaryAudioSourcePaused = false;
	}

	private void Update()
	{
		if (_IsInactive)
		{
			return;
		}
		if (!IsPaused(returnTrueIfStillFadingOut: false))
		{
			_audioObjectTime += AudioController.systemDeltaTime;
			_primaryFader.time = _audioObjectTime;
			_secondaryFader.time = _audioObjectTime;
		}
		if (_playScheduledTimeDsp > 0.0 && _audioObjectTime > _playStartTimeLocal)
		{
			_playScheduledTimeDsp = -1.0;
		}
		if (!_paused && !_applicationPaused)
		{
			bool flag = IsPrimaryPlaying();
			bool flag2 = IsSecondaryPlaying();
			if (!flag && !flag2)
			{
				bool flag3 = true;
				if (!_stopRequested && flag3 && completelyPlayedDelegate != null)
				{
					completelyPlayedDelegate(this);
					flag3 = !IsPlaying();
				}
				if (_isCurrentPlaylistTrack && (bool)SingletonMonoBehaviour<AudioController>.DoesInstanceExist())
				{
					SingletonMonoBehaviour<AudioController>.Instance._NotifyPlaylistTrackCompleteleyPlayed(this);
				}
				if (flag3)
				{
					DestroyAudioObject();
					return;
				}
			}
			else
			{
				if (!_stopRequested && _IsAudioLoopSequenceMode() && !IsSecondaryPlaying() && timeUntilEnd < 1f + Mathf.Max(0f, audioItem.loopSequenceOverlap) && _playScheduledTimeDsp < 0.0)
				{
					_ScheduleNextInLoopSequence();
				}
				if (!primaryAudioSource.loop)
				{
					if (_isCurrentPlaylistTrack && (bool)_audioController && _audioController.crossfadePlaylist && audioTime > clipLength - _audioController.musicCrossFadeTime_Out)
					{
						if ((bool)SingletonMonoBehaviour<AudioController>.DoesInstanceExist())
						{
							SingletonMonoBehaviour<AudioController>.Instance._NotifyPlaylistTrackCompleteleyPlayed(this);
						}
					}
					else
					{
						_StartFadeOutIfNecessary();
						if (flag2)
						{
							SwitchAudioSources();
							_StartFadeOutIfNecessary();
							SwitchAudioSources();
						}
					}
				}
			}
		}
		_UpdateFadeVolume();
	}

	private void _StartFadeOutIfNecessary()
	{
		if (subItem == null)
		{
			Debug.LogWarning("subItem == null");
			return;
		}
		float num = audioTime;
		if (!isFadingOutOrScheduled && subItem.FadeOut > 0f && num > clipLength - subItem.FadeOut)
		{
			FadeOut(subItem.FadeOut);
		}
	}

	private bool _IsAudioLoopSequenceMode()
	{
		AudioItem audioItem = this.audioItem;
		if (audioItem != null)
		{
			switch (audioItem.Loop)
			{
			case AudioItem.LoopMode.LoopSequence:
			case (AudioItem.LoopMode)3:
				return true;
			case AudioItem.LoopMode.PlaySequenceAndLoopLast:
			case AudioItem.LoopMode.IntroLoopOutroSequence:
				return !primaryAudioSource.loop;
			}
		}
		return false;
	}

	private bool _ScheduleNextInLoopSequence()
	{
		int num = ((audioItem.loopSequenceCount <= 0) ? audioItem.subItems.Length : audioItem.loopSequenceCount);
		if (_finishSequence)
		{
			if (audioItem.Loop != AudioItem.LoopMode.IntroLoopOutroSequence)
			{
				return false;
			}
			if (_loopSequenceCount <= num - 3)
			{
				return false;
			}
			if (_loopSequenceCount >= num - 1)
			{
				return false;
			}
		}
		if (audioItem.loopSequenceCount > 0 && audioItem.loopSequenceCount <= _loopSequenceCount + 1)
		{
			return false;
		}
		double dspTime = AudioSettings.dspTime + (double)timeUntilEnd + (double)_GetRandomLoopSequenceDelay(audioItem);
		AudioItem sndItem = audioItem;
		SwitchAudioSources();
		_audioController.PlayAudioItem(sndItem, _volumeFromScriptCall, Vector3.zero, null, 0f, 0f, playWithoutAudioObject: false, this, dspTime);
		_loopSequenceCount++;
		if (audioItem.Loop == AudioItem.LoopMode.PlaySequenceAndLoopLast || audioItem.Loop == AudioItem.LoopMode.IntroLoopOutroSequence)
		{
			if (audioItem.Loop == AudioItem.LoopMode.IntroLoopOutroSequence)
			{
				if (!_finishSequence && num <= _loopSequenceCount + 2)
				{
					primaryAudioSource.loop = true;
				}
			}
			else if (num <= _loopSequenceCount + 1)
			{
				primaryAudioSource.loop = true;
			}
		}
		return true;
	}

	private void _UpdateFadeVolume()
	{
		bool finishedFadeOut;
		float num = _EqualizePowerForCrossfading(_primaryFader.Get(out finishedFadeOut));
		if (finishedFadeOut)
		{
			if (_stopRequested)
			{
				_Stop();
				return;
			}
			if (!_IsAudioLoopSequenceMode())
			{
				if (_shouldStopIfPrimaryFadedOut)
				{
					_Stop();
				}
				return;
			}
		}
		if (num != _volumeFromPrimaryFade)
		{
			_volumeFromPrimaryFade = num;
			_ApplyVolumePrimary();
		}
		if (_audioSource2 != null)
		{
			float num2 = _EqualizePowerForCrossfading(_secondaryFader.Get(out finishedFadeOut));
			if (finishedFadeOut)
			{
				_audioSource2.Stop();
			}
			else if (num2 != _volumeFromSecondaryFade)
			{
				_volumeFromSecondaryFade = num2;
				_ApplyVolumeSecondary();
			}
		}
	}

	private float _EqualizePowerForCrossfading(float v)
	{
		if (!_audioController.EqualPowerCrossfade)
		{
			return v;
		}
		return InverseTransformVolume(Mathf.Sin(v * (float)Math.PI * 0.5f));
	}

	private void OnApplicationPause(bool b)
	{
		SetApplicationPaused(b);
	}

	private void SetApplicationPaused(bool isPaused)
	{
		_applicationPaused = isPaused;
	}

	public void DestroyAudioObject()
	{
		if (IsPlaying())
		{
			_Stop();
		}
		ObjectPoolController.Destroy(base.gameObject);
		_IsInactive = true;
	}

	public static float TransformVolume(float volume)
	{
		return Mathf.Pow(volume, 1.6f);
	}

	public static float InverseTransformVolume(float volume)
	{
		return Mathf.Pow(volume, 0.625f);
	}

	public static float TransformPitch(float pitchSemiTones)
	{
		return Mathf.Pow(2f, pitchSemiTones / 12f);
	}

	public static float InverseTransformPitch(float pitch)
	{
		return Mathf.Log(pitch) / Mathf.Log(2f) * 12f;
	}

	internal void _ApplyVolumeBoth()
	{
		float num = volumeTotalWithoutFade;
		float num2 = TransformVolume(num * _volumeFromPrimaryFade);
		primaryAudioSource.volume = num2;
		if ((bool)secondaryAudioSource)
		{
			num2 = TransformVolume(num * _volumeFromSecondaryFade);
			secondaryAudioSource.volume = num2;
		}
	}

	internal void _ApplyVolumePrimary()
	{
		float num = TransformVolume(volumeTotalWithoutFade * _volumeFromPrimaryFade);
		primaryAudioSource.volume = num;
	}

	internal void _ApplyVolumeSecondary()
	{
		if ((bool)secondaryAudioSource)
		{
			float num = TransformVolume(volumeTotalWithoutFade * _volumeFromSecondaryFade);
			secondaryAudioSource.volume = num;
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		AudioItem audioItem = this.audioItem;
		if (audioItem != null && audioItem.overrideAudioSourceSettings)
		{
			_RestoreAudioSourceSettings();
		}
		_SetReferences0();
	}

	private void _RestoreAudioSourceSettings()
	{
		primaryAudioSource.minDistance = _audioSource_MinDistance_Saved;
		primaryAudioSource.maxDistance = _audioSource_MaxDistance_Saved;
		if (secondaryAudioSource != null)
		{
			secondaryAudioSource.minDistance = _audioSource_MinDistance_Saved;
			secondaryAudioSource.maxDistance = _audioSource_MaxDistance_Saved;
		}
	}

	public bool DoesBelongToCategory(string categoryName)
	{
		for (AudioCategory parentCategory = category; parentCategory != null; parentCategory = parentCategory.parentCategory)
		{
			if (parentCategory.Name == categoryName)
			{
				return true;
			}
		}
		return false;
	}

	private float _GetRandomLoopSequenceDelay(AudioItem audioItem)
	{
		float num = 0f - audioItem.loopSequenceOverlap;
		if (audioItem.loopSequenceRandomDelay > 0f)
		{
			num += UnityEngine.Random.Range(0f, audioItem.loopSequenceRandomDelay);
		}
		return num;
	}
}
