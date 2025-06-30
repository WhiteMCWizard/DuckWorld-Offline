using System.Collections;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.ConnectThePipes;

public class CTPWaterFlowManager : MonoBehaviour
{
	[SerializeField]
	private GameObject waterFillParticlePrefab;

	[SerializeField]
	private GameObject waterEndParticlePrefab;

	[SerializeField]
	private float waterDrainTime = 3f;

	[SerializeField]
	private GameObject[] waterPuddlePrefabs;

	[SerializeField]
	private AnimationCurve waterPuddleCurve;

	[SerializeField]
	private AnimationCurve waterJetCurve;

	private bool hasDeadEnd;

	private CTPPipe[,] pipeGrid;

	private GameObject currentLevelInstance;

	private float waterFlowStartTime;

	private float waterFlowDuration;

	public bool IsWaterFlowing { get; protected set; }

	public int ActiveStreams { get; protected set; }

	public void StartWaterFlowFromBeginPipes(GameObject currentLevelInstance, float startWaterWaitTime)
	{
		StartCoroutine(doStartWaterFlowFromBeginPipes(currentLevelInstance, startWaterWaitTime));
	}

	private IEnumerator doStartWaterFlowFromBeginPipes(GameObject currentLevelInstance, float startWaterWaitTime)
	{
		yield return new WaitForSeconds(startWaterWaitTime);
		AudioController.Play("CTP_water_rushing_through_pipes_loop");
		Shader.SetGlobalFloat("_Drain", 0f);
		this.currentLevelInstance = currentLevelInstance;
		IsWaterFlowing = true;
		setupPipeGrid();
		hasDeadEnd = false;
		ActiveStreams = 0;
		waterFlowStartTime = Time.time;
		GameEvents.Invoke(new ConnectThePipesGame.WaterFlowStarted());
		CTPPipe[] componentsInChildren = currentLevelInstance.GetComponentsInChildren<CTPPipe>();
		foreach (CTPPipe pipe in componentsInChildren)
		{
			if (pipe is CTPBeginPipe)
			{
				StartCoroutine(doWaterFlow(pipe.transform.localPosition, Vector3.right));
			}
		}
	}

	public void StartWaterFlowFromPosition(Vector3 pos, Vector3 inDir, int startDelay = 0)
	{
		StartCoroutine(doWaterFlow(pos, inDir, startDelay));
	}

	public void StartWaterDrainEffect()
	{
		StartCoroutine(doDrainEffect());
	}

	private IEnumerator doDrainEffect()
	{
		CTPPipe[] filledPipes = (from pipe in currentLevelInstance.GetComponentsInChildren<CTPPipe>()
			where pipe.HasWaterInTube()
			select pipe).ToArray();
		GameEvents.Invoke(new ConnectThePipesGame.WaterFlowStopped());
		Stopwatch sw = new Stopwatch(waterDrainTime);
		AudioController.Stop("CTP_water_rushing_through_pipes_loop");
		while ((bool)sw)
		{
			yield return null;
			Shader.SetGlobalFloat("_Drain", sw.Progress);
		}
		for (int i = 0; i < filledPipes.Length; i++)
		{
			filledPipes[i].ResetWater();
		}
		yield return new WaitForSeconds(1f);
		IsWaterFlowing = false;
		onWaterStoppedDraining();
	}

	private IEnumerator doWaterFlow(Vector3 pos, Vector3 inDir, int startDelay = 0)
	{
		GameObject waterFlowParticles = null;
		if (getPipeAt(pos) != null)
		{
			waterFlowParticles = Object.Instantiate(waterFillParticlePrefab, getPipeAt(pos).transform.position, getPipeAt(pos).transform.rotation) as GameObject;
			StartCoroutine(setActiveAfter(waterFlowParticles, (float)startDelay * 0.5f));
		}
		ActiveStreams++;
		Vector3 outDir = Vector3.zero;
		int count = 0;
		while (getPipeAt(pos) != null && getPipeAt(pos).CanFlowWater(inDir, out outDir))
		{
			getPipeAt(pos).StartDoFillWaterEffect(inDir, waterFlowParticles, startDelay + count++);
			yield return null;
			pos += outDir;
			inDir = outDir;
		}
		float waitTime = ((float)startDelay + (float)count) * 0.5f;
		if (waterFlowDuration < waitTime)
		{
			waterFlowDuration = waitTime;
		}
		yield return new WaitForSeconds(waitTime);
		if (waterFlowParticles != null && waterFlowParticles.transform.HasComponent<ParticleSystem>())
		{
			waterFlowParticles.GetComponent<ParticleSystem>().Stop(withChildren: true);
		}
		if (getPipeAt(pos) == null || (!getPipeAt(pos).HasComponent<CTPEndPipe>() && outDir == Vector3.zero))
		{
			StartCoroutine(makeWaterPuddleAt(getPipeAt(pos - inDir), inDir));
			StartCoroutine(spawnWaterJetAt(getPipeAt(pos - inDir), inDir));
			AudioController.Play("CTP_water_spillage", getPipeAt(pos - inDir).transform);
			hasDeadEnd = true;
		}
		if (--ActiveStreams <= 0)
		{
			onWaterStoppedFlowing();
		}
	}

