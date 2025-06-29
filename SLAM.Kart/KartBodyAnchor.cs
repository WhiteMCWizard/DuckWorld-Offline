using System;
using System.Collections.Generic;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.Kart;

public class KartBodyAnchor : MonoBehaviour
{
	[Serializable]
	private struct KartAnchorPoint
	{
		public KartSystem.ItemCategory Category;

		public Transform Anchor;
	}

	[SerializeField]
	private KartAnchorPoint[] anchorPoints;

	public IEnumerable<Transform> GetAnchors(KartSystem.ItemCategory category)
	{
		return from p in anchorPoints
			where p.Category == category
			select p.Anchor;
	}
}
