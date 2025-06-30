using System.Collections;
using UnityEngine;

namespace SLAM.CraneOperator;

public class TruckDropZone : DropZone
{
	[SerializeField]
	private Renderer gridRenderer;

	[SerializeField]
	private AnimationCurve gridFadeCurve;

	[SerializeField]
	private float gridFadeTime;

	[SerializeField]
	private float gridFadeInAmount;

	[SerializeField]
	private float gridFadeOutAmount;

	[SerializeField]
	private Animator _animator;

	private CraneOperatorGame craneOperatorGame;

	public Animator Animator => _animator;

	private void Start()
	{
		Color color = gridRenderer.material.color;
		color.a = gridFadeOutAmount;
		gridRenderer.material.color = color;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag.Equals("Crate"))
		{
			StopAllCoroutines();
			StartCoroutine(fadeGrid(gridFadeInAmount));
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag.Equals("Crate"))
		{
			StopAllCoroutines();
			StartCoroutine(fadeGrid(gridFadeOutAmount));
		}
	}

	public void SetOperator(CraneOperatorGame cog)
	{
		craneOperatorGame = cog;
	}

	public override void OnDrop(Crate crate)
	{
		base.OnDrop(crate);
		craneOperatorGame.CratesInTruckChanged(this, crates);
		StopAllCoroutines();
		StartCoroutine(fadeGrid(gridFadeOutAmount));
	}

	public override void OnPickup(Crate crate)
	{
		base.OnPickup(crate);
		craneOperatorGame.CratesInTruckChanged(this, crates);
		StopAllCoroutines();
		StartCoroutine(fadeGrid(gridFadeOutAmount));
	}

	private IEnumerator fadeGrid(float targetAlpha)
	{
		Color startColor = gridRenderer.material.color;
		Color endColor = new Color(startColor.r, startColor.g, startColor.b, targetAlpha);
		Stopwatch sw = new Stopwatch(Mathf.Abs(gridFadeInAmount - gridFadeOutAmount) / Mathf.Abs(endColor.a - startColor.a) * gridFadeTime);
		while (!sw.Expired)
		{
			yield return null;
			gridRenderer.material.color = Color.Lerp(startColor, endColor, gridFadeCurve.Evaluate(sw.Progress));
		}
	}
}
