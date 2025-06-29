using SLAM.Avatar;
using SLAM.Kart;
using UnityEngine;

namespace SLAM.KartRacing;

public class KR_AISettings : MonoBehaviour
{
	[SerializeField]
	private float rubberBanding;

	[SerializeField]
	private float baseDifficulty;

	[SerializeField]
	private KartConfigurationData config = new KartConfigurationData();

	[SerializeField]
	private AvatarConfigurationData avatar = new AvatarConfigurationData();

	[SerializeField]
	[Tooltip("A prefab takes precedence over the avatar settings")]
	private GameObject avatarPrefab;

	[SerializeField]
	private string localizationName;

	public KartConfigurationData Config => config;

	public AvatarConfigurationData Avatar => avatar;

	public GameObject AvatarPrefab => avatarPrefab;

	public float RubberBanding => rubberBanding;

	public float BaseDifficulty => baseDifficulty;

	public string LocalizationName => localizationName;
}
