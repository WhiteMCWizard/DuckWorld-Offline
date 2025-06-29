using UnityEngine;

namespace SLAM.Engine;

public class HeartsView : View
{
	[SerializeField]
	private UIGrid heartGrid;

	[SerializeField]
	private GameObject heartTogglePrefab;

	private HeartIcon[] hearts;

	private int heartIndex;

	public void SetTotalHeartCount(int heartCount)
	{
		heartGrid.transform.DestroyChildren();
		hearts = new HeartIcon[heartCount];
		heartIndex = heartCount - 1;
		for (int i = 0; i < heartCount; i++)
		{
			GameObject gameObject = NGUITools.AddChild(heartGrid.gameObject, heartTogglePrefab);
			hearts[i] = gameObject.GetComponent<HeartIcon>();
			gameObject.name = "Heart" + i;
		}
		heartGrid.repositionNow = true;
		heartGrid.Reposition();
	}

	public void FoundHeart(Vector3 atWorldPos)
	{
		heartIndex++;
		heartIndex = Mathf.Clamp(heartIndex, 0, hearts.Length - 1);
		HeartIcon heartIcon = hearts[heartIndex];
		heartIcon.PlayShowAnimation(atWorldPos);
	}

	public void LoseHeart()
	{
		HeartIcon heartIcon = hearts[heartIndex];
		heartIndex--;
		heartIndex = Mathf.Clamp(heartIndex, 0, hearts.Length - 1);
		heartIcon.PlayLoseAnimation();
	}
}
