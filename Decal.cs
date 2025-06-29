using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class Decal : MonoBehaviour
{
	public Material material;

	public Sprite sprite;

	public float maxAngle = 90f;

	public float pushDistance = 0.001f;

	public LayerMask affectedLayers = -1;

	public string id;

	private void Start()
	{
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.matrix = base.transform.localToWorldMatrix;
		Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
	}

	public Bounds GetBounds()
	{
		Vector3 lossyScale = base.transform.lossyScale;
		Vector3 vector = -lossyScale / 2f;
		Vector3 vector2 = lossyScale / 2f;
		Vector3[] array = new Vector3[8]
		{
			new Vector3(vector.x, vector.y, vector.z),
			new Vector3(vector2.x, vector.y, vector.z),
			new Vector3(vector.x, vector2.y, vector.z),
			new Vector3(vector2.x, vector2.y, vector.z),
			new Vector3(vector.x, vector.y, vector2.z),
			new Vector3(vector2.x, vector.y, vector2.z),
			new Vector3(vector.x, vector2.y, vector2.z),
			new Vector3(vector2.x, vector2.y, vector2.z)
		};
		for (int i = 0; i < 8; i++)
		{
			ref Vector3 reference = ref array[i];
			reference = base.transform.TransformDirection(array[i]);
		}
		vector = (vector2 = array[0]);
		Vector3[] array2 = array;
		foreach (Vector3 rhs in array2)
		{
			vector = Vector3.Min(vector, rhs);
			vector2 = Vector3.Max(vector2, rhs);
		}
		return new Bounds(base.transform.position, vector2 - vector);
	}
}
