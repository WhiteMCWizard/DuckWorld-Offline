using System;
using System.Collections.Generic;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.AssemblyLine;

public class ALRobotPartGenerator : MonoBehaviour
{
	private struct PartProbability
	{
		public ALRobotPart RobotPart;

		public int Probability;
	}

	[SerializeField]
	private List<ALRobotPart> partPrefabs;

	[SerializeField]
	private ALDropZone[] dropZones;

	private AnimationCurve spawnRandomPartCurve;

	private List<ALRobotPart> filteredRobotPartPrefabs;

	private ALRobotPart previousPart;

	private int partIndex;

	public ALRobotPart GeneratePart()
	{
		if (UnityEngine.Random.value > getRandomPartChance())
		{
			List<PartProbability> list = new List<PartProbability>();
			ALDropZone[] array = dropZones;
			ALDropZone dropZone;
			for (int i = 0; i < array.Length; i++)
			{
				dropZone = array[i];
				if (dropZone.DesignatedKind < 0)
				{
					continue;
				}
				IEnumerable<ALRobotPart> enumerable = filteredRobotPartPrefabs.Where((ALRobotPart rb) => rb != previousPart && rb.Kind == dropZone.DesignatedKind && (dropZone.PlacedTypes & rb.Type) != rb.Type);
				foreach (ALRobotPart item in enumerable)
				{
					list.Add(new PartProbability
					{
						RobotPart = item,
						Probability = dropZone.DroppedParts.Count
					});
				}
			}
			if (list.Count > 0)
			{
				int num = list.Sum((PartProbability pp) => pp.Probability);
				int num2 = UnityEngine.Random.Range(0, num + 1);
				int num3 = 0;
				for (int num4 = 0; num4 < list.Count; num4++)
				{
					num3 += list[num4].Probability;
					if (num3 >= num2)
					{
						return previousPart = list[num4].RobotPart;
					}
				}
			}
		}
		if (partIndex + 1 > filteredRobotPartPrefabs.Count || filteredRobotPartPrefabs[partIndex] == previousPart)
		{
			partIndex = 0;
			filteredRobotPartPrefabs.Shuffle();
		}
		return filteredRobotPartPrefabs[partIndex++];
	}

	private float getRandomPartChance()
	{
		float num = ((IEnumerable<ALDropZone>)dropZones).Select((Func<ALDropZone, float>)((ALDropZone d) => d.DroppedParts.Count)).Sum();
		float time = num / ((float)dropZones.Length * (float)Enum.GetValues(typeof(ALRobotPart.RobotPartType)).Length);
		return spawnRandomPartCurve.Evaluate(time);
	}

	public void SetDifficulty(AssemblyLineGame.AssemblyLineGameDifficulty diff)
	{
		filteredRobotPartPrefabs = partPrefabs.Where((ALRobotPart p) => diff.AllowedKinds.Contains(p.Kind)).ToList().Shuffle();
		spawnRandomPartCurve = diff.RandomPartChanceCurve;
	}
}
