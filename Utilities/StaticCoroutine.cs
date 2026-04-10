using System.Collections;
using UnityEngine;

public class StaticCoroutine : MonoBehaviour
{
	private static StaticCoroutine mInstance;

	private static StaticCoroutine instance
	{
		get
		{
			if (mInstance == null)
			{
				mInstance = Object.FindObjectOfType(typeof(StaticCoroutine)) as StaticCoroutine;
				if (mInstance == null)
				{
					mInstance = new GameObject("StaticCoroutine").AddComponent<StaticCoroutine>();
				}
			}
			return mInstance;
		}
	}

	private void Awake()
	{
		if (mInstance == null)
		{
			Object.DontDestroyOnLoad(base.gameObject);
			mInstance = this;
		}
	}

	private IEnumerator Perform(IEnumerator coroutine)
	{
		yield return StartCoroutine(coroutine);
	}

	public static Coroutine Start(IEnumerator coroutine)
	{
		return instance.StartCoroutine(instance.Perform(coroutine));
	}

	private void Die()
	{
		mInstance = null;
		Object.Destroy(base.gameObject);
	}

	private void OnApplicationQuit()
	{
		mInstance = null;
	}
}
