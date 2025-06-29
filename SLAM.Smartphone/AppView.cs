using SLAM.Engine;

namespace SLAM.Smartphone;

public class AppView : View
{
	public virtual void ReturnFromBackground()
	{
		base.gameObject.SetActive(value: true);
	}

	public virtual void EnterBackground()
	{
		base.gameObject.SetActive(value: false);
	}

	public virtual void OnBackClicked()
	{
	}
}
