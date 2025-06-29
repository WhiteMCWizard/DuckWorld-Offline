using SLAM.Engine;
using SLAM.Slinq;
using SLAM.ToolTips;
using UnityEngine;

namespace SLAM.ConnectThePipes;

public class CTPTutorialView01 : TutorialView
{
	[SerializeField]
	private ToolTip pipeLineToolTip;

	[SerializeField]
	private ToolTip pipeToolTip;

	[SerializeField]
	private ToolTip hydrantLineToolTip;

	[SerializeField]
	private ToolTip hydrantToolTip;

	[SerializeField]
	private PipesToTurn[] pipesToTurn;

	private CTPBeginPipe beginPipe;

	private void OnEnable()
	{
		GameEvents.Subscribe<ConnectThePipesGame.PipeClickedEvent>(onPipeClicked);
		GameEvents.Subscribe<ConnectThePipesGame.WaterFlowStarted>(onWaterFlows);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<ConnectThePipesGame.PipeClickedEvent>(onPipeClicked);
		GameEvents.Unsubscribe<ConnectThePipesGame.WaterFlowStarted>(onWaterFlows);
	}

	protected override void Start()
	{
		base.Start();
		beginPipe = Controller<ConnectThePipesGame>().CurrentSettings.LevelRoot.GetComponentInChildren<CTPBeginPipe>();
		pipeLineToolTip.Show(pipesToTurn.First().pipe.transform);
		pipeToolTip.Show(pipeLineToolTip.GO.transform);
	}

	private void onPipeClicked(ConnectThePipesGame.PipeClickedEvent evt)
	{
		PipesToTurn pipesToTurn = this.pipesToTurn.FirstOrDefault((PipesToTurn p) => p.pipe == evt.pipe);
		if (pipesToTurn != null)
		{
			pipesToTurn.Turns++;
		}
		PipesToTurn pipesToTurn2 = this.pipesToTurn.FirstOrDefault((PipesToTurn p) => !p.TurnedCorrectly);
		if (pipesToTurn2 != null)
		{
			hideAllToolTips();
			pipeLineToolTip.Show(pipesToTurn2.pipe.transform);
			pipeToolTip.Show(pipeLineToolTip.GO.transform);
		}
		else
		{
			hideAllToolTips();
			hydrantLineToolTip.Show(beginPipe.transform);
			hydrantToolTip.Show(hydrantLineToolTip.GO.transform);
		}
	}

	private void onWaterFlows(ConnectThePipesGame.WaterFlowStarted evt)
	{
		hideAllToolTips();
	}

	private void hideAllToolTips()
	{
		hydrantLineToolTip.Hide();
		pipeLineToolTip.Hide();
		pipeToolTip.Hide();
		hydrantToolTip.Hide();
	}
}
