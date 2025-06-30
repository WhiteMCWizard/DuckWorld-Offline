using SLAM.Avatar;
using SLAM.Engine;
using SLAM.SaveSystem;
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
		var avatarConfig = SaveManager.Instance.GetSaveData().avatar.Config;
		txtrBodyshot.mainTexture = SingletonMonobehaviour<PhotoBooth>.Instance.StartFilming(avatarConfig, PhotoBooth.Pose.Present);

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
