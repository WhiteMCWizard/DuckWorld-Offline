using UnityEngine;

public class SceneFadeView : MonoBehaviour
{
	[SerializeField]
	private UISprite sprite;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void SetColor(Color c)
	{
		sprite.color = c;
	}

	public void SetState(bool state)
	{
		if (sprite.enabled != state)
		{
			sprite.enabled = state;
		}
	}
}
