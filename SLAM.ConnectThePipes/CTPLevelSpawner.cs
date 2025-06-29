using UnityEngine;

namespace SLAM.ConnectThePipes;

public class CTPLevelSpawner : MonoBehaviour
{
	public GameObject[] SpawnLevelSet(int difficulty)
	{
		int num = difficulty - 1;
		for (int i = 0; i < base.transform.childCount; i++)
		{
			base.transform.GetChild(i).gameObject.SetActive(i == num);
		}
		Transform child = base.transform.GetChild(num);
		GameObject[] array = new GameObject[child.childCount];
		for (int j = 0; j < array.Length; j++)
		{
			array[j] = child.GetChild(j).gameObject;
		}
		return array;
	}
}
