using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SLAM.Engine;

public class FiniteStateMachine : MonoBehaviour
{
	public class State
	{
		private string name = string.Empty;

		private Action enter;

		private Action update;

		private Action leave;

		public string Name => name;

		public State(string name, Action enterCallback, Action updateCallback, Action leaveCallback)
		{
			this.name = name;
			enter = enterCallback;
			update = updateCallback;
			leave = leaveCallback;
		}

		public void Enter()
		{
			if (enter != null)
			{
				enter();
			}
		}

		public void Update()
		{
			if (update != null)
			{
				update();
			}
		}

		public void Leave()
		{
			if (leave != null)
			{
				leave();
			}
		}
	}

	private List<State> _states = new List<State>();

	private State _currentState;

	public List<State> States
	{
		get
		{
			return _states;
		}
		protected set
		{
			_states = value;
		}
	}

	public State CurrentState
	{
		get
		{
			return _currentState;
		}
		protected set
		{
			_currentState = value;
		}
	}

	public virtual void AddState(string name, Action enterCallback, Action updateCallback, Action leaveCallback)
	{
		if (!hasState(name))
		{
			States.Add(new State(name, enterCallback, updateCallback, leaveCallback));
		}
		else
		{
			Debug.LogError("Cannot add state: " + name + " as it already exists. Please use unique state names.");
		}
	}

	public virtual void SwitchTo(string stateName)
	{
		if (!hasState(stateName))
		{
			Debug.LogError("Cannot find state: " + stateName + ((CurrentState == null) ? string.Empty : ("Aborting switch and remaining in current state: " + CurrentState.Name)));
			return;
		}
		for (int i = 0; i < States.Count; i++)
		{
			State state = States[i];
			if (state.Name == stateName)
			{
				if (CurrentState != null)
				{
					CurrentState.Leave();
				}
				CurrentState = state;
				CurrentState.Enter();
			}
		}
	}

	public virtual void SwitchTo(string stateName, float inSeconds, bool stopOtherTimedSwitches)
	{
		if (stopOtherTimedSwitches)
		{
			StopAllCoroutines();
		}
		StartCoroutine(switchRoutine(stateName, inSeconds));
	}

	public virtual void SwitchTo(string stateName, float inSeconds)
	{
		SwitchTo(stateName, inSeconds, stopOtherTimedSwitches: false);
	}

	protected virtual void Update()
	{
		if (CurrentState != null)
		{
			CurrentState.Update();
		}
	}

	protected IEnumerator switchRoutine(string toState, float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		SwitchTo(toState);
	}

	protected bool hasState(string stateName)
	{
		for (int i = 0; i < States.Count; i++)
		{
			if (States[i].Name == stateName)
			{
				return true;
			}
		}
		return false;
	}
}
