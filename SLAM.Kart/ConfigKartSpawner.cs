using UnityEngine;

namespace SLAM.Kart;

public class ConfigKartSpawner : KartSpawner
{
	[SerializeField]
	private KartConfigurationData config;

	public override KartConfigurationData Config => config;

	public void SetConfiguration(KartConfigurationData conf)
	{
		config = conf;
	}
}
