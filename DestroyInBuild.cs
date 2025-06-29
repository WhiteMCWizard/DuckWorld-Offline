using UnityEngine;

[AddComponentMenu("Berend/DestroyInBuild")]
public class DestroyInBuild : MonoBehaviour
{
	private void Awake()
	{
		Object.Destroy(base.gameObject);
	}
}
