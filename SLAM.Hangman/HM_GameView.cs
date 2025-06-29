using System.Collections.Generic;
using SLAM.Engine;
using UnityEngine;

namespace SLAM.Hangman;

public class HM_GameView : View
{
	[SerializeField]
	private UITexture startView;

	[SerializeField]
	private UIGrid keyGrid;

	[SerializeField]
	private GameObject buttonPrefab;

	[SerializeField]
	private GameObject spotPrefab;

	[SerializeField]
	private Transform spotContainer;

	[SerializeField]
	private UISprite[] beagleBoys;

	[SerializeField]
	private UISprite sadBeagleBoy;

	[SerializeField]
	private UILabel categoryLabel;

	[SerializeField]
	private UIToggle safe;

	[SerializeField]
	private UILabel lblWordCount;

	[SerializeField]
	private UIProgressBar timer;

	[SerializeField]
	private UIToggle[] correctAnswersInLevel;

	[SerializeField]
	private GameObject answerPlingPrefab;

	[SerializeField]
	private GameObject timePlingPrefab;

	[SerializeField]
	private Color correctTextColor = Color.green;

	[SerializeField]
	private Color incorrectTextColor = Color.red;

	private UILabel[] spots;

	private UIButton[] buttons;

	protected override void Start()
	{
		startView.gameObject.SetActive(value: true);
	}

	public void InitKeys(HangmanGame game)
	{
		if (buttons != null)
		{
			UIButton[] array = buttons;
			foreach (UIButton uIButton in array)
			{
				keyGrid.RemoveChild(uIButton.transform);
				Object.Destroy(uIButton.gameObject);
			}
		}
		buttons = new UIButton[26];
		int num = "A".ToCharArray()[0];
		int num2 = "a".ToCharArray()[0];
		int num3 = 0;
		while (num3 < 26)
		{
			GameObject gameObject = NGUITools.AddChild(keyGrid.gameObject, buttonPrefab);
			gameObject.GetComponentInChildren<UILabel>().text = new string(new char[1] { (char)num });
			HM_Key hM_Key = gameObject.AddComponent<HM_Key>();
			hM_Key.Init(game, (char)num);
			buttons[num3] = gameObject.GetComponent<UIButton>();
			num3++;
			num++;
			num2++;
		}
		keyGrid.Reposition();
	}

	public void InitSpots(string answerOrig)
	{
		startView.gameObject.SetActive(value: false);
		if (spots != null)
		{
			UILabel[] array = spots;
			foreach (UILabel uILabel in array)
			{
				Object.Destroy(uILabel.gameObject);
			}
		}
		List<UILabel> list = new List<UILabel>();
		float num = 0f;
		float num2 = (float)answerOrig.Length * -19.7f;
		while (num < (float)answerOrig.Length)
		{
			string text = answerOrig.Substring((int)num, 1);
			if (text != " ")
			{
				GameObject gameObject = Object.Instantiate(spotPrefab);
				gameObject.transform.parent = spotContainer;
				gameObject.transform.localPosition = Vector3.right * num2;
				gameObject.transform.localRotation = Quaternion.identity;
				gameObject.transform.localScale = Vector3.one;
				gameObject.transform.parent = spotContainer.parent;
				UILabel component = gameObject.GetComponent<UILabel>();
				component.text = string.Empty;
				list.Add(component);
			}
			num += 1f;
			num2 += 39.4f;
		}
		spots = list.ToArray();
	}

	public void UpdateSpots(char[] known)
	{
		for (int i = 0; i < Mathf.Min(known.Length, spots.Length); i++)
		{
			spots[i].text = known[i].ToString();
		}
	}

	public void UpdateTimer(float percent)
	{
		timer.value = percent;
	}

	public void SetBeagleBoy(int bb)
	{
		sadBeagleBoy.gameObject.SetActive(value: false);
		for (int i = 0; i < beagleBoys.Length; i++)
		{
			if (bb == -1)
			{
				if (beagleBoys[i].color.a == 1f)
				{
					sadBeagleBoy.gameObject.SetActive(value: true);
					sadBeagleBoy.transform.position = beagleBoys[i].transform.position;
					beagleBoys[i].gameObject.SetActive(value: false);
				}
			}
			else
			{
				beagleBoys[i].gameObject.SetActive(value: true);
			}
			beagleBoys[i].color = new Color(1f, 1f, 1f, (i != bb) ? 0.25f : 1f);
		}
		bool flag = bb >= 7;
		safe.value = flag;
		beagleBoys[beagleBoys.Length - 1].color = new Color(1f, 1f, 1f, flag ? 1 : 0);
		if (flag)
		{
			beagleBoys[beagleBoys.Length - 2].color = new Color(1f, 1f, 1f, 0f);
		}
	}

	public void SetButtonState(char c, bool correct)
	{
		int num = c.ToString().ToLower().ToCharArray()[0] - "a".ToCharArray()[0];
		buttons[num].disabledColor = ((!correct) ? Color.red : Color.green);
		buttons[num].isEnabled = false;
		buttons[num].GetComponent<UIWidget>().depth = 10;
		buttons[num].GetComponentInChildren<UILabel>().depth = 11;
		buttons[num].gameObject.AddComponent<Rigidbody>();
		buttons[num].GetComponent<Rigidbody>().AddTorque(buttons[num].transform.forward * (Random.value - 0.5f) * 50f);
	}

	public void DisableButtons(List<char> chars)
	{
		foreach (char @char in chars)
		{
			int num = @char.ToString().ToLower().ToCharArray()[0] - "a".ToCharArray()[0];
			buttons[num].GetComponentInChildren<UILabel>().color = Color.grey;
			buttons[num].disabledColor = Color.grey;
			buttons[num].isEnabled = false;
			buttons[num].GetComponent<UIWidget>().depth = 10;
			buttons[num].GetComponentInChildren<UILabel>().depth = 11;
			buttons[num].gameObject.AddComponent<Rigidbody>();
			buttons[num].GetComponent<Rigidbody>().AddTorque(buttons[num].transform.forward * (Random.value - 0.5f) * 50f);
		}
	}

	public void SetLevel(int level, int total)
	{
		lblWordCount.text = Localization.Get("HM_HUD_WORD_LABEL") + " " + level + "/" + total;
	}

	public void SetCategory(string category)
	{
		categoryLabel.text = category;
	}

	public void SetWordFinished(bool correct)
	{
		if (correct)
		{
			SetBeagleBoy(-1);
		}
		UILabel[] array = spots;
		foreach (UILabel uILabel in array)
		{
			uILabel.color = ((!correct) ? incorrectTextColor : correctTextColor);
		}
	}
}
