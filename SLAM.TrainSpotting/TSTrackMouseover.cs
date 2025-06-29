using UnityEngine;

namespace SLAM.TrainSpotting;

public class TSTrackMouseover : MonoBehaviour
{
	private SpriteRenderer sprtRenderer;

	[SerializeField]
	private Sprite normalSprite;

	[SerializeField]
	private Sprite mouseoverSprite;

	private void Awake()
	{
		sprtRenderer = GetComponentInParent<SpriteRenderer>();
	}

	public void SetMouseover(bool hasMouseOver)
	{
		if (hasMouseOver && sprtRenderer.sprite != mouseoverSprite)
		{
			sprtRenderer.sprite = mouseoverSprite;
		}
		else if (!hasMouseOver && sprtRenderer.sprite != normalSprite)
		{
			sprtRenderer.sprite = normalSprite;
		}
	}
}
