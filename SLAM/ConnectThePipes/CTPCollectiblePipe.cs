using System.Collections;
using UnityEngine;

namespace SLAM.ConnectThePipes;

public class CTPCollectiblePipe : CTPPipe
{
	[SerializeField]
	private GameObject collectibleVisual;

	private new IEnumerator doFillWaterEffect(Vector3 waterInDirection, GameObject particleEffect, int startDelay)
	{
		yield return new WaitForSeconds((float)startDelay * 0.5f);
		yield return new WaitForSeconds(0.25f);
		AudioController.Play("CTP_pickup_duck", base.transform);
		collectibleVisual.gameObject.SetActive(value: false);
		ConnectThePipesGame.CollectiblePickupEvent collectiblePickupEvent = new ConnectThePipesGame.CollectiblePickupEvent();
		collectiblePickupEvent.Pipe = this;
		GameEvents.Invoke(collectiblePickupEvent);
	}

	public override void StartDoFillWaterEffect(Vector3 inDir, GameObject waterFlowParticles, int p)
	{
		base.StartDoFillWaterEffect(inDir, waterFlowParticles, p);
		StartCoroutine(doFillWaterEffect(inDir, waterFlowParticles, p));
	}

	public override void ResetWater()
	{
		collectibleVisual.gameObject.SetActive(value: true);
		ConnectThePipesGame.CollectibleLostEvent collectibleLostEvent = new ConnectThePipesGame.CollectibleLostEvent();
		collectibleLostEvent.Pipe = this;
		GameEvents.Invoke(collectibleLostEvent);
		base.ResetWater();
	}
}
