namespace SLAM.DuckQuiz;

public class DQQuestion
{
	public string Text;

	public DQAnswer[] Answers;

	public DQGameController.QuestionCategory Category;

	public DQGameController.QuestionDifficulty Difficulty;

	public override string ToString()
	{
		string text = Text + "\n";
		for (int i = 0; i < Answers.Length; i++)
		{
			if (Answers[i].Correct)
			{
				text = "(" + (i + 1) + ")" + text;
				text += "<b>";
			}
			text += Answers[i].Text;
			if (Answers[i].Correct)
			{
				text += "</b>";
			}
			text += "\n";
		}
		return text;
	}
}
