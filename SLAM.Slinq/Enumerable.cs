using System;
using System.Collections;
using System.Collections.Generic;

namespace SLAM.Slinq;

internal static class Enumerable
{
	public interface IOrderedEnumerable<TElement> : IEnumerable, IEnumerable<TElement>
	{
		IOrderedEnumerable<TElement> CreateOrderedEnumerable<TKey>(Func<TElement, TKey> keySelector, IComparer<TKey> comparer, bool descending);
	}

	internal abstract class OrderedEnumerable<TElement> : IEnumerable, IOrderedEnumerable<TElement>, IEnumerable<TElement>
	{
		internal IEnumerable<TElement> source;

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		IOrderedEnumerable<TElement> IOrderedEnumerable<TElement>.CreateOrderedEnumerable<TKey>(Func<TElement, TKey> keySelector, IComparer<TKey> comparer, bool descending)
		{
			OrderedEnumerable<TElement, TKey> orderedEnumerable = new OrderedEnumerable<TElement, TKey>(source, keySelector, comparer, descending);
			orderedEnumerable.parent = this;
			return orderedEnumerable;
		}

		public IEnumerator<TElement> GetEnumerator()
		{
			Buffer<TElement> buffer = new Buffer<TElement>(source);
			if (buffer.count > 0)
			{
				EnumerableSorter<TElement> sorter = GetEnumerableSorter(null);
				int[] map = sorter.Sort(buffer.items, buffer.count);
				sorter = null;
				for (int i = 0; i < buffer.count; i++)
				{
					yield return buffer.items[map[i]];
				}
			}
		}

		internal abstract EnumerableSorter<TElement> GetEnumerableSorter(EnumerableSorter<TElement> next);
	}

	internal class OrderedEnumerable<TElement, TKey> : OrderedEnumerable<TElement>
	{
		internal OrderedEnumerable<TElement> parent;

		internal Func<TElement, TKey> keySelector;

		internal IComparer<TKey> comparer;

		internal bool descending;

		internal OrderedEnumerable(IEnumerable<TElement> source, Func<TElement, TKey> keySelector, IComparer<TKey> comparer, bool descending)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (keySelector == null)
			{
				throw new ArgumentNullException("keySelector");
			}
			base.source = source;
			parent = null;
			this.keySelector = keySelector;
			object obj;
			if (comparer != null)
			{
				obj = comparer;
			}
			else
			{
				obj = Comparer<TKey>.Default;
			}
			this.comparer = (IComparer<TKey>)obj;
			this.descending = descending;
		}

