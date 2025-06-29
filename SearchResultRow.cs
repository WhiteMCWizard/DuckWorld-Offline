using SLAM;
using SLAM.Smartphone;
using SLAM.Webservices;
using UnityEngine;

public class SearchResultRow : MonoBehaviour
{
	[SerializeField]
	private UILabel nameLabel;

	[SerializeField]
	private UILabel streetLabel;

	[SerializeField]
	private UIButton inviteButton;

	[SerializeField]
	private UITexture mugshot;

	private UserProfile data;

	private Texture texture;

	public void SetData(UserProfile data)
	{
		this.data = data;
		nameLabel.text = data.FirstName + " " + data.LastName;
		streetLabel.text = data.Address;
		mugshot.mainTexture = data.MugShot;
	}

	public void OnInviteClicked()
	{
		inviteButton.isEnabled = false;
		InviteUserEvent inviteUserEvent = new InviteUserEvent();
		inviteUserEvent.User = data;
		GameEvents.Invoke(inviteUserEvent);
	}
}
