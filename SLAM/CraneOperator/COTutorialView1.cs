using System.Collections;
using SLAM.Engine;
using SLAM.ToolTips;
using UnityEngine;

namespace SLAM.CraneOperator;

public class COTutorialView1 : TutorialView
{
	[Header("Game Specific Variables")]
	[SerializeField]
	private ToolTip pointerToolTip;

	[SerializeField]
	private ToolTip movingCraneToolTip;

	[SerializeField]
	private ToolTip goToCrateToolTip;

	[SerializeField]
	private ToolTip pickupCrateToolTip;

	[SerializeField]
	private ToolTip goToTruckToolTip;

	[SerializeField]
	private ToolTip releaseCrateToolTip;

	private Crane crane;

	private Crate crate;

	private TruckDropZone zone;

	private Collider zoneCollider;

	protected override void Start()
	{
		base.Start();
		crate = Object.FindObjectOfType<Crate>();
		crane = Object.FindObjectOfType<Crane>();
		zone = Object.FindObjectOfType<TruckDropZone>();
		zoneCollider = zone.GetComponent<Collider>();
		StartCoroutine(step1());
	}

	private IEnumerator step1()
	{
		pointerToolTip.Show(crane.ClawRoot);
		movingCraneToolTip.Show(pointerToolTip.GO.transform);
		while (!SLAMInput.Provider.GetButton(SLAMInput.Button.Up) && !SLAMInput.Provider.GetButton(SLAMInput.Button.Down) && !SLAMInput.Provider.GetButton(SLAMInput.Button.Left) && !SLAMInput.Provider.GetButton(SLAMInput.Button.Right))
		{
			yield return null;
		}
		pointerToolTip.Hide();
		movingCraneToolTip.Hide();
		StartCoroutine(step2());
	}

	private IEnumerator step2()
	{
		goToCrateToolTip.Hide();
		goToCrateToolTip.Show(crate.transform);
		Crate pickupCrate;
		while (!crane.CanPickupCrate(out pickupCrate))
		{
			yield return null;
		}
		goToCrateToolTip.Hide();
		StartCoroutine(step3());
	}

	private IEnumerator step3()
	{
		bool proceedForward = true;
		pointerToolTip.Hide();
		pickupCrateToolTip.Hide();
		pointerToolTip.Show(crane.ClawRoot);
		pickupCrateToolTip.Show(pointerToolTip.GO.transform);
		while (!crane.IsCarryingCrate)
		{
			if (!crane.CanPickupCrate(out var _))
			{
				proceedForward = false;
				break;
			}
			yield return null;
		}
		pointerToolTip.Hide();
		pickupCrateToolTip.Hide();
		COTutorialView1 cOTutorialView = this;
		IEnumerator routine;
		if (proceedForward)
		{
			IEnumerator enumerator = step4();
			routine = enumerator;
		}
		else
		{
			routine = step2();
		}
		cOTutorialView.StartCoroutine(routine);
	}

	private IEnumerator step4()
	{
		bool proceedForward = true;
		goToTruckToolTip.Hide();
		goToTruckToolTip.Show(zone.transform);
		while (!zoneCollider.bounds.Contains(crate.transform.position) || !crane.CanDropCrate(crane.PickedUpCrate))
		{
			if (!crane.IsCarryingCrate)
			{
				proceedForward = false;
				break;
			}
			yield return null;
		}
		goToTruckToolTip.Hide();
		COTutorialView1 cOTutorialView = this;
		IEnumerator routine;
		if (proceedForward)
		{
			IEnumerator enumerator = step5();
			routine = enumerator;
		}
		else
		{
			routine = step3();
		}
		cOTutorialView.StartCoroutine(routine);
	}

	private IEnumerator step5()
	{
		bool proceedForward = true;
		pointerToolTip.Hide();
		releaseCrateToolTip.Hide();
		pointerToolTip.Show(crane.ClawRoot);
		releaseCrateToolTip.Show(pointerToolTip.GO.transform);
		while (zone.CrateCount < 1)
		{
			if (!crane.CanDropCrate(crane.PickedUpCrate))
			{
				proceedForward = false;
				break;
			}
			yield return null;
		}
		pointerToolTip.Hide();
		releaseCrateToolTip.Hide();
		if (!proceedForward)
		{
			StartCoroutine(step4());
		}
	}
}
