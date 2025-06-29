using SLAM.Avatar;
using SLAM.Engine;
using UnityEngine;

namespace SLAM.AvatarCreator;

public class AC_NamePickerView : View
{
	[SerializeField]
	private ACNameSpinnerWidget spinnerFirstName;

	[SerializeField]
	private ACNameSpinnerWidget spinnerLastname1;

	[SerializeField]
	private ACNameSpinnerWidget spinnerLastname2;

	public void SetInfo(AvatarSystem.Gender gender)
	{
		switch (gender)
		{
		case AvatarSystem.Gender.Boy:
			spinnerFirstName.TranslationKey = "AC_FIRSTNAMES_BOY";
			break;
		case AvatarSystem.Gender.Girl:
			spinnerFirstName.TranslationKey = "AC_FIRSTNAMES_GIRL";
			break;
		}
		StartCoroutine(spinnerFirstName.UpdateNames());
		StartCoroutine(spinnerLastname1.UpdateNames());
		StartCoroutine(spinnerLastname2.UpdateNames());
	}

	public void OnBackClicked()
	{
		Controller<AvatarCreatorController>().OpenCustomiseAvatarView();
	}

	public void OnDoneClicked()
	{
		string playername = $"{spinnerFirstName.SelectedName} {spinnerLastname1.SelectedName}{spinnerLastname2.SelectedName}".Trim();
		Controller<AvatarCreatorController>().AskUserToSave(playername);
	}
}
