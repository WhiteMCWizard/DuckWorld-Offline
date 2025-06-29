using SLAM.Slinq;
using UnityEngine;

namespace SLAM.ConnectThePipes;

public class CTPHighlightOnMouseover : MonoBehaviour
{
	[SerializeField]
	private Renderer[] targetRenderers;

	[SerializeField]
	private Color targetEmissionColor;

	private Color[] origEmissionColors;

	private void OnMouseEnter()
	{
		if (!Object.FindObjectOfType<CTPInputManager>().AreControlsLocked)
		{
			origEmissionColors = targetRenderers.Select((Renderer t) => t.material.GetColor("_EmissiveColor")).ToArray();
			for (int num = 0; num < targetRenderers.Length; num++)
			{
				targetRenderers[num].material.SetColor("_EmissiveColor", targetEmissionColor);
			}
		}
	}

	private void OnMouseExit()
	{
		if (origEmissionColors != null && origEmissionColors.Length > 0)
		{
			for (int i = 0; i < targetRenderers.Length; i++)
			{
				targetRenderers[i].material.SetColor("_EmissiveColor", origEmissionColors[i]);
			}
		}
	}
}
