using System;
using SLAM.InputSystem;
using UnityEngine;

public class UIDragButton : UIWidgetContainer
{
	[NonSerialized]
	private bool mDragging;

	[SerializeField]
	private KeyControl control;

	private void Update()
	{
		if (!mDragging && UICamera.hoveredObject == base.gameObject)
		{
			bool flag = false;
			for (int i = 0; i < 3; i++)
			{
				if (UICamera.GetMouse(i).pressStarted)
				{
					flag = true;
				}
			}
			for (int j = 0; j < UICamera.activeTouches.Count; j++)
			{
				if (UICamera.GetTouch(j).pressStarted)
				{
					flag = true;
				}
			}
			if (UICamera.controller.pressStarted)
			{
				flag = true;
			}
			if (flag)
			{
				mDragging = true;
				if (control != null)
				{
					control.onButtonPress(base.gameObject, state: true);
				}
			}
		}
		if (!mDragging)
		{
			return;
		}
		bool flag2 = false;
		for (int k = 0; k < 3; k++)
		{
			if (UICamera.Raycast(UICamera.GetMouse(k).pos) && UICamera.hoveredObject == base.gameObject)
			{
				flag2 = true;
			}
		}
		for (int l = 0; l < UICamera.activeTouches.Count; l++)
		{
			if (UICamera.Raycast(UICamera.GetTouch(l).pos) && UICamera.hoveredObject == base.gameObject)
			{
				flag2 = true;
			}
		}
		if (!flag2)
		{
			mDragging = false;
			if (control != null)
			{
				control.onButtonPress(base.gameObject, state: false);
			}
		}
	}
}
