using UnityEngine;

public static class ArrayHelper
{
	public static T AddArrayElement<T>(ref T[] array) where T : new()
	{
		return AddArrayElement(ref array, new T());
	}

	public static T AddArrayElement<T>(ref T[] array, T elToAdd)
	{
		if (array == null)
		{
			array = new T[1];
			array[0] = elToAdd;
			return elToAdd;
		}
		T[] array2 = new T[array.Length + 1];
		array.CopyTo(array2, 0);
		array2[array.Length] = elToAdd;
		array = array2;
		return elToAdd;
	}

	public static void DeleteArrayElement<T>(ref T[] array, int index)
	{
		if (index >= array.Length || index < 0)
		{
			Debug.LogWarning("invalid index in DeleteArrayElement: " + index);
			return;
		}
		T[] array2 = new T[array.Length - 1];
		for (int i = 0; i < index; i++)
		{
			array2[i] = array[i];
		}
		for (int i = index + 1; i < array.Length; i++)
		{
			array2[i - 1] = array[i];
		}
		array = array2;
	}
}
