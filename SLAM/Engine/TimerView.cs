using System;
using System.Collections.Generic;
using UnityEngine;

namespace SLAM.Engine;

public class TimerView : View
{
	private enum TimeFormat
	{
		Seconds,
		MinutesSeconds,
		MinutesSecondsMilliseconds
	}

	[Serializable]
	public class UrgencyLevel
	{
		public UITweener[] Animations;

		[Tooltip("When the RemainingTime is below this we start blinking")]
		public int TimeThreshold;

		public AudioClip clip;
	}

	[SerializeField]
	private UrgencyLevel[] urgencyLevels;

	[SerializeField]
	private UILabel timerLabel;

	[SerializeField]
	private TimeFormat format = TimeFormat.MinutesSeconds;

	private Alarm timer;

	private int currentUrgencyLevelIndex;

	private float previousTimeleft;

	private bool previousTimerStatus;

	private List<AudioObject> soundList = new List<AudioObject>();

	private UrgencyLevel CurrentUrgencyLevel => urgencyLevels[Mathf.Clamp(currentUrgencyLevelIndex, 0, urgencyLevels.Length - 1)];

	public bool TimerIsActive => Time.deltaTime > 0f;

	public override void Close(Callback callback, bool immediately)
	{
		toggleUrgencySounds(play: false);
		base.Close(callback, immediately);
	}

	protected override void Update()
	{
		base.Update();
		if (timer != null)
		{
			if (TimerIsActive != previousTimerStatus)
			{
				toggleUrgencySounds(TimerIsActive);
			}
			updateText();
			if (previousTimeleft > (float)CurrentUrgencyLevel.TimeThreshold && timer.TimeLeft < (float)CurrentUrgencyLevel.TimeThreshold)
			{
				showUrgencyAnimations();
				currentUrgencyLevelIndex++;
			}
			previousTimeleft = timer.TimeLeft;
			previousTimerStatus = TimerIsActive;
		}
	}

	public void SetTimer(Alarm timer)
	{
		this.timer = timer;
	}

	private void updateText()
	{
		float timeLeft = timer.TimeLeft;
		string text = string.Empty;
		switch (format)
		{
		case TimeFormat.Seconds:
			text = ((int)timeLeft).ToString();
			break;
		case TimeFormat.MinutesSeconds:
			text = StringFormatter.GetFormattedTime(timeLeft);
			break;
		case TimeFormat.MinutesSecondsMilliseconds:
			text = StringFormatter.GetFormattedTime(timeLeft, miliseconds: true);
			break;
		}
		timerLabel.text = text;
	}

	private void showUrgencyAnimations()
	{
		for (int i = 0; i < urgencyLevels.Length; i++)
		{
			for (int j = 0; j < urgencyLevels[i].Animations.Length; j++)
			{
				urgencyLevels[i].Animations[j].enabled = false;
			}
		}
		for (int k = 0; k < CurrentUrgencyLevel.Animations.Length; k++)
		{
			CurrentUrgencyLevel.Animations[k].PlayForward();
		}
		if (CurrentUrgencyLevel.clip != null)
		{
			soundList.Add(AudioController.Play(CurrentUrgencyLevel.clip.name));
		}
	}

	private void toggleUrgencySounds(bool play)
	{
		foreach (AudioObject sound in soundList)
		{
			if (play)
			{
				sound.Play(0f);
			}
			else
			{
				sound.Pause();
			}
		}
	}
}
