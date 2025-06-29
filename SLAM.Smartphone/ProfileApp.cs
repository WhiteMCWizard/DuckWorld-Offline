using System;
using System.Collections;
using SLAM.Webservices;
using UnityEngine;

namespace SLAM.Smartphone;

public class ProfileApp : AppController
{
	public override void Open()
	{
		OpenView<ProfileView>().SetData(UserProfile.Current);
	}

	public override void CreateIcon(UISprite icon)
	{
		base.CreateIcon(icon);
		StartCoroutine(waitForProfileAndCreateIcon(icon));
	}

	private IEnumerator waitForProfileAndCreateIcon(UISprite icon)
	{
		while (UserProfile.Current == null || UserProfile.Current.MugShot == null)
		{
			yield return null;
		}
		GameObject g = new GameObject("Mugshot")
		{
			layer = icon.gameObject.layer
		};
		UITexture sprt = g.AddComponent<UITexture>();
		sprt.pivot = UIWidget.Pivot.TopLeft;
		sprt.width = icon.width - 4;
		sprt.height = icon.height - 4;
		sprt.shader = Shader.Find("Unlit - Transparent Colored");
		sprt.mainTexture = UserProfile.Current.MugShot;
		sprt.depth = icon.depth + 1;
		g.transform.parent = icon.transform;
		g.transform.localPosition = new Vector3(4f, -4f, 0f);
		g.transform.localRotation = Quaternion.identity;
		g.transform.localScale = Vector3.one;
	}

	protected override void checkForNotifications(Action<AppChangedEvent> eventCallback)
	{
	}
}
