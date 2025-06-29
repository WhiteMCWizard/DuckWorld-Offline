using System;
using UnityEngine;

namespace SLAM.InputSystem;

public abstract class JoystickControl : Control
{
	[SerializeField]
	protected UIWidget widget;

	[SerializeField]
	protected float radius = 0.12f;

	[SerializeField]
	protected AnimationCurve inputCurve;

	[SerializeField]
	protected float inputDecay = 0.9f;

	[SerializeField]
	protected Vector3 inputScale = Vector3.one;

	private int trackingFingerId = -1;

	private Vector3 widgetCenter;

	protected Vector3 inputAxis;

	public UIWidget Widget => widget;

	public float Radius => radius;

	private void OnDrawGizmos()
	{
		if (widget != null)
		{
			Vector3 vector = ((!Application.isPlaying) ? widget.transform.position : widgetCenter);
			Vector3 vector2 = new Vector3(vector.x + Mathf.Cos(0f) * Radius, vector.y + Mathf.Sin(0f) * Radius);
			for (float num = 0f; num < (float)Math.PI * 2f; num += 0.01f)
			{
				Vector3 vector3 = vector;
				vector3.x += Mathf.Cos(num) * Radius;
				vector3.y += Mathf.Sin(num) * Radius;
				Gizmos.DrawLine(vector2, vector3);
				vector2 = vector3;
			}
		}
	}

	private void OnEnable()
	{
		widgetCenter = widget.transform.position;
	}

	private void OnDisable()
	{
		widget.transform.position = widgetCenter;
	}

	protected virtual void Reset()
	{
		widget = GetComponentInChildren<UIWidget>();
	}

	protected virtual void Update()
	{
		updateInputAxis();
		updateWidgetPosition();
	}

	protected virtual void updateWidgetPosition()
	{
		widget.transform.position = widgetCenter + inputAxis * radius;
	}

	protected virtual void updateInputAxis()
	{
		for (int i = 0; i < Input.touchCount; i++)
		{
			Touch touch = Input.GetTouch(i);
			Vector3 vector = UICamera.currentCamera.ScreenToWorldPoint(touch.position);
			if (touch.phase == TouchPhase.Began && Vector3.Distance(widgetCenter, vector) < radius)
			{
				trackingFingerId = touch.fingerId;
			}
			if (touch.fingerId == trackingFingerId)
			{
				if (touch.phase == TouchPhase.Ended)
				{
					trackingFingerId = -1;
					continue;
				}
				float num = Mathf.Clamp01(Vector3.Distance(widgetCenter, vector) / radius);
				inputAxis = (vector - widgetCenter).normalized * num;
				inputAxis.x = Mathf.Sign(inputAxis.x) * inputCurve.Evaluate(Mathf.Abs(inputAxis.x)) * inputScale.x;
				inputAxis.y = Mathf.Sign(inputAxis.y) * inputCurve.Evaluate(Mathf.Abs(inputAxis.y)) * inputScale.y;
			}
		}
		if (trackingFingerId < 0)
		{
			inputAxis *= inputDecay * Time.deltaTime;
		}
	}
}
