using UnityEngine;

[AddComponentMenu("Berend/Show Velocity")]
public class ShowRigidbodyVelocity : MonoBehaviour
{
	private Rigidbody rb;

	private void Awake()
	{
		Object.Destroy(this);
	}

	private void Start()
	{
		rb = GetComponentInChildren<Rigidbody>();
		if (rb == null)
		{
			Debug.LogError("LEVEL DESIGNER ERROR! No rigidbody found for ShowRigidbodyVelocity on " + base.gameObject.name + "! Make sure either i or a child has a RigidBody", base.gameObject);
			Object.Destroy(this);
		}
	}

	private void OnGUI()
	{
		GUIStyle gUIStyle = new GUIStyle();
		gUIStyle.fontSize = 16;
		gUIStyle.normal.textColor = Color.black;
		GUI.Label(new Rect(10f, 10f, 200f, 48f), rb.velocity.magnitude.ToString(), gUIStyle);
	}
}
