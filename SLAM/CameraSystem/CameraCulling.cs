using System;
using UnityEngine;

namespace SLAM.CameraSystem;

[ExecuteInEditMode]
public class CameraCulling : MonoBehaviour
{
	[Serializable]
	public struct CameraCullingSetting
	{
		[SerializeField]
		public LayerMask Layer;

		[Range(0f, 500f)]
		[SerializeField]
		public float Distance;
	}

	[SerializeField]
	private CameraCullingSetting[] settings = new CameraCullingSetting[0];

	private void Start()
	{
		setCullingDistance();
	}

	private void OnValidate()
	{
		setCullingDistance();
	}

	private void setCullingDistance()
	{
		float[] array = new float[32];
		for (int i = 0; i < 32; i++)
		{
			int num = (int)Mathf.Pow(2f, i);
			for (int j = 0; j < settings.Length; j++)
			{
				if (settings[j].Layer.value == -1 || (settings[j].Layer.value & num) == num)
				{
					array[i] = settings[j].Distance;
				}
			}
		}
		GetComponent<Camera>().layerCullDistances = array;
	}
}
