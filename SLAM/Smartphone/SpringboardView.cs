using SLAM.Engine;
using UnityEngine;

namespace SLAM.Smartphone;

public class SpringboardView : View
{
	[SerializeField]
	private GameObject appIconPrefab;

	[SerializeField]
	private UIGrid appGrid;

	[SerializeField]
	private Color style = Color.white;

	public Color Style => style;

	public void CreateApps(AppController[] apps)
	{
		appGrid.transform.DestroyChildren();
		for (int i = 0; i < apps.Length; i++)
		{
			if (!apps[i].Hidden)
			{
				GameObject gameObject = NGUITools.AddChild(appGrid.gameObject, appIconPrefab);
				gameObject.GetComponent<AppIcon>().SetData(apps[i]);
			}
		}
		UIGrid uIGrid = appGrid;
		bool repositionNow = true;
		appGrid.repositionNow = repositionNow;
		uIGrid.enabled = repositionNow;
	}
}
