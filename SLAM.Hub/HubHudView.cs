using System.Collections.Generic;
using SLAM.Engine;
using SLAM.Slinq;
using SLAM.Webservices;
using UnityEngine;

namespace SLAM.Hub;

public class HubHudView : HubMarkerView
{
	[SerializeField]
	private Camera uiCamera;

	[SerializeField]
	private Material premiumMaterial;

	[SerializeField]
	private Material lockedMaterial;

	[SerializeField]
	private Material unlockedMaterial;

	private HubLocationProvider locationUnderMouse;

	private bool hasFocus = true;

	private void OnEnable()
	{
		initializeLocationMarkers();
	}

	private void OnApplicationFocus(bool focusStatus)
	{
		hasFocus = focusStatus;
	}

	protected override void Update()
	{
		if (!hasFocus)
		{
			return;
		}
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out var hitInfo) && hitInfo.collider.HasComponent<HubMarkerButton>())
		{
			HubMarkerButton component = hitInfo.collider.GetComponent<HubMarkerButton>();
			HubLocationProvider hubLocationProvider = component.Data as HubLocationProvider;
			if (hubLocationProvider != locationUnderMouse)
			{
				locationUnderMouse = hubLocationProvider;
				AudioController.Play((component.Data as HubLocationProvider).MouseOverSound.name);
			}
		}
		else
		{
			locationUnderMouse = null;
		}
	}

	public override void Close(Callback callback, bool immediately)
	{
		clearMarkers();
		base.Close(callback, immediately);
	}

	public void OpenSettingsWindow()
	{
		Controller<HubController>().OpenSettingsWindow();
	}

	public void OpenInstructionsWindow()
	{
		Controller<HubController>().OpenInstructionsWindow();
	}

	private void initializeLocationMarkers()
	{
		HubLocationProvider[] array = Object.FindObjectsOfType<HubLocationProvider>();
		HubLocationProvider highlightLocation = getHighlightLocation(array);
		for (int i = 0; i < array.Length; i++)
		{
			HubLocationProvider hubLocationProvider = array[i];
			Material locationMaterial = getLocationMaterial(hubLocationProvider);
			HubMarkerIcon locationIcon = getLocationIcon(hubLocationProvider);
			spawnMarkerDelayed((float)i * 0.1f, hubLocationProvider.IconLocation.position, hubLocationProvider.IconLocation.rotation, hubLocationProvider.IconLocation.localScale, locationMaterial, locationIcon, hubLocationProvider == highlightLocation, delegate(HubMarkerButton button)
			{
				if (!Controller<HubController>().TrialHasEnded)
				{
					AudioController.Play("Interface_buttonClick_primary");
					Controller<HubController>().SelectLocation(button.Data as HubLocationProvider);
				}
			}, hubLocationProvider);
		}
	}

	private HubLocationProvider getHighlightLocation(HubLocationProvider[] locations)
	{
		for (int i = 0; i < locations.Length; i++)
		{
			Location loc = Controller<HubController>().GetLocation(locations[i]);
			IEnumerable<Game> collection = loc.Games.Where((Game g) => g.Type == Game.GameType.AdventureGame && g.IsPremiumAvailable && g.IsUnlocked);
			if (UserProfile.Current != null && UserProfile.Current.IsSA)
			{
				collection = loc.Games.Where((Game g) => g.Type == Game.GameType.AdventureGame && g.IsPremiumAvailable && g.IsUnlocked && g.IsUnlockedSA);
			}
			if (collection.Any((Game g) => Controller<HubController>().ProgressionManager.IsUnlocked(g)) && collection.Any((Game g) => !Controller<HubController>().ProgressionManager.IsUnlocked(g)))
			{
				return locations[i];
			}
			Game game = collection.FirstOrDefault((Game g) => !g.NextGameId.HasValue || loc.GetGame(g.NextGameId.Value) == null);
			if (game != null && Controller<HubController>().ProgressionManager.IsUnlocked(game) && (!game.NextGameId.HasValue || !Controller<HubController>().ProgressionManager.IsUnlocked(game.NextGameId.Value)))
			{
				return locations[i];
			}
		}
		if (locations.Length <= 1)
		{
			return locations.First();
		}
		return null;
	}

	private Material getLocationMaterial(HubLocationProvider locProv)
	{
		Location location = Controller<HubController>().GetLocation(locProv);
		if (location.Games.All((Game g) => !g.IsPremiumAvailable))
		{
			return premiumMaterial;
		}
		if (location.Games.All((Game g) => !g.IsUnlocked) || (location.Games.All((Game g) => !g.IsUnlockedSA) && UserProfile.Current != null && UserProfile.Current.IsSA))
		{
			return lockedMaterial;
		}
		return unlockedMaterial;
	}

	private HubMarkerIcon getLocationIcon(HubLocationProvider locProv)
	{
		Location location = Controller<HubController>().GetLocation(locProv);
		if (UserProfile.Current.IsFree && location.Games.All((Game g) => g.FreeLevelTo <= 0))
		{
			return HubMarkerIcon.Premium;
		}
		if (location.Games.All((Game g) => !g.IsUnlocked) || (location.Games.All((Game g) => !g.IsUnlockedSA) && UserProfile.Current != null && UserProfile.Current.IsSA))
		{
			return HubMarkerIcon.Locked;
		}
		return locProv.MarkerIcon;
	}
}
