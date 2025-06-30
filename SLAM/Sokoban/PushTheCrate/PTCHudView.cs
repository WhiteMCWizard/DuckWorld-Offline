using UnityEngine;

namespace SLAM.Sokoban.PushTheCrate;

public class PTCHudView : SKBHudView
{
	[SerializeField]
	private UISlider sldrTime;

	protected override void Update()
	{
		base.Update();
		sldrTime.value = Controller<PushTheCrateGameController>().ElapsedTime / Controller<PushTheCrateGameController>().LevelDuration;
	}
}
