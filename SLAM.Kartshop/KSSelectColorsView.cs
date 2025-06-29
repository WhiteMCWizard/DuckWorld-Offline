using System;
using SLAM.Engine;
using SLAM.Kart;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.Kartshop;

public class KSSelectColorsView : View
{
	[SerializeField]
	private UIPagination categoriesPagination;

	[SerializeField]
	private UIPagination primaryColorsPagination;

	[SerializeField]
	private UIPagination secondaryColorsPagination;

	private KartConfigurationData currentKartConfig;

	private ColorData[] primaryColors;

	private ColorData[] secondaryColors;

	private void OnEnable()
	{
		UIPagination uIPagination = categoriesPagination;
		uIPagination.OnItemCreated = (Action<GameObject, object>)Delegate.Combine(uIPagination.OnItemCreated, new Action<GameObject, object>(onCategoryCreated));
		UIPagination uIPagination2 = primaryColorsPagination;
		uIPagination2.OnItemCreated = (Action<GameObject, object>)Delegate.Combine(uIPagination2.OnItemCreated, new Action<GameObject, object>(onPrimaryColorPaginationCreated));
		UIPagination uIPagination3 = secondaryColorsPagination;
		uIPagination3.OnItemCreated = (Action<GameObject, object>)Delegate.Combine(uIPagination3.OnItemCreated, new Action<GameObject, object>(onSecondaryColorPaginationCreated));
		GameEvents.Subscribe<KSShopCategoryClickedEvent>(onCategoryClicked);
		GameEvents.Subscribe<KSShopColorItemClickedEvent>(onColorClicked);
	}

	private void OnDisable()
	{
		UIPagination uIPagination = categoriesPagination;
		uIPagination.OnItemCreated = (Action<GameObject, object>)Delegate.Remove(uIPagination.OnItemCreated, new Action<GameObject, object>(onCategoryCreated));
		UIPagination uIPagination2 = primaryColorsPagination;
		uIPagination2.OnItemCreated = (Action<GameObject, object>)Delegate.Remove(uIPagination2.OnItemCreated, new Action<GameObject, object>(onPrimaryColorPaginationCreated));
		UIPagination uIPagination3 = secondaryColorsPagination;
		uIPagination3.OnItemCreated = (Action<GameObject, object>)Delegate.Remove(uIPagination3.OnItemCreated, new Action<GameObject, object>(onSecondaryColorPaginationCreated));
		GameEvents.Unsubscribe<KSShopCategoryClickedEvent>(onCategoryClicked);
		GameEvents.Unsubscribe<KSShopColorItemClickedEvent>(onColorClicked);
	}

	public void SetInfo(Color[] primaryColors, Color[] secondaryColors, KSShopCategoryDefinition[] categories, KartConfigurationData kartConfig, KartSystem.ItemCategory selected)
	{
		currentKartConfig = kartConfig;
		categoriesPagination.UpdateInfo(categories);
		KSShopCategoryDefinition kSShopCategoryDefinition = categories.FirstOrDefault((KSShopCategoryDefinition c) => c.Category == selected);
		if (kSShopCategoryDefinition == null)
		{
			kSShopCategoryDefinition = categories[0];
		}
		categoriesPagination.OpenPageContaining(kSShopCategoryDefinition);
		this.primaryColors = primaryColors.Select((Color s) => new ColorData
		{
			Color = s,
			IsPrimary = true
		}).ToArray();
		this.secondaryColors = secondaryColors.Select((Color s) => new ColorData
		{
			Color = s,
			IsPrimary = false
		}).ToArray();
		primaryColorsPagination.UpdateInfo(this.primaryColors);
		secondaryColorsPagination.UpdateInfo(this.secondaryColors);
		selectCategory(kSShopCategoryDefinition);
	}

	private void onCategoryClicked(KSShopCategoryClickedEvent evt)
	{
		selectCategory(evt.Data);
	}

	private void onColorClicked(KSShopColorItemClickedEvent evt)
	{
		ColorData data = evt.Item.Data;
		Controller<KSShopController>().SetColor(data.IsPrimary, data.Color);
		selectColor(data, (!data.IsPrimary) ? secondaryColorsPagination : primaryColorsPagination);
	}

	private void onPrimaryColorPaginationCreated(GameObject go, object data)
	{
		go.GetComponent<KSShopColorItem>().Initialize((ColorData)data);
	}

	private void onSecondaryColorPaginationCreated(GameObject go, object data)
	{
		ColorData colorData = data as ColorData;
		colorData.IsPrimary = false;
		go.GetComponent<KSShopColorItem>().Initialize(colorData);
	}

	private void onCategoryCreated(GameObject go, object data)
	{
		KSShopCategory component = go.GetComponent<KSShopCategory>();
		component.Initialize((KSShopCategoryDefinition)data);
	}

	private void selectCategory(KSShopCategoryDefinition catDef)
	{
		GameObject[] itemsOnPage = categoriesPagination.ItemsOnPage;
		foreach (GameObject gameObject in itemsOnPage)
		{
			KSShopCategory component = gameObject.GetComponent<KSShopCategory>();
			if (component != null)
			{
				component.SetSelected(catDef == component.Data);
			}
		}
		Color primaryColor = currentKartConfig.GetPrimaryColor(currentKartConfig.GetItem(catDef.Category).GUID);
		Color secondaryColor = currentKartConfig.GetSecondaryColor(currentKartConfig.GetItem(catDef.Category).GUID);
		ColorData data = primaryColors.FirstOrDefault((ColorData c) => c.Color == primaryColor);
		ColorData data2 = secondaryColors.FirstOrDefault((ColorData c) => c.Color == secondaryColor);
		selectColor(data, primaryColorsPagination);
		selectColor(data2, secondaryColorsPagination);
	}

	private void selectColor(ColorData data, UIPagination pag)
	{
		pag.OpenPageContaining(data);
		GameObject[] itemsOnPage = pag.ItemsOnPage;
		foreach (GameObject gameObject in itemsOnPage)
		{
			KSShopColorItem component = gameObject.GetComponent<KSShopColorItem>();
			if (component != null)
			{
				component.SetSelected(data == component.Data);
			}
		}
	}
}
