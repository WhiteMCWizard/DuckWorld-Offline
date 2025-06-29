using System.Collections;
using System.Collections.Generic;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.InputSystem;

public class MobileInputProvider : MonoBehaviour, IInputProvider
{
	private List<KeyCode> lastKeyState = new List<KeyCode>();

	private List<KeyCode> currentKeyState = new List<KeyCode>();

	private List<SLAMInput.Button> lastButtonState = new List<SLAMInput.Button>();

	private List<SLAMInput.Button> currentButtonState = new List<SLAMInput.Button>();

	private Dictionary<string, float> axisData = new Dictionary<string, float>();

	public static MobileInputProvider Create()
	{
		GameObject gameObject = new GameObject("MobileInputProvider");
		Object.DontDestroyOnLoad(gameObject);
		return gameObject.AddComponent<MobileInputProvider>();
	}

	private void LateUpdate()
	{
		lastKeyState = currentKeyState.ToList();
		lastButtonState = currentButtonState.ToList();
	}

	public void SetKeyDown(KeyCode key)
	{
		StartCoroutine(setKeyDownAtEndOfFrame(key));
	}

	public void SetKeyUp(KeyCode key)
	{
		StartCoroutine(setKeyUpAtEndOfFrame(key));
	}

	private IEnumerator setKeyDownAtEndOfFrame(KeyCode key)
	{
		yield return new WaitForEndOfFrame();
		currentKeyState.Add(key);
	}

	private IEnumerator setKeyUpAtEndOfFrame(KeyCode key)
	{
		yield return new WaitForEndOfFrame();
		currentKeyState.Remove(key);
	}

	public void SetButtonDown(SLAMInput.Button button)
	{
		StartCoroutine(setButtonDownAtEndOfFrame(button));
	}

	public void SetButtonUp(SLAMInput.Button button)
	{
		StartCoroutine(setButtonUpAtEndOfFrame(button));
	}

	private IEnumerator setButtonDownAtEndOfFrame(SLAMInput.Button button)
	{
		yield return new WaitForEndOfFrame();
		currentButtonState.Add(button);
	}

	private IEnumerator setButtonUpAtEndOfFrame(SLAMInput.Button button)
	{
		yield return new WaitForEndOfFrame();
		currentButtonState.Remove(button);
	}

	public void SetAxis(string axisName, float value)
	{
		if (axisData.ContainsKey(axisName))
		{
			axisData[axisName] = value;
		}
		else
		{
			axisData.Add(axisName, value);
		}
	}

	public bool GetButtonDown(SLAMInput.Button button)
	{
		return currentButtonState.Contains(button) && !lastButtonState.Contains(button);
	}

	public bool GetButtonUp(SLAMInput.Button button)
	{
		return !currentButtonState.Contains(button) && lastButtonState.Contains(button);
	}

	public bool GetButton(SLAMInput.Button button)
	{
		return currentButtonState.Contains(button);
	}

	public bool GetKeyDown(KeyCode key)
	{
		return currentKeyState.Contains(key) && !lastKeyState.Contains(key);
	}

	public bool GetKeyUp(KeyCode key)
	{
		return !currentKeyState.Contains(key) && lastKeyState.Contains(key);
	}

	public bool GetKey(KeyCode key)
	{
		return currentKeyState.Contains(key);
	}

	public bool AnyKeyDown()
	{
		return currentKeyState.Count > 0 && lastKeyState.Count <= 0;
	}

	public bool AnyKey()
	{
		return currentKeyState.Count > 0;
	}

	public bool GetMouseButton(int buttonId)
	{
		return Input.GetMouseButton(buttonId);
	}

	public bool GetMouseButtonDown(int buttonId)
	{
		return Input.GetMouseButtonDown(buttonId);
	}

	public bool GetMouseButtonUp(int buttonId)
	{
		return Input.GetMouseButtonUp(buttonId);
	}

	public float GetAxis(string axisName)
	{
		return (!axisData.ContainsKey(axisName)) ? 0f : axisData[axisName];
	}
}
