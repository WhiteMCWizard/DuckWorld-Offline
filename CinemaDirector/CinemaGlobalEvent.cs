namespace CinemaDirector;

public abstract class CinemaGlobalEvent : TimelineItem
{
	public abstract void Trigger();

	public virtual void Reverse()
	{
	}
}
