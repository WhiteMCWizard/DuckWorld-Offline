using UnityEngine;

namespace SLAM.ToolTips;

public class GameObjectIdentifier : MonoBehaviour
{
	[SerializeField]
	private Identifier id;

	private void OnEnable()
	{
		if (id != Identifier.None)
		{
			if (!IdentifierToGameObjectMapping.mapping.ContainsKey(id))
			{
				IdentifierToGameObjectMapping.mapping.Add(id, base.gameObject);
			}
			else
			{
				Debug.LogWarning("Hey Buddy, the identifier has already been assigned to another gameobject", IdentifierToGameObjectMapping.mapping[id]);
			}
		}
	}

	private void OnDisable()
	{
		if (id != Identifier.None && IdentifierToGameObjectMapping.mapping.ContainsKey(id))
		{
			IdentifierToGameObjectMapping.mapping.Remove(id);
		}
	}
}
