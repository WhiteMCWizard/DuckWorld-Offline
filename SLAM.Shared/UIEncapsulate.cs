using System.Collections;
using UnityEngine;

namespace SLAM.Shared;

[RequireComponent(typeof(UIRect))]
public class UIEncapsulate : MonoBehaviour
{
	[SerializeField]
	private UIWidget[] toEncapsulate;

	[SerializeField]
	private Vector4 padding;

	private UIRect uiRect;

	private bool isDirty;

	private void Start()
	{
		Encapsulate();
	}

	private void OnEnable()
	{
		Encapsulate();
	}

	private void LateUpdate()
	{
		if (isDirty)
		{
			doEncapsulate();
		}
	}

	public void Encapsulate()
	{
		StartCoroutine(markDirty());
	}

	private IEnumerator markDirty()
	{
		yield return null;
		isDirty = true;
	}

	[ContextMenu("Execute")]
	private void doEncapsulate()
	{
		if (uiRect == null)
		{
			uiRect = GetComponent<UIRect>();
		}
		uiRect.SetRect(0f, 0f, 0f, 0f);
		Bounds bounds = default(Bounds);
		for (int i = 0; i < toEncapsulate.Length; i++)
		{
			if (toEncapsulate[i].cachedGameObject.activeInHierarchy)
			{
				bounds.Encapsulate(toEncapsulate[i].CalculateBounds(base.transform));
			}
		}
		uiRect.SetRect(bounds.min.x + padding.x, bounds.min.y + padding.w, bounds.size.x - padding.x + padding.z, bounds.size.y - padding.w + padding.y);
		isDirty = false;
	}
}
