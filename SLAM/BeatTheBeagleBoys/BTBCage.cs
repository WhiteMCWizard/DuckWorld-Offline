using SLAM.Engine;
using UnityEngine;

namespace SLAM.BeatTheBeagleBoys;

[RequireComponent(typeof(FiniteStateMachine))]
public class BTBCage : MonoBehaviour
{
	private const string STATE_OPENED = "Opened";

	private const string STATE_CLOSED = "Closed";

	[SerializeField]
	private Animator doorAnim;

	public AudioClip animalClip;

	private BTBArea area;

	protected FiniteStateMachine stateMachine;

	public bool IsHacked { get; set; }

	public bool IsOpened => stateMachine.CurrentState.Name == "Opened";

	private void Awake()
	{
		area = base.transform.GetComponentInParent<BTBArea>();
		if (!(area == null))
		{
			stateMachine = GetComponent<FiniteStateMachine>();
			stateMachine.AddState("Opened", OnEnterStateOpened, null, null);
			stateMachine.AddState("Closed", null, null, null);
			stateMachine.SwitchTo("Closed");
		}
	}

	public void OpenCage()
	{
		doorAnim.SetBool("isOpen", value: true);
		stateMachine.SwitchTo("Opened");
	}

	public void CloseCage()
	{
		doorAnim.SetBool("isOpen", value: false);
		stateMachine.SwitchTo("Closed");
	}

	public void OnHacked()
	{
		IsHacked = true;
		if (!IsOpened)
		{
			OpenCage();
		}
	}

	public void OnSteal()
	{
		if (IsOpened)
		{
			BTBGameController.AnimalStolenEvent animalStolenEvent = new BTBGameController.AnimalStolenEvent();
			animalStolenEvent.Thief = area.CurrentThief;
			animalStolenEvent.Area = area;
			GameEvents.Invoke(animalStolenEvent);
		}
	}

	public void OnReset()
	{
		IsHacked = false;
		if (IsOpened)
		{
			CloseCage();
		}
	}

	private void OnEnterStateOpened()
	{
		AudioController.Play("alarm");
	}
}
