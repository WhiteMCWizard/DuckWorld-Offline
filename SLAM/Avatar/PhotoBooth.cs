using System;
using System.Collections;
using System.Collections.Generic;
using SLAM.Slinq;
using SLAM.Webservices;
using UnityEngine;

namespace SLAM.Avatar;

public class PhotoBooth : SingletonMonobehaviour<PhotoBooth>
{
	[Serializable]
	public class GameCharacterMugshot
	{
		public Game.GameCharacter Character;

		public Texture2D Mugshot;
	}

	public enum Pose
	{
		Idle,
		Cheer,
		Sad,
		Present
	}

	[SerializeField]
	private GameCharacterMugshot[] characterMugshots;

	[SerializeField]
	private Transform avatarSpawnLocation;

	[SerializeField]
	private Camera mugshotCamera;

	[SerializeField]
	private Camera filmCamera;

	[SerializeField]
	private int photoWidth = 256;

	[SerializeField]
	private int photoHeight = 256;

	[SerializeField]
	private Color ambientLight = Color.white;

	[SerializeField]
	private RuntimeAnimatorController boyController;

	[SerializeField]
	private RuntimeAnimatorController girlController;

	[SerializeField]
	private List<Light> mugshotLights;

	[SerializeField]
	private List<Light> filmLights;

	private int photoBoothLayer;

	private GameObject avatarGO;

	private RenderTexture filmTexture;

	private IEnumerable<Light> sceneLights;

	private bool isFilming;

	private int filmAtFrame = -1;

	private void Start()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		filmTexture = new RenderTexture(384, 384, 24);
		filmTexture.filterMode = FilterMode.Trilinear;
		filmTexture.wrapMode = TextureWrapMode.Clamp;
		filmTexture.anisoLevel = 0;
		photoBoothLayer = LayerMask.NameToLayer("PhotoBooth");
		base.gameObject.SetLayerRecursively(photoBoothLayer);
		mugshotCamera.enabled = false;
		filmCamera.enabled = false;
		filmCamera.targetTexture = filmTexture;
		foreach (Light mugshotLight in mugshotLights)
		{
			mugshotLight.enabled = false;
		}
		foreach (Light filmLight in filmLights)
		{
			filmLight.enabled = false;
		}
	}

	public void SayCheese(AvatarConfigurationData config, Action<Texture2D> callback)
	{
		StartCoroutine(doSayCheese(config, callback));
	}

	public RenderTexture StartFilming(Pose pose)
	{
		return StartFilming(AvatarSystem.GetPlayerConfiguration(), pose);
	}

	public RenderTexture StartFilming(AvatarConfigurationData config, Pose pose)
	{
		if (!isFilming && Time.frameCount != filmAtFrame)
		{
			isFilming = true;
			filmAtFrame = Time.frameCount;
			avatarGO = spawnAvatar(config);
			Animator component = avatarSpawnLocation.GetComponent<Animator>();
			component.runtimeAnimatorController = ((config.Gender != AvatarSystem.Gender.Boy) ? girlController : boyController);
			component.Rebind();
			component.SetTrigger(pose.ToString());
			FixOtherLightsCullingMask();
			filmCamera.enabled = true;
			foreach (Light filmLight in filmLights)
			{
				filmLight.enabled = true;
			}
			return filmTexture;
		}
		Debug.LogWarning("Hey Buddy, filming has already been started. This probably means that your game logic is bad and StartFilming is being called more then once.");
		return null;
	}

	public void StopFilming()
	{
		if (!isFilming || Time.frameCount == filmAtFrame)
		{
			return;
		}
		isFilming = false;
		filmAtFrame = Time.frameCount;
		filmCamera.enabled = false;
		foreach (Light filmLight in filmLights)
		{
			filmLight.enabled = false;
		}
		UnityEngine.Object.Destroy(avatarGO);
	}

	public Texture2D GetMugshotFor(Game.GameCharacter character)
	{
		GameCharacterMugshot gameCharacterMugshot = characterMugshots.FirstOrDefault((GameCharacterMugshot m) => m.Character == character);
		Texture2D result = null;
		if (gameCharacterMugshot != null)
		{
			result = gameCharacterMugshot.Mugshot;
		}
		else
		{
			Debug.LogWarning(string.Concat("Hey Buddy, No mugshot found for ", character, ". Please add it to the Photobooth prefab."));
		}
		return result;
	}

	private IEnumerator doSayCheese(AvatarConfigurationData config, Action<Texture2D> callback)
	{
		foreach (Light light in mugshotLights)
		{
			light.enabled = true;
		}
		avatarGO = spawnAvatar(config);
		avatarSpawnLocation.GetComponent<Animator>().SetTrigger("Mugshot");
		yield return null;
		yield return null;
		Texture2D mugshot = MakeRender(mugshotCamera);
		UnityEngine.Object.Destroy(avatarGO);
		foreach (Light light2 in mugshotLights)
		{
			light2.enabled = false;
		}
		callback?.Invoke(mugshot);
	}

	private Texture2D MakeRender(Camera withCamera)
	{
		Color color = RenderSettings.ambientLight;
		RenderSettings.ambientLight = ambientLight;
		RenderTexture targetTexture = (RenderTexture.active = new RenderTexture(photoWidth, photoHeight, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default));
		withCamera.enabled = true;
		Texture2D texture2D = new Texture2D(photoWidth, photoHeight, TextureFormat.ARGB32, mipmap: false);
		withCamera.targetTexture = targetTexture;
		withCamera.Render();
		texture2D.ReadPixels(new Rect(0f, 0f, photoWidth, photoHeight), 0, 0);
		texture2D.Apply();
		withCamera.enabled = false;
		RenderSettings.ambientLight = color;
		return texture2D;
	}

	private GameObject spawnAvatar(AvatarConfigurationData config)
	{
		GameObject gameObject = AvatarSystem.SpawnAvatar(config);
		gameObject.transform.parent = avatarSpawnLocation;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.rotation = avatarSpawnLocation.rotation;
		gameObject.SetLayerRecursively(photoBoothLayer);
		Animator componentInChildren = avatarSpawnLocation.GetComponentInChildren<Animator>();
		componentInChildren.runtimeAnimatorController = ((config.Gender != AvatarSystem.Gender.Boy) ? girlController : boyController);
		componentInChildren.Rebind();
		return gameObject;
	}

	private void FixOtherLightsCullingMask()
	{
		sceneLights = UnityEngine.Object.FindObjectsOfType<Light>();
		foreach (Light sceneLight in sceneLights)
		{
			if (!mugshotLights.Contains(sceneLight) && !filmLights.Contains(sceneLight))
			{
				sceneLight.cullingMask &= ~LayerMask.GetMask("PhotoBooth");
			}
		}
	}
}
