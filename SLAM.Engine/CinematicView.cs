using System;
using System.Collections;
using SLAM.Shared;
using UnityEngine;

namespace SLAM.Engine;

public class CinematicView : View
{
	private const bool FORWARD = true;

	private const bool BACKWARD = false;

	[SerializeField]
	private UITypewriter textLabel;

	[SerializeField]
	private UITweener speechFadeTween;

	[SerializeField]
	private UITweener[] fullBlindsTweens;

	[SerializeField]
	private UITweener[] halfBlindsTweens;

	[SerializeField]
	private GameObject skipButton;

	private bool clickedThisFrame;

	public bool IsAnimating { get; private set; }

	protected override void Start()
	{
		base.Start();
		skipButton.SetActive(value: false);
	}

	public void OpenBlindsFull()
	{
		doTweens(fullBlindsTweens);
		speechFadeTween.PlayForward();
	}

	public void CloseBlindsFull()
	{
		SetText(string.Empty);
		doTweens(fullBlindsTweens, forward: false);
	}

	public void OpenBlindsHalf()
	{
		doTweens(halfBlindsTweens);
		speechFadeTween.PlayForward();
	}

	public void CloseBlindsHalf()
	{
		SetText(string.Empty);
		doTweens(halfBlindsTweens, forward: false);
	}

	public void SetText(string text)
	{
		textLabel.SetText(text);
	}

	public void SetText(string fixedText, string animatedText)
	{
		textLabel.SetText(fixedText, animatedText);
	}

	public IEnumerator OpenBlindsAndWaitForClick(Action callback = null)
	{
		OpenBlindsHalf();
		while (IsAnimating)
		{
			yield return null;
		}
		skipButton.SetActive(value: true);
		while (!clickedThisFrame)
		{
			yield return null;
		}
		skipButton.SetActive(value: false);
		clickedThisFrame = false;
		CloseBlindsHalf();
		while (IsAnimating)
		{
			yield return null;
		}
		callback?.Invoke();
	}

	public void OnSkipClicked()
	{
		clickedThisFrame = true;
	}

	private void doTweens(UITweener[] tweeners, bool forward = true)
	{
		if (IsAnimating)
		{
			Debug.LogWarning("Hey Buddy, still tweening here ... give me some time to finish.");
			return;
		}
		tweeners[0].SetOnFinished(new EventDelegate(onTweenFinished));
		for (int i = 0; i < tweeners.Length; i++)
		{
			if (forward)
			{
				tweeners[i].PlayForward();
			}
			else
			{
				tweeners[i].PlayReverse();
			}
		}
		IsAnimating = true;
	}

	private void onTweenFinished()
	{
		IsAnimating = false;
	}
}
