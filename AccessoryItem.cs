using UnityEngine;

public class AccessoryItem : MonoBehaviour
{
	private enum BindToObject
	{
		AvatarObjectA,
		AvatarObjectB,
		CousinObject,
		CousinHead,
		AvatarHead
	}

	[SerializeField]
	private BindToObject bindTo;

	[SerializeField]
	private Vector3 offset;

	private string[] objectPaths = new string[5] { "boy01_skel/Object_A_Loc", "boy01_skel/Object_B_Loc", "Root_Skel_Jnt/Object_Loc", "Root_Skel_Jnt/Hips_Jnt/Spline_Jnt/Chest_Jnt/Neck_Jnt/Head_Jnt", "boy01_skel/hips_jnt/spine_jnt/chest_jnt/neck_jnt/head_jnt" };

	private void Start()
	{
		Transform transform = null;
		foreach (Transform item in base.transform.parent)
		{
			if (item.name.Contains("Model_Master"))
			{
				transform = item;
				break;
			}
		}
		if (!(transform == null))
		{
			transform = transform.FindChild(objectPaths[(int)bindTo]);
			if (!(transform == null))
			{
				base.transform.parent = transform;
				base.transform.localPosition = offset;
				base.transform.localRotation = Quaternion.identity;
				Object.Destroy(this);
			}
		}
	}
}
