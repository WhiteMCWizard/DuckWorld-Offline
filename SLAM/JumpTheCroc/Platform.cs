using System.Collections;
using UnityEngine;

namespace SLAM.JumpTheCroc;

public class Platform : MonoBehaviour
{
	[SerializeField]
	private Transform landPosition;

	[SerializeField]
	private Transform sign;

	[SerializeField]
	private TextMesh textMesh;

	[SerializeField]
	private AnimationCurve SignCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

	[SerializeField]
	private float signAnimDuration = 0.7f;

	[SerializeField]
	private PrefabSpawner landParticles;

	private float restoreDuration = 1f;

	private Vector3 normalPosition;

	private float landDuration = -1f;

	private AnimationCurve landCurve;

	private int answer;

	private Transform mainCam;

	public Transform LandingStrip => landPosition;

	public int Answer => answer;

	public void Init(string answer, float landDuration, AnimationCurve landCurve)
	{
		if (textMesh != null)
		{
			textMesh.text = answer;
		}
		normalPosition = LandingStrip.position;
		this.landDuration = landDuration;
		this.landCurve = landCurve;
	}

	public IEnumerator LandAt(Transform avatar)
	{
		AudioController.Play("JTC_avatar_land", base.transform);
		AudioController.Play("JTC_avatar_land_splash", base.transform);
		if (landParticles != null)
		{
			landParticles.SpawnAt(base.transform.position);
		}
		float time = 0f;
		Vector3 from = LandingStrip.position;
		Vector3 to = from + Vector3.down * 3f;
		while (time < landDuration)
		{
			time += Time.deltaTime;
			Vector3 position = Vector3.Lerp(from, to, landCurve.Evaluate(time / landDuration));
			base.transform.position = position;
			avatar.position = position;
			yield return null;
		}
	}

	public IEnumerator RestoreOriginalPosition()
	{
		float time = 0f;
		Vector3 from = base.transform.position;
		Vector3 to = normalPosition;
		while (time < restoreDuration)
		{
			time += Time.deltaTime;
			base.transform.position = Vector3.Lerp(from, to, time / restoreDuration);
			yield return null;
		}
	}

	public void SetAnswer(int answer)
	{
		this.answer = answer;
		if (textMesh != null)
		{
			textMesh.text = this.answer.ToString();
		}
		if (sign != null)
		{
			StartCoroutine(animateSign(0));
		}
	}

	public void HideSign()
	{
		StartCoroutine(animateSign(1));
	}

	private IEnumerator animateSign(int dir)
	{
		Quaternion startRotation = Quaternion.Euler(110f, 0f, 0f);
		Quaternion endRotation = Quaternion.LookRotation(base.transform.position - Camera.main.transform.position, Vector3.up);
		sign.gameObject.SetActive(value: true);
		float time = 0f;
		AudioController.Play("JTC_numbersigns_raise", sign);
		while (time < signAnimDuration)
		{
			time += Time.deltaTime;
			sign.rotation = Quaternion.Slerp(startRotation, endRotation, SignCurve.Evaluate(Mathf.Abs((float)dir - time / signAnimDuration)));
			yield return null;
		}
	}
}
