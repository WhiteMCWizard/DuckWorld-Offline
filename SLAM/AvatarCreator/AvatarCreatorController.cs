using SLAM.Avatar;
using SLAM.BuildSystem;
using SLAM.Engine;
using SLAM.MotionComics._3D;
using SLAM.Slinq;
using SLAM.Webservices;
using UnityEngine;

namespace SLAM.AvatarCreator;

public class AvatarCreatorController : ViewController
{
	private const int MAX_HAIR_COUNT = 6;

	private const int MAX_SKIN_COUNT = 6;

	[SerializeField]
	private RuntimeAnimatorController boyController;

	[SerializeField]
	private RuntimeAnimatorController girlController;

	[SerializeField]
	private Transform avatarRoot;

	[SerializeField]
	private AC_CustomiseAvatarView customiseView;

	[SerializeField]
	private AC_NamePickerView nameView;

	[SerializeField]
	private LoadingView loadingView;

	[SerializeField]
	private AvatarConfigurationData[] configurations;

	private GameObject spawnedAvatar;

	private AvatarConfigurationData configData;

	private AvatarConfigurationData avatarConfig;

	private AvatarItemLibrary avatarItemLibrary;

	public Animator AvatarAnimator => avatarRoot.GetComponentInChildren<Animator>();

	protected void Awake()
	{
		AddView(customiseView);
		AddView(nameView);
		AddView(loadingView);
		avatarConfig = configurations.FirstOrDefault();
		avatarItemLibrary = AvatarItemLibrary.GetItemLibrary(avatarConfig);
		updateAvatarModel(avatarConfig);
		updateCustomiseOptions();
	}

	protected override void Start()
	{
		OpenCustomiseAvatarView();
		if (!SingletonMonoBehaviour<AudioController>.DoesInstanceExist())
		{
			return;
		}
		if (AudioController.GetCategory("Music") != null && AudioController.GetCategory("Music").AudioItems.Length > 0)
		{
			AudioItem[] audioItems = AudioController.GetCategory("Music").AudioItems;
			foreach (AudioItem audioItem in audioItems)
			{
				AudioController.Play(audioItem.Name);
			}
		}
		else
		{
			Debug.LogWarning("Hey buddy, this game doesn't have music? Make sure there is an AudioController with a category 'Music'!");
		}
		if (AudioController.GetCategory("Ambience") != null && AudioController.GetCategory("Ambience").AudioItems.Length > 0)
		{
			AudioItem[] audioItems2 = AudioController.GetCategory("Ambience").AudioItems;
			foreach (AudioItem audioItem2 in audioItems2)
			{
				AudioController.Play(audioItem2.Name);
			}
		}
		else
		{
			Debug.LogWarning("Hey buddy, this game doesn't have ambience sounds? Make sure there is an AudioController with a category 'Ambience'!");
		}
	}

	private void OnEnable()
	{
		GameEvents.Subscribe<HairClickedEvent>(onHairClicked);
		GameEvents.Subscribe<SkinClickedEvent>(onSkinClicked);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<HairClickedEvent>(onHairClicked);
		GameEvents.Unsubscribe<SkinClickedEvent>(onSkinClicked);
	}

	public void Rotate()
	{
		AvatarAnimator.SetTrigger("TurnAround");
	}

	public void OpenCustomiseAvatarView()
	{
		CloseAllViews();
		OpenView<AC_CustomiseAvatarView>();
	}

	public void OpenNamePickerView()
	{
		CloseAllViews();
		OpenView<AC_NamePickerView>().SetInfo(configData.Gender);
	}

	public void AskUserToSave(string playername)
	{
		GameEvents.Invoke(new PopupEvent(Localization.Get("UI_ARE_YOU_SURE"), Localization.Get("AC_LABEL_AREYOUSURE"), Localization.Get("UI_YES"), Localization.Get("UI_NO"), delegate
		{
			saveAndContinue(playername);
		}, null));
	}

