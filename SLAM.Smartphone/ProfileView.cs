using SLAM.Avatar;
using SLAM.Engine;
using SLAM.Webservices;
using UnityEngine;

namespace SLAM.Smartphone;

public class ProfileView : AppView
{
	[SerializeField]
	private UITexture txtrBodyshot;

	[SerializeField]
	private UILabel lblName;

	[SerializeField]
	private UILabel lblAddress;

	public void SetData(UserProfile profile)
	{
		ApiClient.GetAvatarConfiguration(profile.Id, delegate(PlayerAvatarData avtrData)
		{
			txtrBodyshot.mainTexture = SingletonMonobehaviour<PhotoBooth>.Instance.StartFilming(avtrData.Config, PhotoBooth.Pose.Present);
		});
		lblName.text = profile.Name;
		lblAddress.text = profile.Address;
	}

	public override void Close(Callback callback, bool immediately)
	{
		txtrBodyshot.mainTexture = null;
		SingletonMonobehaviour<PhotoBooth>.Instance.StopFilming();
		base.Close(callback, immediately);
	}
}
