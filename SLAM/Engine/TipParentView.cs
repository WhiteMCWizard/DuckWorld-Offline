using System.Text.RegularExpressions;
using SLAM.Webservices;
using UnityEngine;

namespace SLAM.Engine;

public class TipParentView : View
{
	[SerializeField]
	private UIInput emailInput;

	[SerializeField]
	private UIButton sendButton;

	public bool SendButtonEnabled
	{
		get
		{
			return sendButton.isEnabled;
		}
		set
		{
			sendButton.isEnabled = value;
		}
	}

	protected override void Update()
	{
		base.Update();
		SendButtonEnabled = emailInput.value.Length > 0 && isValidEmail(emailInput.value);
	}

	public void OnCloseClicked()
	{
		Close();
	}

	public void OnSendClicked()
	{
		if (base.IsOpen)
		{
			Close();
		}
	}

	private bool isValidEmail(string email)
	{
		Regex regex = new Regex("^([\\w\\.\\-]+)@([\\w\\-]+)((\\.(\\w){2,3})+)$");
		Match match = regex.Match(email);
		return match.Success;
	}
}
