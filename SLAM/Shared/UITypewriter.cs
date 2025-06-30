using System.Collections;
using UnityEngine;

namespace SLAM.Shared;

[RequireComponent(typeof(UILabel))]
public class UITypewriter : MonoBehaviour
{
	[SerializeField]
	private float lettersPerSecond = 20f;

	[SerializeField]
	private float delayOnPeriod;

	[SerializeField]
	private float delayOnNewLine;

	private UILabel label;

	private void Awake()
	{
		label = GetComponent<UILabel>();
	}

	public void SetText(string animatedText)
	{
		SetText(string.Empty, animatedText);
	}

	public void SetText(string fixedText, string animatedText)
	{
		StopAllCoroutines();
		StartCoroutine(doTypeWriterEffect(fixedText, animatedText));
	}

	private IEnumerator doTypeWriterEffect(string fixedText, string animatedText)
	{
		label.text = fixedText;
		yield return new WaitForSeconds(delayOnPeriod);
		for (int i = 0; i < animatedText.Length; i++)
		{
			float delay = 0f;
			switch (animatedText[i])
			{
			case '\n':
				delay += delayOnNewLine;
				break;
			case '!':
			case '.':
			case '?':
				delay += delayOnPeriod;
				break;
			default:
				delay += 1f / lettersPerSecond;
				break;
			}
			label.text += animatedText[i];
			yield return new WaitForSeconds(delay);
		}
	}
}
