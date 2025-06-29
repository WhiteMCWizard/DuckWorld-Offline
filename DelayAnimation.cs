using UnityEngine;

[AddComponentMenu("SLAM/Delay animation")]
public class DelayAnimation : MonoBehaviour
{
	[Range(0f, 1f)]
	[SerializeField]
	private float minRandom;

	[Range(0f, 1f)]
	[SerializeField]
	private float maxRandom = 1f;

	[SerializeField]
	[Range(0.1f, 10f)]
	private float speed = 1f;

	private void Start()
	{
		Animation component = GetComponent<Animation>();
		component.Stop();
		component[component.clip.name].normalizedTime = Random.Range(minRandom, maxRandom);
		component[component.clip.name].speed = speed;
		component.Play();
	}
}
