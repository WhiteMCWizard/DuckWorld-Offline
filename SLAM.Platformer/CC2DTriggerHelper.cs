using UnityEngine;

namespace SLAM.Platformer;

public class CC2DTriggerHelper : MonoBehaviour
{
	private CharacterController2D _parentCharacterController;

	public void setParentCharacterController(CharacterController2D parentCharacterController)
	{
		_parentCharacterController = parentCharacterController;
	}

	private void OnTriggerEnter(Collider col)
	{
		if (col.isTrigger)
		{
			_parentCharacterController.OnTriggerEnter(col);
		}
	}

	private void OnTriggerStay(Collider col)
	{
		if (col.isTrigger)
		{
			_parentCharacterController.OnTriggerStay(col);
		}
	}

	private void OnTriggerExit(Collider col)
	{
		if (col.isTrigger)
		{
			_parentCharacterController.OnTriggerExit(col);
		}
	}

	private void OnDestroy()
	{
		CC2DPlayer component = _parentCharacterController.GetComponent<CC2DPlayer>();
		if (component != null)
		{
			component.OnActionKeyPressed = null;
		}
	}
}
