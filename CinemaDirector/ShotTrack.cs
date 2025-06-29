namespace CinemaDirector;

[TimelineTrack("Shot Track", TimelineTrackGenre.GlobalTrack, new CutsceneItemGenre[] { CutsceneItemGenre.CameraShot })]
public class ShotTrack : TimelineTrack
{
	public event ShotEndsEventHandler ShotEnds;

	public event ShotBeginsEventHandler ShotBegins;

	public override void Initialize()
	{
		elapsedTime = 0f;
		CinemaShot cinemaShot = null;
		TimelineItem[] timelineItems = GetTimelineItems();
		for (int i = 0; i < timelineItems.Length; i++)
		{
			CinemaShot cinemaShot2 = (CinemaShot)timelineItems[i];
			cinemaShot2.Initialize();
		}
		TimelineItem[] timelineItems2 = GetTimelineItems();
		for (int j = 0; j < timelineItems2.Length; j++)
		{
			CinemaShot cinemaShot3 = (CinemaShot)timelineItems2[j];
			if (cinemaShot3.Firetime == 0f)
			{
				cinemaShot = cinemaShot3;
			}
			else
			{
				cinemaShot3.End();
			}
		}
		if (cinemaShot != null)
		{
			cinemaShot.Trigger();
			if (this.ShotBegins != null)
			{
				this.ShotBegins(this, new ShotEventArgs(cinemaShot));
			}
		}
	}

	public override void UpdateTrack(float time, float deltaTime)
	{
		float num = elapsedTime;
		elapsedTime = time;
		TimelineItem[] timelineItems = GetTimelineItems();
		for (int i = 0; i < timelineItems.Length; i++)
		{
			CinemaShot cinemaShot = (CinemaShot)timelineItems[i];
			float num2 = cinemaShot.CutTime + cinemaShot.Duration;
			if (num <= cinemaShot.CutTime && elapsedTime >= cinemaShot.CutTime && elapsedTime < num2)
			{
				cinemaShot.Trigger();
				if (this.ShotBegins != null)
				{
					this.ShotBegins(this, new ShotEventArgs(cinemaShot));
				}
			}
			else if (num >= num2 && elapsedTime < num2 && elapsedTime >= cinemaShot.CutTime)
			{
				cinemaShot.Trigger();
				if (this.ShotBegins != null)
				{
					this.ShotBegins(this, new ShotEventArgs(cinemaShot));
				}
			}
			else if (num >= cinemaShot.CutTime && num < num2 && elapsedTime >= num2)
			{
				cinemaShot.End();
				if (this.ShotEnds != null)
				{
					this.ShotEnds(this, new ShotEventArgs(cinemaShot));
				}
			}
			else if (num > cinemaShot.CutTime && num < num2 && elapsedTime < cinemaShot.CutTime)
			{
				cinemaShot.End();
				if (this.ShotEnds != null)
				{
					this.ShotEnds(this, new ShotEventArgs(cinemaShot));
				}
			}
		}
	}

	public override void SetTime(float time)
	{
		CinemaShot cinemaShot = null;
		CinemaShot cinemaShot2 = null;
		TimelineItem[] timelineItems = GetTimelineItems();
		for (int i = 0; i < timelineItems.Length; i++)
		{
			CinemaShot cinemaShot3 = (CinemaShot)timelineItems[i];
			float num = cinemaShot3.CutTime + cinemaShot3.Duration;
			if (elapsedTime >= cinemaShot3.CutTime && elapsedTime < num)
			{
				cinemaShot = cinemaShot3;
			}
			if (time >= cinemaShot3.CutTime && time < num)
			{
				cinemaShot2 = cinemaShot3;
			}
		}
		if (cinemaShot2 != cinemaShot)
		{
			if (cinemaShot != null)
			{
				cinemaShot.End();
				if (this.ShotEnds != null)
				{
					this.ShotEnds(this, new ShotEventArgs(cinemaShot));
				}
			}
			if (cinemaShot2 != null)
			{
				cinemaShot2.Trigger();
				if (this.ShotBegins != null)
				{
					this.ShotBegins(this, new ShotEventArgs(cinemaShot2));
				}
			}
		}
		elapsedTime = time;
	}
}
