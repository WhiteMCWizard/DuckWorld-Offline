using System;
using System.Collections;
using System.Collections.Generic;
using SLAM.Analytics;
using SLAM.Avatar;
using SLAM.BuildSystem;
using SLAM.Engine;
using SLAM.MotionComics._3D;
using SLAM.Slinq;
using SLAM.Smartphone;
using SLAM.Webservices;
using UnityEngine;

namespace SLAM.Hub;

public class HubController : ViewController
{
	private static int lastSelectedLocationId = -1;

	private static int pendingLocationId = -1;

	private static List<int> playedGameIds = new List<int>();

	public bool TrialHasEnded;

	[SerializeField]
	private GameProgressionManager progressionManager;

	[SerializeField]
	private View[] hubViews;

	[SerializeField]
	private AudioClip musicClip;

	[SerializeField]
	private AudioClip musicClipIntro;

	[SerializeField]
	private AudioClip defaultAmbientClip;

	[SerializeField]
	private AudioClip introAmbientClip;

	[SerializeField]
	private AudioClip idleAmbienceClip;

	[SerializeField]
	private float ambientFadeLength = 2f;

	[SerializeField]
	private HubCameraManager hubCameraManager;

	[SerializeField]
	private GameObject smartphone2DRoot;

	[SerializeField]
	private GameObject smartphone3DRoot;

	[SerializeField]
	private UIAtlas screenshotAtlas;

	private Location[] hubData;

	private HubLocationProvider selectedLocation;

	private AudioObject playingAmbientObject;

	public static int LastSelectedLocationId => lastSelectedLocationId;

	public static int PendingLocationId => pendingLocationId;

	public bool IsSmartphoneVisible { get; protected set; }

	protected bool isShowingLocation => selectedLocation == null;

	public GameProgressionManager ProgressionManager => progressionManager;

	public bool IsFirstPlayViewOpen => IsViewOpen<HubFirstPlayView>();

	protected void Awake()
	{
		AddViews(hubViews);
		TrackingEvent trackingEvent = new TrackingEvent();
		trackingEvent.Type = TrackingEvent.TrackingType.HubOpened;
		trackingEvent.Arguments = new Dictionary<string, object>();
		GameEvents.Invoke(trackingEvent);
	}

	private new IEnumerator Start()
	{
		if (lastSelectedLocationId <= 0)
		{
			AudioController.PlayMusic(musicClipIntro.name);
			hubCameraManager.PlayIntroAnimation();
			if (introAmbientClip != null)
			{
				playingAmbientObject = AudioController.Play(introAmbientClip.name);
				playingAmbientObject.FadeOut(8.133f);
				playingAmbientObject.SwitchAudioSources();
			}
			playingAmbientObject.PlayNow(defaultAmbientClip.name, 0f, 1f, 0f);
			playingAmbientObject.FadeIn(8.133f);
		}
		else
		{
			AudioController.PlayMusic(musicClip.name);
			selectedLocation = UnityEngine.Object.FindObjectsOfType<HubLocationProvider>().FirstOrDefault((HubLocationProvider hlp) => hlp.LocationId == lastSelectedLocationId);
			hubCameraManager.WarpToLocation(selectedLocation);
			playingAmbientObject = AudioController.Play(selectedLocation.AmbientLoop.name);
		}
		yield return StartCoroutine(waitForDataLoadAndIntroAnimationToContinue());
		if (AvatarSystem.GetPlayerConfiguration() == null)
		{
			OpenView<HubFirstPlayView>();
		}
		else if (lastSelectedLocationId > 0)
		{
			OpenView<HubLocationView>().UpdateInfo(Location.GetById(selectedLocation.LocationId, hubData), selectedLocation);
		}
		else
		{
			OpenView<HubHudView>();
		}
		if (pendingLocationId > -1)
		{
			DeselectLocation(delegate
			{
				SelectLocation(UnityEngine.Object.FindObjectsOfType<HubLocationProvider>().FirstOrDefault((HubLocationProvider hlp) => hlp.LocationId == pendingLocationId));
				pendingLocationId = -1;
			});
		}
		smartphone2DRoot.SetActive(!IsViewOpen<HubFirstPlayView>() && !playedGameIds.Contains(-1));
	}

	private void OnEnable()
	{
		GameEvents.Subscribe<SmartphoneVisibilityChangedEvent>(onSmartphoneVisibilityChanged);
		GameEvents.Subscribe<Webservice.LogoutEvent>(onLogout);
		GameEvents.Subscribe<Webservice.TrialEndedEvent>(onTrialEnded);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<SmartphoneVisibilityChangedEvent>(onSmartphoneVisibilityChanged);
		GameEvents.Unsubscribe<Webservice.LogoutEvent>(onLogout);
		GameEvents.Unsubscribe<Webservice.TrialEndedEvent>(onTrialEnded);
	}

	private void onLogout(Webservice.LogoutEvent evt)
	{
		lastSelectedLocationId = -1;
		playedGameIds = new List<int>();
	}

