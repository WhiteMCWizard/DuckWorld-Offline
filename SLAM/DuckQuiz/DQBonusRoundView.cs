using SLAM.Engine;
using UnityEngine;

namespace SLAM.DuckQuiz;

public class DQBonusRoundView : View
{
	[SerializeField]
	private UILabel lblCategoryName;

	public void SetInfo(DQGameController.QuestionCategory category)
	{
		lblCategoryName.text = Localization.Get("DQ_CATEGORIE_" + category.ToString().ToUpper());
	}
}
