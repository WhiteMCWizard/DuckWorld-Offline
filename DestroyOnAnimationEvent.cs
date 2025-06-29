using UnityEngine;

public class DestroyOnAnimationEvent : MonoBehaviour
{
	[SerializeField]
	private GameObject objToDestroy;

	private void OnValidate()
	{
		if (objToDestroy == null)
		{
			objToDestroy = base.gameObject;
		}
	}

	public void DestroyObject()
	{
		Object.Destroy((!(objToDestroy == null)) ? objToDestroy : base.gameObject);
	}
}
