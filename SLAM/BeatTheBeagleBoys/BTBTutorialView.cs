using System.Collections;
using System.Collections.Generic;
using SLAM.Engine;
using SLAM.Slinq;
using SLAM.ToolTips;
using UnityEngine;

namespace SLAM.BeatTheBeagleBoys;

public class BTBTutorialView : TutorialView
{
	[SerializeField]
	private ToolTip clickHereToolTip;

	private int hintLimit = 5;

	private int hintCount;

	private List<BTBMonitor> monitorQue = new List<BTBMonitor>();

	private void OnEnable()
	{
		GameEvents.Subscribe<BTBGameController.MonitorAlarmedEvent>(onMonitorAlarmed);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<BTBGameController.MonitorAlarmedEvent>(onMonitorAlarmed);
	}

	protected override void Update()
	{
		base.Update();
		if (monitorQue.Count > 0 && !clickHereToolTip.IsVisible)
		{
			StartCoroutine(step1(monitorQue.First()));
		}
	}

	private IEnumerator step1(BTBMonitor monitor)
	{
		monitorQue.Remove(monitor);
		clickHereToolTip.Show(monitor.transform);
		while (monitor.IsAlarmed && monitor.Area.HasThief && monitor.Area.CurrentThief.IsRunningToCage && monitor.Area.Cage.IsHacked)
		{
			yield return null;
		}
		clickHereToolTip.Hide();
	}

	private void onMonitorAlarmed(BTBGameController.MonitorAlarmedEvent evt)
	{
		if (++hintCount <= hintLimit)
		{
			monitorQue.Add(evt.monitor);
		}
	}
}
