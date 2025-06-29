using SLAM.Engine;
using UnityEngine;

namespace SLAM.ToolTips;

public class ToolTip : MonoBehaviour
{
	private const float SLOWMOTION = 0.5f;

	[Tooltip("The prefab being displayed as tooltip.")]
	[SerializeField]
	private GameObject toolTipPrefab;

	[SerializeField]
	[Tooltip("The tooltip will appear on the location of the target.")]
	private GameObject displayTargetGO;

	[Tooltip("The ID assigned to the target.")]
	[SerializeField]
	private Identifier displayTargetGOID;

	[SerializeField]
	private Vector3 displayOffset;

	[Range(0f, 360f)]
	[SerializeField]
	private float displayRotation;

	[SerializeField]
	[Tooltip("The tooltip moves along with the target.")]
	private bool followTarget;

	[SerializeField]
	[Tooltip("Slowdown time while tooltip is displayed.")]
	private bool slowdownTime;

	[Tooltip("The tooltip display duration.")]
	[Range(0f, 10f)]
	[SerializeField]
	private float lifeTime;

	private View targetView;

	private Camera targetCamera;

	private GameObject tooltipGO;

	private float originalTimeScale;

	private Vector3 activeOffset;

	private bool targetIsUIElement;

	private Alarm timer;

	public GameObject GO => tooltipGO;

	public GameObject TargetGO => displayTargetGO;

	public bool IsVisible => tooltipGO != null;

	private void Awake()
	{
		activeOffset = displayOffset;
		TutorialView tutorialView = Object.FindObjectOfType<TutorialView>();
		if (tutorialView != null)
		{
			Initialize(tutorialView, tutorialView.GetComponentInParent<Camera>());
		}
		else
		{
			Debug.LogWarning("Hey Buddy, you need a TutorialView for displaying tooltips.", this);
		}
	}

	private void Start()
	{
		if (displayTargetGOID != Identifier.None)
		{
			IdentifierToGameObjectMapping.mapping.TryGetValue(displayTargetGOID, out displayTargetGO);
		}
		base.enabled = false;
	}

	private void LateUpdate()
	{
		if (tooltipGO != null && displayTargetGO != null && followTarget)
		{
			updateToolTipToPosition();
		}
		if (tooltipGO != null && displayTargetGO == null)
		{
			Hide();
		}
	}

	public void Initialize(TutorialView targetV, Camera targetC)
	{
		targetView = targetV;
		targetCamera = targetC;
		originalTimeScale = Time.timeScale;
		if (toolTipPrefab == null)
		{
			Debug.LogWarning("Hey Buddy, " + base.name + " has no tooltip prefab. What should it show?");
		}
	}

	public void Show()
	{
		Show(displayTargetGO.transform, Vector3.zero);
	}

	public void Show(Vector3 offset)
	{
		Show(displayTargetGO.transform, offset);
	}

	public void Show(Transform target)
	{
		Show(target, Vector3.zero);
	}

	public void Show(Transform target, Vector3 offset)
	{
		base.enabled = true;
		activeOffset = ((!(offset != Vector3.zero)) ? activeOffset : offset);
		displayTooltipOnTarget(target);
	}

	public void Hide()
	{
		if (slowdownTime)
		{
			Time.timeScale = originalTimeScale;
		}
		Object.Destroy(tooltipGO);
	}

	private void displayTooltipOnTarget(Transform target)
	{
		displayTargetGO = target.gameObject;
		if (targetView != null && targetView.IsOpen)
		{
			if (slowdownTime)
			{
				Time.timeScale = 0.5f;
			}
			if (lifeTime > 0f)
			{
				timer = Alarm.Create();
				timer.StartCountdown(lifeTime, Hide);
			}
			targetIsUIElement = target.GetComponentInParent<View>() != null;
			tooltipGO = spawnToolTipAtPosition(toolTipPrefab, target.position + activeOffset);
			if (displayRotation > 0f)
			{
				tooltipGO.transform.localRotation = Quaternion.Euler(0f, 0f, displayRotation);
			}
		}
	}

	protected virtual GameObject spawnToolTipAtPosition(GameObject prefab, Vector3 position)
	{
		GameObject gameObject = NGUITools.AddChild(targetView.gameObject, prefab);
		gameObject.transform.position = ((!targetIsUIElement) ? convertWorldPosToScreen(position) : ((Vector2)position));
		gameObject.transform.localPosition = (Vector2)gameObject.transform.localPosition;
		return gameObject;
	}

	protected virtual void updateToolTipToPosition()
	{
		Vector3 vector = displayTargetGO.transform.position + activeOffset;
		tooltipGO.transform.position = ((!targetIsUIElement) ? convertWorldPosToScreen(vector) : ((Vector2)vector));
		tooltipGO.transform.localPosition = (Vector2)tooltipGO.transform.localPosition;
	}

	private Vector2 convertWorldPosToScreen(Vector3 worldpos)
	{
		return targetCamera.ScreenToWorldPoint(Camera.main.WorldToScreenPoint(worldpos));
	}
}
