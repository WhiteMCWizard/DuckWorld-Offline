using System.Collections.Generic;
using UnityEngine;

namespace CinemaDirector;

public class CutsceneQueue : MonoBehaviour
{
	public List<Cutscene> Cutscenes;

	private int index;

	private void Start()
	{
		if (Cutscenes != null && Cutscenes.Count > 0)
		{
			Cutscenes[index].CutsceneFinished += CutsceneQueue_CutsceneFinished;
			Cutscenes[index].Play();
		}
	}

	private void CutsceneQueue_CutsceneFinished(object sender, CutsceneEventArgs e)
	{
		Cutscenes[index].CutsceneFinished -= CutsceneQueue_CutsceneFinished;
		if (Cutscenes != null && index + 1 < Cutscenes.Count)
		{
			index++;
			Cutscenes[index].Play();
			Cutscenes[index].CutsceneFinished += CutsceneQueue_CutsceneFinished;
		}
	}
}