	private void onTrialEnded(Webservice.TrialEndedEvent evt)
	{
		TrialHasEnded = true;
		CloseAllViews();
		smartphone2DRoot.SetActive(value: false);
		smartphone3DRoot.SetActive(value: false);
		if (selectedLocation != null)
		{
			DeselectLocation(delegate
			{
				GetView<HubHudView>().Close();
			});
		}
		else
		{
			GetView<HubHudView>().Close();
		}
		GameEvents.Invoke(new PopupEvent(Localization.Get("HUB_POPUP_TRIAL_PERIOD_ENDED_TITLE"), Localization.Get("HUB_POPUP_TRIAL_PERIOD_ENDED_DESCRIPTION"), Localization.Get("UI_OK"), string.Empty, delegate
		{
			LogoutCurrentUser();
		}, null, closeButtonVisible: false));
	}

	private void onSmartphoneVisibilityChanged(SmartphoneVisibilityChangedEvent evt)
	{
		IsSmartphoneVisible = evt.IsVisible;
		if (selectedLocation == null)
		{
			View view = GetView<HubHudView>();
			if (IsSmartphoneVisible && view.IsOpen)
			{
				CloseView(view);
			}
			else if (!view.IsOpen)
			{
				OpenView(view);
			}
		}
		else if (IsSmartphoneVisible)
		{
			DeselectLocation();
		}
	}

	private IEnumerator waitForDataLoadAndIntroAnimationToContinue()
	{
		smartphone2DRoot.SetActive(value: false);
		AvatarSystem.LoadPlayerConfiguration();
		DataStorage.GetLocationsData(delegate(Location[] locations)
		{
			hubData = locations;
		});
		while (!hasDataBeenLoadedToBegin())
		{
			yield return null;
		}
		while (hubCameraManager.IsPlayingIntroAnimation)
		{
			yield return null;
		}
	}

	private bool hasDataBeenLoadedToBegin()
	{
		return hubData != null && progressionManager.GameDetails != null;
	}

	public void LogoutCurrentUser()
	{
		GameEvents.Invoke(new Webservice.LogoutEvent());
	}

	public void OpenSettingsWindow()
	{
		if (!IsFirstPlayViewOpen)
		{
			UnityEngine.Object.FindObjectOfType<SmartphoneController>().HideNotificationCenter();
			SmartphoneVisibilityChangedEvent smartphoneVisibilityChangedEvent = new SmartphoneVisibilityChangedEvent();
			smartphoneVisibilityChangedEvent.IsVisible = true;
			GameEvents.Invoke(smartphoneVisibilityChangedEvent);
		}
		if (!IsViewOpen<HubMenuView>())
		{
			OpenView<HubMenuView>();
		}
	}

	public void CloseSettingsWindow()
	{
		if (!IsFirstPlayViewOpen)
		{
			UnityEngine.Object.FindObjectOfType<SmartphoneController>().ShowNotificationCenter();
			SmartphoneVisibilityChangedEvent smartphoneVisibilityChangedEvent = new SmartphoneVisibilityChangedEvent();
			smartphoneVisibilityChangedEvent.IsVisible = false;
			GameEvents.Invoke(smartphoneVisibilityChangedEvent);
		}
		CloseView<HubMenuView>();
	}

	public void OpenPremiumGameLockedWindow(int gameId)
	{
		if (screenshotAtlas.GetSprite(gameId.ToString()) != null)
		{
			HubPremiumGameView hubPremiumGameView = (IsViewOpen<HubPremiumGameView>() ? GetView<HubPremiumGameView>() : OpenView<HubPremiumGameView>());
			hubPremiumGameView.SetInfo(gameId, Localization.Get("HUB_POPUP_PREMIUMGAME_TITLE"), Localization.Get("HUB_POPUP_PREMIUMGAME_DESCRIPTION"), Localization.Get("UI_REGISTER"), Localization.Get("UI_TIP_PARENTS"), delegate
			{
				ApiClient.OpenPropositionPage(gameId);
			}, delegate
			{
				OpenView<TipParentView>();
			});
		}
		else
		{
			GameEvents.Invoke(new PopupEvent(Localization.Get("HUB_POPUP_PREMIUMGAME_TITLE"), Localization.Get("HUB_POPUP_PREMIUMGAME_DESCRIPTION"), Localization.Get("UI_REGISTER"), Localization.Get("UI_TIP_PARENTS"), delegate
			{
				ApiClient.OpenPropositionPage(gameId);
			}, delegate
			{
				OpenView<TipParentView>();
			}));
		}
	}

	public void OpenInstructionsWindow()
	{
		CloseView<HubHudView>();
		OpenView<PauseView>();
	}

	public void CloseInstructionsWindow()
	{
		OpenView<HubHudView>();
		CloseView<PauseView>();
	}

