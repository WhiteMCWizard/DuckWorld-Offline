using UnityEngine;

namespace SLAM.MoneyDive;

public class MD_SpeedFX : MonoBehaviour
{
	[SerializeField]
	private float alphaMin;

	[SerializeField]
	private float alphaMax = 0.5f;

	[SerializeField]
	private float stripesSpeed = 2f;

	[SerializeField]
	private AnimationCurve curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

	private Color color;

	private Material material;

	private float time;

	private float duration;

	private Renderer fxRenderer;

	private void Start()
	{
		fxRenderer = GetComponent<Renderer>();
		material = fxRenderer.material;
		color = material.GetColor("_TintColor");
		fxRenderer.material = material;
	}

	private void Update()
	{
		time -= Time.deltaTime;
		float num = curve.Evaluate(1f - Mathf.Clamp01(time / duration));
		color.a = Mathf.Lerp(alphaMin, alphaMax, num);
		material.SetColor("_TintColor", color);
		Vector2 mainTextureOffset = material.mainTextureOffset;
		mainTextureOffset.y -= Time.deltaTime * num * stripesSpeed;
		material.mainTextureOffset = mainTextureOffset;
		fxRenderer.material = material;
	}

	public void Show(float duration)
	{
		base.gameObject.SetActive(value: true);
		time = (this.duration = duration);
	}
}
