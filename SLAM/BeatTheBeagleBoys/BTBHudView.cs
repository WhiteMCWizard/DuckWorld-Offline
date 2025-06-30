using SLAM.Engine;
using UnityEngine;

namespace SLAM.BeatTheBeagleBoys;

public class BTBHudView : HUDView
{
	[SerializeField]
	private UILabel lblTimer;

	private Alarm gameTimer;

	public void SetInfo(Alarm gameTimer)
	{
		this.gameTimer = gameTimer;
	}

	protected override void Update()
	{
		if (gameTimer != null)
		{
			lblTimer.text = StringFormatter.GetFormattedTime(gameTimer.TimerDuration - gameTimer.CurrentTime);
		}
	}
}
