using System;
using UnityEngine;

namespace SLAM.InputSystem;

public class KeyControl : Control
{
	[SerializeField]
	private KeyCode keyToEmulate;

	[SerializeField]
	private UIWidget widget;

	public KeyCode KeyToEmulate => keyToEmulate;

	public UIWidget Widget => widget;

	private void OnEnable()
	{
		if (widget != null)
		{
			UIEventListener uIEventListener = UIEventListener.Get(Widget.gameObject);
			uIEventListener.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(uIEventListener.onPress, new UIEventListener.BoolDelegate(onButtonPress));
		}
	}

	private void OnDisable()
	{
		if (widget != null)
		{
			UIEventListener uIEventListener = UIEventListener.Get(Widget.gameObject);
			uIEventListener.onPress = (UIEventListener.BoolDelegate)Delegate.Remove(uIEventListener.onPress, new UIEventListener.BoolDelegate(onButtonPress));
		}
	}

	public void onButtonPress(GameObject go, bool state)
	{
		if (state)
		{
			(SLAMInput.Provider as MobileInputProvider).SetKeyDown(keyToEmulate);
		}
		else
		{
			(SLAMInput.Provider as MobileInputProvider).SetKeyUp(keyToEmulate);
		}
	}
}
