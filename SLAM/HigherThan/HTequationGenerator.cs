using System.Globalization;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.HigherThan;

public class HTequationGenerator : MonoBehaviour
{
	public Equation GetEquation(EquationDifficultySetting setting)
	{
		Equation result = default(Equation);
		if (Random.value >= 0.5f)
		{
			createAnswer(setting.rightEquation, ref result.leftAnswer, ref result.leftText);
			createAnswer(setting.leftEquation, ref result.rightAnswer, ref result.rightText);
		}
		else
		{
			createAnswer(setting.leftEquation, ref result.leftAnswer, ref result.leftText);
			createAnswer(setting.rightEquation, ref result.rightAnswer, ref result.rightText);
		}
		if (setting.leftEquation.Type == EquationType.Float && setting.rightEquation.Type == EquationType.Float)
		{
			result.rightAnswer = (float)(int)result.leftAnswer + result.rightAnswer - (float)(int)result.rightAnswer;
			result.rightText = result.rightAnswer.ToString("F2", CultureInfo.GetCultureInfo("nl-NL").NumberFormat);
		}
		if (result.leftAnswer == result.rightAnswer)
		{
			return GetEquation(setting);
		}
		return result;
	}

	private void createAnswer(EquationSetting settings, ref float answer, ref string text)
	{
		int num = Random.Range(settings.MinNumber, settings.MaxNumber);
		int num2 = Random.Range(settings.MinNumber, settings.MaxNumber);
		while (settings.ExcludedNumbers.Contains(num))
		{
			num = Random.Range(settings.MinNumber, settings.MaxNumber);
		}
		switch (settings.Type)
		{
		case EquationType.Addition:
			if (settings.RestrictedToTenths && num % 10 + num2 % 10 > 10)
			{
				num2 = num2 - num2 % 10 + Random.Range(1, 10 - num % 10);
			}
			answer = num + num2;
			text = num + " + " + num2;
			break;
		case EquationType.Substraction:
			num2 = Random.Range(1, num);
			if (settings.RestrictedToTenths && num % 10 - num2 % 10 < 0)
			{
				num2 = num2 - num2 % 10 + Random.Range(1, num % 10);
			}
			answer = num - num2;
			text = num + " - " + num2;
			break;
		case EquationType.Multiplication:
			answer = num * num2;
			text = num + " x " + num2;
			break;
		case EquationType.Division:
			answer = num2;
			text = (num * num2).ToString() + " : " + num;
			break;
		case EquationType.Number:
			answer = num;
			text = num.ToString();
			break;
		case EquationType.Float:
			answer = (float)num + Random.Range(0f, 1f);
			text = answer.ToString("F2", CultureInfo.GetCultureInfo("nl-NL").NumberFormat);
			break;
		case EquationType.Fractions:
		{
			float num3 = Random.Range(settings.MinNumber, settings.MaxNumber);
			float num4 = Random.Range(settings.MinNumber, settings.MaxNumber);
			answer = ((!(num3 < num4)) ? (num4 / num3) : (num3 / num4));
			text = string.Format((!(num3 < num4)) ? "{1}/{0}" : "{0}/{1}", num3, num4);
			break;
		}
		}
	}
}
