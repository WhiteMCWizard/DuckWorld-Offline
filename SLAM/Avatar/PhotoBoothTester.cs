using UnityEngine;

namespace SLAM.Avatar;

public class PhotoBoothTester : MonoBehaviour
{
	[SerializeField]
	private UITexture avatarTexture;

	[SerializeField]
	private UITexture mugshotTexture;

	public void OnStartClicked()
	{
		avatarTexture.mainTexture = SingletonMonobehaviour<PhotoBooth>.Instance.StartFilming(PhotoBooth.Pose.Present);
	}

	public void OnStopClicked()
	{
		SingletonMonobehaviour<PhotoBooth>.Instance.StopFilming();
	}

	public void OnRefreshMugshotClicked()
	{
		SingletonMonobehaviour<PhotoBooth>.Instance.SayCheese(AvatarSystem.GetPlayerConfiguration(), delegate(Texture2D photos)
		{
			mugshotTexture.mainTexture = photos;
		});
	}
}
