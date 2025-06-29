using System.Collections;
using UnityEngine;

namespace SLAM.JumpTheCroc;

public class JTC_CrocSpawner : MonoBehaviour
{
	[SerializeField]
	private GameObject CrocPrefab;

	private Vector3 leftLimit;

	private Vector3 rightLimit;

	private Animator crocAnim;

	private int move;

	private bool stopped;

	private void Awake()
	{
		move = Random.Range(0, 5);
		GameObject gameObject = Object.Instantiate(CrocPrefab, Vector3.zero, (Random.Range(0, 2) != 0) ? Quaternion.AngleAxis(270f, Vector3.up) : Quaternion.AngleAxis(90f, Vector3.up)) as GameObject;
		gameObject.transform.parent = base.transform;
		crocAnim = gameObject.GetComponent<Animator>();
		StartCoroutine(GrowlBehaviour());
	}

	private void Update()
	{
		if (!stopped && crocAnim.transform.position.z < Camera.main.transform.position.z)
		{
			StopAllCoroutines();
			stopped = true;
		}
	}

	private IEnumerator GrowlBehaviour()
	{
		while (base.enabled)
		{
			float growlTime = Random.value * 25f;
			while (growlTime > 0f)
			{
				growlTime -= Time.deltaTime;
				yield return null;
			}
			yield return new WaitForSeconds(7f);
			AudioController.Play("JTC_croc_growl", crocAnim.transform);
		}
	}

	private IEnumerator SleepBehaviour()
	{
		crocAnim.SetTrigger("Up");
		yield return new WaitForSeconds(7f);
		crocAnim.SetTrigger("Sleep");
		yield return new WaitForSeconds(Random.Range(2f, 20f));
	}

	private IEnumerator SwimBehaviour()
	{
		crocAnim.SetTrigger("Swim");
		yield return new WaitForSeconds(Random.Range(3.3f, 6f));
	}

	private IEnumerator BiteBehaviour()
	{
		crocAnim.SetTrigger("Bite");
		yield return new WaitForSeconds(3.3f);
	}

	private IEnumerator HideBehaviour()
	{
		crocAnim.SetTrigger("Sleep");
		yield return null;
		crocAnim.SetTrigger("Down");
		yield return new WaitForSeconds(3.3f);
	}

	private IEnumerator DoCrocBehaviour()
	{
		float wait = Random.Range(0f, 5f);
		yield return new WaitForSeconds(wait);
		switch (move)
		{
		default:
			yield return StartCoroutine(SleepBehaviour());
			break;
		case 2:
			yield return StartCoroutine(SwimBehaviour());
			ResetCroc();
			StartCoroutine(DoCrocBehaviour());
			break;
		case 3:
			yield return null;
			break;
		}
	}

	private void ResetCroc()
	{
		crocAnim.transform.position = Vector3.Lerp(leftLimit, rightLimit, Random.Range(0f, 1f));
		crocAnim.transform.rotation = ((Random.Range(0, 2) != 0) ? Quaternion.AngleAxis(270f, Vector3.up) : Quaternion.AngleAxis(90f, Vector3.up));
	}

	public void Hide()
	{
		StopAllCoroutines();
		StartCoroutine(HideBehaviour());
	}

	public void Scare()
	{
		if (move == 0 || move == 1)
		{
			StopAllCoroutines();
			switch (Random.Range(0, 4))
			{
			case 0:
				StartCoroutine(BiteBehaviour());
				break;
			case 1:
				break;
			default:
				StartCoroutine(HideBehaviour());
				break;
			}
		}
	}

	public void SpawnBetween(Vector3 left, Vector3 right)
	{
		leftLimit = left;
		rightLimit = right;
		if (move == 2)
		{
			leftLimit += new Vector3(2f, 0f, 0f);
			rightLimit -= new Vector3(2f, 0f, 0f);
		}
		Vector3 position = Vector3.Lerp(left, right, Random.Range(0f, 1f));
		crocAnim.transform.position = position;
		StartCoroutine(DoCrocBehaviour());
	}

	public IEnumerator SpawnKillerCroc(Vector3 atPosition)
	{
		crocAnim.transform.position = atPosition;
		crocAnim.transform.rotation = ((Random.Range(0, 2) != 0) ? Quaternion.AngleAxis(225f, Vector3.up) : Quaternion.AngleAxis(45f, Vector3.up));
		crocAnim.SetTrigger("Kill");
		yield return new WaitForSeconds(3f);
	}
}
