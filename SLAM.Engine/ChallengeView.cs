using SLAM.Webservices;
using UnityEngine;

namespace SLAM.Engine;

public class ChallengeView : View
{
	[SerializeField]
	private UIPagination pagination;

	[SerializeField]
	private UILabel titleLabel;

	[SerializeField]
	private UILabel lblChallengeResult;

	public void OnCloseClicked()
	{
		Controller<GameController>().CloseChallenge();
	}

	public void SetData(string level)
	{
		titleLabel.text = StringFormatter.GetLocalizationFormatted("UI_WINDOW_TITLE", level);
	}

	public void SetFriends(UserProfile[] friends)
	{
		pagination.OnItemCreated = delegate(GameObject go, object obj)
		{
			if (go.activeInHierarchy)
			{
				go.GetComponentInChildren<ChallengeFriendRow>().SetInfo(obj as UserProfile);
			}
		};
		pagination.UpdateInfo(friends);
	}

	public void OnFriendChallenged(UserProfile profile)
	{
		string key = "SF_CHALLENGE_SEND";
		lblChallengeResult.text = string.Format(Localization.Get(key), profile.Name, Controller<GameController>().TotalScore);
		ChallengeFriendRow[] componentsInChildren = GetComponentsInChildren<ChallengeFriendRow>();
		ChallengeFriendRow[] array = componentsInChildren;
		foreach (ChallengeFriendRow challengeFriendRow in array)
		{
			challengeFriendRow.GetComponentInChildren<UIButton>().gameObject.SetActive(value: false);
		}
	}
}
