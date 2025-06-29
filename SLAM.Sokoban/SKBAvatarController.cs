using System.Collections;
using UnityEngine;

namespace SLAM.Sokoban;

public class SKBAvatarController : MonoBehaviour
{
	private const float GRIDSIZE = 1f;

	[SerializeField]
	private float pushingInterval = 0.1f;

	[SerializeField]
	private AnimationCurve movementCurve;

	[SerializeField]
	private AnimationCurve pushingCurve;

	[SerializeField]
	private float movementDuration;

	[SerializeField]
	private float pushMovementDuration;

	[SerializeField]
	private SokobanGameController gameManager;

	[SerializeField]
	private RuntimeAnimatorController animationController;

	[SerializeField]
	private AudioClip[] pushCrateClips;

	private Animator animator;

	public int MoveCount { get; protected set; }

	public bool IsMoving { get; protected set; }

	public bool IsPushing { get; protected set; }

	private void Start()
	{
		animator = GetComponentInChildren<Animator>();
	}

	private void Update()
	{
		if (!IsMoving)
		{
			Vector3 position = base.transform.position;
			if (SLAMInput.Provider.GetButton(SLAMInput.Button.Up))
			{
				position += Vector3.forward * 1f;
			}
			else if (SLAMInput.Provider.GetButton(SLAMInput.Button.Right))
			{
				position += Vector3.right * 1f;
			}
			else if (SLAMInput.Provider.GetButton(SLAMInput.Button.Down))
			{
				position += Vector3.back * 1f;
			}
			else if (SLAMInput.Provider.GetButton(SLAMInput.Button.Left))
			{
				position += Vector3.left * 1f;
			}
			if (position != base.transform.position && canMoveToPosition(position))
			{
				StartCoroutine(MoveToTargetPosition(base.transform, position));
				animator.SetBool("isMoving", value: true);
				MoveCount++;
			}
			else
			{
				animator.SetBool("isMoving", IsMoving);
			}
		}
		animator.SetBool("isPushing", IsPushing);
	}

	private bool canMoveToPosition(Vector3 newTargPos)
	{
		Vector3 vector = newTargPos - base.transform.position;
		RaycastHit hitInfo;
		bool flag = Physics.Raycast(base.transform.position + Vector3.up * 0.5f, vector, out hitInfo, 1f);
		if (flag)
		{
			if (hitInfo.transform.GetComponent<SKBCrate>() != null && !Physics.Raycast(hitInfo.transform.position + Vector3.up * 0.5f, vector, out var _, 1f))
			{
				flag = false;
				IsPushing = true;
				StartCoroutine(MoveToTargetPosition(hitInfo.transform, hitInfo.transform.position + vector * 1f));
				GetComponent<AudioSource>().PlayOneShot(pushCrateClips.GetRandom());
			}
			else if (hitInfo.collider.isTrigger && hitInfo.transform.GetComponent<SKBLevelExit>() != null)
			{
				gameManager.LevelCompleted();
			}
		}
		return !flag;
	}

	public IEnumerator MoveToTargetPosition(Transform trans, Vector3 targetPosition)
	{
		if (trans == base.transform)
		{
			IsMoving = true;
		}
		base.transform.rotation = Quaternion.LookRotation(targetPosition - base.transform.position);
		animator.SetBool("isMoving", value: true);
		if (trans.HasComponent<Rigidbody>())
		{
			trans.GetComponent<Rigidbody>().Sleep();
		}
		if (IsPushing)
		{
			AudioController.Play("PTC_push_crate");
		}
		float duration = ((!IsPushing) ? movementDuration : pushMovementDuration);
		AnimationCurve curve = ((!IsPushing) ? movementCurve : pushingCurve);
		Stopwatch sw = new Stopwatch(duration * Vector3.Distance(trans.position, targetPosition) / 1f);
		Vector3 startPos = trans.position;
		while (!sw.Expired)
		{
			yield return null;
			if (trans == null)
			{
				yield break;
			}
			trans.position = Vector3.Lerp(startPos, targetPosition, curve.Evaluate(sw.Progress));
		}
		if (IsPushing)
		{
			AudioController.Stop("PTC_push_crate");
			animator.SetBool("pushIdle", value: true);
			yield return new WaitForSeconds(pushingInterval);
			animator.SetBool("pushIdle", value: false);
		}
		if (trans == base.transform)
		{
			IsPushing = false;
			IsMoving = false;
		}
	}
}
