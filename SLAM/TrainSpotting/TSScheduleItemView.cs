using System;
using System.Collections;
using UnityEngine;

namespace SLAM.TrainSpotting;

public class TSScheduleItemView : MonoBehaviour
{
	[SerializeField]
	private UILabel lblTime;

	[SerializeField]
	private UILabel lblTrack;

	[SerializeField]
	private UISprite sprtBackground;

	[SerializeField]
	private Color incorrectColor = Color.red;

	[SerializeField]
	private Color correctColor = Color.green;

	[SerializeField]
	private Color clickedColor = Color.blue;

	[SerializeField]
	private Color trackAssignedColor = Color.gray;

	[SerializeField]
	private Color alarmedColor = Color.yellow;

	private bool trainIsDeparted;

	private UIGrid crowdGrid;

	private int crowdCounter;

	public TrainSpottingGame.TrainInfo CurrentTrain { get; protected set; }

	public void SetInfo(TrainSpottingGame.TrainInfo trainInfo)
	{
		CurrentTrain = trainInfo;
		updateTimeLabel();
		StartCoroutine(monitorTrain());
	}

	private void OnEnable()
	{
		GameEvents.Subscribe<TrainSpottingGame.TrainArrivedEvent>(onTrainArrived);
		GameEvents.Subscribe<TrainSpottingGame.TrainDepartedEvent>(onTrainDeparted);
		GameEvents.Subscribe<TrainSpottingGame.TrainPassedByEvent>(onTrainPassedBy);
		GameEvents.Subscribe<TrainSpottingGame.CrowdEnteredTrain>(onCrowdEnteredTrain);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<TrainSpottingGame.TrainArrivedEvent>(onTrainArrived);
		GameEvents.Unsubscribe<TrainSpottingGame.TrainDepartedEvent>(onTrainDeparted);
		GameEvents.Unsubscribe<TrainSpottingGame.TrainPassedByEvent>(onTrainPassedBy);
		GameEvents.Unsubscribe<TrainSpottingGame.CrowdEnteredTrain>(onCrowdEnteredTrain);
	}

	private void Start()
	{
		crowdGrid = GetComponentInChildren<UIGrid>();
		crowdGrid.gameObject.SetActive(value: false);
	}

	private void onCrowdEnteredTrain(TrainSpottingGame.CrowdEnteredTrain evt)
	{
		if (evt.TrainInfo == CurrentTrain)
		{
			crowdGrid.GetChild(crowdCounter).GetComponent<UIToggle>().value = true;
			crowdCounter++;
		}
	}

	private void onTrainArrived(TrainSpottingGame.TrainArrivedEvent evt)
	{
		if (evt.TrainInfo == CurrentTrain)
		{
			crowdGrid.gameObject.SetActive(value: true);
			lblTime.color = Color.white;
			lblTrack.color = Color.white;
			lblTrack.text = evt.Track.TrackName;
			base.gameObject.name = evt.Track.TrackName;
			GetComponentInParent<UIGrid>().Reposition();
			GetComponentInParent<UIGrid>().repositionNow = true;
			StartCoroutine(animateToColor(trackAssignedColor, 0.5f * Time.timeScale));
			sprtBackground.GetComponent<UIButton>().defaultColor = trackAssignedColor;
		}
	}

	private IEnumerator monitorTrain()
	{
		TrainSpottingGame gameController = UnityEngine.Object.FindObjectOfType<TrainSpottingGame>();
		while (gameController.AbsoluteElapsedTime < CurrentTrain.TargetDepartureTime)
		{
			yield return null;
		}
		TrainSpottingGame.TrainShouldDepartEvent trainShouldDepartEvent = new TrainSpottingGame.TrainShouldDepartEvent();
		trainShouldDepartEvent.scheduleItem = this;
		trainShouldDepartEvent.trainInfo = CurrentTrain;
		GameEvents.Invoke(trainShouldDepartEvent);
		while (!trainIsDeparted)
		{
			AudioController.Play("TS_time_up");
			yield return StartCoroutine(animateToColor(alarmedColor, 0.5f));
			yield return StartCoroutine(animateToColor(trackAssignedColor, 0.5f));
		}
	}

