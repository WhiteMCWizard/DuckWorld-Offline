using System;
using UnityEngine;

public class PrefabLightmapData : MonoBehaviour
{
	[Serializable]
	private struct RendererInfo
	{
		public Renderer renderer;

		public int lightmapIndex;

		public Vector4 lightmapOffsetScale;
	}

	[Serializable]
	private struct LightMapSet
	{
		public Texture2D far;

		public Texture2D near;
	}

	[SerializeField]
	private RendererInfo[] m_RendererInfo;

	[SerializeField]
	private LightMapSet[] m_Lightmaps;

	[SerializeField]
	private Light[] bakedLights;

	[SerializeField]
	[Tooltip("If true this lightmap will be added to the already active lightmaps. If false this lightmap will be the only lightmap active.")]
	private bool additiveLightmap;

	private void Awake()
	{
		if (m_RendererInfo != null && m_RendererInfo.Length != 0)
		{
			LightmapData[] lightmaps = LightmapSettings.lightmaps;
			int num = (additiveLightmap ? lightmaps.Length : 0);
			LightmapData[] array = new LightmapData[num + m_Lightmaps.Length];
			if (additiveLightmap)
			{
				lightmaps.CopyTo(array, 0);
			}
			for (int i = 0; i < m_Lightmaps.Length; i++)
			{
				array[i + num] = new LightmapData();
				array[i + num].lightmapFar = m_Lightmaps[i].far;
				array[i + num].lightmapNear = m_Lightmaps[i].near;
			}
			ApplyRendererInfo(m_RendererInfo, num);
			for (int j = 0; j < bakedLights.Length; j++)
			{
				bakedLights[j].alreadyLightmapped = true;
			}
			LightmapSettings.lightmaps = array;
		}
	}

	private static void ApplyRendererInfo(RendererInfo[] infos, int lightmapOffsetIndex)
	{
		for (int i = 0; i < infos.Length; i++)
		{
			RendererInfo rendererInfo = infos[i];
			rendererInfo.renderer.lightmapIndex = rendererInfo.lightmapIndex + lightmapOffsetIndex;
			rendererInfo.renderer.lightmapScaleOffset = rendererInfo.lightmapOffsetScale;
		}
	}
}
