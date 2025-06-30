using System.Collections;
using SLAM.Engine;
using UnityEngine;

namespace SLAM.TrainSpotting;

public class TSTutorialView : View
{
	[SerializeField]
	private UILabel lblText;

	[SerializeField]
	private float lettersPerSecond = 0.005f;

	private void OnEnable()
	{
		StartCoroutine(doTutorialSequence());
	}

	private IEnumerator doTutorialSequence()
	{
		TrainSpottingGame.TrainInfo train = null;
		yield return CoroutineUtils.WaitForGameEvent(delegate(TrainSpottingGame.TrainQueuedEvent tq)
		{
			train = tq.TrainInfo;
		});
		Controller<TrainSpottingGame>().PauseTrains();
		doSetText("De trein naar {0} is net aangekomen. We moeten hem eerst op een spoor zetten, klik op de trein in het bord om hem te selecteren.", train.Destination);
		yield return CoroutineUtils.WaitForGameEvent<TrainSpottingGame.TrainScheduleItemClickedEvent>();
		doSetText("Goed gedaan! Klik nu op het bordje van een spoor om de trein daar heen te laten gaan.");
		yield return CoroutineUtils.WaitForGameEvent<TrainSpottingGame.TrainArrivedEvent>();
		doSetText(string.Format("De trein rijd nu naar spoor {0}, en moet om {1} wegrijden. Wacht tot de klok aangeeft dat het {1} is, en klik dan weer snel op het spoor om de trein te laten rijden.\nLet op! De passagiers moeten eerst in de trein zitten.", train.Track.TrackName, Controller<TrainSpottingGame>().GetFormattedTime(train.TargetDepartureTime)));
		bool hasDeparted = false;
		bool wasCorrect = false;
		CoroutineUtils.WaitForGameEvent(delegate(TrainSpottingGame.TrainDepartedEvent td)
		{
			wasCorrect = td.WasOnTime;
			hasDeparted = true;
		});
		while (Controller<TrainSpottingGame>().AbsoluteElapsedTime < train.TargetDepartureTime && !hasDeparted)
		{
			yield return null;
		}
		if (!hasDeparted)
		{
			doSetText($"De trein moet nu gaan, klik op spoor {train.Track.TrackName}!");
			while (!hasDeparted)
			{
				yield return null;
			}
		}
		if (!wasCorrect)
		{
			yield return doSetText("Oef, de trein is niet op tijd weggereden. Probeer het opnieuw!");
			Controller<TrainSpottingGame>().ResumeTrains();
			StartCoroutine(doTutorialSequence());
		}
		else
		{
			yield return doSetText("Goed gedaan! Je bent nu klaar om Trainspotting te spelen.");
			Controller<TrainSpottingGame>().ResumeTrains();
			yield return new WaitForSeconds(2f);
			Close();
		}
	}

	private Coroutine doSetText(string text, params object[] args)
	{
		StopCoroutine("setText");
		return StartCoroutine("setText", string.Format(text, args));
	}

	private IEnumerator setText(string text)
	{
		lblText.text = string.Empty;
		for (int i = 0; i < text.Length; i++)
		{
			lblText.text += text[i];
			yield return new WaitForSeconds(lettersPerSecond * Time.timeScale);
		}
	}
}
