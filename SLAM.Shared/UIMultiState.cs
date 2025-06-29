using System;
using UnityEngine;

namespace SLAM.Shared;

public class UIMultiState : MonoBehaviour
{
	[Serializable]
	private class MultiState
	{
		public string Name = "Default";

		public GameObject[] StateObjects;

		public void Set(bool state)
		{
			for (int i = 0; i < StateObjects.Length; i++)
			{
				StateObjects[i].SetActive(state);
			}
		}
	}

	[SerializeField]
	private MultiState[] states;

	[SerializeField]
	private string startState = "Default";

	private bool isEnabled = true;

	public string CurrentState { get; protected set; }

	public bool IsEnabled
	{
		get
		{
			return isEnabled;
		}
		set
		{
			if (isEnabled != value)
			{
				isEnabled = value;
				for (int i = 0; i < states.Length; i++)
				{
					states[i].Set(isEnabled && states[i].Name == CurrentState);
				}
			}
		}
	}

	private void OnEnable()
	{
		if (string.IsNullOrEmpty(CurrentState))
		{
			for (int i = 0; i < states.Length; i++)
			{
				states[i].Set(state: false);
			}
			SwitchTo(startState);
		}
	}

	public void SwitchTo(string newState)
	{
		if (CurrentState != newState)
		{
			GetState(CurrentState)?.Set(state: false);
			GetState(newState)?.Set(state: true);
			CurrentState = newState;
		}
	}

	private MultiState GetState(string name)
	{
		for (int i = 0; i < states.Length; i++)
		{
			if (states[i].Name == name)
			{
				return states[i];
			}
		}
		return null;
	}
}
