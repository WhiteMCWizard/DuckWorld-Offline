using System.Collections.Generic;
using SLAM.Achievements;
using UnityEngine;

namespace SLAM.Smartphone;

public class AchievementsMainView : AppView
{
	[SerializeField]
	private UIGrid grid;

	[SerializeField]
	private GameObject achievementRowPrefab;

	public void SetData(List<UserAchievement> achievements)
	{
		grid.transform.DestroyChildren();
		grid.transform.DetachChildren();
		for (int i = 0; i < achievements.Count; i++)
		{
			UserAchievement userAchievement = achievements[i];
			if (!userAchievement.Info.Hidden || (userAchievement.Info.Hidden && userAchievement.Completed))
			{
				GameObject gameObject = NGUITools.AddChild(grid.gameObject, achievementRowPrefab);
				gameObject.name = userAchievement.Info.Id + 95 + userAchievement.Info.Name;
				gameObject.GetComponent<AchievementRow>().SetData(userAchievement);
			}
		}
		grid.enabled = true;
		grid.Reposition();
	}
}
