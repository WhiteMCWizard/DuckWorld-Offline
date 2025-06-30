using SLAM.Webservices;
using UnityEngine;

namespace SLAM.Engine;

public class ChallengeFriendRow : MonoBehaviour
{
	[SerializeField]
	private UILabel lblName;

	[SerializeField]
	private UILabel lblStreet;

	private UserProfile profileData;

	public void SetInfo(UserProfile profileData)
	{
		this.profileData = profileData;
		lblName.text = profileData.Name;
		lblStreet.text = profileData.Address;
	}

	public void OnChallengeClicked()
	{
		Object.FindObjectOfType<GameController>().StartChallengeOtherPlayer(profileData);
	}
}
