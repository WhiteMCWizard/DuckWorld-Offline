using UnityEngine;

[ExecuteInEditMode]
public class Billboard : MonoBehaviour
{
	private void LateUpdate()
	{
		if (Camera.main != null)
		{
			base.transform.LookAt(base.transform.position + Camera.main.transform.rotation * Vector3.back, Camera.main.transform.rotation * Vector3.up);
		}
	}
}
