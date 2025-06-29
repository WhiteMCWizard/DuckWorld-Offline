using UnityEngine;

public class Truck : MonoBehaviour
{
	private void OnTruckArrive()
	{
		AudioController.Play("CO_truck_arrive");
		AudioController.Play("CO_truck_idle_loop");
	}

	private void OnTruckLeave()
	{
		AudioController.Play("CO_truck_depart");
	}
}
