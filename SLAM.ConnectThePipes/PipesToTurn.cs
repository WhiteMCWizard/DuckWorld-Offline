using System;
using UnityEngine;

namespace SLAM.ConnectThePipes;

[Serializable]
public class PipesToTurn
{
	public CTPPipe pipe;

	[Range(1f, 3f)]
	public int RequiredTurns = 1;

	[HideInInspector]
	public int Turns;

	public bool TurnedCorrectly => Turns % 4 == RequiredTurns || (pipe.name.Contains("Straight") && Turns % 2 == RequiredTurns);
}
