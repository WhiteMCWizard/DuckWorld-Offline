using System.Collections;
using System.Collections.Generic;
using SLAM.BuildSystem;
using SLAM.Slinq;
using SLAM.Smartphone;
using SLAM.Webservices;
using UnityEngine;

namespace SLAM.Invites;

public class InviteSystem : SingletonMonobehaviour<InviteSystem>
{
	public class GameInviteEvent
	{
		public Game Game;
	}

	public class GameInviteResponseEvent
	{
		public Game Game;

		public bool Accepted;
	}

	[SerializeField]
	private int inviteIntervalSec = 300;

	[SerializeField]
	private int phoneNotificationDelayMin = 30;

	[SerializeField]
	private int phoneNotificationDelayMax = 90;

	[SerializeField]
	private int requiredLevelsUnlocked;

	[SerializeField]
	private Game.GameType[] allowedGameTypes;

	[SerializeField]
	[GameId]
	private int[] excludedGames;

	private List<int> GameInvitationsIds;

	private int levelsUnlocked;

	private float timeInHub;

	private float requiredTimeInHub = 10f;

	private float nextInvitationTime;

	private bool isSmartphoneOpen;

	public static int AcceptedInvitationGameId { get; private set; }

	public static int PendingInvitationGameId { get; private set; }

	public static bool HasReceivedInvitation { get; private set; }

	public static bool HasPendingInvitation => PendingInvitationGameId > 0;

	public static List<Game> Games { get; private set; }

	private bool SceneIsHub => "Hub" == Application.loadedLevelName;

	private void OnEnable()
	{
		GameEvents.Subscribe<GameInviteResponseEvent>(onGameInviteResponse);
		GameEvents.Subscribe<Webservice.LogoutEvent>(onLogout);
		GameEvents.Subscribe<SmartphoneVisibilityChangedEvent>(onSmartphoneVisibilityChanged);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<GameInviteResponseEvent>(onGameInviteResponse);
		GameEvents.Unsubscribe<Webservice.LogoutEvent>(onLogout);
		GameEvents.Unsubscribe<SmartphoneVisibilityChangedEvent>(onSmartphoneVisibilityChanged);
	}

	private IEnumerator Start()
	{
		Object.DontDestroyOnLoad(base.gameObject);
		while (UserProfile.Current == null)
		{
			yield return null;
		}
		if (UserProfile.Current.IsFree)
		{
			base.enabled = false;
		}
		DataStorage.GetLocationsData(delegate(Location[] locs)
		{
			Games = new List<Game>();
			GameInvitationsIds = new List<int>();
			for (int i = 0; i < locs.Length; i++)
			{
				Games.AddRange(locs[i].Games.Where((Game g) => g.IsUnlocked && allowedGameTypes.Contains(g.Type) && !excludedGames.Contains(g.Id)));
			}
		});
		DataStorage.GetProgressionData(delegate(UserGameDetails[] details)
		{
			if (details != null)
			{
				foreach (UserGameDetails userGameDetails in details)
				{
					levelsUnlocked += userGameDetails.Progression.Count() + 1;
				}
			}
		});
		requiredTimeInHub = Random.Range(phoneNotificationDelayMin, phoneNotificationDelayMax);
	}

	private void Update()
	{
		if (Games == null)
		{
			return;
		}
		if (SceneIsHub)
		{
			timeInHub += Time.deltaTime;
			if (!isSmartphoneOpen && !HasReceivedInvitation && Time.time > nextInvitationTime && timeInHub > requiredTimeInHub)
			{
				doInvite();
			}
		}
		else
		{
			timeInHub = 0f;
		}
	}

	private void doInvite()
	{
		Game game = null;
		nextInvitationTime = Time.time + (float)inviteIntervalSec;
		requiredTimeInHub = Random.Range(phoneNotificationDelayMin, phoneNotificationDelayMax);
		if (levelsUnlocked < requiredLevelsUnlocked)
		{
			levelsUnlocked = 0;
			DataStorage.GetProgressionData(delegate(UserGameDetails[] details)
			{
				foreach (UserGameDetails userGameDetails in details)
				{
					levelsUnlocked += userGameDetails.Progression.Count();
				}
			});
			Debug.Log($"Hey Buddy, you need to unlock {requiredLevelsUnlocked - levelsUnlocked} ({levelsUnlocked}/{requiredLevelsUnlocked}) more levels before will receive an invitation.");
			return;
		}
		if (GameInvitationsIds.Count >= Games.Count)
		{
			GameInvitationsIds.RemoveAll((int gi) => true);
		}
		game = (HasPendingInvitation ? Games.FirstOrDefault((Game g) => g.Id == PendingInvitationGameId) : ((UserProfile.Current == null || !UserProfile.Current.IsSA) ? Games.Where((Game g) => !GameInvitationsIds.Contains(g.Id)).GetRandom() : Games.Where((Game g) => !GameInvitationsIds.Contains(g.Id) && g.IsUnlockedSA).GetRandom()));
		if (game != null)
		{
			GameInvitationsIds.Add(game.Id);
			PendingInvitationGameId = game.Id;
			GameInviteEvent gameInviteEvent = new GameInviteEvent();
			gameInviteEvent.Game = game;
			GameEvents.Invoke(gameInviteEvent);
		}
	}

	public static void ReceivedGameInvitation()
	{
		HasReceivedInvitation = true;
	}

	public static void AcceptGameInvitation()
	{
		if (HasPendingInvitation)
		{
			GameInviteResponseEvent gameInviteResponseEvent = new GameInviteResponseEvent();
			gameInviteResponseEvent.Game = Games.FirstOrDefault((Game g) => g.Id == PendingInvitationGameId);
			gameInviteResponseEvent.Accepted = true;
			GameEvents.Invoke(gameInviteResponseEvent);
		}
	}

	public static void DeclineGameInvitation()
	{
		if (HasPendingInvitation)
		{
			GameInviteResponseEvent gameInviteResponseEvent = new GameInviteResponseEvent();
			gameInviteResponseEvent.Game = Games.FirstOrDefault((Game g) => g.Id == PendingInvitationGameId);
			gameInviteResponseEvent.Accepted = false;
			GameEvents.Invoke(gameInviteResponseEvent);
		}
	}

	public static void CloseGameInvitation()
	{
		PendingInvitationGameId = -1;
		AcceptedInvitationGameId = -1;
		HasReceivedInvitation = false;
	}

	private void onGameInviteResponse(GameInviteResponseEvent evt)
	{
		PendingInvitationGameId = -1;
		AcceptedInvitationGameId = ((!evt.Accepted) ? (-1) : evt.Game.Id);
		HasReceivedInvitation = false;
		nextInvitationTime = Time.time + (float)inviteIntervalSec;
		requiredTimeInHub = Random.Range(phoneNotificationDelayMin, phoneNotificationDelayMax);
		if (evt.Accepted)
		{
			SceneManager.Load(evt.Game.SceneName);
		}
	}

	private void onLogout(Webservice.LogoutEvent evt)
	{
		Object.Destroy(base.gameObject);
	}

	private void onSmartphoneVisibilityChanged(SmartphoneVisibilityChangedEvent evt)
	{
		isSmartphoneOpen = evt.IsVisible;
	}
}
