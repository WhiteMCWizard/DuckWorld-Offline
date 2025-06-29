using System.Collections;
using SLAM.Engine;
using UnityEngine;

namespace SLAM.Platformer.MonkeyBattle;

public class MB_HudView : HUDView
{
	[SerializeField]
	private Texture2D[] splatTextures;

	[SerializeField]
	private Transform bananaAnchor;

	[SerializeField]
	private UIProgressBar prgsFlintheartHealth;

	[SerializeField]
	private MB_Flintheart flintheart;

	[SerializeField]
	private int minSplatCount = 2;

	[SerializeField]
	private int maxSplatCount = 5;

	[SerializeField]
	private float minSplatVisibleTime = 1f;

	[SerializeField]
	private float maxSplatVisibleTime = 2f;

	[SerializeField]
	private float scale;

	private void OnEnable()
	{
		GameEvents.Subscribe<PlayerHitEvent>(onPlayerHitEvent);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<PlayerHitEvent>(onPlayerHitEvent);
	}

	private void onPlayerHitEvent(PlayerHitEvent evt)
	{
		if (base.gameObject.activeInHierarchy)
		{
			StartCoroutine(doSplats());
		}
	}

	private IEnumerator doSplats()
	{
		int splatCount = Random.Range(minSplatCount, maxSplatCount + 1);
		UITexture[] txtrs = new UITexture[splatCount];
		float w = GetComponentInParent<Camera>().pixelWidth;
		float h = GetComponentInParent<Camera>().pixelHeight;
		for (int i = 0; i < splatCount; i++)
		{
			GameObject go = new GameObject("splat-" + i, typeof(UITexture));
			go.transform.parent = bananaAnchor;
			go.transform.localPosition = new Vector3(Random.value * w - w / 2f, Random.value * h - h / 2f, 0f);
			go.transform.localScale = Vector3.one;
			go.layer = base.gameObject.layer;
			txtrs[i] = go.GetComponent<UITexture>();
			txtrs[i].alpha = 0.9f;
			txtrs[i].mainTexture = splatTextures[i % splatTextures.Length];
			txtrs[i].width = (int)((float)txtrs[i].mainTexture.width * scale);
			txtrs[i].height = (int)((float)txtrs[i].mainTexture.height * scale);
		}
		for (int j = 0; j < splatCount; j++)
		{
			yield return new WaitForSeconds(Random.Range(minSplatVisibleTime, maxSplatVisibleTime));
			StartCoroutine(fadeAndDestroy(txtrs[j]));
		}
	}

	private IEnumerator fadeAndDestroy(UITexture txtr)
	{
		Stopwatch sw = new Stopwatch(1f);
		while ((bool)sw)
		{
			yield return null;
			txtr.alpha = 0.9f - sw.Progress;
		}
		Object.Destroy(txtr.gameObject);
	}

	protected override void Update()
	{
		float target = flintheart.Health / 100f;
		prgsFlintheartHealth.value = Mathf.MoveTowards(prgsFlintheartHealth.value, target, Time.deltaTime * 3f);
	}
}
