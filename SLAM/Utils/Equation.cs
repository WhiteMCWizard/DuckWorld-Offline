using System;
using System.Collections.Generic;
using SLAM.Slinq;

namespace SLAM.Utils;

[Serializable]
public class Equation
{
	private int numberA;

	private int numberB;

	private Manipulator sign;

	private List<int> offsets = new List<int> { 2, 3 };

	public string EquationString => $"{numberA} {SignString} {numberB} = ?";

	public int WrongAnswer
	{
		get
		{
			int num = offsets.ElementAt(offsets.Count - 1) + offsets.ElementAt(offsets.Count - 2);
			offsets.Add(num);
			return (offsets.Count % 2 != 0 && CorrectAnswer - num > 0) ? (CorrectAnswer - num) : (CorrectAnswer + num);
		}
	}

	public int CorrectAnswer => sign switch
	{
		Manipulator.addition => numberA + numberB, 
		Manipulator.substraction => numberA - numberB, 
		Manipulator.multiplication => numberA * numberB, 
		Manipulator.division => numberA / numberB, 
		_ => numberA + numberB, 
	};

	public string SignString => sign switch
	{
		Manipulator.addition => "+", 
		Manipulator.substraction => "-", 
		Manipulator.multiplication => "x", 
		Manipulator.division => ":", 
		_ => "+", 
	};

	public string NumberA => numberA.ToString();

	public string NumberB => numberB.ToString();

	public Equation(int n1, int n2, Manipulator sign)
	{
		this.sign = sign;
		numberA = n1;
		numberB = n2;
	}
}
