using System.Collections.Generic;
using SLAM.Analytics;
using SLAM.Avatar;
using SLAM.Engine;
using SLAM.Slinq;
using SLAM.Webservices;
using UnityEngine;

namespace SLAM.Shops;

[RequireComponent(typeof(Inventory))]
public class InventoryController : ViewController
{
	[SerializeField]
	protected AvatarSpawn avatarSpawn;

	[SerializeField]
	protected HUDView hudView;

	[SerializeField]
	protected InventoryView inventoryView;

	[SerializeField]
	protected LoadingView loadingView;

	[SerializeField]
	protected Inventory.Filter filter;

	[SerializeField]
	private int gameId;

	[SerializeField]
	private AudioClip ambienceAudio;

	protected Inventory inventory;

	protected AvatarItemLibrary avatarLibrary;

	protected GameObject avatarGO;

	protected Animator avatarAnimator;

	public AvatarConfigurationData AvatarConfig { get; protected set; }

	protected override void Start()
	{
		base.Start();
		AddView(hudView);
		AddView(inventoryView);
		AddView(loadingView);
		AvatarConfig = AvatarSystem.GetPlayerConfiguration().Clone() as AvatarConfigurationData;
		avatarLibrary = AvatarItemLibrary.GetItemLibrary(AvatarConfig);
		filter.Gender = AvatarConfig.Gender;
		filter.Race = AvatarConfig.Race;
		inventory = GetComponent<Inventory>();
		avatarGO = avatarSpawn.SpawnAvatar();
		OpenView<LoadingView>();
		inventory.RetrieveInventory(filter, OnInventoryRetrieved);
		AudioController.Play(ambienceAudio.name);
		DataStorage.GetLocationsData(delegate(Location[] locs)
		{
			GameEvents.Invoke(new TrackingEvent
			{
				Type = TrackingEvent.TrackingType.GameStart,
				Arguments = new Dictionary<string, object>
				{
					{ "GameId", gameId },
					{ "Difficulty", 0 },
					{
						"LocationName",
						locs.FirstOrDefault((Location l) => l.Games.Any((Game loc) => loc.Id == gameId)).Name
					},
					{
						"GameName",
						locs.FirstOrDefault((Location l) => l.Games.Any((Game loc) => loc.Id == gameId)).GetGame(gameId).Name
					}
				}
			});
		});
	}

	private void OnDestroy()
	{
		DataStorage.GetLocationsData(delegate(Location[] locs)
		{
			GameEvents.Invoke(new TrackingEvent
			{
				Type = TrackingEvent.TrackingType.GameQuit,
				Arguments = new Dictionary<string, object>
				{
					{ "GameId", gameId },
					{ "Difficulty", 0 },
					{
						"Time",
						Time.timeSinceLevelLoad
					},
					{
						"LocationName",
						locs.FirstOrDefault((Location l) => l.Games.Any((Game loc) => loc.Id == gameId)).Name
					},
					{
						"GameName",
						locs.FirstOrDefault((Location l) => l.Games.Any((Game loc) => loc.Id == gameId)).GetGame(gameId).Name
					}
				}
			});
		});
	}

	protected virtual void OnEnable()
	{
		GameEvents.Subscribe<ShopVariationClickedEvent>(OnVariationClicked);
	}

	protected virtual void OnDisable()
	{
		GameEvents.Unsubscribe<ShopVariationClickedEvent>(OnVariationClicked);
	}

	public virtual void SaveAvatar()
	{
		GameEvents.Invoke(new PopupEvent(Localization.Get("UI_ARE_YOU_SURE"), Localization.Get("WR_POPUP_SAVE_OUTFIT"), Localization.Get("UI_YES"), Localization.Get("UI_NO"), delegate
		{
			SingletonMonobehaviour<PhotoBooth>.Instance.SayCheese(AvatarConfig, delegate(Texture2D mugshot)
			{
				GameEvents.Invoke(new TrackingEvent
				{
					Type = TrackingEvent.TrackingType.AvatarSaved
				});
				AvatarSystem.SavePlayerConfiguration(AvatarConfig, mugshot);
				avatarAnimator.SetTrigger("SelectAvatar");
			});
		}, null));
	}

	public virtual void GoToHub()
	{
		OpenView<LoadingView>();
		SingletonMonobehaviour<PhotoBooth>.Instance.SayCheese(AvatarConfig, delegate(Texture2D mugshot)
		{
			AvatarSystem.SavePlayerConfiguration(AvatarConfig, mugshot);
			GameEvents.Invoke(new TrackingEvent
			{
				Type = TrackingEvent.TrackingType.AvatarSaved
			});
			Time.timeScale = 1f;
			Application.LoadLevel("Hub");
		});
	}

	protected virtual void OnInventoryRetrieved()
	{
		avatarAnimator = avatarSpawn.GetComponent<Animator>();
		CloseView<LoadingView>();
		OpenView<HUDView>();
		InventoryView inventoryView = OpenView<InventoryView>();
		inventoryView.Load(inventory.Items);
		if (inventory.Items.Length == 0)
		{
			Debug.LogWarning("Inventory for shop '" + filter.ShopId + "' is empty, no items will be displayed.");
		}
	}

	protected virtual void OnVariationClicked(ShopVariationClickedEvent evt)
	{
		AvatarConfig.ReplaceItem(evt.Data.Item.LibraryItem, avatarLibrary);
		AvatarSystem.UpdateAvatar(avatarGO, AvatarConfig);
		if (avatarAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
		{
			switch (evt.Data.Item.LibraryItem.Category)
			{
			case AvatarSystem.ItemCategory.Hair:
				avatarAnimator.SetTrigger("NewHair");
				break;
			case AvatarSystem.ItemCategory.Torso:
				avatarAnimator.SetTrigger("NewShirt");
				AudioController.Play("Avatar_clothes_switchShirt");
				break;
			case AvatarSystem.ItemCategory.Legs:
				avatarAnimator.SetTrigger("NewPants");
				AudioController.Play("Avatar_clothes_switchPants");
				break;
			case AvatarSystem.ItemCategory.Feet:
				avatarAnimator.SetTrigger("NewShoes");
				AudioController.Play("Avatar_clothes_switchShoes");
				break;
			case AvatarSystem.ItemCategory.Eyes:
				break;
			}
		}
	}

	public void RotateCharacter()
	{
		avatarAnimator.SetTrigger("TurnAround");
	}

	protected void RefreshAvatar()
	{
		AvatarSystem.UpdateAvatar(avatarGO, AvatarConfig);
	}
}
