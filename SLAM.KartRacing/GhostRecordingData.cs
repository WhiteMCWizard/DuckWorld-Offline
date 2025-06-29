using System;
using System.Collections.Generic;
using SLAM.Kart;

namespace SLAM.KartRacing;

[Serializable]
public struct GhostRecordingData
{
	public KartConfigurationData Kart;

	public List<GhostFrameData> Records;
}
