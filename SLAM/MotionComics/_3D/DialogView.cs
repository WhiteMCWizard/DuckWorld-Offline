using System;
using System.Collections.Generic;
using CinemaDirector;
using SLAM.Engine;
using SLAM.Shared;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.MotionComics._3D;

public class DialogView : BalloonView
{
	[SerializeField]
	private UILabel speechLabel;

	[SerializeField]
	private UITypewriter speechTypeEffect;

	private Dictionary<TimelineTrack, List<GameObject>> speechBalloons = new Dictionary<TimelineTrack, List<GameObject>>();

	public void SetInfo(string name, string text, NGUIText.Alignment alignment, bool append)
	{
		speechLabel.alignment = alignment;
		if (append)
		{
			speechTypeEffect.SetText(speechLabel.text + Environment.NewLine + name, text);
		}
		else
		{
			speechTypeEffect.SetText(name, text);
		}
	}

	public SpeechBalloon CreateBalloonOnTrack(TimelineTrack track, BalloonType type)
	{
		SpeechBalloon speechBalloon = CreateBalloon(type);
		if (speechBalloons.ContainsKey(track))
		{
			speechBalloons[track].Add(speechBalloon.gameObject);
		}
		else
		{
			speechBalloons.Add(track, new List<GameObject> { speechBalloon.gameObject });
		}
		return speechBalloon;
	}

	public SpeechBalloon GetLastBalloonOnTrack(TimelineTrack track)
	{
		if (speechBalloons.ContainsKey(track))
		{
			if (speechBalloons[track].Count > 0)
			{
				return speechBalloons[track].Last().GetComponent<SpeechBalloon>();
			}
			Debug.LogError("Hey Buddy, cannot append since the given track does not have speech balloons.", track);
		}
		else
		{
			Debug.LogError("Hey Buddy, cannot append since the given track does not exist.", track);
		}
		return CreateBalloonOnTrack(track, BalloonType.TailLeftBottomPointingLeft);
	}

	public void DestroyBalloonsOnTrack(TimelineTrack track)
	{
		if (!speechBalloons.ContainsKey(track))
		{
			return;
		}
		foreach (GameObject item in speechBalloons[track])
		{
			UnityEngine.Object.Destroy(item);
		}
		speechBalloons[track] = new List<GameObject>();
	}
}
