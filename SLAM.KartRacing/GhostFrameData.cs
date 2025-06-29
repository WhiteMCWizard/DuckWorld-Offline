using System;

namespace SLAM.KartRacing;

[Serializable]
public struct GhostFrameData
{
	public float Timestamp;

	public JsonVector3 Position;

	public JsonVector3 Rotation;
}
