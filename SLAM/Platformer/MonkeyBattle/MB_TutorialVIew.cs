using System.Collections;
using SLAM.Engine;
using SLAM.Slinq;
using SLAM.ToolTips;
using UnityEngine;

namespace SLAM.Platformer.MonkeyBattle;

public class MB_TutorialVIew : TutorialView
{
	[SerializeField]
	private ToolTip moveToolTip;

	[SerializeField]
	private ToolTip movePointerToolTip;

	[SerializeField]
	private ToolTip turretToolTip;

	[SerializeField]
	private ToolTip turretPointerToolTip;

	private MB_PlayerController avatar;

	private MB_Turret[] turrets;

	protected override void Start()
	{
		base.Start();
		avatar = Object.FindObjectOfType<MB_PlayerController>();
		turrets = Object.FindObjectsOfType<MB_Turret>();
		StartCoroutine(explainMoving());
	}

	private IEnumerator explainMoving()
	{
		movePointerToolTip.Show(avatar.transform);
		moveToolTip.Show(avatar.transform);
		while (!SLAMInput.Provider.GetButton(SLAMInput.Button.Right) && !SLAMInput.Provider.GetButton(SLAMInput.Button.Left))
		{
			yield return null;
		}
		yield return new WaitForSeconds(1f);
		movePointerToolTip.Hide();
		moveToolTip.Hide();
		yield return new WaitForSeconds(1f);
		StartCoroutine(explainTurrets());
	}

	private IEnumerator explainTurrets()
	{
		MB_Turret turret = turrets.FirstOrDefault((MB_Turret t) => t.enabled);
		if (turret != null)
		{
			turretPointerToolTip.Show(turret.transform);
			turretToolTip.Show(turretPointerToolTip.GO.transform);
			while (!turret.IsActivated)
			{
				yield return null;
			}
			turretPointerToolTip.Hide();
			turretToolTip.Hide();
		}
		else
		{
			yield return null;
			StartCoroutine(explainTurrets());
		}
	}
}
