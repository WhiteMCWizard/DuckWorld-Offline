using System.Collections;
using UnityEngine;

namespace SLAM.Platformer.MonkeyBattle;

public class MB_Monkey : MonoBehaviour
{
	[SerializeField]
	private GameObject bananaPrefab;

	[SerializeField]
	private GameObject monkeyHandObject;

	[SerializeField]
	private Animator animator;

	private GameObject player;

	private float nextFireTime;

	private float fireIntervalMin = 2f;

	private float fireIntervalMax = 4f;

	private void OnEnable()
	{
		nextFireTime = float.MaxValue;
		player = GameObject.FindGameObjectWithTag("Player");
		GameEvents.Subscribe<MonkeyBattleGame.GameStartedEvent>(onGameStartedEvent);
		GameEvents.Subscribe<MonkeyBattleGame.GameEndedEvent>(onGameEndedEvent);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<MonkeyBattleGame.GameStartedEvent>(onGameStartedEvent);
		GameEvents.Unsubscribe<MonkeyBattleGame.GameEndedEvent>(onGameEndedEvent);
	}

	private void OnDrawGizmos()
	{
		Vector3 direction = -base.transform.right;
		Vector3 normalized = (player.transform.position - base.transform.position).normalized;
		normalized.y = direction.y;
		Gizmos.color = Color.red;
		GizmosUtils.DrawArrow(base.transform.position, direction);
		Gizmos.color = Color.green;
		GizmosUtils.DrawArrow(base.transform.position, normalized);
	}

	private void onGameStartedEvent(MonkeyBattleGame.GameStartedEvent evt)
	{
		fireIntervalMin = evt.settings.monkeyFireIntervalMin;
		fireIntervalMax = evt.settings.monkeyFireIntervalMax;
		nextFireTime = Time.time + fireIntervalMax;
	}

	private void onGameEndedEvent(MonkeyBattleGame.GameEndedEvent evt)
	{
		base.enabled = false;
		nextFireTime = float.MaxValue;
	}

	private void Update()
	{
		if (fireIntervalMax > 0f && Time.time > nextFireTime)
		{
			nextFireTime = float.MaxValue;
			StartCoroutine(fireBanana());
		}
	}

	private IEnumerator fireBanana()
	{
		GameObject go = Object.Instantiate(bananaPrefab);
		animator.SetBool("throw", value: true);
		go.GetComponent<MB_Banana>().enabled = false;
		go.transform.parent = monkeyHandObject.transform;
		go.transform.localPosition = Vector3.zero;
		go.transform.localRotation = Quaternion.identity;
		go.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
		yield return new WaitForSeconds(2.5f);
		float d;
		do
		{
			yield return null;
			Vector3 fwd = -base.transform.right;
			Vector3 dir = (player.transform.position - base.transform.position).normalized;
			dir.y = fwd.y;
			d = Vector3.Dot(fwd, dir);
		}
		while (!(d > 0.5f));
		animator.SetBool("throw", value: false);
		yield return new WaitForSeconds(0.5f);
		go.transform.parent = null;
		go.transform.localScale = Vector3.one;
		go.GetComponent<MB_Banana>().enabled = true;
		go.GetComponent<MB_Banana>().ShootAtTarget(player.transform.position, 15f);
		nextFireTime = Time.time + Random.Range(fireIntervalMin, fireIntervalMax);
	}
}
