using UnityEngine;

namespace SLAM.ZooTransport;

public class ZTcargo : MonoBehaviour
{
	public enum CrateType
	{
		Crocodile = 1,
		Elephant,
		Fish,
		Flamingo,
		Giraffe,
		Monkey
	}

	private const float minAudioInterval = 4f;

	private const float maxAudioInterval = 10f;

	private const float minAudioTriggerForce = 1.5f;

	public CrateType Type;

	[SerializeField]
	private float monkeyJumpForce = 400f;

	[SerializeField]
	private GameObject fishTurnoverFxPrefab;

	[SerializeField]
	private GameObject fishBumpPrefab;

	private float helperTimer;

	private bool inTruck;

	private MeshRenderer renderer;

	public void SetInTruck(bool inTruck)
	{
		this.inTruck = inTruck;
	}

	private void Start()
	{
		renderer = GetComponentInChildren<MeshRenderer>();
		if (Type != CrateType.Fish)
		{
			Invoke("playSFX", Random.Range(4f, 10f));
		}
	}

	private void playSFX()
	{
		AudioController.Play($"Content_{Type.ToString().ToLower()}_random");
		Invoke("playSFX", Random.Range(4f, 10f));
	}

	public void ManualUpdate()
	{
		switch (Type)
		{
		case CrateType.Fish:
			UpdateFish();
			break;
		case CrateType.Monkey:
			UpdateMonkey();
			break;
		case CrateType.Flamingo:
		case CrateType.Giraffe:
			break;
		}
	}

	public void OnCollisionEnter(Collision col)
	{
		AudioController.Play("Crate_impact");
		if (Type == CrateType.Fish)
		{
			AudioController.Play("Content_fish_random");
			spawnParticle(fishBumpPrefab);
		}
		if (!inTruck)
		{
			ZooTransportGame.instance.LostCrate();
			Object.Destroy(this);
		}
	}

	public void OnCollisionStay(Collision col)
	{
		if (col.relativeVelocity.magnitude > 1.5f)
		{
			AudioController.Play("Crate_impact");
		}
	}

	private void UpdateFish()
	{
		float num = base.transform.localEulerAngles.z;
		if (num > 180f)
		{
			num -= 360f;
		}
		if (Mathf.Abs(num) > 80f)
		{
			spawnParticle(fishTurnoverFxPrefab);
			AudioController.Play("Content_fish_random");
			ZooTransportGame.instance.LostCrate();
			Object.Destroy(this);
		}
	}

	private void UpdateMonkey()
	{
		float magnitude = GetComponent<Rigidbody>().velocity.magnitude;
		if (magnitude < 7f)
		{
			helperTimer += Time.deltaTime / 3f;
		}
		else
		{
			helperTimer -= Time.deltaTime / 2f;
		}
		helperTimer = Mathf.Clamp(helperTimer, 0f, 1f);
		renderer.material.color = Color.red * helperTimer;
		float num = Mathf.Cos(57.29578f * (Time.timeSinceLevelLoad / 10f));
		float num2 = Random.value * num * 0.3f + num * 0.7f;
		if (num2 > 0.95f)
		{
			GetComponent<Rigidbody>().AddForce(Vector3.up * num2 * Mathf.Max(0f, helperTimer - 0.3f) * Time.fixedDeltaTime * monkeyJumpForce * (1f - magnitude / 7f), ForceMode.VelocityChange);
		}
	}

	private void spawnParticle(GameObject particlePrefab)
	{
		GameObject gameObject = Object.Instantiate(particlePrefab);
		gameObject.transform.parent = base.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.rotation = base.transform.rotation;
		gameObject.transform.localScale = Vector3.one;
	}
}