	private void onTrainDeparted(TrainSpottingGame.TrainDepartedEvent evt)
	{
		if (evt.TrainInfo == CurrentTrain)
		{
			StopAllCoroutines();
			trainIsDeparted = true;
			StartCoroutine(doTrainDepartedSequence(evt.WasOnTime));
		}
	}

	private IEnumerator doTrainDepartedSequence(bool wasOnTime)
	{
		if (base.enabled && base.gameObject.activeInHierarchy)
		{
			yield return StartCoroutine(animateToColor((!wasOnTime) ? incorrectColor : correctColor, 0.5f));
		}
		if (base.enabled && base.gameObject.activeInHierarchy)
		{
			StartCoroutine(doRemoveScheduleItemSequence());
		}
	}

	private void onTrainPassedBy(TrainSpottingGame.TrainPassedByEvent evt)
	{
		if (evt.TrainInfo == CurrentTrain)
		{
			StartCoroutine(doTrainPassedBySequence());
		}
	}

	private IEnumerator doTrainPassedBySequence()
	{
		yield return StartCoroutine(animateToColor(incorrectColor, 0.5f * Time.timeScale));
		base.gameObject.AddComponent<Rigidbody>();
		GetComponent<Rigidbody>().AddTorque(base.transform.forward * (UnityEngine.Random.value - 0.5f) * 50f);
		GetComponentInChildren<Collider>().enabled = false;
		StartCoroutine(doRemoveScheduleItemSequence());
	}

	private IEnumerator doRemoveScheduleItemSequence()
	{
		yield return StartCoroutine(animateToColor(sprtBackground.color, new Color(sprtBackground.color.r, sprtBackground.color.g, sprtBackground.color.b, 0f), 0.5f));
		GetComponentInParent<UIGrid>().RemoveChild(base.transform);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private IEnumerator animateToColor(Color endColor, float time)
	{
		return animateToColor(sprtBackground.color, endColor, time);
	}

	private IEnumerator animateToColor(Color startColor, Color endColor, float time)
	{
		Stopwatch sw = new Stopwatch(time);
		while ((bool)sw)
		{
			yield return null;
			sprtBackground.color = Color.Lerp(startColor, endColor, sw.Progress);
		}
	}

	private void updateTimeLabel()
	{
		TimeSpan timeSpan = TimeSpan.FromSeconds(CurrentTrain.TargetDepartureTime);
		if (CurrentTrain.TimeMode == TrainSpottingGame.TimeMode.Analog)
		{
			lblTime.text = TS_TimeManager.GetWrittenTime(CurrentTrain.TargetDepartureTime);
		}
		else if (CurrentTrain.TimeMode == TrainSpottingGame.TimeMode.Digital24)
		{
			lblTime.text = $"{timeSpan.Hours:00}:{timeSpan.Minutes:00}";
		}
		else
		{
			lblTime.text = $"{((timeSpan.Hours <= 12) ? timeSpan.Hours : (timeSpan.Hours - 12)):00}:{timeSpan.Minutes:00}";
		}
	}

	public void OnItemClicked()
	{
		if (base.gameObject.activeInHierarchy)
		{
			TrainSpottingGame.TrainScheduleItemClickedEvent trainScheduleItemClickedEvent = new TrainSpottingGame.TrainScheduleItemClickedEvent();
			trainScheduleItemClickedEvent.ScheduleItem = this;
			GameEvents.Invoke(trainScheduleItemClickedEvent);
			if (GetComponentInChildren<UIButton>() != null)
			{
				GetComponentInChildren<UIButton>().defaultColor = clickedColor;
			}
		}
	}

	public void UndoHighlight()
	{
		GetComponentInChildren<UIButton>().ResetDefaultColor();
	}
}
