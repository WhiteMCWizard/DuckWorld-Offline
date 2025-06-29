using System;
using UnityEngine;

public class AmbientLightPerLayer : MonoBehaviour
{
	[Serializable]
	public class AmbientLightSetting
	{
		public LayerMask Layer;

		public Color Color = Color.white;

		public float Threshold = 0.3f;
	}

	[SerializeField]
	private AmbientLightSetting[] settings;

	private void Start()
	{
		SkinnedMeshRenderer[] array = Resources.FindObjectsOfTypeAll<SkinnedMeshRenderer>();
		foreach (SkinnedMeshRenderer skinnedMeshRenderer in array)
		{
			AmbientLightSetting[] array2 = settings;
			foreach (AmbientLightSetting ambientLightSetting in array2)
			{
				if (!isInLayerMask(skinnedMeshRenderer.gameObject, ambientLightSetting.Layer))
				{
					continue;
				}
				Material[] materials = skinnedMeshRenderer.materials;
				for (int k = 0; k < materials.Length; k++)
				{
					if (materials[k].HasProperty("_AmbientThreshold") && materials[k].HasProperty("_Ambient"))
					{
						materials[k].SetColor("_Ambient", ambientLightSetting.Color);
						materials[k].SetFloat("_AmbientThreshold", ambientLightSetting.Threshold);
					}
				}
				skinnedMeshRenderer.materials = materials;
			}
		}
	}

	private bool isInLayerMask(GameObject obj, LayerMask layerMask)
	{
		int num = 1 << obj.layer;
		return (layerMask.value & num) > 0;
	}
}
