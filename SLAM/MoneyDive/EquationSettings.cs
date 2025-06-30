using System;
using System.Collections.Generic;
using SLAM.Utils;

namespace SLAM.MoneyDive;

[Serializable]
public class EquationSettings
{
	public Manipulator Manipulator;

	public bool IsRestrictedToTenths;

	public int MaximumTable;

	public List<int> ExcludedTables;
}
