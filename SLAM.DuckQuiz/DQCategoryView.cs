using SLAM.Engine;
using UnityEngine;

namespace SLAM.DuckQuiz;

public class DQCategoryView : View
{
	[SerializeField]
	private UILabel lblCategoryName;

	[SerializeField]
	private UISprite sprtCategoryBackground;

	public void SetInfo(DQGameController.QuestionCategory category)
	{
		sprtCategoryBackground.spriteName = getSpriteNameForQuestionCategory(category);
		lblCategoryName.text = Localization.Get("DQ_" + getSpriteNameForQuestionCategory(category).ToUpper());
	}

	private string getSpriteNameForQuestionCategory(DQGameController.QuestionCategory questionCategory)
	{
		return questionCategory switch
		{
			DQGameController.QuestionCategory.Animals => "categorie_Animals", 
			DQGameController.QuestionCategory.Disney => "categorie_Disney", 
			DQGameController.QuestionCategory.Duck => "categorie_Donald", 
			DQGameController.QuestionCategory.History => "categorie_History", 
			DQGameController.QuestionCategory.Geography => "categorie_Geo", 
			_ => "categorie_Mix", 
		};
	}
}
