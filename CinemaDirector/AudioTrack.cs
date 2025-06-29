namespace CinemaDirector;

[TimelineTrack("Audio Track", TimelineTrackGenre.GlobalTrack, new CutsceneItemGenre[] { CutsceneItemGenre.AudioClipItem })]
public class AudioTrack : TimelineTrack
{
	public CinemaAudio[] AudioClips => GetComponentsInChildren<CinemaAudio>();

	public override void SetTime(float time)
	{
		TimelineItem[] timelineItems = GetTimelineItems();
		foreach (TimelineItem timelineItem in timelineItems)
		{
			CinemaAudio cinemaAudio = timelineItem as CinemaAudio;
			if (cinemaAudio != null)
			{
				float time2 = time - cinemaAudio.Firetime;
				cinemaAudio.SetTime(time2);
			}
		}
	}

	public override void Pause()
	{
		TimelineItem[] timelineItems = GetTimelineItems();
		foreach (TimelineItem timelineItem in timelineItems)
		{
			CinemaAudio cinemaAudio = timelineItem as CinemaAudio;
			if (cinemaAudio != null)
			{
				cinemaAudio.Pause();
			}
		}
	}

	public override void UpdateTrack(float time, float deltaTime)
	{
		float num = elapsedTime;
		elapsedTime = time;
		TimelineItem[] timelineItems = GetTimelineItems();
		foreach (TimelineItem timelineItem in timelineItems)
		{
			CinemaAudio cinemaAudio = timelineItem as CinemaAudio;
			if (cinemaAudio != null)
			{
				if ((num < cinemaAudio.Firetime || num <= 0f) && elapsedTime >= cinemaAudio.Firetime)
				{
					cinemaAudio.Trigger();
				}
				if (elapsedTime > cinemaAudio.Firetime && elapsedTime <= cinemaAudio.Firetime + cinemaAudio.Duration)
				{
					float time2 = time - cinemaAudio.Firetime;
					cinemaAudio.UpdateTime(time2, deltaTime);
				}
				if (num <= cinemaAudio.Firetime + cinemaAudio.Duration && elapsedTime > cinemaAudio.Firetime + cinemaAudio.Duration)
				{
					cinemaAudio.End();
				}
			}
		}
	}

	public override void Resume()
	{
		TimelineItem[] timelineItems = GetTimelineItems();
		foreach (TimelineItem timelineItem in timelineItems)
		{
			CinemaAudio cinemaAudio = timelineItem as CinemaAudio;
			if (cinemaAudio != null && base.Cutscene.RunningTime > cinemaAudio.Firetime && base.Cutscene.RunningTime < cinemaAudio.Firetime + cinemaAudio.Duration)
			{
				cinemaAudio.Resume();
			}
		}
	}

	public override void Stop()
	{
		elapsedTime = 0f;
		TimelineItem[] timelineItems = GetTimelineItems();
		foreach (TimelineItem timelineItem in timelineItems)
		{
			CinemaAudio cinemaAudio = timelineItem as CinemaAudio;
			if (cinemaAudio != null)
			{
				cinemaAudio.Stop();
			}
		}
	}
}
