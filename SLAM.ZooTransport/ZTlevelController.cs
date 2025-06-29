using UnityEngine;

namespace SLAM.ZooTransport;

public class ZTlevelController : MonoBehaviour
{
	private enum Direction
	{
		Down,
		Straight,
		Up
	}

	[SerializeField]
	private int[] roadPiecesPerDifficulty = new int[3];

	[SerializeField]
	private GameObject[] levels;

	[SerializeField]
	private GameObject[] cargoSets;

	[SerializeField]
	private GameObject EndRoad;

	[SerializeField]
	private GameObject[] highRamps;

	[SerializeField]
	private GameObject[] longRamps;

	[SerializeField]
	private GameObject[] shortRamps;

	[SerializeField]
	private GameObject[] straightRoads;

	private GameObject[] roadPieces;

	private float downChance = 0.5f;

	private float straightChance = 0.5f;

	private float steepChance = 0.5f;

	private int difficulty;

	private ZTcargoCheck cargo;

	private Direction currentDir;

	public void SetCargo(ZTcargoCheck cargo)
	{
		this.cargo = cargo;
	}

	public void SetDifficulty(int difficulty)
	{
		this.difficulty = difficulty;
	}

	public void ReloadLevel()
	{
		LoadLevel(difficulty);
	}

	public void LoadLevel(int difficulty)
	{
		for (int i = 0; i < levels.Length; i++)
		{
			levels[i].SetActive(i == difficulty);
		}
		cargo.SetCargo(cargoSets[difficulty]);
	}

	public void GenerateLevel(int difficulty)
	{
		roadPieces = new GameObject[roadPiecesPerDifficulty[difficulty]];
		currentDir = Direction.Straight;
		bool flag = false;
		for (int i = 0; i < roadPieces.Length; i++)
		{
			if (i == 0)
			{
				roadPieces[i] = Object.Instantiate(straightRoads[Random.Range(0, straightRoads.Length)]);
				roadPieces[i].transform.position = new Vector3(20f, 0f, 0f);
				continue;
			}
			Direction direction = currentDir;
			float value = Random.value;
			if ((value < straightChance && !flag && i != roadPieces.Length - 2) || i == roadPieces.Length - 1)
			{
				roadPieces[i] = Object.Instantiate(straightRoads[Random.Range(0, straightRoads.Length)]);
				currentDir = Direction.Straight;
				straightChance -= 0.2f;
			}
			else
			{
				value = Random.value;
				if (value < steepChance && currentDir != Direction.Straight && i != roadPieces.Length - 2)
				{
					roadPieces[i] = Object.Instantiate(highRamps[Random.Range(0, highRamps.Length)]);
					flag = true;
					steepChance -= 0.125f;
				}
				else
				{
					roadPieces[i] = Object.Instantiate(longRamps[Random.Range(0, longRamps.Length)]);
					flag = false;
					if (currentDir != Direction.Straight)
					{
						steepChance = Mathf.Clamp(steepChance + 0.125f, 0f, 1f);
					}
				}
				if (currentDir == Direction.Straight)
				{
					if (Random.value < downChance)
					{
						currentDir = Direction.Down;
						downChance = Mathf.Clamp(downChance - 0.1f, 0f, 1f);
					}
					else
					{
						currentDir = Direction.Up;
						downChance = Mathf.Clamp(downChance + 0.1f, 0f, 1f);
					}
				}
				if (currentDir == Direction.Down)
				{
					roadPieces[i].transform.Rotate(Vector3.up, 180f);
				}
				straightChance += 0.2f;
			}
			Vector3 position = roadPieces[i - 1].transform.position;
			position.x += roadPieces[i - 1].GetComponent<Collider>().bounds.extents.x + roadPieces[i].GetComponent<Collider>().bounds.extents.x;
			if (direction == Direction.Up)
			{
				float num = roadPieces[i - 1].GetComponent<Collider>().bounds.max.y - roadPieces[i - 1].transform.position.y;
				position.y += num;
			}
			else if (currentDir == Direction.Down)
			{
				float num2 = roadPieces[i].GetComponent<Collider>().bounds.max.y - roadPieces[i].transform.position.y;
				position.y -= num2;
			}
			if (i == roadPieces.Length / 2)
			{
				position.y -= 1.5f;
			}
			position.z = 0f;
			roadPieces[i].transform.position = position;
		}
		EndRoad.transform.position = roadPieces[roadPieces.Length - 1].transform.position + Vector3.right * 4f;
	}
}
