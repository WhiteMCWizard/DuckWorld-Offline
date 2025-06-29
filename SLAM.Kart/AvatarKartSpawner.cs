namespace SLAM.Kart;

public class AvatarKartSpawner : KartSpawner
{
	public override KartConfigurationData Config => KartSystem.PlayerKartConfiguration;
}
