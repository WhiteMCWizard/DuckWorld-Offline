using UnityEngine;

namespace SLAM.InputSystem;

public interface IInputProvider
{
	bool GetKeyDown(KeyCode key);

	bool GetKeyUp(KeyCode key);

	bool GetKey(KeyCode key);

	bool GetButtonDown(SLAMInput.Button button);

	bool GetButtonUp(SLAMInput.Button button);

	bool GetButton(SLAMInput.Button button);

	bool AnyKeyDown();

	bool AnyKey();

	bool GetMouseButton(int buttonId);

	bool GetMouseButtonDown(int buttonId);

	bool GetMouseButtonUp(int buttonId);

	float GetAxis(string axisName);
}
