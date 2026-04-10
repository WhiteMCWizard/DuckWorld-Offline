using System;
using System.Collections.Generic;
using SLAM;
using UnityEngine;

[RequireComponent(typeof(UIGrid))]
public class UIPagination : MonoBehaviour
{
	[SerializeField]
	private int numberOfItemsPerPage = 5;

	[SerializeField]
	private GameObject prefab;

	[SerializeField]
	private UIButton previousArrow;

	[SerializeField]
	private UIButton nextArrow;

	private Paginator<object> paginator;

	private UIGrid grid;

	public Action<GameObject, object> OnItemCreated;

	public Action<UIGrid> OnPreReposition;

	public Action<UIGrid> OnPostReposition;

	public Action OnPageUpdated;

	public GameObject[] ItemsOnPage { get; private set; }

	private void Awake()
	{
		grid = GetComponent<UIGrid>();
		paginator = new Paginator<object>(null, numberOfItemsPerPage);
		UpdateButtonStates();
	}

	public void UpdateInfo(object[] items)
	{
		paginator = new Paginator<object>(items, numberOfItemsPerPage);
		OnPreviousPageClicked();
	}

	public void OnNextPageClicked()
	{
		paginator.NextPage(out var items);
		UpdateButtonStates();
		UpdateGridItems(items);
	}

	public void OnPreviousPageClicked()
	{
		paginator.PreviousPage(out var items);
		UpdateButtonStates();
		UpdateGridItems(items);
	}

	public void RefreshCurrentPage()
	{
		UpdateGridItems(paginator.ItemsOnCurrentPage());
	}

	public void OpenPageContaining(object item)
	{
		paginator.OpenPageContaining(item);
		RefreshCurrentPage();
		UpdateButtonStates();
	}

	private void UpdateGridItems(object[] data)
	{
		if (grid == null)
		{
			grid = GetComponent<UIGrid>();
		}
		for (int num = grid.transform.childCount - 1; num > -1; num--)
		{
			UnityEngine.Object.Destroy(grid.transform.GetChild(num).gameObject);
			grid.transform.GetChild(num).transform.parent = null;
		}
		for (int i = 0; i < data.Length; i++)
		{
			GameObject gameObject = NGUITools.AddChild(base.gameObject, prefab);
			gameObject.name = i + string.Empty;
			if (OnItemCreated != null)
			{
				OnItemCreated(gameObject, data[i]);
			}
		}
		if (OnPreReposition != null)
		{
			OnPreReposition(grid);
		}
		grid.enabled = true;
		grid.Reposition();
		List<GameObject> list = new List<GameObject>();
		for (int j = 0; j < grid.transform.childCount; j++)
		{
			list.Add(grid.transform.GetChild(j).gameObject);
		}
		ItemsOnPage = list.ToArray();
		if (OnPostReposition != null)
		{
			OnPostReposition(grid);
		}
		if (OnPageUpdated != null)
		{
			OnPageUpdated();
		}
	}

	private void UpdateButtonStates()
	{
		if (nextArrow != null)
		{
			nextArrow.gameObject.SetActive(paginator.HasNextPage);
		}
		if (previousArrow != null)
		{
			previousArrow.gameObject.SetActive(paginator.HasPreviousPage);
		}
	}
}
