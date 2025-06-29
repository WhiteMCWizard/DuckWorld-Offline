using UnityEngine;

public class KRTimeRow : MonoBehaviour
{
	[SerializeField]
	private UILabel nameLabel;

	[SerializeField]
	private UILabel timeLabel;

	[SerializeField]
	private UILabel rankLabel;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void SetData(int position, string nameKey, float timeInMS)
	{
		rankLabel.text = $"{position}.";
		nameLabel.text = Localization.Get(nameKey);
		timeLabel.text = StringFormatter.GetFormattedTimeFromMiliseconds(timeInMS);
		base.gameObject.name = rankLabel.text;
	}
}
