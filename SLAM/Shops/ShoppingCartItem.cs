using UnityEngine;

namespace SLAM.Shops;

public class ShoppingCartItem : MonoBehaviour
{
	[SerializeField]
	private UITexture icon;

	[SerializeField]
	private UILabel price;

	public ShopVariationDefinition Data { get; private set; }

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void Init(ShopVariationDefinition data)
	{
		Data = data;
		Material material = Data.Item.LibraryItem.Material;
		Color color;
		Color color2;
		Color color3;
		Color color4;
		if (material.HasProperty("_RedColor"))
		{
			color = material.GetColor("_RedColor");
			color2 = material.GetColor("_GreenColor");
			color3 = material.GetColor("_BlueColor");
			color4 = material.GetColor("_BaseColor");
		}
		else if (material.HasProperty("_SkinColor"))
		{
			color = new Color(0f, 0f, 0f, 1f);
			color2 = material.GetColor("_GreenColor");
			color3 = material.GetColor("_BlueColor");
			color4 = Color.black;
		}
		else
		{
			Debug.LogWarning("I dont know how to shade this material, please help!", material);
			color = Color.gray;
			color2 = Color.gray * 0.5f;
			color3 = Color.gray * 1.5f;
			color4 = Color.black;
		}
		Material material2 = new Material(Shader.Find("SLAM/NGUI/ClothingColorVariationRGB"));
		material2.mainTexture = Data.Texture;
		material2.SetColor("_RedColor", color);
		material2.SetColor("_GreenColor", color2);
		material2.SetColor("_BlueColor", color3);
		material2.SetColor("_BaseColor", color4);
		icon.material = material2;
		price.text = Data.Price.ToString();
	}

	public void OnRemoveClicked()
	{
		ShoppingCartItemRemovedEvent shoppingCartItemRemovedEvent = new ShoppingCartItemRemovedEvent();
		shoppingCartItemRemovedEvent.Removeditem = Data;
		GameEvents.Invoke(shoppingCartItemRemovedEvent);
	}
}
