using System.Collections.Generic;
using UnityEngine;

namespace SLAM;

public class Paginator<T>
{
	private List<T> items = new List<T>();

	private int itemsPerPage = 5;

	private int currentIndex;

	public int ItemsInCatalog => items.Count;

	public int ItemCountOnCurrentPage => Mathf.Min(ItemsInCatalog - currentIndex, itemsPerPage);

	public bool HasNextPage => currentIndex + ItemCountOnCurrentPage <= ItemsInCatalog - 1;

	public bool HasPreviousPage => currentIndex > 0;

	public Paginator(T[] items, int countPerPage)
	{
		if (items != null)
		{
			this.items.AddRange(items);
		}
		itemsPerPage = countPerPage;
	}

	public bool NextPage(out T[] items)
	{
		MoveIndex(itemsPerPage);
		items = ItemsOnCurrentPage();
		return currentIndex + ItemCountOnCurrentPage <= ItemsInCatalog - 1;
	}

	public bool PreviousPage(out T[] items)
	{
		MoveIndex(-itemsPerPage);
		items = ItemsOnCurrentPage();
		return currentIndex > 0;
	}

	public T[] ItemsOnCurrentPage()
	{
		return items.GetRange(currentIndex, ItemCountOnCurrentPage).ToArray();
	}

	public void OpenPageContaining(T item)
	{
		int num = items.IndexOf(item) / itemsPerPage * itemsPerPage;
		MoveIndex(num - currentIndex);
	}

	private void MoveIndex(int delta)
	{
		int num = currentIndex + delta;
		if (num >= 0 && num < ItemsInCatalog)
		{
			currentIndex += delta;
			currentIndex = Mathf.Clamp(currentIndex, 0, ItemsInCatalog);
		}
	}
}
