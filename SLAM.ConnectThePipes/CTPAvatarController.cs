using UnityEngine;

namespace SLAM.ConnectThePipes;

public class CTPAvatarController : MonoBehaviour
{
	[SerializeField]
	private CTPInputManager inputManager;

	private Animator animator;

	private void Start()
	{
		animator = GetComponentInChildren<Animator>();
	}

	private void OnEnable()
	{
		GameEvents.Subscribe<ConnectThePipesGame.LevelCompletedEvent>(onLevelCompleted);
		GameEvents.Subscribe<ConnectThePipesGame.WaterFlowStopped>(onWaterFlowStopped);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<ConnectThePipesGame.LevelCompletedEvent>(onLevelCompleted);
		GameEvents.Unsubscribe<ConnectThePipesGame.WaterFlowStopped>(onWaterFlowStopped);
	}

	private void onLevelCompleted(ConnectThePipesGame.LevelCompletedEvent evt)
	{
		animator.SetTrigger("success");
	}

	private void onWaterFlowStopped(ConnectThePipesGame.WaterFlowStopped evt)
	{
		animator.SetTrigger("failure");
	}

	public void WarpTo(CTPBeginPipe pipe)
	{
		base.transform.position = pipe.AvatarWalkToObject.position;
	}

	public void WalkTo(CTPBeginPipe pipe)
	{
		animator.SetTrigger("walking");
		base.transform.position = pipe.AvatarWalkToObject.position;
	}

	public void StartWaterFlow()
	{
		animator.SetTrigger("open");
	}

	private void AvatarReachedHydrant()
	{
	}
}
