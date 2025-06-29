using UnityEngine;

namespace SLAM.Platformer.MonkeyBattle;

public class MB_SwingAudio : MonoBehaviour
{
	[SerializeField]
	private AudioClip swingSound;

	[SerializeField]
	private float swingDistance;

	[SerializeField]
	private Transform avatar;

	private float cooldownTimer;

	private void Start()
	{
	}

	private void Update()
	{
		if (cooldownTimer > 0f)
		{
			cooldownTimer -= Time.deltaTime;
		}
		else if (Vector3.Distance(base.transform.position, avatar.position) < swingDistance)
		{
			AudioController.Play(swingSound.name, base.transform);
			cooldownTimer = 3f;
		}
	}
}
