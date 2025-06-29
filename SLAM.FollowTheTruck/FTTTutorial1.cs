using System.Collections;
using SLAM.Engine;
using SLAM.Slinq;
using SLAM.ToolTips;
using UnityEngine;

namespace SLAM.FollowTheTruck;

public class FTTTutorial1 : TutorialView
{
	[SerializeField]
	private ToolTip keyboardTooltip;

	[SerializeField]
	private ToolTip dodgeObstacleTooltip;

	[SerializeField]
	private ToolTip featherTooltip;

	[SerializeField]
	private Transform avatarObject;

	protected override void Start()
	{
		base.Start();
		StartCoroutine(step1());
	}

	private IEnumerator step1()
	{
		keyboardTooltip.Show(avatarObject);
		GameObject obj = null;
		yield return CoroutineUtils.WaitForGameEvent(delegate(FTTCargoThrownEvent c)
		{
			obj = c.Object;
		});
		keyboardTooltip.Hide();
		dodgeObstacleTooltip.Show(obj.transform);
		while (!avatarPassedObject(obj.transform))
		{
			yield return null;
		}
		dodgeObstacleTooltip.Hide();
		FTTPickup closestFeather = (from c in Object.FindObjectsOfType<FTTPickup>()
			orderby Vector3.Distance(c.transform.position, avatarObject.transform.position)
			select c).First();
		featherTooltip.Show(closestFeather.transform);
		while (closestFeather != null && !avatarPassedObject(closestFeather.transform))
		{
			yield return null;
		}
		featherTooltip.Hide();
	}

	private bool avatarPassedObject(Transform obj)
	{
		return Vector3.Dot(avatarObject.transform.forward.normalized, (obj.transform.position - avatarObject.transform.position).normalized) < 0f;
	}
}
