using System.Collections;
using UnityEngine;

public class ParticleEmitterDestroy : MonoBehaviour
{
	[SerializeField]
	private GameObject objectToDestroy;

	private IEnumerator Start()
	{
		ParticleSystem particleEmitter = GetComponentInChildren<ParticleSystem>();
		while (particleEmitter.IsAlive(withChildren: true))
		{
			yield return null;
		}
		GameObject objToDestroy = ((!(objectToDestroy == null)) ? objectToDestroy : base.gameObject);
		Object.Destroy(objToDestroy);
	}
}
