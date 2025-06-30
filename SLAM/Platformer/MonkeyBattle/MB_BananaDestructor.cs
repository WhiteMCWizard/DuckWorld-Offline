using UnityEngine;

namespace SLAM.Platformer.MonkeyBattle;

public class MB_BananaDestructor : MonoBehaviour
{
	[SerializeField]
	private bool spawnDecal;

	[SerializeField]
	private GameObject[] splatDecalPrefab;

	private void HitByBanana(MB_Banana banana)
	{
		Object.Destroy(banana.gameObject);
		if (spawnDecal && Physics.Raycast(banana.transform.position, Vector3.down, out var hitInfo, float.MaxValue))
		{
			Vector3 position = hitInfo.point + Vector3.up * 0.01f;
			SingletonMonobehaviour<MB_DecalManager>.Instance.SpawnDecalAt(splatDecalPrefab.GetRandom(), position, Quaternion.Euler(90f, Random.Range(0f, 360f), 0f), new Vector3(0.9f, 0.9f, 0.9f), new Vector3(1.5f, 1.5f, 1.5f));
		}
	}
}
