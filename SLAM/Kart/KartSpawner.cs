using UnityEngine;

namespace SLAM.Kart;

public abstract class KartSpawner : MonoBehaviour
{
	[SerializeField]
	protected bool spawnImmediately = true;

	[SerializeField]
	private Material materialOverride;

	public abstract KartConfigurationData Config { get; }

	protected virtual void Awake()
	{
		if (spawnImmediately)
		{
			SpawnKart();
		}
	}

	public virtual void SpawnKart()
	{
		KartSystem.UpdateKart(base.gameObject, Config);
		if (materialOverride != null)
		{
			applyMaterialOverride();
		}
	}

	private void applyMaterialOverride()
	{
		Renderer[] componentsInChildren = GetComponentsInChildren<Renderer>();
		foreach (Renderer renderer in componentsInChildren)
		{
			Material[] materials = renderer.materials;
			for (int j = 0; j < materials.Length; j++)
			{
				materials[j] = materialOverride;
			}
			renderer.materials = materials;
		}
	}
}
