using System;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.BeatTheBeagleBoys;

public class BTBArea : MonoBehaviour
{
	public enum BTBAreaTheme
	{
		Bird,
		Reptile,
		Primate
	}

	[Serializable]
	public class RoutePoints
	{
		public Transform entryPoint;

		public Transform stealPoint;

		public Transform animalPoint;

		public Transform exitPoint;
	}

	[SerializeField]
	private BTBCage cage;

	[SerializeField]
	private BTBAreaTheme theme;

	[SerializeField]
	private BTBCamera[] cameras;

	[SerializeField]
	private RoutePoints[] routes;

	public int ThiefsEncountered { get; private set; }

	public BTBCage Cage => cage;

	public BTBAreaTheme Theme => theme;

	public RoutePoints[] Routes => routes;

	public bool CanSteal => cage != null && cage.IsOpened;

	public bool IsMonitored => cameras.Count((BTBCamera c) => c.enabled) > 0;

	public BTBThief CurrentThief { get; protected set; }

	public BTBGuard CurrentGuard { get; protected set; }

	public bool HasThief => CurrentThief != null;

	public bool HasGuard => CurrentGuard != null;

	private void Start()
	{
		for (int i = 0; i < cameras.Length; i++)
		{
			cameras[i].enabled = false;
			cameras[i].CameraComp.enabled = false;
		}
	}

	public void OnThiefEntered(BTBThief thief)
	{
		ThiefsEncountered++;
		CurrentThief = thief;
	}

	public void OnThiefExited(BTBThief thief)
	{
		BTBGameController.ThiefExitedEvent thiefExitedEvent = new BTBGameController.ThiefExitedEvent();
		thiefExitedEvent.Thief = CurrentThief;
		thiefExitedEvent.Area = this;
		GameEvents.Invoke(thiefExitedEvent);
		CurrentThief = null;
		if (cage.IsHacked || cage.IsOpened)
		{
			cage.OnReset();
		}
	}

	public void OnGuardEntered(BTBGuard guard)
	{
		CurrentGuard = guard;
	}

	public void OnGuardExited(BTBGuard guard)
	{
		CurrentGuard = null;
	}

	public BTBCamera ActivateCamera()
	{
		BTBCamera bTBCamera = cameras.OrderBy((BTBCamera c) => c.ThiefsEncountered).First();
		if (HasThief)
		{
			bTBCamera.ThiefsEncountered++;
		}
		bTBCamera.enabled = true;
		bTBCamera.CameraComp.enabled = true;
		return bTBCamera;
	}

	public bool OnInteract()
	{
		if (!HasGuard && HasThief && CurrentThief.IsRunningToCage && cage.IsOpened && cage.IsHacked)
		{
			cage.CloseCage();
			GameEvents.Invoke(new BTBGameController.CageClosedEvent());
			return true;
		}
		return false;
	}
}
