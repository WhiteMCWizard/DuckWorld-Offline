using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class CurveLineRenderer : MonoBehaviour
{
	public enum LineType
	{
		Default,
		Rounded
	}

	private const float epsilon = 0.0001f;

	public LineType type = LineType.Rounded;

	public float width = 1f;

	public float radius = 1f;

	public float roundedAngle = 15f;

	public Vector3 normal = Vector3.up;

	public bool reverseSideEnabled = true;

	public List<Vector3> vertices = new List<Vector3>
	{
		new Vector3(0f, 0f, 0f),
		new Vector3(0f, 0f, 1f)
	};

	[SerializeField]
	private float uvModifier = 0.05f;

	private MeshFilter meshFilter;

	public float TotalLength { get; protected set; }

	public float UvModifier
	{
		get
		{
			return uvModifier;
		}
		set
		{
			uvModifier = value;
		}
	}

	private void Start()
	{
		meshFilter = GetComponent<MeshFilter>();
		Invalidate();
	}

	public void SetVertices(List<Vector3> vertexList)
	{
		vertices.Clear();
		vertices.AddRange(vertexList);
		Invalidate();
	}

	public void AddVertex(Vector3 vertex)
	{
		vertices.Add(vertex);
		Invalidate();
	}

	public void ClearVertices()
	{
		vertices.Clear();
		Invalidate();
	}

	[ContextMenu("Invalidate")]
	public void Invalidate()
	{
		Rebuild();
	}

	private void Rebuild()
	{
		if (meshFilter == null)
		{
			return;
		}
		Mesh mesh = meshFilter.mesh;
		if (mesh == null)
		{
			mesh = new Mesh();
			meshFilter.mesh = mesh;
		}
		mesh.Clear();
		if (vertices == null || vertices.Count < 2 || width <= 0f || normal == Vector3.zero || (type == LineType.Rounded && (roundedAngle <= 0f || radius <= 0f)))
		{
			return;
		}
		Vector3 normalized = normal.normalized;
		List<Vector3> list = ((type != LineType.Default) ? getRoundedVertices() : getDefaultVertices());
		List<Vector3> list2 = new List<Vector3>();
		Vector3 vector = list[1] - list[0];
		vector.Normalize();
		Vector3 vector2 = Vector3.Cross(vector, normalized).normalized;
		list2.Add(list[0] - vector2 * width * 0.5f);
		list2.Add(list[0] + vector2 * width * 0.5f);
		Vector3 rhs = normalized;
		for (int i = 1; i < list.Count - 1; i++)
		{
			Vector3 vector3 = list[i + 1] - list[i];
			vector3.Normalize();
			Vector3 vector4 = Vector3.Cross(vector3, rhs).normalized;
			if (vector4 == Vector3.zero || (vector4 + vector2).magnitude < 0.0001f)
			{
				vector4 = vector2;
				rhs *= -1f;
			}
			Vector3 normalized2 = (vector2 + vector4).normalized;
			float num = width / Mathf.Sin(Vector3.Angle(vector, normalized2) * (float)Math.PI / 180f);
			list2.Add(list[i] - normalized2 * num * 0.5f);
			list2.Add(list[i] + normalized2 * num * 0.5f);
			vector = vector3;
			vector2 = vector4;
		}
		list2.Add(list[list.Count - 1] - vector2 * width * 0.5f);
		list2.Add(list[list.Count - 1] + vector2 * width * 0.5f);
		BuildMesh(mesh, list2, list.Count - 1);
	}

	private void BuildMesh(Mesh mesh, List<Vector3> meshVertices, int segmentCount)
	{
		int num = segmentCount * 2 * 3 * ((!reverseSideEnabled) ? 1 : 2);
		List<int> list = new List<int>();
		for (int i = 0; i < num; i++)
		{
			list.Add(i);
		}
		List<Vector3> list2 = new List<Vector3>();
		List<Vector2> list3 = new List<Vector2>();
		float num2 = 0f;
		TotalLength = 0f;
		for (int j = 0; j < segmentCount; j++)
		{
			Vector3 vector = meshVertices[j * 2] / 2f + meshVertices[j * 2 + 1] / 2f;
			Vector3 vector2 = meshVertices[j * 2 + 2] / 2f + meshVertices[j * 2 + 3] / 2f;
			float num3 = (vector2 - vector).magnitude * uvModifier;
			Vector2 item = new Vector2(0f, num2);
			Vector2 item2 = new Vector2(1f, num2);
			Vector2 item3 = new Vector2(0f, num2 + num3);
			Vector2 item4 = new Vector2(1f, num2 + num3);
			num2 += num3;
			list2.Add(meshVertices[j * 2]);
			list2.Add(meshVertices[j * 2 + 1]);
			list2.Add(meshVertices[j * 2 + 2]);
			list2.Add(meshVertices[j * 2 + 2]);
			list2.Add(meshVertices[j * 2 + 1]);
			list2.Add(meshVertices[j * 2 + 3]);
			list3.Add(item);
			list3.Add(item2);
			list3.Add(item3);
			list3.Add(item3);
			list3.Add(item2);
			list3.Add(item4);
			if (reverseSideEnabled)
			{
				list2.Add(meshVertices[j * 2 + 1]);
				list2.Add(meshVertices[j * 2]);
				list2.Add(meshVertices[j * 2 + 2]);
				list2.Add(meshVertices[j * 2 + 1]);
				list2.Add(meshVertices[j * 2 + 2]);
				list2.Add(meshVertices[j * 2 + 3]);
				list3.Add(item2);
				list3.Add(item);
				list3.Add(item3);
				list3.Add(item2);
				list3.Add(item3);
				list3.Add(item4);
			}
		}
		mesh.vertices = list2.ToArray();
		mesh.triangles = list.ToArray();
		mesh.uv = list3.ToArray();
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		mesh.Optimize();
		TotalLength = num2;
	}

	private List<Vector3> getDefaultVertices()
	{
		return new List<Vector3>(vertices);
	}

	private List<Vector3> getRoundedVertices()
	{
		Vector3 normalized = normal.normalized;
		float num = ((!(radius < width / 2f)) ? radius : (width / 2f));
		List<Vector3> list = new List<Vector3>();
		Vector3 vector = vertices[1] - vertices[0];
		vector.Normalize();
		Vector3 vector2 = Vector3.Cross(vector, normalized).normalized;
		list.Add(vertices[0]);
		for (int i = 1; i < vertices.Count - 1; i++)
		{
			Vector3 vector3 = vertices[i + 1] - vertices[i];
			Vector3 vector4 = ((!(Mathf.Abs(Vector3.Angle(Vector3.Cross(vector, vector3), normalized) - 90f) < 1f)) ? Vector3.Cross(vector3, normalized).normalized : vector2);
			vector3.Normalize();
			if (Vector3.Angle(vector3, vector) < 1f)
			{
				list.Add(vertices[i]);
			}
			else
			{
				float num2 = Mathf.Min((vertices[i] - vertices[i - 1]).magnitude, (vertices[i + 1] - vertices[i]).magnitude) * 0.5f;
				float num3 = num;
				Vector3 normalized2 = ((-vector).normalized + vector3.normalized).normalized;
				if (normalized2.magnitude < 0.0001f || (vector2 - vector4).magnitude < 0.0001f)
				{
					Vector3 vector5 = -Vector3.Cross(vector, vector2);
					Vector3 vector6 = -Vector3.Cross(vector3, vector4);
					normalized2 = (vector5 + vector6).normalized;
				}
				float num4 = Vector3.Angle(vector, normalized2);
				float num5 = num3 / Mathf.Sin(num4 * (float)Math.PI / 180f);
				float num6 = Mathf.Sqrt(num5 * num5 - num3 * num3);
				if (num6 > num2)
				{
					num6 = num2;
					num5 = num6 / Mathf.Cos(num4 * (float)Math.PI / 180f);
				}
				Vector3 vector7 = vertices[i] + normalized2 * num5;
				Vector3 vector8 = vertices[i] - normalized2 * num5;
				Vector3 vector9 = vertices[i] + vector3 * num6;
				Vector3 vector10 = vertices[i] + vector * (0f - num6);
				Vector3 vector11 = ((!(Mathf.Abs(Vector3.Dot(vector10 - vector7, vector)) < 0.0001f)) ? vector8 : vector7);
				Vector3 axis = Vector3.Cross(vector, vector3);
				Vector3 vector12 = vector10 - vector11;
				num4 = Vector3.Angle(vector10 - vector11, vector9 - vector11);
				int num7 = (int)(num4 / roundedAngle + 0.5f);
				float angle = num4 / (float)num7;
				Quaternion quaternion = Quaternion.AngleAxis(angle, axis);
				list.Add(vector10);
				for (int j = 0; j < num7 - 1; j++)
				{
					vector12 = quaternion * vector12;
					list.Add(vector11 + vector12);
				}
				list.Add(vector9);
			}
			vector = vector3;
			vector2 = vector4;
		}
		list.Add(vertices[vertices.Count - 1]);
		return list;
	}
}
