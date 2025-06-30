using SLAM.Avatar;
using SLAM.Engine;
using UnityEngine;

namespace SLAM.AvatarCreator;

public class AC_CustomiseAvatarView : View
{
	[SerializeField]
	private UIGrid hairGrid;

	[SerializeField]
	private UIGrid skinGrid;

	[SerializeField]
	private GameObject hairRowPrefab;

	[SerializeField]
	private GameObject skinRowPrefab;

	public void SetInfo(AvatarItemLibrary.AvatarItem[] hairs, Color[] skinColors)
	{
		hairGrid.transform.DestroyChildren();
		hairGrid.transform.DetachChildren();
		for (int i = 0; i < hairs.Length; i++)
		{
			GameObject gameObject = NGUITools.AddChild(hairGrid.gameObject, hairRowPrefab);
			gameObject.GetComponent<AC_Hair>().SetInfo(hairs[i], i == 0);
		}
		hairGrid.enabled = true;
		hairGrid.Reposition();
		skinGrid.transform.DestroyChildren();
		skinGrid.transform.DetachChildren();
		for (int j = 0; j < skinColors.Length; j++)
		{
			GameObject gameObject2 = NGUITools.AddChild(skinGrid.gameObject, skinRowPrefab);
			gameObject2.GetComponent<AC_Skin>().SetInfo(skinColors[j], j == 0);
		}
		skinGrid.enabled = true;
		skinGrid.Reposition();
	}

	public void OnNextClicked()
	{
		Controller<AvatarCreatorController>().OpenNamePickerView();
	}

	public void OnRotateClicked()
	{
		Controller<AvatarCreatorController>().Rotate();
	}

	public void OnDogBoyClicked()
	{
		Controller<AvatarCreatorController>().UpdateAvatar(AvatarSystem.Race.Dog, AvatarSystem.Gender.Boy);
	}

	public void OnDogGirlClicked()
	{
		Controller<AvatarCreatorController>().UpdateAvatar(AvatarSystem.Race.Dog, AvatarSystem.Gender.Girl);
	}

	public void OnCatBoyClicked()
	{
		Controller<AvatarCreatorController>().UpdateAvatar(AvatarSystem.Race.Cat, AvatarSystem.Gender.Boy);
	}

	public void OnCatGirlClicked()
	{
		Controller<AvatarCreatorController>().UpdateAvatar(AvatarSystem.Race.Cat, AvatarSystem.Gender.Girl);
	}

	public void OnDuckBoyClicked()
	{
		Controller<AvatarCreatorController>().UpdateAvatar(AvatarSystem.Race.Duck, AvatarSystem.Gender.Boy);
	}

	public void OnDuckGirlClicked()
	{
		Controller<AvatarCreatorController>().UpdateAvatar(AvatarSystem.Race.Duck, AvatarSystem.Gender.Girl);
	}

	private void onSkinColorChange()
	{
		if (UIToggle.current.value)
		{
		}
	}

	private void onHairColorChange()
	{
	}
}
