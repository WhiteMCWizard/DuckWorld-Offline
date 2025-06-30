using System.Collections;
using SLAM.SaveSystem;
using SLAM.Webservices;
using UnityEngine;

namespace SLAM.Utilities;

public class FreePlayerWatermark : MonoBehaviour
{
	[SerializeField]
	private Texture2D watermarkTexture;

	private IEnumerator Start()
	{
		base.enabled = false;
		Object.DontDestroyOnLoad(base.gameObject);
		while (UserProfile.Current == null)
		{
			yield return null;
		}
		base.enabled = UserProfile.Current.IsFree;
	}

	private void Awake()
	{
		GameEvents.Subscribe<Webservice.LogoutEvent>(onLogout);
	}

	private void OnDestroy()
	{
		GameEvents.Unsubscribe<Webservice.LogoutEvent>(onLogout);
	}

	private void onLogout(Webservice.LogoutEvent evt)
	{
		Object.Destroy(base.gameObject);
	}

	private void OnGUI()
	{
		Rect position = new Rect(Screen.width - 10 - watermarkTexture.width, Screen.height - 10 - watermarkTexture.height, watermarkTexture.width, watermarkTexture.height);
		GUI.DrawTexture(position, watermarkTexture);
	}
}
