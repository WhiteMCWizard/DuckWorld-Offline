using System.Collections.Generic;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.Utils;

public class EquationGenerator
{
	private List<int> _tables;

	private List<Manipulator> _manipulators;

	private bool _restrictedToTenths;

	public List<int> Tables
	{
		get
		{
			return _tables;
		}
		set
		{
			_tables = value;
		}
	}

	public List<Manipulator> Manipulators
	{
		get
		{
			return _manipulators;
		}
		set
		{
			_manipulators = value;
		}
	}

	public bool RestrictedToTenths
	{
		get
		{
			return _restrictedToTenths;
		}
		set
		{
			_restrictedToTenths = value;
		}
	}

	public EquationGenerator()
	{
		_tables = new List<int>(new int[5] { 2, 3, 4, 5, 10 });
		_manipulators = new List<Manipulator>(new Manipulator[1]);
	}

	public List<Equation> GetEquations(int amount)
	{
		List<Equation> list = new List<Equation>();
		if (_manipulators.Count <= 0)
		{
			Debug.LogWarning("Hey Buddy, cannot generate an equation without a manipulator");
		}
		for (int i = 0; i < amount; i++)
		{
			foreach (Manipulator manipulator in _manipulators)
			{
				Equation item = GenerateEquation(manipulator);
				list.Add(item);
				i++;
			}
		}
		list.Shuffle();
		return list;
	}

	private Equation GenerateEquation(Manipulator m)
	{
		int max = _tables.Max();
		int min = _tables.Min();
		int numberA = _tables.GetRandom();
		int num = _tables.GetRandom();
		switch (m)
		{
		case Manipulator.addition:
			num = ((!RestrictedToTenths) ? _tables.Where((int b) => b + numberA < max).GetRandom() : _tables.Where((int b) => b + numberA < max && b % 10 + numberA % 10 <= 10).GetRandom());
			break;
		case Manipulator.substraction:
			num = ((!RestrictedToTenths) ? _tables.Where((int b) => numberA - b > min).GetRandom() : _tables.Where((int b) => numberA - b > min && numberA % 10 - b % 10 >= 0).GetRandom());
			break;
		case Manipulator.division:
			numberA *= num;
			break;
		}
		return new Equation(numberA, num, m);
	}
}
