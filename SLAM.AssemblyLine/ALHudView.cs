using System.Collections;
using SLAM.Engine;
using UnityEngine;

namespace SLAM.AssemblyLine;

public class ALHudView : HUDView
{
	[SerializeField]
	private UILabel lblCompletedBubble;

	[SerializeField]
	private float bubbleSpeed;

	[SerializeField]
	private AnimationCurve bubbleCurveX;

	[SerializeField]
	private AnimationCurve bubbleCurveY;

	[SerializeField]
	private UILabel lblRobotsCompleted;

	[SerializeField]
	private float robotCompletedBubbleDelay = 0.5f;

	private void OnEnable()
	{
		GameEvents.Subscribe<AssemblyLineGame.RobotCompletedEvent>(onRobotCompleted);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<AssemblyLineGame.RobotCompletedEvent>(onRobotCompleted);
	}

	public void InitHUD(int requiredRobots)
	{
		lblRobotsCompleted.text = $"{0}/{requiredRobots}";
	}

	private void onRobotCompleted(AssemblyLineGame.RobotCompletedEvent evt)
	{
		if (base.gameObject.activeInHierarchy)
		{
			StartCoroutine(doRobotCompletedRoutine(evt));
		}
	}

	private IEnumerator doRobotCompletedRoutine(AssemblyLineGame.RobotCompletedEvent completedEvent)
	{
		yield return new WaitForSeconds(robotCompletedBubbleDelay);
		yield return spawnBubble(lblCompletedBubble, "1", UICamera.currentCamera.ScreenToWorldPoint(Camera.main.WorldToScreenPoint(completedEvent.DropZone.transform.position)), lblRobotsCompleted.transform.position);
		lblRobotsCompleted.text = $"{Controller<AssemblyLineGame>().CompletedRobots}/{Controller<AssemblyLineGame>().RequiredRobotCount}";
		lblRobotsCompleted.GetComponent<UITweener>().ResetToBeginning();
		lblRobotsCompleted.GetComponent<UITweener>().PlayForward();
	}

	private Coroutine spawnBubble(UILabel lblPrefab, string text, Vector3 position, Vector3 targetPos)
	{
		UILabel uILabel = Object.Instantiate(lblPrefab);
		uILabel.transform.parent = base.transform;
		uILabel.transform.position = position;
		uILabel.transform.localScale = Vector3.one;
		uILabel.text += text;
		return StartCoroutine(animateBubbleTowards(uILabel.transform, targetPos));
	}

	private IEnumerator animateBubbleTowards(Transform trans, Vector3 endPos)
	{
		Vector3 startPos = trans.position;
		Stopwatch sw = new Stopwatch(bubbleSpeed);
		while (!sw.Expired)
		{
			yield return null;
			trans.position = new Vector3(MathUtilities.LerpUnclamped(startPos.x, endPos.x, bubbleCurveX.Evaluate(sw.Progress)), MathUtilities.LerpUnclamped(startPos.y, endPos.y, bubbleCurveY.Evaluate(sw.Progress)));
		}
		Object.Destroy(trans.gameObject);
	}
}
