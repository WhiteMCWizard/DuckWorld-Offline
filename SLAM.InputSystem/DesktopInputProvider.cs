using UnityEngine;

namespace SLAM.InputSystem;

public class DesktopInputProvider : IInputProvider
{
	public virtual bool GetButtonDown(SLAMInput.Button button)
	{
		if (button == SLAMInput.Button.UpOrAction)
		{
			return GetButtonDown(SLAMInput.Button.Action) || GetButtonDown(SLAMInput.Button.Up);
		}
		return Input.GetButtonDown(button.ToString());
	}

	public virtual bool GetButtonUp(SLAMInput.Button button)
	{
		if (button == SLAMInput.Button.UpOrAction)
		{
			return GetButtonUp(SLAMInput.Button.Action) || GetButtonUp(SLAMInput.Button.Up);
		}
		return Input.GetButtonUp(button.ToString());
	}

	public virtual bool GetButton(SLAMInput.Button button)
	{
		if (button == SLAMInput.Button.UpOrAction)
		{
			return GetButton(SLAMInput.Button.Action) || GetButton(SLAMInput.Button.Up);
		}
		return Input.GetButton(button.ToString());
	}

	public virtual bool GetKeyDown(KeyCode key)
	{
		return Input.GetKeyDown(key);
	}

	public virtual bool GetKeyUp(KeyCode key)
	{
		return Input.GetKeyUp(key);
	}

	public virtual bool GetKey(KeyCode key)
	{
		return Input.GetKey(key);
	}

	public virtual bool AnyKeyDown()
	{
		return Input.anyKeyDown;
	}

	public virtual bool AnyKey()
	{
		return Input.anyKey;
	}

	public virtual bool GetMouseButton(int buttonId)
	{
		return Input.GetMouseButton(buttonId);
	}

	public virtual bool GetMouseButtonDown(int buttonId)
	{
		return Input.GetMouseButtonDown(buttonId);
	}

	public virtual bool GetMouseButtonUp(int buttonId)
	{
		return Input.GetMouseButtonUp(buttonId);
	}

	public virtual float GetAxis(string axisName)
	{
		return Input.GetAxis(axisName);
	}
}
