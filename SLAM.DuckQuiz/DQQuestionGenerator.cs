using System;
using System.Collections.Generic;
using System.IO;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.DuckQuiz;

public class DQQuestionGenerator : MonoBehaviour
{
	[Serializable]
	public struct DQQuestionsAsset
	{
		private const char CSV_SEPERATOR_CHARACTER = ';';

		[SerializeField]
		private TextAsset textAsset;

		[Popup(new string[] { "Dutch", "English" })]
		[SerializeField]
		private string languageKey;

		[SerializeField]
		private DQGameController.QuestionCategory category;

		[SerializeField]
		private DQGameController.QuestionDifficulty difficulty;

		public DQGameController.QuestionCategory Category => category;

		public DQGameController.QuestionDifficulty Difficulty => difficulty;

		public string LanguageKey => languageKey;

		public IEnumerable<DQQuestion> GetQuestions(DQQuestionGenerator gen)
		{
			string[] lines = textAsset.text.Split(new string[3] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
			return Category switch
			{
				DQGameController.QuestionCategory.Flags => parseFlagQuestion(lines, gen), 
				DQGameController.QuestionCategory.Capitals => parseCapitalQuestion(lines), 
				_ => parseNormalQuestions(lines), 
			};
		}

		private IEnumerable<DQQuestion> parseNormalQuestions(string[] lines)
		{
			for (int i = 1; i < lines.Length; i++)
			{
				string[] questionParts = lines[i].Split(new char[1] { ';' }, StringSplitOptions.RemoveEmptyEntries);
				if (questionParts.Length < 3)
				{
					Debug.Log("Line #" + i + " in " + textAsset.name + " is invalid, not enough answers?");
					yield return null;
				}
				DQAnswer[] answers = new DQAnswer[questionParts.Length - 2];
				for (int j = 0; j < answers.Length; j++)
				{
					answers[j] = new DQAnswer
					{
						Correct = (j == 0),
						Text = questionParts[j + 2]
					};
				}
				yield return new DQQuestion
				{
					Text = questionParts[1],
					Answers = answers.Shuffle(),
					Category = Category,
					Difficulty = Difficulty
				};
			}
		}

		private IEnumerable<DQQuestion> parseFlagQuestion(string[] lines, DQQuestionGenerator gen)
		{
			List<DQFlagQuestion> questions = new List<DQFlagQuestion>();
			string[] allowedContinents = getAllowedContinentsPerDifficulty(Difficulty);
			for (int i = 1; i < lines.Length; i++)
			{
				string[] questionParts = lines[i].Split(new char[1] { ';' }, StringSplitOptions.RemoveEmptyEntries);
				if (questionParts.Length < 4)
				{
					Debug.LogError("Line #" + i + " in " + textAsset.name + " is invalid, not enough answers?");
				}
				else if (allowedContinents.Contains(questionParts[4]))
				{
					questions.Add(new DQFlagQuestion
					{
						Text = questionParts[1],
						Continent = questionParts[4],
						FlagName = Path.GetFileNameWithoutExtension(questionParts[3]),
						Flag = gen.GetFlagTexture(Path.GetFileNameWithoutExtension(questionParts[3])),
						Category = Category,
						Difficulty = Difficulty,
						Answers = new DQAnswer[0]
					});
				}
			}
			questions = questions.Shuffle();
			IEnumerable<DQAnswer> allAnswers = questions.Select((DQFlagQuestion q) => new DQAnswer
			{
				Text = q.FlagName,
				Correct = false
			});
			for (int j = 0; j < questions.Count; j++)
			{
				List<DQAnswer> answers = allAnswers.Where((DQAnswer a) => a.Text != questions[j].FlagName).Take(3).ToList();
				answers.Add(new DQAnswer
				{
					Text = questions[j].FlagName,
					Correct = true
				});
				questions[j].Answers = answers.Shuffle().ToArray();
				yield return questions[j];
			}
		}

		private IEnumerable<DQQuestion> parseCapitalQuestion(string[] lines)
		{
			List<DQCapitalQuestion> questions = new List<DQCapitalQuestion>();
			string[] allowedContinents = getAllowedContinentsPerDifficulty(Difficulty);
			for (int i = 1; i < lines.Length; i++)
			{
				string[] questionParts = lines[i].Split(new char[1] { ';' }, StringSplitOptions.RemoveEmptyEntries);
				if (questionParts.Length < 4)
				{
					Debug.LogError("Line #" + i + " in " + textAsset.name + " is invalid, not enough answers?");
				}
				else if (allowedContinents.Contains(questionParts[4]))
				{
					questions.Add(new DQCapitalQuestion
					{
						Text = $"{questionParts[1]} {questionParts[2]}",
						Continent = questionParts[4],
						Capital = questionParts[3],
						Category = Category,
						Difficulty = Difficulty,
						Answers = new DQAnswer[0]
					});
				}
			}
			questions = questions.Shuffle();
			IEnumerable<DQBonusAnswer> allAnswers = questions.Select((DQCapitalQuestion q) => new DQBonusAnswer
			{
				Text = q.Capital,
				Correct = false,
				Continent = q.Continent
			});
			for (int j = 0; j < questions.Count; j++)
			{
				List<DQBonusAnswer> answers = allAnswers.Where((DQBonusAnswer a) => a.Text != questions[j].Capital && a.Continent == questions[j].Continent).Take(3).ToList();
				answers.Add(new DQBonusAnswer
				{
					Text = questions[j].Capital,
					Correct = true
				});
				questions[j].Answers = answers.Shuffle().ToArray();
				yield return questions[j];
			}
		}

		private string[] getAllowedContinentsPerDifficulty(DQGameController.QuestionDifficulty diff)
		{
			return diff switch
			{
				DQGameController.QuestionDifficulty.Medium => new string[2] { "Noord- en Midden-Amerika", "Zuid-Amerika" }, 
				DQGameController.QuestionDifficulty.Hard => new string[2] { "Afrika", "Azië" }, 
				_ => new string[1] { "Europa" }, 
			};
		}
	}

	[SerializeField]
	private Texture2D[] flags;

	[SerializeField]
	private DQQuestionsAsset[] assets;

	public IEnumerable<DQQuestion> GetQuestions(string languageKey, DQGameController.QuestionCategory category, DQGameController.QuestionDifficulty difficulty)
	{
		return assets.Where((DQQuestionsAsset asset) => (asset.Category & category) == asset.Category && (asset.Difficulty & difficulty) == asset.Difficulty && asset.LanguageKey == languageKey).SelectMany((DQQuestionsAsset d) => d.GetQuestions(this)).ToList()
			.Shuffle();
	}

	public Texture2D GetFlagTexture(string flagName)
	{
		return flags.FirstOrDefault((Texture2D f) => f.name == flagName);
	}
}
