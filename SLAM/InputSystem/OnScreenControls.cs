using System.Collections;
using SLAM.Engine;
using UnityEngine;

namespace SLAM.InputSystem;

public class OnScreenControls : MonoBehaviour
{
	private IEnumerator Start()
	{
		if (!(SLAMInput.Provider is MobileInputProvider))
		{
			Object.Destroy(base.gameObject);
			yield break;
		}
		GameController controller = Object.FindObjectOfType<GameController>();
		FiniteStateMachine stateMachine = controller.GetComponent<FiniteStateMachine>();
		UIPanel panel = GetComponentInChildren<UIPanel>();
		panel.alpha = 0f;
		while (stateMachine.CurrentState.Name == "Loading" || stateMachine.CurrentState.Name == "Ready to begin")
		{
			yield return null;
		}
		Stopwatch sw = new Stopwatch(0.2f);
		while (!sw.Expired)
		{
			yield return null;
			panel.alpha = sw.Progress;
		}
		while (stateMachine.CurrentState.Name != "Finished")
		{
			yield return null;
		}
		sw = new Stopwatch(0.2f);
		while (!sw.Expired)
		{
			yield return null;
			panel.alpha = 1f - sw.Progress;
		}
	}
}
