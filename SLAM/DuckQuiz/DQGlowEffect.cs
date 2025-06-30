using System.Collections;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.DuckQuiz;

public class DQGlowEffect : MonoBehaviour
{
	[SerializeField]
	private GameObject[] glowObjects;

	private void OnValidate()
	{
		glowObjects = glowObjects.OrderBy((GameObject g) => g.transform.position.x).ToArray();
	}

	private IEnumerator Start()
	{
		string[] effects = new string[2] { "doAlternate", "doWave" };
		int i = 0;
		while (true)
		{
			for (int j = 0; j < glowObjects.Length; j++)
			{
				glowObjects[j].SetActive(value: false);
			}
			yield return StartCoroutine(effects[i % effects.Length]);
			for (int k = 0; k < glowObjects.Length; k++)
			{
				glowObjects[k].SetActive(value: false);
			}
			yield return new WaitForSeconds(0.5f);
			i++;
		}
	}

	private IEnumerator doWave()
	{
		for (int i = 0; i < 12; i++)
		{
			for (int j = 0; j < glowObjects.Length; j++)
			{
				glowObjects[(i % 4 >= 2) ? (glowObjects.Length - j - 1) : j].SetActive(i % 2 == 0);
				yield return new WaitForSeconds(Mathf.Lerp(0.06f, 0.02f, (float)j / (float)(glowObjects.Length - 1)));
			}
			yield return new WaitForSeconds(0.2f);
		}
	}

	private IEnumerator doAlternate()
	{
		for (int i = 0; i < 20; i++)
		{
			for (int j = 0; j < glowObjects.Length; j++)
			{
				glowObjects[j].SetActive(j % 2 == 0);
			}
			yield return new WaitForSeconds(0.2f);
			for (int k = 0; k < glowObjects.Length; k++)
			{
				glowObjects[k].SetActive(k % 2 != 0);
			}
			yield return new WaitForSeconds(0.2f);
		}
	}
}
