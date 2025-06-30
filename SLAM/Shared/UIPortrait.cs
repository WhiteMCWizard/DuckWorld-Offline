using SLAM.Avatar;
using SLAM.Webservices;
using UnityEngine;

namespace SLAM.Shared;

public class UIPortrait : MonoBehaviour
{
	[SerializeField]
	private UITexture portraitTexture;

	private GameObject avatarPortrait;

	private Material portraitMaterial;

	private void Start()
	{
	}

	public void SetCharacter(Game.GameCharacter gameChar)
	{
		Texture2D mainTexture = ((gameChar != Game.GameCharacter.NPC_AVATAR_NAME) ? SingletonMonobehaviour<PhotoBooth>.Instance.GetMugshotFor(gameChar) : UserProfile.Current.MugShot);
		if (portraitMaterial == null)
		{
			portraitMaterial = Object.Instantiate(portraitTexture.material);
			portraitTexture.material = portraitMaterial;
		}
		portraitMaterial.mainTexture = mainTexture;
	}
}
