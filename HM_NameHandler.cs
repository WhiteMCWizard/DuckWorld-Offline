using System;
using UnityEngine;

public class HM_NameHandler : MonoBehaviour
{
	[ContextMenuItem("Handle Translations", "HandleTranslations")]
	[ContextMenuItem("Handle XML", "HandleXML")]
	[Multiline(5)]
	[ContextMenuItem("Handle Names", "HandleNames")]
	[SerializeField]
	private string names;

	[SerializeField]
	[Multiline(5)]
	private string result;

	private void HandleNames()
	{
		result = names.Replace("\r", string.Empty);
		result = result.Replace("\t", ":");
		result = result.Replace("\n", ",");
		string[] array = result.Split(",".ToCharArray());
		result = string.Empty;
		string[] array2 = array;
		foreach (string text in array2)
		{
			if (text.Length > 0)
			{
				string[] array3 = text.Split(':');
				result = result + array3[0].Remove(1).ToUpper() + array3[0].Substring(1) + ":";
				result = result + array3[1].Remove(1).ToUpper() + array3[1].Substring(1) + ",";
			}
		}
		Debug.Log(result);
	}

	private void HandleTranslations()
	{
		string text = "<Row ss:Height=\"13.4079\">";
		string text2 = "</Row>";
		string text3 = "<Cell ss:StyleID=\"Default\"><Data ss:Type=\"String\">";
		string text4 = "</Data></Cell>";
		string[] array = names.Split(",".ToCharArray());
		string text5 = string.Empty;
		for (int i = 0; i < array.Length; i++)
		{
			string[] array2 = array[i].Split(":".ToCharArray());
			text5 += text;
			text5 += text3;
			text5 += array2[1];
			text5 += text4;
			text5 += text3;
			text5 += array2[0];
			text5 += text4;
			text5 += text2;
		}
		result = text5;
		Debug.Log(text5);
	}

	private void HandleXML()
	{
		string[] array = names.Split(new string[1] { "<Row" }, StringSplitOptions.RemoveEmptyEntries);
		string text = string.Empty;
		for (int i = 2; i < array.Length; i++)
		{
			string[] array2 = array[i].Split(new string[1] { "<Cell" }, StringSplitOptions.RemoveEmptyEntries);
			for (int j = 1; j < array2.Length; j++)
			{
				array2[j] = array2[j].Split(new string[1] { "</Data>" }, StringSplitOptions.RemoveEmptyEntries)[0];
				string[] array3 = array2[j].Split(">".ToCharArray());
				array2[j] = array3[array3.Length - 1];
				text = text + array2[j] + ":";
			}
			text = text.Remove(text.Length - 1);
			text += ",";
		}
		Debug.Log(text);
	}
}