		internal override EnumerableSorter<TElement> GetEnumerableSorter(EnumerableSorter<TElement> next)
		{
			EnumerableSorter<TElement> enumerableSorter = new EnumerableSorter<TElement, TKey>(keySelector, comparer, descending, next);
			if (parent != null)
			{
				enumerableSorter = parent.GetEnumerableSorter(enumerableSorter);
			}
			return enumerableSorter;
		}
	}

	internal abstract class EnumerableSorter<TElement>
	{
		internal abstract void ComputeKeys(TElement[] elements, int count);

		internal abstract int CompareKeys(int index1, int index2);

		internal int[] Sort(TElement[] elements, int count)
		{
			ComputeKeys(elements, count);
			int[] array = new int[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = i;
			}
			QuickSort(array, 0, count - 1);
			return array;
		}

		private void QuickSort(int[] map, int left, int right)
		{
			do
			{
				int num = left;
				int num2 = right;
				int index = map[num + (num2 - num >> 1)];
				while (true)
				{
					if (num < map.Length && CompareKeys(index, map[num]) > 0)
					{
						num++;
						continue;
					}
					while (num2 >= 0 && CompareKeys(index, map[num2]) < 0)
					{
						num2--;
					}
					if (num > num2)
					{
						break;
					}
					if (num < num2)
					{
						int num3 = map[num];
						map[num] = map[num2];
						map[num2] = num3;
					}
					num++;
					num2--;
					if (num > num2)
					{
						break;
					}
				}
				if (num2 - left <= right - num)
				{
					if (left < num2)
					{
						QuickSort(map, left, num2);
					}
					left = num;
				}
				else
				{
					if (num < right)
					{
						QuickSort(map, num, right);
					}
					right = num2;
				}
			}
			while (left < right);
		}
	}

	internal class EnumerableSorter<TElement, TKey> : EnumerableSorter<TElement>
	{
		internal Func<TElement, TKey> keySelector;

		internal IComparer<TKey> comparer;

		internal bool descending;

		internal EnumerableSorter<TElement> next;

		internal TKey[] keys;

		internal EnumerableSorter(Func<TElement, TKey> keySelector, IComparer<TKey> comparer, bool descending, EnumerableSorter<TElement> next)
		{
			this.keySelector = keySelector;
			this.comparer = comparer;
			this.descending = descending;
			this.next = next;
		}

		internal override void ComputeKeys(TElement[] elements, int count)
		{
			keys = new TKey[count];
			for (int i = 0; i < count; i++)
			{
				keys[i] = keySelector(elements[i]);
			}
			if (next != null)
			{
				next.ComputeKeys(elements, count);
			}
		}

		internal override int CompareKeys(int index1, int index2)
		{
			int num = comparer.Compare(keys[index1], keys[index2]);
			if (num == 0)
			{
				if (next == null)
				{
					return index1 - index2;
				}
				return next.CompareKeys(index1, index2);
			}
			return (!descending) ? num : (-num);
		}
	}

	private struct Buffer<TElement>
	{
		internal TElement[] items;

		internal int count;

		internal Buffer(IEnumerable<TElement> source)
		{
			TElement[] array = null;
			int num = 0;
			if (source is ICollection<TElement> collection)
			{
				num = collection.Count;
				if (num > 0)
				{
					array = new TElement[num];
					collection.CopyTo(array, 0);
				}
			}
			else
			{
				foreach (TElement item in source)
				{
					if (array == null)
					{
						array = new TElement[4];
					}
					else if (array.Length == num)
					{
						TElement[] array2 = new TElement[checked(num * 2)];
						Array.Copy(array, 0, array2, 0, num);
						array = array2;
					}
					array[num] = item;
					num++;
				}
			}
			items = array;
			count = num;
		}

		internal TElement[] ToArray()
		{
			if (count == 0)
			{
				return new TElement[0];
			}
			if (items.Length == count)
			{
				return items;
			}
			TElement[] array = new TElement[count];
			Array.Copy(items, 0, array, 0, count);
			return array;
		}
	}

	public static List<T> ToList<T>(this IEnumerable<T> collection)
	{
		return new List<T>(collection);
	}

	public static T[] ToArray<T>(this IEnumerable<T> collection)
	{
		return collection.ToList().ToArray();
	}

	public static bool Contains<T>(this IEnumerable<T> collection, T obj)
	{
		if (collection == null)
		{
			throw new ArgumentNullException("source");
		}
		return collection.Where((T otherObj) => otherObj.Equals(obj)).GetEnumerator().MoveNext();
	}

	public static bool Contains<T>(this T[] array, T obj)
	{
		if (array == null)
		{
			throw new ArgumentNullException("source");
		}
		for (int i = 0; i < array.Length; i++)
		{
			T val = array[i];
			if (val.Equals(obj))
			{
				return true;
			}
		}
		return false;
	}

	public static bool Any<T>(this IEnumerable<T> collection, Func<T, bool> compare)
	{
		if (collection == null)
		{
			throw new ArgumentNullException("source");
		}
		foreach (T item in collection)
		{
			if (compare(item))
			{
				return true;
			}
		}
		return false;
	}

	public static bool All<T>(this IEnumerable<T> collection, Func<T, bool> compare)
	{
		if (collection == null)
		{
			throw new ArgumentNullException("source");
		}
		foreach (T item in collection)
		{
			if (!compare(item))
			{
				return false;
			}
		}
		return true;
	}

	public static int Count<T>(this IEnumerable<T> collection)
	{
		if (collection == null)
		{
			throw new ArgumentNullException("source");
		}
		IEnumerator<T> enumerator = collection.GetEnumerator();
		int num = 0;
		while (enumerator.MoveNext())
		{
			num++;
		}
		return num;
	}

	public static int Count<T>(this IEnumerable<T> collection, Func<T, bool> predicate)
	{
		if (collection == null)
		{
			throw new ArgumentNullException("source");
		}
		IEnumerator<T> enumerator = collection.GetEnumerator();
		int num = 0;
		while (enumerator.MoveNext())
		{
			if (predicate(enumerator.Current))
			{
				num++;
			}
		}
		return num;
	}

	public static int Sum(this IEnumerable<int> collection)
	{
		if (collection == null)
		{
			throw new ArgumentNullException("source");
		}
		int num = 0;
		foreach (int item in collection)
		{
			num += item;
		}
		return num;
	}

	public static int Sum<T>(this IEnumerable<T> collection, Func<T, int> selector)
	{
		return collection.Select(selector).Sum();
	}

	public static float Sum(this IEnumerable<float> collection)
	{
		if (collection == null)
		{
			throw new ArgumentNullException("source");
		}
		float num = 0f;
		foreach (float item in collection)
		{
			float num2 = item;
			num += num2;
		}
		return num;
	}

	public static float Sum<T>(this IEnumerable<T> collection, Func<T, float> selector)
	{
		if (collection == null)
		{
			throw new ArgumentNullException("source");
		}
		float num = 0f;
		foreach (T item in collection)
		{
			num += selector(item);
		}
		return num;
	}

	public static int Min(this IEnumerable<int> collection)
	{
		if (collection == null)
		{
			throw new ArgumentNullException("source");
		}
		int num = int.MaxValue;
		foreach (int item in collection)
		{
			if (item < num)
			{
				num = item;
			}
		}
		return num;
	}

	public static int Min<T>(this IEnumerable<T> collection, Func<T, int> selector)
	{
		return collection.Select(selector).Min();
	}

	public static T Min<T>(this IEnumerable<T> collection, Func<T, float> selector)
	{
		float num = float.MaxValue;
		T result = default(T);
		foreach (T item in collection)
		{
			float num2 = selector(item);
			if (num2 < num)
			{
				num = num2;
				result = item;
			}
		}
		return result;
	}

	public static int Max(this IEnumerable<int> collection)
	{
		if (collection == null)
		{
			throw new ArgumentNullException("source");
		}
		int num = int.MinValue;
		foreach (int item in collection)
		{
			if (item > num)
			{
				num = item;
			}
		}
		return num;
	}

	public static int Max<T>(this IEnumerable<T> collection, Func<T, int> selector)
	{
		int num = int.MinValue;
		foreach (T item in collection)
		{
			int num2 = selector(item);
			if (num2 > num)
			{
				num = num2;
			}
		}
		return num;
	}

	public static T Max<T>(this IEnumerable<T> collection, Func<T, float> selector)
	{
		float num = float.MinValue;
		T result = default(T);
		foreach (T item in collection)
		{
			float num2 = selector(item);
			if (num2 > num)
			{
				num = num2;
				result = item;
			}
		}
		return result;
	}

	public static IEnumerable<U> Select<T, U>(this IEnumerable<T> collection, Func<T, U> selector)
	{
		if (collection == null)
		{
			throw new ArgumentNullException("source");
		}
		return SelectIterator(collection, selector);
	}

	private static IEnumerable<U> SelectIterator<T, U>(IEnumerable<T> collection, Func<T, U> selector)
	{
		foreach (T item in collection)
		{
			yield return selector(item);
		}
	}

	public static IEnumerable<TResult> SelectMany<T, TResult>(this IEnumerable<T> source, Func<T, IEnumerable<TResult>> selector)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		return SelectManyIterator(source, selector);
	}

	private static IEnumerable<TResult> SelectManyIterator<T, TResult>(IEnumerable<T> source, Func<T, IEnumerable<TResult>> selector)
	{
		foreach (T element in source)
		{
			foreach (TResult item in selector(element))
			{
				yield return item;
			}
		}
	}

	public static IEnumerable<T> Take<T>(this IEnumerable<T> source, int count)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		return TakeIterator(source, count);
	}

	private static IEnumerable<T> TakeIterator<T>(IEnumerable<T> source, int count)
	{
		if (count <= 0)
		{
			yield break;
		}
		foreach (T item in source)
		{
			yield return item;
			int num;
			count = (num = count - 1);
			if (num == 0)
			{
				break;
			}
		}
	}

	public static IEnumerable<T> TakeWhere<T>(this IEnumerable<T> source, Func<T, bool> compare)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		return TakeWhereIterator(source, compare);
	}

	private static IEnumerable<T> TakeWhereIterator<T>(IEnumerable<T> source, Func<T, bool> compare)
	{
		foreach (T element in source)
		{
			if (compare(element))
			{
				yield return element;
			}
		}
	}

	public static IEnumerable<T> Skip<T>(this IEnumerable<T> source, int count)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		return SkipIterator(source, count);
	}

	private static IEnumerable<T> SkipIterator<T>(IEnumerable<T> source, int count)
	{
		foreach (T item in source)
		{
			if (count > 0)
			{
				count--;
			}
			else
			{
				yield return item;
			}
		}
	}

	public static T ElementAt<T>(this IEnumerable<T> collection, int index)
	{
		if (collection == null)
		{
			throw new ArgumentNullException("source");
		}
		if (index < 0)
		{
			throw new IndexOutOfRangeException("index");
		}
		using IEnumerator<T> enumerator = collection.GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (index == 0)
			{
				return enumerator.Current;
			}
			index--;
		}
		throw new IndexOutOfRangeException("index");
	}

	public static IEnumerable<T> Join<T>(this IEnumerable<T> collection, params IEnumerable<T>[] others)
	{
		foreach (T item in collection)
		{
			yield return item;
		}
		foreach (IEnumerable<T> col in others)
		{
			foreach (T item2 in col)
			{
				yield return item2;
			}
		}
	}

	public static T First<T>(this IEnumerable<T> collection)
	{
		if (collection == null)
		{
			throw new ArgumentNullException("collection");
		}
		using (IEnumerator<T> enumerator = collection.GetEnumerator())
		{
			if (enumerator.MoveNext())
			{
				return enumerator.Current;
			}
		}
		throw new InvalidOperationException();
	}

	public static T First<T>(this IEnumerable<T> collection, Func<T, bool> compare)
	{
		if (collection == null)
		{
			throw new ArgumentNullException("collection");
		}
		foreach (T item in collection)
		{
			if (compare(item))
			{
				return item;
			}
		}
		throw new InvalidOperationException();
	}

	public static T Last<T>(this IEnumerable<T> collection)
	{
		T result = default(T);
		foreach (T item in collection)
		{
			result = item;
		}
		return result;
	}

	public static T LastOrDefault<T>(this IEnumerable<T> collection, Func<T, bool> compare)
	{
		if (collection == null)
		{
			throw new ArgumentNullException("collection");
		}
		if (compare == null)
		{
			throw new ArgumentNullException("compare");
		}
		T result = default(T);
		foreach (T item in collection)
		{
			if (compare(item))
			{
				result = item;
			}
		}
		return result;
	}

	public static T FirstOrDefault<T>(this IEnumerable<T> collection)
	{
		if (collection == null)
		{
			throw new ArgumentNullException("collection");
		}
		using (IEnumerator<T> enumerator = collection.GetEnumerator())
		{
			if (enumerator.MoveNext())
			{
				return enumerator.Current;
			}
		}
		return default(T);
	}

	public static T FirstOrDefault<T>(this IEnumerable<T> collection, Func<T, bool> compare)
	{
		foreach (T item in collection)
		{
			if (compare(item))
			{
				return item;
			}
		}
		return default(T);
	}

	public static IEnumerable<T> OfType<T>(this IEnumerable collection)
	{
		foreach (object item in collection)
		{
			if (item is T)
			{
				yield return (T)item;
			}
		}
	}

	public static IEnumerable<T> Where<T>(this IEnumerable<T> collection, Func<T, bool> compare)
	{
		return WhereIterator(collection, compare);
	}

	private static IEnumerable<T> WhereIterator<T>(IEnumerable<T> collection, Func<T, bool> compare)
	{
		foreach (T item in collection)
		{
			if (compare(item))
			{
				yield return item;
			}
		}
	}

	public static IOrderedEnumerable<T> OrderBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector)
	{
		return new OrderedEnumerable<T, TKey>(source, keySelector, null, descending: false);
	}

	public static IOrderedEnumerable<T> OrderBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector, IComparer<TKey> comparer)
	{
		return new OrderedEnumerable<T, TKey>(source, keySelector, comparer, descending: false);
	}

	public static IOrderedEnumerable<T> OrderByDescending<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector)
	{
		return new OrderedEnumerable<T, TKey>(source, keySelector, null, descending: true);
	}

	public static IOrderedEnumerable<T> OrderByDescending<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector, IComparer<TKey> comparer)
	{
		return new OrderedEnumerable<T, TKey>(source, keySelector, comparer, descending: true);
	}
}
