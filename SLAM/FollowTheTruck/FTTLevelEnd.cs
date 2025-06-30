namespace SLAM.FollowTheTruck;

public class FTTLevelEnd : FTTInteractable<FTTAvatarController>
{
	public override void OnInteract(FTTAvatarController controller)
	{
		FTTGameEndedEvent fTTGameEndedEvent = new FTTGameEndedEvent();
		fTTGameEndedEvent.Success = true;
		GameEvents.Invoke(fTTGameEndedEvent);
	}
}
