using UnityEngine;

namespace SLAM.DuckQuiz;

public class DQAnswerButton : MonoBehaviour
{
	[SerializeField]
	private GameObject sprtCorrect;

	[SerializeField]
	private GameObject sprtIncorrect;

	[SerializeField]
	private UILabel lblAnswerText;

	public DQAnswer Answer { get; protected set; }

	public void SetInfo(DQAnswer answer)
	{
		base.gameObject.SetActive(value: true);
		GetComponent<Collider>().enabled = true;
		Answer = answer;
		lblAnswerText.text = Answer.Text;
		Color color = GetComponent<UISprite>().color;
		color.a = 1f;
		GetComponent<UISprite>().color = color;
	}

	public void SetCorrect()
	{
		sprtCorrect.SetActive(value: true);
	}

	public void SetIncorrect()
	{
		sprtIncorrect.SetActive(value: true);
	}

	private void OnDisable()
	{
		UITweener[] componentsInChildren = GetComponentsInChildren<UITweener>(includeInactive: true);
		foreach (UITweener uITweener in componentsInChildren)
		{
			uITweener.ResetToBeginning();
			uITweener.enabled = true;
		}
		sprtCorrect.SetActive(value: false);
		sprtIncorrect.SetActive(value: false);
		GetComponent<Collider>().enabled = true;
	}

	public void DisableInteraction()
	{
		GetComponent<Collider>().enabled = false;
	}
}
