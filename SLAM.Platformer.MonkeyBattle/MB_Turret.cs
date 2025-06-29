using System.Collections;
using UnityEngine;

namespace SLAM.Platformer.MonkeyBattle;

public class MB_Turret : MonoBehaviour
{
	[SerializeField]
	private Transform avatarSpot;

	[SerializeField]
	private Transform gunPivot;

	[SerializeField]
	private float fireInterval = 1f;

	[SerializeField]
	private GameObject targetObject;

	[SerializeField]
	private float shootingForce;

	[SerializeField]
	private Object bananaPrefab;

	[SerializeField]
	private PrefabSpawner shootParticleSpawner;

	[SerializeField]
	private AudioClip fireSound;

	[SerializeField]
	private AudioClip depletedSound;

	private Animator animator;

	private float nextFireTime;

	private PlayerPathController player;

	private float shootTime;

	private float shootTimeMax;

	public bool IsActivated;

	private void Awake()
	{
		animator = GetComponent<Animator>();
	}

	private void OnEnable()
	{
		GameEvents.Subscribe<MonkeyBattleGame.GameStartedEvent>(onGameStart);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<MonkeyBattleGame.GameStartedEvent>(onGameStart);
		if ((bool)player)
		{
			player.UnregisterAction();
		}
	}

	private void OnTriggerEnter(Collider col)
	{
		if (col.CompareTag("Player") && base.enabled)
		{
			player = col.GetComponentInParent<PlayerPathController>();
			player.RegisterAction(doEnterCannon);
		}
	}

	private void OnTriggerExit(Collider col)
	{
		if (col.CompareTag("Player") && base.enabled)
		{
			player = col.GetComponentInParent<PlayerPathController>();
			player.UnregisterAction();
		}
	}

	private void Update()
	{
		shootTime = ((!IsActivated) ? 0f : (shootTime + Time.deltaTime));
		if (IsActivated)
		{
			aimAtTarget();
			if (SLAMInput.Provider.GetButtonDown(SLAMInput.Button.UpOrAction))
			{
				doFireCannon();
			}
			else if (SLAMInput.Provider.GetButtonDown(SLAMInput.Button.Left) || SLAMInput.Provider.GetButtonDown(SLAMInput.Button.Right) || SLAMInput.Provider.GetButtonDown(SLAMInput.Button.Down) || shootTime > shootTimeMax)
			{
				releasePlayerFromTurret();
			}
		}
	}

	private void onGameStart(MonkeyBattleGame.GameStartedEvent evt)
	{
		shootTimeMax = evt.settings.turretShootTimeMax;
	}

	private Vector3 doEnterCannon(Vector3 inVelocity)
	{
		StartCoroutine(enterCannonSequence());
		return Vector3.zero;
	}

	private void aimAtTarget()
	{
		Quaternion b = Quaternion.LookRotation((targetObject.transform.position + Vector3.up - gunPivot.transform.position).normalized);
		gunPivot.transform.rotation = Quaternion.Lerp(gunPivot.transform.rotation, b, Time.deltaTime * 5f);
	}

	private IEnumerator enterCannonSequence()
	{
		player.enabled = false;
		Vector3 startPos = player.transform.position;
		Vector3 endPos = avatarSpot.position;
		Quaternion startRot = player.transform.rotation;
		Quaternion endRot = avatarSpot.rotation;
		MonkeyBattleGame.TurretEnteredEvent turretEnteredEvent = new MonkeyBattleGame.TurretEnteredEvent();
		turretEnteredEvent.turret = this;
		GameEvents.Invoke(turretEnteredEvent);
		Stopwatch sw = new Stopwatch(0.2f);
		player.GetComponent<Animator>().SetBool("inShootPosition", value: true);
		while ((bool)sw)
		{
			yield return null;
			player.GetComponent<Animator>().SetFloat("horizontalVelocity", 1f);
			player.transform.position = Vector3.Lerp(startPos, endPos, sw.Progress);
			player.transform.rotation = Quaternion.Lerp(startRot, endRot, sw.Progress);
		}
		player.GetComponent<Animator>().SetFloat("horizontalVelocity", 0f);
		player.transform.parent = avatarSpot.transform;
		aimAtTarget();
		IsActivated = true;
	}

	private IEnumerator exitCannonSequence()
	{
		player.transform.parent = null;
		IsActivated = false;
		Vector3 startPos = player.transform.position;
		Vector3 endPos = player.GetPositionOnPath();
		endPos.y = (startPos.y = 0.01f);
		Quaternion startRot = player.transform.rotation;
		Quaternion endRot = Quaternion.LookRotation((endPos - player.transform.position).normalized);
		MonkeyBattleGame.TurretExitedEvent turretExitedEvent = new MonkeyBattleGame.TurretExitedEvent();
		turretExitedEvent.turret = this;
		GameEvents.Invoke(turretExitedEvent);
		player.GetComponent<Animator>().SetBool("inShootPosition", value: false);
		Stopwatch sw = new Stopwatch(0.2f);
		while ((bool)sw)
		{
			yield return null;
			player.GetComponent<Animator>().SetFloat("horizontalVelocity", 1f);
			player.transform.position = Vector3.Lerp(startPos, endPos, sw.Progress);
			player.transform.rotation = Quaternion.Lerp(startRot, endRot, sw.Progress);
		}
		player.GetComponent<Animator>().SetFloat("horizontalVelocity", 0f);
		player.GetComponent<Animator>().SetFloat("verticalVelocity", 0f);
		player.enabled = true;
	}

	private void doFireCannon()
	{
		if (Time.time > nextFireTime && !player.AreControlsLocked)
		{
			nextFireTime = Time.time + fireInterval;
			Fire(gunPivot.transform.forward, gunPivot);
		}
	}

	private void releasePlayerFromTurret()
	{
		if (IsActivated)
		{
			AudioController.Play(depletedSound.name, base.transform);
			GetComponentInParent<MB_TurretManager>().SwitchToNewTurret(this);
			StartCoroutine(exitCannonSequence());
		}
	}

	public void Fire(Vector3 dir, Transform origin)
	{
		AudioController.Play(fireSound.name, base.transform);
		animator.SetTrigger("shoot");
		player.GetComponent<Animator>().SetTrigger("shoot");
		GameObject gameObject = Object.Instantiate(bananaPrefab, origin.position, origin.rotation) as GameObject;
		gameObject.GetComponent<MB_Banana>().ShootInDirection(dir, shootingForce);
		shootParticleSpawner.Spawn();
	}

	public void Appear()
	{
		base.enabled = true;
		gunPivot.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
		animator.SetTrigger("appear");
	}

	public void Disappear()
	{
		base.enabled = false;
		gunPivot.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
		animator.SetTrigger("disappear");
	}

	public void HitByBanana(MB_Banana other)
	{
		if (base.enabled && IsActivated)
		{
			Object.Destroy(other.gameObject);
			releasePlayerFromTurret();
		}
	}
}
