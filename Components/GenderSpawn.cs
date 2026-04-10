using SLAM.Avatar;
using UnityEngine;

public class GenderSpawn : MonoBehaviour
{
	[SerializeField]
	private GameObject malePrefab;

	[SerializeField]
	private GameObject femalePrefab;

	private void Start()
	{
		GameObject gameObject = ((AvatarSystem.GetPlayerConfiguration().Gender != AvatarSystem.Gender.Boy) ? Object.Instantiate(femalePrefab) : Object.Instantiate(malePrefab));
		gameObject.transform.parent = base.transform.parent;
		gameObject.transform.localPosition = base.transform.localPosition;
		gameObject.transform.localRotation = base.transform.localRotation;
		gameObject.transform.localScale = base.transform.localScale;
		Animator component = base.transform.parent.GetComponent<Animator>();
		if (component != null)
		{
			component.Rebind();
		}
		Object.Destroy(base.gameObject);
	}
}
