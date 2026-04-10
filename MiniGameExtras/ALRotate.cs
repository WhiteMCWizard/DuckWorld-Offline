using UnityEngine;

public class ALRotate : MonoBehaviour
{
	[SerializeField]
	private float speed = 1f;

	private void Update()
	{
		base.transform.Rotate(Vector3.back, speed * Time.deltaTime);
	}
}
