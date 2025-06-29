using UnityEngine;

namespace SLAM.ToolTips;

public class MessageToolTip : ToolTip
{
	public string Text { get; protected set; }

	public void SetText(string text)
	{
		Text = text;
	}

	public void ShowText(string text)
	{
		SetText(text);
		Show();
	}

	protected override GameObject spawnToolTipAtPosition(GameObject prefab, Vector3 worldpos)
	{
		GameObject gameObject = base.spawnToolTipAtPosition(prefab, worldpos);
		gameObject.GetComponentInChildren<UILabel>().text = Text;
		return gameObject;
	}
}
