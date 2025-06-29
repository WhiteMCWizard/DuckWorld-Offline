using System.Collections;
using UnityEngine;

namespace SLAM.Platformer;

public class P_Door : P_PressUpTrigger
{
	[SerializeField]
	private P_Door otherDoor;

	[SerializeField]
	public Transform myExit;

	[SerializeField]
	protected float lockAvatarDurations = 2.61f;

	[SerializeField]
	private float walkThroughDoorTime = 1f;

	[SerializeField]
	private float fadeToBlackTime = 0.6f;

	private SidescrollerBehaviour sidescrollerCamera;

	private bool warpCamera;

	protected override void Start()
	{
		base.Start();
		sidescrollerCamera = Object.FindObjectOfType<SidescrollerBehaviour>();
	}

	public void TeleportObject(Transform obj)
	{
		obj.position = myExit.position;
		if (warpCamera)
		{
			sidescrollerCamera.WarpTo(myExit.position);
		}
	}

	protected override UpAction DoAction()
	{
		base.DoAction();
		EnterDoor(this, otherDoor);
		return new UpAction(Action.EnterDoor, lockAvatarDurations, this);
	}

	private void EnterDoor(P_Door entrance, P_Door exit)
	{
		StartCoroutine(DoDoorRoutine(entrance, exit));
	}

	private IEnumerator DoDoorRoutine(P_Door entrance, P_Door exit)
	{
		Vector3 from = entrance.myExit.transform.position;
		Vector3 to = entrance.myExit.transform.position + Vector3.forward * 1.5f;
		base.Player.SwitchTo("Walking");
		base.Player.GetComponent<Animator>().SetBool("Walking", value: true);
		base.Player.transform.rotation = Quaternion.AngleAxis(0f, Vector3.up);
		base.Player.Pause();
		float time = 0f;
		while (time < walkThroughDoorTime)
		{
			time += Time.deltaTime;
			base.Player.transform.position = Vector3.Lerp(from, to, time / walkThroughDoorTime);
			yield return null;
		}
		DoorEnterEvent doorEnterEvent = new DoorEnterEvent();
		doorEnterEvent.fadeToBlackTime = fadeToBlackTime;
		GameEvents.Invoke(doorEnterEvent);
		exit.TeleportObject(base.Player.transform);
		from = exit.myExit.position + Vector3.forward * 1.5f;
		to = exit.myExit.position;
		to.z = 0f;
		base.Player.transform.rotation = Quaternion.AngleAxis(180f, Vector3.up);
		time = 0f;
		while (time < walkThroughDoorTime)
		{
			time += Time.deltaTime;
			base.Player.transform.position = Vector3.Lerp(from, to, time / walkThroughDoorTime);
			base.Player.transform.rotation = Quaternion.AngleAxis(180f, Vector3.up);
			yield return null;
		}
		base.Player.GetComponent<Animator>().SetBool("Walking", value: false);
		base.Player.transform.rotation = Quaternion.AngleAxis(90f, Vector3.up);
		base.Player.Resume();
	}
}
