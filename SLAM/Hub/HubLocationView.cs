using System;
using System.Collections;
using System.Collections.Generic;
using SLAM.Engine;
using SLAM.SaveSystem;
using SLAM.Shared;
using SLAM.Slinq;
using SLAM.Webservices;
using UnityEngine;

namespace SLAM.Hub;

public class HubLocationView : HubMarkerView
{
	public static int RecentlyUnlockedGameId = -1;

	public static List<int> VisitedLocationIds = new List<int>();

	[SerializeField]
	private UILabel titleLabel;

	[SerializeField]
	private UIPortrait portraitSprite;

	[SerializeField]
	private Material lockedMaterial;

	[SerializeField]
	private Material unlockedMaterial;

	[SerializeField]
	private Material premiumMaterial;

	[SerializeField]
	private GameObject fireworksPrefab;

	[SerializeField]
	private GameObject labelPrefab;

	[SerializeField]
	private GameObject labelRoot;

	[SerializeField]
	private AnimationCurve lineAnimationCurve;

	[SerializeField]
	[Tooltip("uv/s")]
	private float pathLineSpeed = 0.5f;

	[SerializeField]
	private Vector3 nameLabelOffset;

	private Location locationInfo;

	private HubLocationProvider locationProvider;

	private int prevTouchCount;

	private bool cheatsEnabled = true;

	protected override void Start()
	{
		base.Start();
	}

