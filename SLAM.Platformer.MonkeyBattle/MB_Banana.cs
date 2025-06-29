using UnityEngine;

namespace SLAM.Platformer.MonkeyBattle;

public class MB_Banana : MonoBehaviour
{
	private const float GRAVITY = -19f;

	[SerializeField]
	private LayerMask hitLayer;

	[SerializeField]
	private float rotationScale;

	[SerializeField]
	private AudioClip hitPlayerAudio;

	[SerializeField]
	private AudioClip hitOtherAudio;

	[SerializeField]
	private AudioClip flyByAudio;

	private Vector3 velocity;

	private float lifeTime;

	private float lifeTimeMax = 10f;

	private float flyByTime = 0.51f;

	private Vector3 gravImp;

	public Vector3 Direction => velocity.normalized;

	public void ShootAtTarget(Vector3 target, float force)
	{
		ShootInDirection((target - base.transform.position).normalized, force);
	}

	public void ShootInDirection(Vector3 dir, float force)
	{
		velocity = dir * force * Random.Range(0.8f, 1.2f);
	}

	private void Update()
	{
		lifeTime += Time.deltaTime;
		if (lifeTime > flyByTime)
		{
			AudioController.Play(flyByAudio.name, base.transform.position);
			flyByTime = lifeTimeMax + 1f;
		}
		if (lifeTime > lifeTimeMax)
		{
			Object.Destroy(base.gameObject);
		}
		Vector3 vector = velocity * Time.deltaTime;
		float magnitude = vector.magnitude;
		Ray ray = new Ray(base.transform.position, vector.normalized);
		if (Physics.Raycast(ray, out var hitInfo, magnitude))
		{
			OnCollide(hitInfo);
		}
		base.transform.position += vector;
		base.transform.Rotate(new Vector3(Random.value, Random.value, Random.value) * rotationScale);
	}

	private void OnCollide(RaycastHit hit)
	{
		GameObject gameObject = hit.collider.gameObject;
		if (hit.collider.gameObject.CompareTag("Player"))
		{
			gameObject = gameObject.transform.parent.gameObject;
			AudioController.Play(hitPlayerAudio.name, base.transform.position);
		}
		else
		{
			AudioController.Play(hitOtherAudio.name, base.transform.position);
		}
		gameObject.SendMessage("HitByBanana", this, SendMessageOptions.DontRequireReceiver);
	}

	private void HitByBanana(MB_Banana other)
	{
		float num = Vector3.Dot(velocity.normalized, other.Direction);
		if (num < 0f)
		{
			SetVelocityDirection(base.transform.forward);
			other.SetVelocityDirection(-base.transform.forward);
		}
	}

	public void SetVelocityDirection(Vector3 dir)
	{
		velocity = dir * velocity.magnitude;
	}
}
