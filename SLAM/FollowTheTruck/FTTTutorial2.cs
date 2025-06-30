using System.Collections;
using SLAM.Engine;
using SLAM.Slinq;
using SLAM.ToolTips;
using UnityEngine;

namespace SLAM.FollowTheTruck;

public class FTTTutorial2 : TutorialView
{
	[SerializeField]
	private ToolTip moveTooltip;

	[SerializeField]
	private Transform avatarObject;

	protected override void Start()
	{
		base.Start();
		StartCoroutine(step1());
	}

	private IEnumerator step1()
	{
		FTTBreakablePier closestBreakable = (from c in Object.FindObjectsOfType<FTTBreakablePier>()
			orderby Vector3.Distance(c.transform.position, avatarObject.transform.position)
			select c).First();
		while (Vector3.Distance(closestBreakable.transform.position, avatarObject.transform.position) > 15f)
		{
			yield return null;
		}
		moveTooltip.Show(closestBreakable.transform);
		while (!avatarPassedObject(closestBreakable.transform))
		{
			yield return null;
		}
		moveTooltip.Hide();
	}

	private bool avatarPassedObject(Transform obj)
	{
		return Vector3.Dot(avatarObject.transform.forward.normalized, (obj.transform.position - avatarObject.transform.position).normalized) < 0f;
	}
}
