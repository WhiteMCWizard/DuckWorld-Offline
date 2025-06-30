using System.Collections;
using System.Collections.Generic;
using SLAM.Engine;
using UnityEngine;

namespace SLAM.Sokoban.CrateMess;

public class CrateMessGameController : SokobanGameController
{
	private const int scorePerLevelComplete = 2;

	public override int GameId => 26;

	public override Dictionary<string, int> ScoreCategories
	{
		get
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			dictionary.Add(StringFormatter.GetLocalizationFormatted("UI_VICTORYWINDOW_COINS_EARNED", getCoinRewardForThisLevel()), getCoinRewardForThisLevel());
			return dictionary;
		}
	}

	public override string IntroNPCKey => "NPC_NAME_SCROOGE";

	public override string IntroTextKey => "CM_CINEMATICINTRO_TEXT";

	public override Portrait DuckCharacter => Portrait.ScroogeDuck;

	public override void Play(LevelSetting selectedLevel)
	{
		base.Play(selectedLevel);
		AudioController.Play("CM_ambience_loop");
		StartCoroutine(animateToNextRoom(null));
	}

	protected override void onMarkerCompleted(MarkerCompletedEvent obj)
	{
		base.onMarkerCompleted(obj);
		if (completedMarkerCount >= targetMarkerCount)
		{
			LevelCompleted();
		}
	}

	protected override IEnumerator animateToNextRoom(GameObject oldLevelInstance)
	{
		yield return StartCoroutine(OpenAndWait<FadeView>());
		avatar.transform.position = base.currentLevelInstance.GetComponentInChildren<SKBSpawnPoint>().transform.position + Vector3.down * 0.5f;
		Camera.main.transform.position = base.currentLevelInstance.GetComponentInChildren<SKBCameraAnchor>().transform.position + cameraOffset;
		yield return StartCoroutine(CloseAndWait<FadeView>());
	}
}
