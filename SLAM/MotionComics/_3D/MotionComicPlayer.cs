using System.Collections;
using System.Collections.Generic;
using CinemaDirector;
using SLAM.Analytics;
using SLAM.BuildSystem;
using SLAM.Engine;
using SLAM.Hub;
using SLAM.SaveSystem;
using SLAM.Shared;
using SLAM.Slinq;
using SLAM.Webservices;
using UnityEngine;

namespace SLAM.MotionComics._3D;

public class MotionComicPlayer : ViewController
{
	private static string sceneToLoad = "CraneOperator";

	[SerializeField]
	private bool useSpeechBalloons;

	[SerializeField]
	private Cutscene cutscene;

	private AudioClip clipToPlayAfterInteract;

	private AssetBundleManager.AssetLoadRequest loadRequest;

	public bool IsLoadingGame { get; protected set; }

	public string SceneToLoad => sceneToLoad;

	public Cutscene Cutscene => cutscene;

	public static void SetSceneToLoad(string scene)
	{
		sceneToLoad = scene;
	}

	protected override void Start()
	{
		base.Start();
		TrackingEvent trackingEvent = new TrackingEvent();
		trackingEvent.Type = TrackingEvent.TrackingType.MotionComicOpened;
		trackingEvent.Arguments = new Dictionary<string, object> { 
		{
			"Game",
			Application.loadedLevelName
		} };
		GameEvents.Invoke(trackingEvent);
		SceneManager.Preload(SceneToLoad);
		AddViews(base.transform.GetComponentsInChildren<View>(includeInactive: true));
		OpenView<HudView>();
		if (!(sceneToLoad == "Hub"))
		{
			return;
		}
		DataStorage.GetLocationsData(delegate(Location[] locations)
		{
			Location location = locations.FirstOrDefault((Location l) => l.Id == HubController.LastSelectedLocationId);
			if (location != null)
			{
				Game game = location.Games.FirstOrDefault((Game g) => g.SceneMotionComicName == Application.loadedLevelName);
				if (game != null)
				{
					ApiClient.SubmitScore(game.Id, 1, game.RequiredDifficultyToUnlockNextGame.ToString(), 0, gameCompleted: false, delegate
					{
						DataStorage.GetProgressionData(null);
					});
					if (game.Name.ToLowerInvariant().Contains("outro"))
					{
						HubController.ZoomOutNextVisit();
					}
				}
			}
		});
	}

	private void OnValidate()
	{
		if (cutscene == null)
		{
			cutscene = Object.FindObjectOfType<Cutscene>();
		}
	}

	private void OnEnable()
	{
		Cutscene.CutsceneFinished += onCutsceneFinished;
	}

	private void OnDisable()
	{
		Cutscene.CutsceneFinished -= onCutsceneFinished;
	}

	private void onCutsceneFinished(object sender, CutsceneEventArgs e)
	{
		if (IsViewOpen<DialogView>())
		{
			CloseView<DialogView>();
		}
		StartCoroutine(waitForDownloadAndPlayGame());
	}

	public void OpenDialog(string npc, string textKey, NGUIText.Alignment alignment, bool append)
	{
		if (!useSpeechBalloons)
		{
			if (!IsViewOpen<DialogView>())
			{
				OpenView<DialogView>();
			}
			string text = ((UserProfile.Current == null) ? "Avatar" : UserProfile.Current.FirstName);
			string text2 = ((!(npc == "AVATAR_NAME")) ? Localization.Get(npc) : text);
			string text3 = Localization.Get(textKey);
			GetView<DialogView>().SetInfo(text2 + ": ", text3, alignment, append);
			AudioController.Play("Interface_newTextLine");
		}
	}

	public void OpenSpeechBalloon(TimelineTrack track, BalloonType balloonType, GameObject target, string textKey, bool append)
	{
		if (useSpeechBalloons)
		{
			if (!IsViewOpen<DialogView>())
			{
				OpenView<DialogView>();
			}
			string text = Localization.Get(textKey);
			SpeechBalloon speechBalloon = null;
			speechBalloon = ((!append) ? GetView<DialogView>().CreateBalloonOnTrack(track, balloonType) : GetView<DialogView>().GetLastBalloonOnTrack(track));
			speechBalloon.SetInfo(text, target, append);
		}
	}

	public void ShowInteractButton(AudioClip clip)
	{
		clipToPlayAfterInteract = clip;
		cutscene.Pause();
		OpenView<InteractView>();
	}

	public void CloseDialog(TimelineTrack track)
	{
		GetView<DialogView>().DestroyBalloonsOnTrack(track);
	}

	public void PlayGame()
	{
		if (!IsLoadingGame)
		{
			IsLoadingGame = true;
			SceneManager.Load(SceneToLoad);
		}
	}

	public void ResumeCutscene()
	{
		if ((bool)SingletonMonoBehaviour<AudioController>.DoesInstanceExist() && clipToPlayAfterInteract != null)
		{
			AudioController.Play(clipToPlayAfterInteract.name);
		}
		cutscene.Play();
		CloseView<InteractView>();
	}

	public void SkipCutscene()
	{
		if (IsViewOpen<DialogView>())
		{
			CloseView<DialogView>();
		}
		cutscene.Pause();
		StartCoroutine(waitForDownloadAndSkip());
	}

	private IEnumerator waitForDownloadAndPlayGame()
	{
		yield return StartCoroutine(waitForDownload());
		PlayGame();
	}

	private IEnumerator waitForDownloadAndSkip()
	{
		yield return StartCoroutine(waitForDownload());
		PlayGame();
	}

	private IEnumerator waitForDownload()
	{
		while (loadRequest != null && !loadRequest.IsDone)
		{
			yield return null;
		}
	}
}
