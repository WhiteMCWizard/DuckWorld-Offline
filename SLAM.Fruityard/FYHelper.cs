using System.Collections;
using UnityEngine;

namespace SLAM.Fruityard;

public class FYHelper : MonoBehaviour
{
	[SerializeField]
	[BitMask(typeof(FruityardGame.FYTreeAction))]
	private FruityardGame.FYTreeAction Task;

	[SerializeField]
	private Animator animator;

	[SerializeField]
	private NavMeshAgent navMeshAgent;

	[SerializeField]
	private bool particleAtSpot;

	private Vector3 originalPosition;

	private Quaternion originalRotation;

	private PrefabSpawner prefabSpawner;

	public bool IsPerformingAction { get; protected set; }

	public Animator Animator => animator;

	private void Start()
	{
		IsPerformingAction = false;
		originalPosition = base.transform.position;
		originalRotation = base.transform.rotation;
		prefabSpawner = GetComponent<PrefabSpawner>();
		animator.Rebind();
	}

	private void OnEnable()
	{
		GameEvents.Subscribe<FruityardGame.LevelCompletedEvent>(onLevelCompleted);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<FruityardGame.LevelCompletedEvent>(onLevelCompleted);
	}

	private void onLevelCompleted(FruityardGame.LevelCompletedEvent obj)
	{
		StopAllCoroutines();
		IsPerformingAction = false;
		Animator.SetInteger("action", -1);
		Animator.SetFloat("velocity", 0f);
		base.transform.position = originalPosition;
		base.transform.rotation = originalRotation;
	}

	public void PerformActionAtSpot(FYSpot clickedSpot)
	{
		if (!IsPerformingAction)
		{
			StopAllCoroutines();
			StartCoroutine(doPerformActionAtSpot(clickedSpot));
		}
	}

	public void PlayAnimation(FruityardGame.FYTreeAction action)
	{
		Animator.SetInteger("action", (int)action);
	}

	private IEnumerator doPerformActionAtSpot(FYSpot clickedSpot)
	{
		IsPerformingAction = true;
		yield return StartCoroutine(walkToLocation(clickedSpot.transform.position - new Vector3(1f, 0f, 1f).normalized));
		yield return StartCoroutine(doAction(clickedSpot));
		IsPerformingAction = false;
		yield return StartCoroutine(walkToLocation(originalPosition));
		base.transform.rotation = originalRotation;
	}

	private IEnumerator doAction(FYSpot clickedSpot)
	{
		FruityardGame.FYTreeAction action = GetAppropiateAction(clickedSpot);
		if (particleAtSpot)
		{
			prefabSpawner.SetDestination(clickedSpot.transform);
		}
		yield return StartCoroutine(clickedSpot.PerformAction(this, action));
		Animator.SetInteger("action", -1);
	}

	public FruityardGame.FYTreeAction GetAppropiateAction(FYSpot clickedSpot)
	{
		if ((clickedSpot.CurrentRequiredAction & Task) == clickedSpot.CurrentRequiredAction)
		{
			return clickedSpot.CurrentRequiredAction;
		}
		return FruityardGame.FYTreeAction.None;
	}

	private IEnumerator walkToLocation(Vector3 endPosition)
	{
		navMeshAgent.SetDestination(endPosition);
		yield return null;
		while (navMeshAgent.pathPending)
		{
			yield return null;
		}
		while (navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance)
		{
			Animator.SetFloat("velocity", navMeshAgent.velocity.magnitude);
			yield return null;
		}
		Animator.SetFloat("velocity", 0f);
		base.transform.rotation = originalRotation;
	}
}
