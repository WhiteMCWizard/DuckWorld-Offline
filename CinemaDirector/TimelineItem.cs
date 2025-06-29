using UnityEngine;

namespace CinemaDirector;

[ExecuteInEditMode]
public abstract class TimelineItem : MonoBehaviour
{
	[SerializeField]
	protected float firetime;

	public float Firetime
	{
		get
		{
			return firetime;
		}
		set
		{
			firetime = value;
			if (firetime < 0f)
			{
				firetime = 0f;
			}
		}
	}

	public Cutscene Cutscene => (!(TimelineTrack == null)) ? TimelineTrack.Cutscene : null;

	public TimelineTrack TimelineTrack
	{
		get
		{
			TimelineTrack timelineTrack = null;
			if (base.transform.parent != null)
			{
				timelineTrack = base.transform.parent.GetComponentInParent<TimelineTrack>();
				if (timelineTrack == null)
				{
					Debug.LogError("No TimelineTrack found on parent!", this);
				}
			}
			else
			{
				Debug.LogError("Timeline Item has no parent!", this);
			}
			return timelineTrack;
		}
	}

	public virtual void Initialize()
	{
	}

	public virtual void Stop()
	{
	}

	public virtual void SetDefaults()
	{
	}

	public virtual void SetDefaults(Object PairedItem)
	{
	}
}
