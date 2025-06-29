using UnityEngine;

public class DestroyOnPlatform : MonoBehaviour
{
	public enum Platform
	{
		Web,
		Mobile,
		Standalone,
		Steam
	}

	[SerializeField]
	private Platform platform;

	private void Awake()
	{
		if (platform == Platform.Standalone)
		{
			Object.Destroy(base.gameObject);
		}
	}
}
