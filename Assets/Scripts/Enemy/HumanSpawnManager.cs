using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanSpawnManager : MonoBehaviour
{
	[SerializeField] private GameObject humanprefab;
	[SerializeField] private Transform humanSpawnY, scroller;
	[SerializeField] private int leftMostSpawn = -50, rightMostSpawn = 51;

	private List<GameObject> humans = new List<GameObject>();

	public void PopulateLevel(int humanCount)
	{
		for (int i = 0; i < humanCount; i++)
		{
			float randX = Random.Range(leftMostSpawn, rightMostSpawn);
			Vector2 pos = new Vector2(randX, humanSpawnY.position.y);
			GameObject human = Instantiate(humanprefab, pos, Quaternion.identity, scroller);
			humans.Add(human);
		}
	}

	public void DestroyHumans()
	{
		foreach (GameObject human in humans)
		{
			if (human != null)
			{
				Destroy(human);
			}
		}
	}
}
