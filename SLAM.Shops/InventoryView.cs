using System;
using SLAM.Engine;
using UnityEngine;

namespace SLAM.Shops;

public class InventoryView : View
{
	[SerializeField]
	private UIPagination categoryPagination;

	[SerializeField]
	private UIPagination variationsPagination;

	protected ShopVariationDefinition currentSelectedVariation;

	protected override void Start()
	{
		base.Start();
		DeselectVariation();
	}

	protected virtual void OnEnable()
	{
		GameEvents.Subscribe<ShopCategoryClickedEvent>(OnCategoryClicked);
		GameEvents.Subscribe<ShopVariationClickedEvent>(OnVariationClicked);
		UIPagination uIPagination = categoryPagination;
		uIPagination.OnItemCreated = (Action<GameObject, object>)Delegate.Combine(uIPagination.OnItemCreated, new Action<GameObject, object>(OnCategoryCreated));
		UIPagination uIPagination2 = variationsPagination;
		uIPagination2.OnItemCreated = (Action<GameObject, object>)Delegate.Combine(uIPagination2.OnItemCreated, new Action<GameObject, object>(OnVariationCreated));
	}

	protected virtual void OnDisable()
	{
		GameEvents.Unsubscribe<ShopCategoryClickedEvent>(OnCategoryClicked);
		GameEvents.Unsubscribe<ShopVariationClickedEvent>(OnVariationClicked);
		UIPagination uIPagination = categoryPagination;
		uIPagination.OnItemCreated = (Action<GameObject, object>)Delegate.Remove(uIPagination.OnItemCreated, new Action<GameObject, object>(OnCategoryCreated));
		UIPagination uIPagination2 = variationsPagination;
		uIPagination2.OnItemCreated = (Action<GameObject, object>)Delegate.Remove(uIPagination2.OnItemCreated, new Action<GameObject, object>(OnVariationCreated));
	}

	public virtual void Load(ShopCategoryDefinition[] allCategories)
	{
		if (allCategories.Length > 0)
		{
			categoryPagination.UpdateInfo(allCategories);
			Select(allCategories[0]);
		}
	}

	public virtual void RefreshVariations()
	{
		variationsPagination.RefreshCurrentPage();
	}

	public virtual void DeselectVariation()
	{
		currentSelectedVariation = null;
	}

	public void OnRotateButtonClicked()
	{
		Controller<InventoryController>().RotateCharacter();
	}

	private void OnCategoryClicked(ShopCategoryClickedEvent evt)
	{
		Select(evt.Data);
	}

	private void OnVariationClicked(ShopVariationClickedEvent evt)
	{
		Select(evt.Data);
	}

	private void OnCategoryCreated(GameObject go, object data)
	{
		go.GetComponent<ShopCategory>().Initialize((ShopCategoryDefinition)data);
	}

	private void OnVariationCreated(GameObject go, object data)
	{
		go.GetComponent<ShopVariation>().Initialize((ShopVariationDefinition)data);
		go.GetComponent<ShopVariation>().SetSelected(Controller<InventoryController>().AvatarConfig.HasItem(((ShopVariationDefinition)data).Item.LibraryItem.GUID));
	}

	private void Select(ShopCategoryDefinition categoryDef)
	{
		GameObject[] itemsOnPage = categoryPagination.ItemsOnPage;
		foreach (GameObject gameObject in itemsOnPage)
		{
			ShopCategory component = gameObject.GetComponent<ShopCategory>();
			if (component != null && categoryDef != null)
			{
				component.SetSelected(categoryDef == component.Data);
			}
		}
		if (categoryDef != null && categoryDef.Items != null && categoryDef.Items.Length > 0)
		{
			variationsPagination.UpdateInfo(categoryDef.Items);
			for (int j = 0; j < categoryDef.Items.Length; j++)
			{
				if (Controller<InventoryController>().AvatarConfig.HasItem(categoryDef.Items[j].Item.LibraryItem.GUID))
				{
					variationsPagination.OpenPageContaining(categoryDef.Items[j]);
				}
			}
		}
		else
		{
			variationsPagination.UpdateInfo(null);
		}
		RefreshVariations();
	}

	protected virtual void Select(ShopVariationDefinition varDef)
	{
		currentSelectedVariation = varDef;
		GameObject[] itemsOnPage = variationsPagination.ItemsOnPage;
		foreach (GameObject gameObject in itemsOnPage)
		{
			ShopVariation component = gameObject.GetComponent<ShopVariation>();
			if (component != null && currentSelectedVariation != null)
			{
				component.SetSelected(currentSelectedVariation.Item.LibraryItem.GUID == component.Data.Item.LibraryItem.GUID);
			}
		}
	}
}
