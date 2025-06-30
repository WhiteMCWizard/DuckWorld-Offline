using System;
using System.Collections;
using SLAM.Engine;
using UnityEngine;

namespace SLAM.Smartphone;

public class SmartphoneView : View
{
	[SerializeField]
	private GameObject smartphoneModel;

	[SerializeField]
	private UILabel timeLabel;

	[SerializeField]
	private UISprite receptionbarSprite;

	[SerializeField]
	private UIProgressBar batterylife;

	[SerializeField]
	private UISprite[] styleSprites;

	[SerializeField]
	private UILabel titleLabel;

	private string lastTitleLabelKey;

	private void OnEnable()
	{
		Localization.onLocalize = (Localization.OnLocalizeNotification)Delegate.Combine(Localization.onLocalize, new Localization.OnLocalizeNotification(onLocalizationChanged));
	}

	private void OnDisable()
	{
		Localization.onLocalize = (Localization.OnLocalizeNotification)Delegate.Remove(Localization.onLocalize, new Localization.OnLocalizeNotification(onLocalizationChanged));
	}

	private void onLocalizationChanged()
	{
		if (!string.IsNullOrEmpty(lastTitleLabelKey))
		{
			titleLabel.text = Localization.Get(lastTitleLabelKey);
		}
	}

	protected override void onOpenFinished()
	{
		base.onOpenFinished();
		StartCoroutine(doTimeUpdateRoutine(ticktock: true));
		receptionbarSprite.fillAmount = (float)UnityEngine.Random.Range(1, 5) / 4f;
		StartCoroutine(doBatteryDrainRoutine());
	}

	public override void Close(Callback callback, bool immediately)
	{
		StopCoroutine(doTimeUpdateRoutine(ticktock: false));
		base.Close(callback, immediately);
	}

	public void OnHomeClicked()
	{
		Controller<SmartphoneController>().GoHome();
	}

	public void OnBackClicked()
	{
		Controller<SmartphoneController>().GoBack();
	}

	public void SetInfo(AppController app)
	{
		SetInfo(app.Title, app.Style);
	}

	public void SetInfo(string titleKey, Color style)
	{
		for (int i = 0; i < styleSprites.Length; i++)
		{
			styleSprites[i].color = style;
		}
		lastTitleLabelKey = titleKey;
		titleLabel.text = ((!string.IsNullOrEmpty(titleKey)) ? Localization.Get(titleKey) : string.Empty);
	}

	private IEnumerator doTimeUpdateRoutine(bool ticktock)
	{
		string time = string.Empty;
		time = ((!ticktock) ? DateTime.Now.ToString("HH mm") : DateTime.Now.ToString("HH:mm"));
		timeLabel.text = time;
		yield return new WaitForSeconds(1f);
		StartCoroutine(doTimeUpdateRoutine(!ticktock));
	}

	private IEnumerator doBatteryDrainRoutine()
	{
		Stopwatch sw = new Stopwatch(120f);
		while (!sw.Expired)
		{
			float progress = 1f - sw.Progress;
			batterylife.value = Mathf.Clamp(progress, 0.15f, 1f);
			batterylife.foregroundWidget.color = ((!(progress < 0.25f)) ? Color.white : Color.red);
			yield return null;
		}
	}
}