	public void OnCameraIdleStart()
	{
		CloseAllViews();
		smartphone2DRoot.SetActive(value: false);
		smartphone3DRoot.SetActive(value: false);
		playingAmbientObject.FadeOut(4f);
		playingAmbientObject.SwitchAudioSources();
		playingAmbientObject.PlayNow(idleAmbienceClip.name, 0f, 1f, 0f);
		playingAmbientObject.FadeIn(4f);
	}

	public void FadeToOverview()
	{
		playingAmbientObject.FadeOut(3f);
		playingAmbientObject.SwitchAudioSources();
		playingAmbientObject.PlayNow(defaultAmbientClip.name, 0f, 1f, 0f);
		playingAmbientObject.FadeIn(3f);
	}

	public void OnCameraIdleStop()
	{
		OpenView<HubHudView>();
		smartphone2DRoot.SetActive(value: true);
		smartphone3DRoot.SetActive(value: true);
	}

	public void SelectLocation(HubLocationProvider location)
	{
		Location location2 = GetLocation(location);
		if (location2.Games.All((Game g) => !g.IsUnlockedSA) && UserProfile.Current != null && UserProfile.Current.IsSA)
		{
			GameEvents.Invoke(new PopupEvent(Localization.Get("HUB_POPUP_LOCKEDGAME_SA_TITLE"), StringFormatter.GetLocalizationFormatted("HUB_POPUP_LOCKEDGAME_SA_DESCRIPTION"), Localization.Get("HUB_POPUP_LOCKEDGAME_SA_BUTTON_CONTINUE"), null, null, null));
		}
		AnimateToLocation(location, OnArriveAtLocation);
		playingAmbientObject.FadeOut(ambientFadeLength);
		playingAmbientObject.SwitchAudioSources();
		playingAmbientObject.PlayNow(selectedLocation.AmbientLoop.name, 0f, 1f, 0f);
		playingAmbientObject.FadeIn(ambientFadeLength);
		if (IsViewOpen<HubHudView>())
		{
			CloseView<HubHudView>();
		}
	}

	public float AnimateToLocation(HubLocationProvider location, Action callback)
	{
		selectedLocation = location;
		smartphone2DRoot.SetActive(value: false);
		return hubCameraManager.AnimateToLocation(location, callback);
	}

	private void OnArriveAtLocation()
	{
		Location byId = Location.GetById(selectedLocation.LocationId, hubData);
		OpenView<HubLocationView>().UpdateInfo(byId, selectedLocation);
		TrackingEvent trackingEvent = new TrackingEvent();
		trackingEvent.Type = TrackingEvent.TrackingType.LocationOpened;
		trackingEvent.Arguments = new Dictionary<string, object> { { "LocationName", byId.Name } };
		GameEvents.Invoke(trackingEvent);
		smartphone2DRoot.SetActive(value: true);
	}

	public void DeselectLocation(Action callback = null)
	{
		CloseView<HubLocationView>();
		selectedLocation = null;
		smartphone2DRoot.SetActive(value: false);
		if (playingAmbientObject != null)
		{
			playingAmbientObject.FadeOut(ambientFadeLength);
			playingAmbientObject.SwitchAudioSources();
			playingAmbientObject.PlayNow(defaultAmbientClip.name, 0f, 1f, 0f);
			playingAmbientObject.FadeIn(ambientFadeLength);
		}
		else
		{
			Debug.LogWarning("Hey buddy, ambient not looping?");
		}
		hubCameraManager.AnimateToOverview(delegate
		{
			if (!IsSmartphoneVisible)
			{
				OpenView<HubHudView>();
			}
			smartphone2DRoot.SetActive(value: true);
			GameEvents.Invoke(new TrackingEvent
			{
				Type = TrackingEvent.TrackingType.HubOpened,
				Arguments = new Dictionary<string, object>()
			});
			if (callback != null)
			{
				callback();
			}
		});
	}

	public void Play(Game game)
	{
		if (IsSmartphoneVisible)
		{
			return;
		}
		if (base.enabled)
		{
			string text = game.SceneName;
			if (!string.IsNullOrEmpty(game.SceneMotionComicName) && ((!playedGameIds.Contains(game.Id) && !UserProfile.Current.IsFree) || text == Application.loadedLevelName))
			{
				text = game.SceneMotionComicName;
				MotionComicPlayer.SetSceneToLoad(game.SceneName);
			}
			playedGameIds.Add(game.Id);
			lastSelectedLocationId = ((!(selectedLocation != null) || game.Id < 0) ? (-1) : selectedLocation.LocationId);
			if (IsViewOpen<HubLocationView>())
			{
				CloseView<HubLocationView>();
			}
			SceneManager.Load(text);
		}
		base.enabled = false;
	}

	public Location GetLocation(HubLocationProvider provider)
	{
		return Location.GetById(provider.LocationId, hubData);
	}

	public static void ZoomOutNextVisit()
	{
		lastSelectedLocationId = -1;
	}

	public static void ZoomInNextVisit(int locationId)
	{
		pendingLocationId = locationId;
	}
}
