using UnityEngine;

public class NguiUnityBugFix : MonoBehaviour
{
	private void OnGUI()
	{
		GUI.contentColor = new Color(0f, 0f, 0f, 0f);
		GUILayout.Label("Test");
	}
}
