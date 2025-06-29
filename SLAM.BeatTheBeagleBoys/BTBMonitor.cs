using System.Collections;
using SLAM.Engine;
using UnityEngine;

namespace SLAM.BeatTheBeagleBoys;

[RequireComponent(typeof(FiniteStateMachine))]
public class BTBMonitor : MonoBehaviour
{
	protected const string STATE_ACTIVE = "Active";

	protected const string STATE_ALARMED = "Alarmed";

	private const int FRAME_WIDTH = 512;

	private const int FRAME_HEIGHT = 512;

	[SerializeField]
	private float noiseTime = 1f;

	[SerializeField]
	private AudioClip alarmClip;

	protected BTBArea currentArea;

	protected float lastActivationTime;

	protected FiniteStateMachine stateMachine;

	private BTBCamera _currentCam;

	private Renderer monitorRenderer;

	private bool areControlsLocked = true;

	private bool animalStolen;

	private IEnumerator alarm;

	public BTBArea Area => currentArea;

	public bool AreControlsLocked
	{
		get
		{
			return areControlsLocked;
		}
		set
		{
			areControlsLocked = value;
		}
	}

	public bool IsAlarmed => stateMachine.CurrentState.Name == "Alarmed";

	public float ActiveTime => Time.time - lastActivationTime;

	protected BTBCamera currentCamera
	{
		get
		{
			return _currentCam;
		}
		set
		{
			if (value != null)
			{
				monitorRenderer.material.SetTexture("_MainTex", value.GetRenderTexture(512, 512));
			}
			_currentCam = value;
		}
	}

	public int UsedCount { get; set; }

	protected virtual void OnEnable()
	{
		GameEvents.Subscribe<BTBGameController.AnimalStolenEvent>(onAnimalStolen);
		GameEvents.Subscribe<BTBGameController.ThiefExitedEvent>(onThiefExited);
	}

	protected virtual void OnDisable()
	{
		GameEvents.Unsubscribe<BTBGameController.AnimalStolenEvent>(onAnimalStolen);
		GameEvents.Unsubscribe<BTBGameController.ThiefExitedEvent>(onThiefExited);
	}

	protected virtual void Awake()
	{
		monitorRenderer = GetComponent<Renderer>();
		stateMachine = GetComponent<FiniteStateMachine>();
		stateMachine.AddState("Active", OnEnterStateActive, WhileStateActive, null);
		stateMachine.AddState("Alarmed", OnEnterStateAlarmed, WhileStateAlarmed, null);
		stateMachine.SwitchTo("Active");
		monitorRenderer.material.SetTextureOffset("_NoiseTex", new Vector2(Random.value, Random.value));
	}

	public void DisplayArea(BTBArea area)
	{
		lastActivationTime = Time.time;
		currentArea = area;
		if (currentArea.HasThief)
		{
			UsedCount++;
		}
		BTBCamera bTBCamera = area.ActivateCamera();
		float num = 1f;
		bTBCamera.CameraComp.aspect = num;
		bTBCamera.CameraComp.rect = new Rect(0f, 1f - num, 1f, num);
		StopAllCoroutines();
		StartCoroutine(doSwitchCameraEffect(currentCamera, bTBCamera));
		stateMachine.SwitchTo("Active");
	}

	private void OnEnterStateActive()
	{
		animalStolen = true;
		if (alarm != null)
		{
			StopCoroutine(alarm);
		}
		StartCoroutine(animatePropertyTo("_AlarmAmount", 0f, 0.2f));
		StartCoroutine(animatePropertyTo("_EdgeHighlightStrength", 3.5f, 0.2f));
		StartCoroutine(animateColorTo("_EdgeHighlightColor", Color.green, 0.2f));
	}

	private void WhileStateActive()
	{
		if (currentArea != null && currentArea.Cage.IsOpened)
		{
			stateMachine.SwitchTo("Alarmed");
			BTBGameController.MonitorAlarmedEvent monitorAlarmedEvent = new BTBGameController.MonitorAlarmedEvent();
			monitorAlarmedEvent.monitor = this;
			GameEvents.Invoke(monitorAlarmedEvent);
		}
	}

	private void OnEnterStateAlarmed()
	{
		StartCoroutine(animatePropertyTo("_AlarmAmount", 1f, 0.2f));
		StartCoroutine(animatePropertyTo("_EdgeHighlightStrength", 1.5f, 0.2f));
		StartCoroutine(animateColorTo("_EdgeHighlightColor", Color.red, 0.2f));
		alarm = doWhileAlarmed();
		StartCoroutine(alarm);
	}

	private IEnumerator doWhileAlarmed()
	{
		animalStolen = false;
		while (!animalStolen)
		{
			yield return StartCoroutine(animatePropertyTo("_EdgeHighlightStrength", 100f, 0.2f));
			yield return StartCoroutine(animatePropertyTo("_EdgeHighlightStrength", 1.5f, 0.2f));
			yield return new WaitForSeconds(0.2f);
		}
	}

	private void WhileStateAlarmed()
	{
		if (alarmClip != null && !AudioController.IsPlaying(alarmClip.name))
		{
			AudioController.Play(alarmClip.name, base.transform);
		}
		if (currentArea == null || !currentArea.Cage.IsOpened)
		{
			stateMachine.SwitchTo("Active");
		}
	}

	private IEnumerator doSwitchCameraEffect(BTBCamera oldCamera, BTBCamera newCamera)
	{
		AudioController.Play("BTB_monitor_static_01", base.transform);
		if (oldCamera != null)
		{
			yield return StartCoroutine(animatePropertyTo("_NoiseAmount", 1f, noiseTime / 2f));
			oldCamera.enabled = false;
		}
		currentCamera = newCamera;
		currentCamera.enabled = true;
		yield return StartCoroutine(animatePropertyTo("_NoiseAmount", 0f, noiseTime / 2f));
	}

	private IEnumerator animatePropertyTo(string propertyName, float endAmount, float duration)
	{
		float startAmount = monitorRenderer.material.GetFloat(propertyName);
		Stopwatch sw = new Stopwatch(duration);
		while ((bool)sw)
		{
			yield return null;
			monitorRenderer.material.SetFloat(propertyName, Mathf.Lerp(startAmount, endAmount, sw.Progress));
		}
	}

	private IEnumerator animateColorTo(string propertyName, Color endColor, float duration)
	{
		Color startColor = monitorRenderer.material.GetColor(propertyName);
		Stopwatch sw = new Stopwatch(duration);
		while ((bool)sw)
		{
			yield return null;
			monitorRenderer.material.SetColor(propertyName, Color.Lerp(startColor, endColor, sw.Progress));
		}
	}

	private void onAnimalStolen(BTBGameController.AnimalStolenEvent evt)
	{
		if (evt.Area == Area)
		{
			animalStolen = true;
			AudioController.Play(evt.Area.Cage.animalClip.name, base.transform);
		}
	}

	private void onThiefExited(BTBGameController.ThiefExitedEvent evt)
	{
		if (evt.Area == Area)
		{
			StartCoroutine(animatePropertyTo("_Saturation", 0f, 0.2f));
		}
	}
}
