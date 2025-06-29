using System;
using SLAM.Webservices;
using UnityEngine;

namespace SLAM.Hub;

public class HubLocationProvider : MonoBehaviour
{
	[Serializable]
	public class HubGameMarker
	{
		[SerializeField]
		private Transform gameMarkerObject;

		[SerializeField]
		private Vector3 buttonOffset;

		[GameId]
		public int GameId;

		[SerializeField]
		private GameObject circleObject;

		[SerializeField]
		private GameObject pathObject;

		[SerializeField]
		private float pathUVLength;

		public Vector3 Position => (!(gameMarkerObject != null)) ? Vector3.zero : gameMarkerObject.position;

		public Quaternion Rotation => (!(gameMarkerObject != null)) ? Quaternion.identity : gameMarkerObject.rotation;

		public Vector3 ButtonOffset => buttonOffset;

		public Vector3 MarkerScale => gameMarkerObject.localScale;

		public GameObject CircleObject => circleObject;

		public GameObject PathObject => pathObject;

		public float PathUvLength => pathUVLength;
	}

	[SerializeField]
	private int locationId;

	[SerializeField]
	private AnimationClip flyToAnimation;

	[SerializeField]
	private Transform zoomInLocation;

	[SerializeField]
	private AudioClip mouseOverSound;

	[SerializeField]
	private AudioClip ambientLoop;

	[SerializeField]
	private Transform iconLocation;

	[SerializeField]
	private HubMarkerView.HubMarkerIcon icon = HubMarkerView.HubMarkerIcon.Location;

	[SerializeField]
	private string iconSpriteName;

	[SerializeField]
	private HubGameMarker[] gameMarkers;

	[SerializeField]
	private Game.GameCharacter gameCharacter;

	public Game.GameCharacter GameCharacter => gameCharacter;

	public string IconSpriteName => iconSpriteName;

	public int LocationId => locationId;

	public HubGameMarker[] GameMarkers => gameMarkers;

	public AnimationClip FlyToAnimation => flyToAnimation;

	public Transform ZoomInLocation => zoomInLocation;

	public AudioClip MouseOverSound => mouseOverSound;

	public AudioClip AmbientLoop => ambientLoop;

	public Transform IconLocation => iconLocation;

	public HubMarkerView.HubMarkerIcon MarkerIcon => icon;

	private void OnDrawGizmos()
	{
		if (gameMarkers != null && gameMarkers.Length > 0)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(gameMarkers[0].Position, 0.1f);
			for (int i = 1; i < gameMarkers.Length; i++)
			{
				Vector3 direction = gameMarkers[i].Position - gameMarkers[i - 1].Position;
				GizmosUtils.DrawArrow(gameMarkers[i - 1].Position, direction);
				Gizmos.DrawSphere(gameMarkers[i].Position, 0.1f);
			}
		}
	}
}
