using System;
using System.Collections;
using System.Collections.Generic;
using CinemaDirector.Helpers;
using UnityEngine;

namespace CinemaDirector;

[Serializable]
[ExecuteInEditMode]
public class Cutscene : MonoBehaviour, IOptimizable
{
	public enum CutsceneState
	{
		Inactive,
		Playing,
		PreviewPlaying,
		Scrubbing,
		Paused
	}

	[SerializeField]
	private float duration = 30f;

	[SerializeField]
	private float playbackSpeed = 1f;

	[SerializeField]
	private bool isSkippable = true;

	[SerializeField]
	private bool isLooping;

	[SerializeField]
	private bool canOptimize = true;

	[NonSerialized]
	private float runningTime;

	[NonSerialized]
	private CutsceneState state;

	private float previousTime;

	private bool hasBeenOptimized;

	private bool hasBeenInitialized;

	private TrackGroup[] trackGroupCache;

	private List<RevertInfo> revertCache = new List<RevertInfo>();

	public float Duration
	{
		get
		{
			return duration;
		}
		set
		{
			duration = value;
			if (duration <= 0f)
			{
				duration = 0.1f;
			}
		}
	}

	public CutsceneState State => state;

	public float RunningTime
	{
		get
		{
			return runningTime;
		}
		set
		{
			runningTime = Mathf.Clamp(value, 0f, duration);
		}
	}

	public TrackGroup[] TrackGroups => GetComponentsInChildren<TrackGroup>();

	public DirectorGroup DirectorGroup => GetComponentInChildren<DirectorGroup>();

	public bool CanOptimize
	{
		get
		{
			return canOptimize;
		}
		set
		{
			canOptimize = value;
		}
	}

	public bool IsSkippable
	{
		get
		{
			return isSkippable;
		}
		set
		{
			isSkippable = value;
		}
	}

	public bool IsLooping
	{
		get
		{
			return isLooping;
		}
		set
		{
			isLooping = value;
		}
	}

	public event CutsceneHandler CutsceneFinished;

	public event CutsceneHandler CutscenePaused;

	public void Optimize()
	{
		if (canOptimize)
		{
			trackGroupCache = GetTrackGroups();
			hasBeenOptimized = true;
		}
		TrackGroup[] trackGroups = GetTrackGroups();
		foreach (TrackGroup trackGroup in trackGroups)
		{
			trackGroup.Optimize();
		}
	}

	public void Play()
	{
		if (state == CutsceneState.Inactive)
		{
			state = CutsceneState.Playing;
			if (!hasBeenOptimized)
			{
				Optimize();
			}
			if (!hasBeenInitialized)
			{
				initialize();
			}
			StartCoroutine("updateCoroutine");
		}
		else if (state == CutsceneState.Paused)
		{
			state = CutsceneState.Playing;
			StartCoroutine("updateCoroutine");
		}
	}

	public void Pause()
	{
		if (state == CutsceneState.Playing)
		{
			StopCoroutine("updateCoroutine");
		}
		if (state == CutsceneState.PreviewPlaying || state == CutsceneState.Playing || state == CutsceneState.Scrubbing)
		{
			TrackGroup[] trackGroups = GetTrackGroups();
			foreach (TrackGroup trackGroup in trackGroups)
			{
				trackGroup.Pause();
			}
		}
		state = CutsceneState.Paused;
		if (this.CutscenePaused != null)
		{
			this.CutscenePaused(this, new CutsceneEventArgs());
		}
	}

	public void Skip()
	{
		if (isSkippable)
		{
			SetRunningTime(Duration);
			state = CutsceneState.Inactive;
			Stop();
		}
	}

	public void Stop()
	{
		RunningTime = 0f;
		TrackGroup[] trackGroups = GetTrackGroups();
		foreach (TrackGroup trackGroup in trackGroups)
		{
			trackGroup.Stop();
		}
		revert();
		if (state == CutsceneState.Playing)
		{
			StopCoroutine("updateCoroutine");
			if (state == CutsceneState.Playing && isLooping)
			{
				state = CutsceneState.Inactive;
				Play();
			}
			else
			{
				state = CutsceneState.Inactive;
			}
		}
		else
		{
			state = CutsceneState.Inactive;
		}
		if (state == CutsceneState.Inactive && this.CutsceneFinished != null)
		{
			this.CutsceneFinished(this, new CutsceneEventArgs());
		}
	}

	public void UpdateCutscene(float deltaTime)
	{
		RunningTime += deltaTime * playbackSpeed;
		TrackGroup[] trackGroups = GetTrackGroups();
		foreach (TrackGroup trackGroup in trackGroups)
		{
			trackGroup.UpdateTrackGroup(RunningTime, deltaTime * playbackSpeed);
		}
		if (state != CutsceneState.Scrubbing && (runningTime >= duration || runningTime < 0f))
		{
			Stop();
		}
	}

