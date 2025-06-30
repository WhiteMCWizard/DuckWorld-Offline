using CinemaDirector;
using SLAM.Engine;
using UnityEngine;

namespace SLAM.MotionComics._3D;

[CutsceneItem("SLAM", "Open Dialog", new CutsceneItemGenre[] { CutsceneItemGenre.GlobalItem })]
public class OpenDialogGlobal : CinemaGlobalEvent
{
	[SerializeField]
	private string npcKey;

	[SerializeField]
	[HideInInspector]
	private string localizationKey;

	[SerializeField]
	private string speechKey;

	[SerializeField]
	[Tooltip("When true doesnt clear the original text, when false it does.")]
	private bool append;

	[HideInInspector]
	[SerializeField]
	private NGUIText.Alignment alignment = NGUIText.Alignment.Left;

	[Header("Speech Balloon Settings")]
	[SerializeField]
	private BalloonType balloonType;

	[SerializeField]
	private GameObject target;

	public override void Trigger()
	{
		if (string.IsNullOrEmpty(speechKey))
		{
			if (localizationKey.Contains(":"))
			{
				int num = localizationKey.IndexOf(":");
				npcKey = localizationKey.Substring(0, num);
				speechKey = localizationKey.Substring(num + 1).Trim();
			}
			else
			{
				npcKey = " ";
				speechKey = localizationKey;
			}
		}
		Object.FindObjectOfType<MotionComicPlayer>().OpenDialog(npcKey, speechKey, alignment, append);
		Object.FindObjectOfType<MotionComicPlayer>().OpenSpeechBalloon(base.TimelineTrack, balloonType, target, speechKey, append);
	}
}
