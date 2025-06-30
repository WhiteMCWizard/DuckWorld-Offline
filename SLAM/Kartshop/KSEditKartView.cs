using System;
using SLAM.Engine;
using SLAM.Kart;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.Kartshop;

public class KSEditKartView : View
{
	[SerializeField]
	private UIPagination categoriesPagination;

	[SerializeField]
	private UIPagination itemsPagination;

	private KartConfigurationData currentKartConfig;

	private void OnEnable()
	{
		UIPagination uIPagination = categoriesPagination;
		uIPagination.OnItemCreated = (Action<GameObject, object>)Delegate.Combine(uIPagination.OnItemCreated, new Action<GameObject, object>(onCategoryCreated));
		UIPagination uIPagination2 = itemsPagination;
		uIPagination2.OnItemCreated = (Action<GameObject, object>)Delegate.Combine(uIPagination2.OnItemCreated, new Action<GameObject, object>(onItemCreated));
		GameEvents.Subscribe<KSShopCategoryClickedEvent>(onCategoryClicked);
		GameEvents.Subscribe<KSShopItemClickedEvent>(onShopItemClicked);
	}

	private void OnDisable()
	{
		UIPagination uIPagination = categoriesPagination;
		uIPagination.OnItemCreated = (Action<GameObject, object>)Delegate.Remove(uIPagination.OnItemCreated, new Action<GameObject, object>(onCategoryCreated));
		UIPagination uIPagination2 = itemsPagination;
		uIPagination2.OnItemCreated = (Action<GameObject, object>)Delegate.Remove(uIPagination2.OnItemCreated, new Action<GameObject, object>(onItemCreated));
		GameEvents.Unsubscribe<KSShopCategoryClickedEvent>(onCategoryClicked);
		GameEvents.Unsubscribe<KSShopItemClickedEvent>(onShopItemClicked);
	}

	public void UpdateCategoryParts(KSShopCategoryDefinition[] categories, KartSystem.ItemCategory selected)
	{
		if (categories.Length > 0)
		{
			categoriesPagination.UpdateInfo(categories);
			KSShopCategoryDefinition kSShopCategoryDefinition = categories.FirstOrDefault((KSShopCategoryDefinition c) => c.Category == selected);
			if (kSShopCategoryDefinition == null)
			{
				kSShopCategoryDefinition = categories[0];
			}
			categoriesPagination.OpenPageContaining(kSShopCategoryDefinition);
			selectCategory(kSShopCategoryDefinition);
		}
	}

	public void UpdateSelection(KartConfigurationData kartConfig)
	{
		currentKartConfig = kartConfig;
	}

	private void onCategoryClicked(KSShopCategoryClickedEvent evt)
	{
		selectCategory(evt.Data);
	}

	private void onShopItemClicked(KSShopItemClickedEvent evt)
	{
		selectItem(evt.Data);
	}

	private void onCategoryCreated(GameObject go, object data)
	{
		go.GetComponent<KSShopCategory>().Initialize((KSShopCategoryDefinition)data);
	}

	private void onItemCreated(GameObject go, object data)
	{
		go.GetComponent<KSShopItem>().Initialize((KSShopItemDefinition)data);
	}

	private void selectCategory(KSShopCategoryDefinition categoryDef)
	{
		GameObject[] itemsOnPage = categoriesPagination.ItemsOnPage;
		foreach (GameObject gameObject in itemsOnPage)
		{
			KSShopCategory component = gameObject.GetComponent<KSShopCategory>();
			if (component != null && categoryDef != null)
			{
				component.SetSelected(categoryDef == component.Data);
			}
		}
		itemsPagination.UpdateInfo(categoryDef.Items);
		itemsPagination.OpenPageContaining(categoryDef);
		KSShopItemDefinition itemDef = categoryDef.Items.FirstOrDefault((KSShopItemDefinition itm) => itm.Item.LibraryItem.GUID == currentKartConfig.GetItem(categoryDef.Category).GUID);
		selectItem(itemDef);
	}

	private void selectItem(KSShopItemDefinition itemDef)
	{
		itemsPagination.OpenPageContaining(itemDef);
		GameObject[] itemsOnPage = itemsPagination.ItemsOnPage;
		foreach (GameObject gameObject in itemsOnPage)
		{
			KSShopItem component = gameObject.GetComponent<KSShopItem>();
			component.SetSelected(component.Data == itemDef);
		}
		Controller<KSShopController>().OnSelectPart(itemDef);
	}
}
