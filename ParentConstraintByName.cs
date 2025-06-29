using UnityEngine;

public class ParentConstraintByName : MonoBehaviour
{
	[SerializeField]
	private string targetName;

	private Transform targetTransform;

	private Transform cacheTransform;

	private void Start()
	{
		if (string.IsNullOrEmpty(targetName))
		{
			Debug.LogWarning("Hey Buddy! targetName is empty!");
			return;
		}
		GameObject gameObject = GameObject.Find(targetName);
		if (gameObject != null)
		{
			targetTransform = gameObject.transform;
			cacheTransform = base.transform;
		}
	}

	private void Update()
	{
		if (targetTransform != null)
		{
			cacheTransform.position = targetTransform.position;
			cacheTransform.rotation = targetTransform.rotation;
		}
	}
}