	protected override void Update()
	{
		if (!cheatsEnabled || (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift)))
		{
			return;
		}
		for (int i = 48; i <= 57; i++)
		{
			if (Input.GetKeyDown((KeyCode)i))
			{
				StartCoroutine(doUnlockSequenceDelayed(0f, markerRoot.transform.GetChild(i - 49).gameObject));
				markerRoot.transform.GetChild(i - 49).GetComponent<HubMarkerButton>().SetClickHandler(onPlayableGameClicked);
			}
		}
	}

	public override void Close(Callback callback, bool immediately)
	{
		clearMarkers();
		labelRoot.transform.DestroyChildren();
		if (locationProvider != null)
		{
			HubLocationProvider.HubGameMarker[] gameMarkers = locationProvider.GameMarkers;
			foreach (HubLocationProvider.HubGameMarker hubGameMarker in gameMarkers)
			{
				if (hubGameMarker.CircleObject != null)
				{
					hubGameMarker.CircleObject.SetActive(value: false);
				}
				if (hubGameMarker.PathObject != null)
				{
					hubGameMarker.PathObject.GetComponent<Renderer>().material.SetFloat("_Progress", 0f);
				}
			}
		}
		base.Close(callback, immediately);
	}

	public void OnCloseClicked()
	{
		Controller<HubController>().DeselectLocation();
	}

	public virtual void OnLogoutClicked()
	{
		GameEvents.Invoke(new Webservice.LogoutEvent());
	}

	public void UpdateInfo(Location info, HubLocationProvider hubLocationProvider)
	{
		locationInfo = info;
		locationProvider = hubLocationProvider;
		titleLabel.text = Localization.Get(info.Name);
		portraitSprite.SetCharacter(hubLocationProvider.GameCharacter);
		float speedMod = 1f;
		if (VisitedLocationIds.Contains(info.Id))
		{
			speedMod = 2f;
		}
		else
		{
			VisitedLocationIds.Add(info.Id);
		}
		createGameMarkers(info, hubLocationProvider, speedMod);
	}

	private void createGameMarkers(Location info, HubLocationProvider hubLocationProvider, float speedMod = 1f)
	{
		VisitedLocationIds.Add(locationInfo.Id);
		IEnumerable<Game> enumerable = info.Games.Where((Game g) => g.Enabled && g.Type == Game.GameType.AdventureGame);
		if (enumerable.Count() > 0)
		{
			StartCoroutine(doAdventureGameSequence(enumerable, hubLocationProvider, speedMod));
		}
		for (int num = 0; num < info.Games.Length; num++)
		{
			if (!enumerable.Contains(info.Games[num]) && info.Games[num].Enabled)
			{
				spawnButtonForGame(info.Games[num], (float)info.Games[num].SortOrder * 0.5f * speedMod);
			}
		}
	}

	private IEnumerator doAdventureGameSequence(IEnumerable<Game> games, HubLocationProvider hubLocationProvider, float speedMod = 1f)
	{
		games = games.OrderBy((Game g) => getAdventureGameIndex(g));
		foreach (Game game in games)
		{
			spawnButtonForGame(game, 0f);
			HubLocationProvider.HubGameMarker marker = hubLocationProvider.GameMarkers.FirstOrDefault((HubLocationProvider.HubGameMarker gm) => gm.GameId == game.Id);
			bool shouldBeLocked = game.Id != RecentlyUnlockedGameId && game.IsUnlocked && Controller<HubController>().ProgressionManager.IsUnlocked(game);
			if (marker.CircleObject != null)
			{
				marker.CircleObject.SetActive(value: true);
				marker.CircleObject.GetComponentInChildren<Renderer>().material.SetFloat("_Blend", (!shouldBeLocked) ? 1 : 0);
			}
			if (marker.PathObject == null)
			{
				break;
			}
			bool nextGameUnlocked = !UserProfile.Current.IsFree && game.NextGameId.HasValue && game.NextGameId.Value != RecentlyUnlockedGameId && Controller<HubController>().ProgressionManager.IsUnlocked(locationInfo.GetGame(game.NextGameId.Value));
			Material mat = marker.PathObject.GetComponent<Renderer>().material;
			mat.SetFloat("_LockedProgress", (!nextGameUnlocked) ? 0f : marker.PathUvLength);
			Stopwatch sw = new Stopwatch(marker.PathUvLength / (pathLineSpeed * speedMod));
			while ((bool)sw)
			{
				yield return null;
				mat.SetFloat("_Progress", lineAnimationCurve.Evaluate(sw.Progress) * marker.PathUvLength);
			}
		}
	}

	private IEnumerator doUnlockSequenceDelayed(float delay, GameObject marker)
	{
		yield return new WaitForSeconds(delay);
		HubMarkerButton btn = marker.GetComponent<HubMarkerButton>();
		Game game = btn.Data as Game;
		if (game.PreviousGameId.HasValue)
		{
			HubLocationProvider.HubGameMarker previousMarker = locationProvider.GameMarkers.FirstOrDefault((HubLocationProvider.HubGameMarker gm) => gm.GameId == game.PreviousGameId);
			if (previousMarker != null && previousMarker.PathObject != null)
			{
				yield return StartCoroutine(animatePathToCompletion(previousMarker));
			}
		}
		HubLocationProvider.HubGameMarker currentMarker = locationProvider.GameMarkers.FirstOrDefault((HubLocationProvider.HubGameMarker gm) => gm.GameId == game.Id);
		UnityEngine.Object.Instantiate(fireworksPrefab, marker.transform.position, Quaternion.identity);
		HubMarkerButton[] array = UnityEngine.Object.FindObjectsOfType<HubMarkerButton>();
		foreach (HubMarkerButton otherBtn in array)
		{
			otherBtn.SetHighlighted(state: false);
		}
		btn.SetMaterial(unlockedMaterial);
		btn.SetClickHandler(onPlayableGameClicked);
		btn.SetHighlighted(state: true);
		RecentlyUnlockedGameId = -1;
		btn.SetIcon(getGameIcon(game));
		if (currentMarker.CircleObject != null)
		{
			Renderer circleRenderer = currentMarker.CircleObject.GetComponentInChildren<Renderer>();
			Stopwatch sw = new Stopwatch(0.2f);
			while ((bool)sw)
			{
				yield return null;
				circleRenderer.material.SetFloat("_Blend", 1f - sw.Progress);
			}
		}
	}

	private IEnumerator animatePathToCompletion(HubLocationProvider.HubGameMarker marker)
	{
		Material mat = marker.PathObject.GetComponent<Renderer>().material;
		Stopwatch sw = new Stopwatch(marker.PathUvLength / pathLineSpeed);
		while ((bool)sw)
		{
			yield return null;
			mat.SetFloat("_LockedProgress", lineAnimationCurve.Evaluate(sw.Progress) * marker.PathUvLength);
		}
	}

	private GameObject spawnButtonForGame(Game game, float delay)
	{
		HubLocationProvider.HubGameMarker hubGameMarker = locationProvider.GameMarkers.FirstOrDefault((HubLocationProvider.HubGameMarker gm) => gm.GameId == game.Id);
		if (hubGameMarker == null)
		{
			Debug.LogWarning(string.Concat("Hey buddy, i couldnt find game marker location for game: ", game.Name, " in provider: ", locationProvider, ".\nPlease add a game maker location in the Locations prefab!"), locationProvider);
			return null;
		}
		getGameLocked(game, out var clickCallback);
		HubMarkerIcon gameIcon = getGameIcon(game);
		Material gameMaterial = getGameMaterial(game);
		bool gameHighlight = getGameHighlight(game);
		GameObject gameObject = spawnMarkerDelayed(delay, hubGameMarker.Position, hubGameMarker.Rotation, hubGameMarker.MarkerScale, gameMaterial, gameIcon, gameHighlight, clickCallback, game);
		gameObject.name = game.Name;
		UserGameDetails userGameDetails = Controller<HubController>().ProgressionManager.GameDetails.FirstOrDefault((UserGameDetails g) => g.GameId == game.Id);
		float progress = ((userGameDetails != null) ? ((float)userGameDetails.Progression.Count() / (float)game.TotalLevels) : ((game.Type != Game.GameType.Shop) ? 0f : 1f));
		StartCoroutine(showTextAndPlayAudioDelayed(delay + 0.7f, hubGameMarker.Position, Localization.Get(game.Name), progress));
		if (game.Id == RecentlyUnlockedGameId && game.IsPremiumAvailable)
		{
			StartCoroutine(doUnlockSequenceDelayed(delay + 1f, gameObject));
		}
		return gameObject;
	}

	private bool getGameLocked(Game game, out Action<HubMarkerButton> clickCallback)
	{
		if (!game.IsUnlocked)
		{
			clickCallback = onFutureGameClicked;
			return true;
		}
		if (UserProfile.Current.IsFree && game.FreeLevelTo <= 0)
		{
			clickCallback = onPremiumGameClicked;
			return true;
		}
		if (UserProfile.Current != null && UserProfile.Current.IsSA && !game.IsUnlockedSA)
		{
			clickCallback = onSanomaAccountLockedGameClicked;
			return true;
		}
		if (!UserProfile.Current.IsFree && game.Type == Game.GameType.AdventureGame && !Controller<HubController>().ProgressionManager.IsUnlocked(game))
		{
			clickCallback = onLockedAdventureGameClicked;
			return true;
		}
		clickCallback = onPlayableGameClicked;
		return false;
	}

	private bool getGameHighlight(Game game)
	{
		return game.IsUnlocked && game.IsPremiumAvailable && game.Type == Game.GameType.AdventureGame && Controller<HubController>().ProgressionManager.IsUnlocked(game) && game.NextGameId.HasValue && !Controller<HubController>().ProgressionManager.IsUnlocked(game.NextGameId.Value);
	}

	private Material getGameMaterial(Game game)
	{
		if (!game.IsPremiumAvailable)
		{
			return premiumMaterial;
		}
		if (getGameLocked(game, out var _) || game.Id == RecentlyUnlockedGameId)
		{
			return lockedMaterial;
		}
		return unlockedMaterial;
	}

	private HubMarkerIcon getGameIcon(Game game)
	{
		if (!game.IsPremiumAvailable)
		{
			return HubMarkerIcon.Premium;
		}
		if (getGameLocked(game, out var _))
		{
			return HubMarkerIcon.Locked;
		}
		if (game.Id == 19)
		{
			return HubMarkerIcon.AvatarHouse;
		}
		return game.Type switch
		{
			Game.GameType.AdventureGame => (HubMarkerIcon)(6 + getAdventureGameIndexWithoutMotionComics(game)), 
			Game.GameType.Shop => HubMarkerIcon.Shop, 
			Game.GameType.Job => HubMarkerIcon.Job, 
			Game.GameType.LocationGame => HubMarkerIcon.Location, 
			_ => throw new Exception("No icon for game type " + game.Type), 
		};
	}

	private int getAdventureGameIndexWithoutMotionComics(Game game)
	{
		if (game.Name.StartsWith("HUB_GAME_MOTIONCOMIC"))
		{
			return 10;
		}
		int num = 0;
		while (game != null && game.PreviousGameId.HasValue)
		{
			game = locationInfo.GetGame(game.PreviousGameId.Value);
			if (game != null && !game.Name.StartsWith("HUB_GAME_MOTIONCOMIC"))
			{
				num++;
			}
		}
		return num;
	}

	private int getAdventureGameIndex(Game game)
	{
		int num = 0;
		while (game != null && game.PreviousGameId.HasValue)
		{
			game = locationInfo.GetGame(game.PreviousGameId.Value);
			num++;
		}
		return num;
	}

	private IEnumerator showTextAndPlayAudioDelayed(float delay, Vector3 worldPos, string text, float progress)
	{
		yield return new WaitForSeconds(delay - 0.7f);
		AudioController.Play("hub_balloon_random", worldPos);
		yield return new WaitForSeconds(0.7f);
		Vector3 pos = worldToScreen(worldPos + nameLabelOffset);
		GameObject labelGo = UnityEngine.Object.Instantiate(labelPrefab);
		labelGo.transform.parent = labelRoot.transform;
		labelGo.transform.localScale = Vector3.one;
		labelGo.transform.position = pos;
		labelGo.GetComponent<UILabel>().text = text;
		UIProgressBar pb = labelGo.GetComponentInChildren<UIProgressBar>();
		pb.value = 0f;
		Stopwatch sw = new Stopwatch(0.5f * progress);
		while (!sw.Expired)
		{
			pb.value = Mathf.Lerp(0f, progress, sw.Progress);
			yield return null;
		}
		pb.value = progress;
	}

	private void onPlayableGameClicked(HubMarkerButton button)
	{
		Controller<HubController>().Play(button.Data as Game);
	}

	private void onPremiumGameClicked(HubMarkerButton button)
	{
		Controller<HubController>().OpenPremiumGameLockedWindow(((Game)button.Data).Id);
	}

	private void onSanomaAccountLockedGameClicked(HubMarkerButton button)
	{
		Game game = button.Data as Game;
		GameEvents.Invoke(new PopupEvent(Localization.Get("HUB_POPUP_LOCKEDGAME_SA_TITLE"), StringFormatter.GetLocalizationFormatted("HUB_POPUP_LOCKEDGAME_SA_DESCRIPTION", Localization.Get(game.Name)), Localization.Get("HUB_POPUP_LOCKEDGAME_SA_BUTTON_CONTINUE"), null, null, null));
	}

	private void onLockedAdventureGameClicked(HubMarkerButton button)
	{
		Game game = button.Data as Game;
		GameEvents.Invoke(new PopupEvent(Localization.Get("HUB_POPUP_LOCKEDADVENTURE_TITLE"), StringFormatter.GetLocalizationFormatted("HUB_POPUP_LOCKEDADVENTURE_DESCRIPTION", Localization.Get(game.Name), Localization.Get(locationInfo.GetGame(game.PreviousGameId.Value).Name), locationInfo.GetGame(game.PreviousGameId.Value).RequiredDifficultyToUnlockNextGame), Localization.Get("HUB_POPUP_LOCKEDADVENTURE_BUTTON_CONTINUE"), null, null, null));
	}

	private void onLockedJobClicked(HubMarkerButton button)
	{
		Game game = button.Data as Game;
		GameEvents.Invoke(new PopupEvent(Localization.Get("HUB_POPUP_LOCKEDJOB_TITLE"), StringFormatter.GetLocalizationFormatted("HUB_POPUP_LOCKEDJOB_DESCRIPTION", Localization.Get(game.Name)), Localization.Get("HUB_POPUP_LOCKEDJOB_BUTTON_CONTINUE"), null, null, null));
	}

	private void onFutureGameClicked(HubMarkerButton button)
	{
		Game game = button.Data as Game;
		GameEvents.Invoke(new PopupEvent(Localization.Get("HUB_POPUP_FUTUREGAME_TITLE"), StringFormatter.GetLocalizationFormatted("HUB_POPUP_FUTUREGAME_DESCRIPTION", Localization.Get(game.Name)), Localization.Get("HUB_POPUP_FUTUREGAME_BUTTON_CONTINUE"), null, null, null));
	}

	private Vector3 worldToScreen(Vector3 pos)
	{
		Vector3 result = UICamera.currentCamera.ScreenToWorldPoint(Camera.main.WorldToScreenPoint(pos));
		result.z = 0f;
		return result;
	}
}
