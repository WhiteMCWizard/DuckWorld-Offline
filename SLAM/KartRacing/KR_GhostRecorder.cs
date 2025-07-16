using System.Collections.Generic;
using SLAM.Kart;
using SLAM.SaveSystem;
using SLAM.Webservices;
using UnityEngine;

namespace SLAM.KartRacing;

public class KR_GhostRecorder : MonoBehaviour
{
	private const float recordInterval = 0.2f;

	private float intervalTimer;

	private float time;

	private int gameIndex;

	private int trackIndex;

	private bool isRecording;

	private bool stopRecordingAfterNextRecord;

	private List<GhostFrameData> recording;

	private KR_KartBase kart;

	private void OnDrawGizmos()
	{
		if (recording != null)
		{
			Gizmos.color = Color.red;
			for (int i = 1; i < recording.Count; i++)
			{
				Gizmos.DrawSphere(recording[i].Position, 0.4f);
				Gizmos.DrawLine(recording[i].Position, recording[i].Position);
			}
		}
	}

	private void OnEnable()
	{
		GameEvents.Subscribe<KR_StartRaceEvent>(onRaceStarted);
		GameEvents.Subscribe<KR_FinishCrossedEvent>(onFinishCrossed);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<KR_StartRaceEvent>(onRaceStarted);
		GameEvents.Unsubscribe<KR_FinishCrossedEvent>(onFinishCrossed);
	}

	private void onRaceStarted(KR_StartRaceEvent evt)
	{
		gameIndex = evt.GameId;
		trackIndex = evt.TrackIndex;
		StartRecording();
	}

	private void onFinishCrossed(KR_FinishCrossedEvent evt)
	{
		if (evt.Kart.gameObject == base.gameObject)
		{
			kart = evt.Kart;
			StopRecording();
		}
	}

	private void onRecordingStopped()
	{
		SaveManager.Instance.SaveGhost(gameIndex, trackIndex, Mathf.CeilToInt(kart.Timer.CurrentTime * 100f), GetRecording());
	}

	private void Update()
	{
		if (!isRecording)
		{
			return;
		}
		time += Time.deltaTime;
		intervalTimer += Time.deltaTime;
		if (intervalTimer >= 0.2f)
		{
			intervalTimer -= 0.2f;
			recording.Add(new GhostFrameData
			{
				Timestamp = time,
				Position = base.transform.position,
				Rotation = base.transform.rotation.eulerAngles
			});
			if (stopRecordingAfterNextRecord)
			{
				isRecording = false;
				onRecordingStopped();
			}
		}
	}

	public void StartRecording()
	{
		isRecording = true;
		stopRecordingAfterNextRecord = false;
		time = 0f;
		recording = new List<GhostFrameData>();
		recording.Add(new GhostFrameData
		{
			Timestamp = time,
			Position = base.transform.position,
			Rotation = base.transform.root.eulerAngles
		});
	}

	public void StopRecording()
	{
		stopRecordingAfterNextRecord = true;
	}

	public GhostRecordingData GetRecording()
	{
		GhostRecordingData result = new GhostRecordingData
		{
			Records = recording
		};
		KR_PhysicsKart component;
		if ((component = GetComponent<KR_PhysicsKart>()) != null)
		{
			result.Kart = component.Spawner.Config;
		}
		else
		{
			result.Kart = GetComponentInChildren<KartSpawner>().Config;
		}
		return result;
	}
}
