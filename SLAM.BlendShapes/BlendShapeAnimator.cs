using UnityEngine;

namespace SLAM.BlendShapes;

public class BlendShapeAnimator : MonoBehaviour
{
	[SerializeField]
	private Transform blendShapeLocator;

	[SerializeField]
	private SkinnedMeshRenderer meshRenderer;

	private Transform[] blendShapes;

	private void Awake()
	{
		blendShapes = new Transform[meshRenderer.sharedMesh.blendShapeCount];
		for (int i = 0; i < meshRenderer.sharedMesh.blendShapeCount; i++)
		{
			string blendShapeName = meshRenderer.sharedMesh.GetBlendShapeName(i);
			blendShapeName = blendShapeName.Substring(blendShapeName.LastIndexOf('.') + 1);
			blendShapes[i] = blendShapeLocator.FindChild(blendShapeName);
		}
	}

	private void Update()
	{
		for (int i = 0; i < blendShapes.Length; i++)
		{
			meshRenderer.SetBlendShapeWeight(i, blendShapes[i].localPosition.x);
		}
	}
}