	private IEnumerator setActiveAfter(GameObject waterFlowParticles, float startDelay)
	{
		waterFlowParticles.SetActive(value: false);
		yield return new WaitForSeconds(startDelay);
		waterFlowParticles.SetActive(value: true);
	}

	private void onWaterStoppedFlowing()
	{
		if (!hasDeadEnd && currentLevelInstance.GetComponentsInChildren<CTPEndPipe>().All((CTPEndPipe p) => p.HasWaterInTube()))
		{
			IsWaterFlowing = false;
			GameEvents.Invoke(new ConnectThePipesGame.LevelCompletedEvent());
		}
		else
		{
			StartWaterDrainEffect();
		}
	}

	private IEnumerator spawnWaterJetAt(CTPPipe pipe, Vector3 lookDir)
	{
		float duration = waterFlowStartTime - Time.time + waterFlowDuration + waterDrainTime;
		Stopwatch sw = new Stopwatch(duration);
		ParticleSystem particles = (Object.Instantiate(waterEndParticlePrefab, pipe.transform.position + lookDir / 2f, Quaternion.LookRotation(lookDir)) as GameObject).GetComponentInChildren<ParticleSystem>();
		float startSpeed = particles.startSpeed;
		float startSize = particles.startSize;
		while ((bool)sw)
		{
			yield return null;
			float t = 1f - waterJetCurve.Evaluate(sw.Progress);
			particles.startSpeed = startSpeed * t;
			particles.startSize = startSize * t;
		}
		particles.Stop(withChildren: true);
	}

	private IEnumerator makeWaterPuddleAt(CTPPipe pipe, Vector3 lookDir)
	{
		GameObject puddle = Object.Instantiate(waterPuddlePrefabs.GetRandom());
		puddle.transform.position = pipe.transform.position + lookDir + new Vector3(0f, -0.25f);
		Material mat = puddle.GetComponentInChildren<Renderer>().material;
		float duration = waterFlowStartTime - Time.time + waterFlowDuration + waterDrainTime;
		Stopwatch sw = new Stopwatch(duration);
		float cur = 1f - waterPuddleCurve.Evaluate(0f);
		while ((bool)sw)
		{
			yield return null;
			float next = 1f - waterPuddleCurve.Evaluate(sw.Progress);
			if (next > cur)
			{
				mat.SetInt("_UseAlpha", 1);
			}
			else
			{
				mat.SetInt("_UseAlpha", 0);
			}
			mat.SetFloat("_Progress", next);
			cur = next;
		}
		Object.Destroy(puddle);
	}

	private void onWaterStoppedDraining()
	{
		GameEvents.Invoke(new ConnectThePipesGame.LevelFailedEvent());
	}

	private CTPPipe getPipeAt(Vector3 pos)
	{
		int num = Mathf.RoundToInt(pos.x);
		int num2 = Mathf.RoundToInt(0f - pos.z);
		if (num >= 0 && num2 >= 0 && num < pipeGrid.GetLength(0) && num2 < pipeGrid.GetLength(1))
		{
			return pipeGrid[num, num2];
		}
		return null;
	}

	private void setupPipeGrid()
	{
		CTPPipe[] componentsInChildren = currentLevelInstance.GetComponentsInChildren<CTPPipe>();
		int num = componentsInChildren.Max((CTPPipe pipe) => (int)Mathf.Abs(pipe.transform.localPosition.x));
		int num2 = componentsInChildren.Max((CTPPipe pipe) => (int)Mathf.Abs(pipe.transform.localPosition.z));
		pipeGrid = new CTPPipe[num + 1, num2 + 1];
		CTPPipe[] array = componentsInChildren;
		foreach (CTPPipe cTPPipe in array)
		{
			pipeGrid[Mathf.RoundToInt(cTPPipe.transform.localPosition.x), Mathf.RoundToInt(0f - cTPPipe.transform.localPosition.z)] = cTPPipe;
		}
	}
}
