using UnityEngine;

namespace SLAM.KartRacing;

public class KREmitParticlesOnTrigger : MonoBehaviour
{
	[SerializeField]
	private GameObject particleEffectPrefab;

	private void OnTriggerEnter(Collider other)
	{
		GameObject gameObject = Object.Instantiate(particleEffectPrefab);
		gameObject.transform.parent = other.transform;
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
	}
}
