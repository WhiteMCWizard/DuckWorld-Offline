using System.Collections.Generic;
using SLAM.Slinq;
using UnityEngine;

public static class EnumerableExtensions
{
	public static T GetRandom<T>(this IEnumerable<T> self)
	{
		if (self.Count() <= 0)
		{
			return default(T);
		}
		return self.ElementAt(Random.Range(0, self.Count()));
	}

	public static List<T> Shuffle<T>(this List<T> self)
	{
		int num = self.Count;
		while (num > 1)
		{
			num--;
			int index = Random.Range(0, num + 1);
			T value = self.ElementAt(index);
			self[index] = self[num];
			self[num] = value;
		}
		return self;
	}

	public static T[] Shuffle<T>(this T[] self)
	{
		int num = self.Length;
		while (num > 1)
		{
			num--;
			int num2 = Random.Range(0, num + 1);
			T val = self[num2];
			self[num2] = self[num];
			self[num] = val;
		}
		return self;
	}
}
