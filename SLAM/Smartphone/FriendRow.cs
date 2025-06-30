using SLAM.SaveSystem;
using SLAM.Webservices;
using UnityEngine;

namespace SLAM.Smartphone;

public class FriendRow : MonoBehaviour
{
	[SerializeField]
	private UILabel nameLabel;

	[SerializeField]
	private UILabel addressLabel;

	[SerializeField]
	private UITexture mugshot;

	private FriendsApp app;

	private UserProfile data;

	private Texture texture;

	public void SetData(UserProfile data, FriendsApp app)
	{
		this.data = data;
		this.app = app;
		nameLabel.text = this.data.Name;
		mugshot.mainTexture = this.data.MugShot;
		addressLabel.text = this.data.Address;
	}

	private void OnClick()
	{
		app.ShowProfile(data);
	}
}
