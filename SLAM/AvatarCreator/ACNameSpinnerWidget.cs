using System.Collections;
using UnityEngine;

namespace SLAM.AvatarCreator;

public class ACNameSpinnerWidget : MonoBehaviour
{
	[SerializeField]
	private UIScrollView scrollView;

	[SerializeField]
	private UICenterOnChild centerOnChild;

	[SerializeField]
	private UIWrapContent wrapContent;

	public string TranslationKey;

	[SerializeField]
	private GameObject namePlatePrefab;

	[SerializeField]
	private float scrollButtonForce = 15f;

	private string[] names;

	public string SelectedName => (!(centerOnChild.centeredObject == null) && !(centerOnChild.centeredObject.GetComponentInChildren<UILabel>() == null)) ? centerOnChild.centeredObject.GetComponentInChildren<UILabel>().text : names[0];

	private void Start()
	{
		wrapContent.onInitializeItem = SetItemData;
	}

	private void SetItemData(GameObject go, int wrapIndex, int realIndex)
	{
		int num = 0;
		int num2 = names.Length;
		if (realIndex > 0)
		{
			int num3 = 1 + realIndex / num2;
			num = num3 * num2 - realIndex;
			num = ((num != num2) ? num : 0);
		}
		else
		{
			num = -realIndex % num2;
		}
		string text = names[num];
		go.GetComponentsInChildren<UILabel>(includeInactive: true)[0].text = text;
	}

	public IEnumerator UpdateNames()
	{
		names = Localization.Get(TranslationKey).Split(',');
		createWheelItems();
		yield return null;
		yield return null;
		yield return null;
		wrapContent.SortAlphabetically();
		scrollView.Scroll(0.01f);
		scrollView.onStoppedMoving = onStopMoving;
		Randomize();
	}

	private void onStopMoving()
	{
		centerOnChild.Recenter();
	}

	private void createWheelItems()
	{
		for (int i = 0; i < 5; i++)
		{
			SetItemData(NGUITools.AddChild(centerOnChild.gameObject, namePlatePrefab), i, i);
		}
	}

	public void ScrollUp()
	{
		scrollView.Scroll(0f - scrollButtonForce);
	}

	public void ScrollDown()
	{
		scrollView.Scroll(scrollButtonForce);
	}

	public void Randomize()
	{
		scrollView.Scroll(Random.Range(scrollButtonForce, scrollButtonForce * 10f));
	}
}