	public void PreviewPlay()
	{
		if (state == CutsceneState.Inactive)
		{
			EnterPreviewMode();
		}
		else if (state == CutsceneState.Paused)
		{
			resume();
		}
		if (Application.isPlaying)
		{
			state = CutsceneState.Playing;
		}
		else
		{
			state = CutsceneState.PreviewPlaying;
		}
	}

	public void ScrubToTime(float newTime)
	{
		float num = Mathf.Clamp(newTime, 0f, Duration) - RunningTime;
		state = CutsceneState.Scrubbing;
		if (num != 0f)
		{
			if (num > 1f / 30f)
			{
				float num2 = RunningTime;
				{
					foreach (float milestone in getMilestones(RunningTime + num))
					{
						float num3 = milestone;
						float deltaTime = num3 - num2;
						UpdateCutscene(deltaTime);
						num2 = num3;
					}
					return;
				}
			}
			UpdateCutscene(num);
		}
		else
		{
			Pause();
		}
	}

	public void SetRunningTime(float time)
	{
		foreach (float milestone in getMilestones(time))
		{
			float num = milestone;
			TrackGroup[] trackGroups = TrackGroups;
			foreach (TrackGroup trackGroup in trackGroups)
			{
				trackGroup.SetRunningTime(num);
			}
		}
		RunningTime = time;
	}

	public void EnterPreviewMode()
	{
		if (state == CutsceneState.Inactive)
		{
			initialize();
			bake();
			SetRunningTime(RunningTime);
			state = CutsceneState.Paused;
		}
	}

	public void ExitPreviewMode()
	{
		Stop();
	}

	protected void OnDestroy()
	{
		if (!Application.isPlaying)
		{
			Stop();
		}
	}

	public void Refresh()
	{
		if (state != CutsceneState.Inactive)
		{
			float num = runningTime;
			Stop();
			EnterPreviewMode();
			SetRunningTime(num);
		}
	}

	private void bake()
	{
		if (!Application.isEditor)
		{
			return;
		}
		TrackGroup[] trackGroups = TrackGroups;
		foreach (TrackGroup trackGroup in trackGroups)
		{
			if (trackGroup is IBakeable)
			{
				(trackGroup as IBakeable).Bake();
			}
		}
	}

	public TrackGroup[] GetTrackGroups()
	{
		if (hasBeenOptimized)
		{
			return trackGroupCache;
		}
		return TrackGroups;
	}

	private void initialize()
	{
		saveRevertData();
		TrackGroup[] trackGroups = TrackGroups;
		foreach (TrackGroup trackGroup in trackGroups)
		{
			trackGroup.Initialize();
		}
	}

	private void saveRevertData()
	{
		revertCache.Clear();
		MonoBehaviour[] componentsInChildren = GetComponentsInChildren<MonoBehaviour>();
		foreach (MonoBehaviour monoBehaviour in componentsInChildren)
		{
			if (monoBehaviour is IRevertable revertable)
			{
				RevertInfo[] array = revertable.CacheState();
				if (array == null)
				{
					Debug.Log($"Cinema Director tried to cache the state of {monoBehaviour.name}, but failed.");
				}
				else
				{
					revertCache.AddRange(array);
				}
			}
		}
	}

	private void revert()
	{
		foreach (RevertInfo item in revertCache)
		{
			if (item != null && ((item.EditorRevert == RevertMode.Revert && !Application.isPlaying) || (item.RuntimeRevert == RevertMode.Revert && Application.isPlaying)))
			{
				item.Revert();
			}
		}
	}

	private List<float> getMilestones(float time)
	{
		List<float> list = new List<float>();
		list.Add(time);
		TrackGroup[] trackGroups = TrackGroups;
		foreach (TrackGroup trackGroup in trackGroups)
		{
			List<float> milestones = trackGroup.GetMilestones(RunningTime, time);
			foreach (float item2 in milestones)
			{
				float item = item2;
				if (!list.Contains(item))
				{
					list.Add(item);
				}
			}
		}
		list.Sort();
		if (time < RunningTime)
		{
			list.Reverse();
		}
		return list;
	}

	private IEnumerator updateCoroutine()
	{
		while (state == CutsceneState.Playing)
		{
			UpdateCutscene(Time.deltaTime);
			yield return null;
		}
	}

	private void resume()
	{
		TrackGroup[] trackGroups = TrackGroups;
		foreach (TrackGroup trackGroup in trackGroups)
		{
			trackGroup.Resume();
		}
	}
}
