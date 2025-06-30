using SLAM.Shared;
using UnityEngine;

namespace SLAM.Engine;

public class BalloonView : View
{
	[SerializeField]
	private GameObject balloonTailLeftBottomPointingLeft;

	[SerializeField]
	private GameObject balloonTailRightBottomPointingRight;

	[SerializeField]
	private GameObject balloonTailLeftBottomPointingRight;

	[SerializeField]
	private GameObject balloonTailRightBottomPointingLeft;

	[SerializeField]
	private GameObject balloonTailLeftSidePointingDown;

	[SerializeField]
	private GameObject balloonTailRightSidePointingDown;

	public virtual SpeechBalloon CreateBalloon(BalloonType type)
	{
		GameObject gameObject = null;
		GameObject gameObject2 = NGUITools.AddChild(prefab: type switch
		{
			BalloonType.TailRightBottomPointingRight => balloonTailRightBottomPointingRight, 
			BalloonType.TailLeftBottomPointingRight => balloonTailLeftBottomPointingRight, 
			BalloonType.TailRightBottomPointingLeft => balloonTailRightBottomPointingLeft, 
			BalloonType.TailLeftSidePointingDown => balloonTailLeftSidePointingDown, 
			BalloonType.TailRightSidePointingDown => balloonTailRightSidePointingDown, 
			_ => balloonTailLeftBottomPointingLeft, 
		}, parent: base.gameObject);
		return gameObject2.GetComponent<SpeechBalloon>();
	}
}
