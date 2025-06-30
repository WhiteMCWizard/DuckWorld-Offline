using SLAM.Webservices;
using UnityEngine;

namespace SLAM.Shared;

public class UIProfilePortrait : MonoBehaviour
{
	[SerializeField]
	private UITexture portrait;

	private void Start()
	{
		portrait.mainTexture = UserProfile.Current.MugShot;
	}
}
