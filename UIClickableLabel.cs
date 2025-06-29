using UnityEngine;

public class UIClickableLabel : MonoBehaviour
{
	private void OnClick()
	{
		UILabel component = GetComponent<UILabel>();
		string urlAtPosition = component.GetUrlAtPosition(UICamera.lastWorldPosition);
		Application.OpenURL(urlAtPosition);
	}
}
