using SLAM.Engine;
using UnityEngine;

namespace SLAM.FirstResponse;

public class LoginController : ViewController
{
	[SerializeField]
	private View[] views;

	private void Awake()
	{
		AddViews(views);
	}

	protected override void Start()
	{
		base.Start();
		OpenView<LoginView>().DemoButtonEnabled = false;
	}

	public void Login(string username, string password)
	{
	}

	public void FreePlay()
	{
	}

	public void TipParentClicked()
	{
		if (!GetView<TipParentView>().IsOpen)
		{
			OpenView<TipParentView>();
		}
	}
}
