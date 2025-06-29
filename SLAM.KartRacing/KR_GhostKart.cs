using SLAM.Avatar;
using SLAM.Kart;
using UnityEngine;

namespace SLAM.KartRacing;

public class KR_GhostKart : KR_KartBase
{
	[SerializeField]
	private Material ghostMaterial;

	[SerializeField]
	private Material ghostAvatarMaterial;

	[SerializeField]
	private AnimationCurve distanceFadeCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

	[SerializeField]
	private float startFadeDistance;

	[SerializeField]
	private float endFadeDistance = 10f;

	[SerializeField]
	private float minAlpha = 0.2f;

	[SerializeField]
	private float maxAlpha = 0.5f;

	private Color ghostColor;

	private Material ghostMatInstance;

	private Material ghostAvatarMatInstance;

	private int stampIndex;

	private float time;

	private GhostRecordingData data;

	private GhostFrameData currentFrame;

	private GhostFrameData nextFrame;

	private Transform player;

	private bool isPlaying;

	protected override void Start()
	{
		base.Start();
		Transform transform = base.transform.FindChildRecursively("KS_Pivot_AvatarPosition");
		GameObject gameObject = GetComponent<AvatarSpawn>().SpawnAvatar();
		gameObject.transform.parent = transform;
		AvatarEyeAnimator componentInChildren = transform.GetComponentInChildren<AvatarEyeAnimator>();
		Object.Destroy(componentInChildren);
		ghostColor = ghostMaterial.GetColor("_MainColor");
		ghostMatInstance = Object.Instantiate(ghostMaterial);
		ghostAvatarMatInstance = Object.Instantiate(ghostAvatarMaterial);
		Renderer[] componentsInChildren = GetComponentsInChildren<Renderer>(includeInactive: true);
		foreach (Renderer renderer in componentsInChildren)
		{
			bool flag = renderer.gameObject.name.StartsWith("KS");
			Material[] array = new Material[renderer.materials.Length];
			for (int j = 0; j < array.Length; j++)
			{
				array[j] = ((!flag) ? ghostAvatarMatInstance : ghostMatInstance);
			}
			renderer.sharedMaterials = array;
		}
		ghostColor.a = minAlpha;
		ghostMatInstance.SetColor("_MainColor", ghostColor);
		ghostAvatarMatInstance.SetColor("_MainColor", ghostColor);
	}

	protected void Update()
	{
		if (isPlaying)
		{
			time += Time.deltaTime;
			if (time > nextFrame.Timestamp && stampIndex < data.Records.Count - 1)
			{
				currentFrame = nextFrame;
				stampIndex++;
				nextFrame = data.Records[stampIndex];
			}
			float num = time - currentFrame.Timestamp;
			float num2 = nextFrame.Timestamp - currentFrame.Timestamp;
			float t = num / num2;
			base.transform.position = Vector3.Lerp(currentFrame.Position, nextFrame.Position, t);
			base.transform.rotation = Quaternion.Slerp(Quaternion.Euler(currentFrame.Rotation), Quaternion.Euler(nextFrame.Rotation), t);
			if (player != null)
			{
				float magnitude = (base.transform.position - player.position).magnitude;
				float t2 = distanceFadeCurve.Evaluate((magnitude - startFadeDistance) / (endFadeDistance - startFadeDistance));
				ghostColor.a = Mathf.Lerp(minAlpha, maxAlpha, t2);
				ghostMatInstance.SetColor("_MainColor", ghostColor);
				ghostAvatarMatInstance.SetColor("_MainColor", ghostColor);
			}
		}
	}

	protected override void FixedUpdate()
	{
	}

	private void OnDrawGizmos()
	{
		if (data.Records != null || data.Records.Count <= 0)
		{
			Gizmos.color = Color.blue;
			for (int i = 1; i < data.Records.Count; i++)
			{
				Gizmos.DrawSphere(data.Records[i].Position, 0.4f);
				Gizmos.DrawLine(data.Records[i].Position, data.Records[i].Position);
			}
		}
	}

	public void SetRecording(GhostRecordingData data, string name)
	{
		this.data = data;
		playerName = name;
		time = 0f;
		stampIndex = 0;
		nextFrame = data.Records[stampIndex];
		ConfigKartSpawner componentInChildren = GetComponentInChildren<ConfigKartSpawner>();
		componentInChildren.SetConfiguration(data.Kart);
		componentInChildren.SpawnKart();
		base.transform.position = data.Records[0].Position;
		base.transform.rotation = Quaternion.Euler(data.Records[0].Rotation);
	}

	protected override void onStartRace(KR_StartRaceEvent evt)
	{
		base.onStartRace(evt);
		for (int i = 0; i < evt.Karts.Length; i++)
		{
			if (evt.Karts[i] is KR_HumanKart)
			{
				player = evt.Karts[i].transform;
				break;
			}
		}
		isPlaying = true;
	}

	protected override void onFinishCrossed(KR_FinishCrossedEvent evt)
	{
		base.onFinishCrossed(evt);
		if (evt.Kart == this)
		{
			isPlaying = false;
			ghostColor.a = maxAlpha;
			ghostMatInstance.SetColor("_MainColor", ghostColor);
		}
	}

	protected override void OnEnterRacing()
	{
		base.Timer.StartCountUp();
	}
}
