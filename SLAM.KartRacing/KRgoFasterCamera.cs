using UnityEngine;

namespace SLAM.KartRacing;

public class KRgoFasterCamera : MonoBehaviour
{
	[SerializeField]
	private float minSpeed = 22f;

	[SerializeField]
	private float maxSpeed = 27f;

	[SerializeField]
	private float defaultFov = 60f;

	[SerializeField]
	private float maxFov = 100f;

	[SerializeField]
	private float fovDamping = 3f;

	[SerializeField]
	private AnimationCurve goFasterCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

	[SerializeField]
	private Camera gameCamera;

	[SerializeField]
	private float fastStripesAlphaMin;

	[SerializeField]
	private float fastStripesAlphaMax = 0.5f;

	[SerializeField]
	private float fastStripesSpeed = 2f;

	[SerializeField]
	private Renderer fastStripesRenderer;

	private float targetFov;

	private Color fasterStripesColor;

	private Material fasterStripesMaterial;

	private KR_HumanKart humanKart;

	private void Start()
	{
		fasterStripesMaterial = fastStripesRenderer.material;
		fasterStripesColor = fasterStripesMaterial.GetColor("_TintColor");
		fastStripesRenderer.material = fasterStripesMaterial;
		fasterStripesColor.a = 0f;
		fasterStripesMaterial.SetColor("_TintColor", fasterStripesColor);
	}

	private void Update()
	{
		if (humanKart != null)
		{
			float z = humanKart.RigidBody.velocity.z;
			float num = (z - minSpeed) / (maxSpeed - minSpeed);
			targetFov = Mathf.Lerp(defaultFov, maxFov, goFasterCurve.Evaluate(num));
			gameCamera.fieldOfView = Mathf.Lerp(gameCamera.fieldOfView, targetFov, Time.deltaTime * fovDamping);
			fasterStripesColor.a = Mathf.Lerp(fastStripesAlphaMin, fastStripesAlphaMax, goFasterCurve.Evaluate(num));
			fasterStripesMaterial.SetColor("_TintColor", fasterStripesColor);
			Vector2 mainTextureOffset = fasterStripesMaterial.mainTextureOffset;
			mainTextureOffset.y -= Time.deltaTime * num * fastStripesSpeed;
			fasterStripesMaterial.mainTextureOffset = mainTextureOffset;
			fastStripesRenderer.material = fasterStripesMaterial;
		}
	}

	private void OnEnable()
	{
		GameEvents.Subscribe<KR_StartRaceEvent>(onRaceStarted);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<KR_StartRaceEvent>(onRaceStarted);
	}

	private void onRaceStarted(KR_StartRaceEvent evt)
	{
		for (int i = 0; i < evt.Karts.Length; i++)
		{
			if (evt.Karts[i] is KR_HumanKart)
			{
				humanKart = evt.Karts[i] as KR_HumanKart;
				break;
			}
		}
	}
}