	public void UpdateAvatar(AvatarSystem.Race race, AvatarSystem.Gender gender)
	{
		avatarConfig = configurations.FirstOrDefault((AvatarConfigurationData c) => c.Race == race && c.Gender == gender);
		avatarItemLibrary = AvatarItemLibrary.GetItemLibrary(avatarConfig);
		updateAvatarModel(avatarConfig);
		updateCustomiseOptions();
	}

	private void updateAvatarModel(AvatarConfigurationData configData)
	{
		if (this.configData != null && configData.Gender == this.configData.Gender && configData.Race == this.configData.Race)
		{
			this.configData = configData;
			AvatarSystem.UpdateAvatar(spawnedAvatar, this.configData);
		}
		else
		{
			this.configData = configData;
			spawnModel();
		}
		if (configData.Gender == AvatarSystem.Gender.Girl)
		{
			AvatarAnimator.runtimeAnimatorController = girlController;
		}
		else
		{
			AvatarAnimator.runtimeAnimatorController = boyController;
		}
	}

	private void updateCustomiseOptions()
	{
		AvatarItemLibrary.AvatarItem[] hairs = avatarItemLibrary.Items.Where((AvatarItemLibrary.AvatarItem i) => i.Category == AvatarSystem.ItemCategory.Hair && !i.Material.name.ToLower().Contains("helmet")).Take(6).ToArray();
		Color[] skinColors = avatarItemLibrary.SkinColors.Take(6).ToArray();
		GetView<AC_CustomiseAvatarView>().SetInfo(hairs, skinColors);
	}

	private void saveAndContinue(string playername)
	{
		CloseAllViews();
		OpenView<LoadingView>();
		AvatarConfigurationData playerConfig = configData;
		string playerAddress = Localization.Get("AC_STREETNAMES").Split(',').GetRandom();
		SingletonMonobehaviour<PhotoBooth>.Instance.SayCheese(playerConfig, delegate(Texture2D mugshot)
		{
			ApiClient.SavePlayerName(playername, playerAddress, delegate
			{
				AvatarSystem.SavePlayerConfiguration(playerConfig, mugshot);
				ApiClient.GetAllShopItems(delegate(ShopItemData[] allShopItems)
				{
					AvatarConfigurationData defaultItems = (AvatarConfigurationData)AvatarItemLibrary.GetItemLibrary(playerConfig).DefaultConfigurations.First().Clone();
					int[] shopItemIds = (from si in allShopItems
						where playerConfig.Items.Contains(si.GUID) || defaultItems.Items.Contains(si.GUID)
						select si.Id).ToArray();
					ApiClient.AddItemsToInventory(shopItemIds, delegate
					{
						MotionComicPlayer.SetSceneToLoad("Hub");
						SceneManager.Load("MC_ADV00_01_Intro");
					});
				});
			});
		});
	}

	private void spawnModel()
	{
		if (spawnedAvatar != null)
		{
			Object.Destroy(spawnedAvatar);
		}
		spawnedAvatar = AvatarSystem.SpawnAvatar(configData);
		spawnedAvatar.transform.parent = avatarRoot;
		spawnedAvatar.transform.localPosition = Vector3.zero;
		spawnedAvatar.transform.localRotation = Quaternion.identity;
		spawnedAvatar.SetLayerRecursively(LayerMask.NameToLayer("Avatar"));
		Invoke("rebind", 0f);
	}

	private void rebind()
	{
		avatarRoot.GetComponent<Animator>().Rebind();
	}

	private void onSkinClicked(SkinClickedEvent evt)
	{
		avatarConfig.SkinColor = evt.Color;
		updateAvatarModel(avatarConfig);
	}

	private void onHairClicked(HairClickedEvent evt)
	{
		avatarConfig.ReplaceItem(evt.Item, avatarItemLibrary);
		updateAvatarModel(avatarConfig);
	}
}
