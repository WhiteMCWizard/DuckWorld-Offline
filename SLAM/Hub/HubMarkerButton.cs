using System;
using UnityEngine;

namespace SLAM.Hub;

public class HubMarkerButton : MonoBehaviour
{
	private const string MAINCOLOR_PROPERTY = "_Color";

	private const float ATLAS_COLUMN_COUNT = 6f;

	[SerializeField]
	private GameObject highlightObject;

	[SerializeField]
	private Renderer renderer;

	private Action<HubMarkerButton> onClick;

	private Color originalColor;

	public object Data { get; protected set; }

	public void SetInfo(Material mat, HubMarkerView.HubMarkerIcon icon, bool highlighted, Action<HubMarkerButton> onClick, object data)
	{
		Data = data;
		this.onClick = onClick;
		highlightObject.SetActive(highlighted);
		SetMaterial(mat);
		SetIcon(icon);
		originalColor = mat.GetColor("_Color");
	}

	public void SetClickHandler(Action<HubMarkerButton> onClick)
	{
		this.onClick = onClick;
	}

	public void SetMaterial(Material mat)
	{
		renderer.material = mat;
		originalColor = mat.GetColor("_Color");
	}

	public void SetHighlighted(bool state)
	{
		highlightObject.SetActive(state);
	}

	private void OnMouseEnter()
	{
		float num = 0.3f;
		float num2 = 1f - num;
		renderer.material.SetColor("_Color", new Color(num + num2 * originalColor.r, num + num2 * originalColor.g, num + num2 * originalColor.b, 1f));
	}

	private void OnMouseExit()
	{
		renderer.material.SetColor("_Color", originalColor);
	}

	private void OnMouseUpAsButton()
	{
		if (onClick != null)
		{
			onClick(this);
			AudioController.Play("Interface_buttonClick_primary");
		}
	}

	public void SetIcon(HubMarkerView.HubMarkerIcon icon)
	{
		float num = 1f / 6f;
		float num2 = (float)Mathf.FloorToInt((float)icon / 6f) * num;
		float x = (float)icon % 6f * num;
		renderer.material.mainTextureOffset = new Vector2(x, 0f - num2);
	}
}
