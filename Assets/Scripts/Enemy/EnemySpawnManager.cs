using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
	[Header("Enemy Types")]
	[SerializeField] private LanderEnemy landerPrefab;
	[SerializeField] private BomberEnemy bomberPrefab;

	[Header("Spawn Parameters")]
	[SerializeField] private Transform landerSpawnY, scroller;
	[SerializeField] private int leftMostSpawn = -50, rightMostSpawn = 51;
	[SerializeField] private int maxLanders, maxBombers;

	private List<Human> humans = new List<Human>();
	private List<EnemyController> spawnedLanders = new List<EnemyController>();
	private List<EnemyController> spawnedBombers = new List<EnemyController>();

	public bool SpawnBombers(int enemyCount)
	{
		if (spawnedBombers.Count >= maxBombers)
		{
			return true;
		}
		for (int i = 0; i < enemyCount; i++)
		{
			if (spawnedBombers.Count >= maxBombers)
			{
				return true;
			}

			float randX = Random.Range(leftMostSpawn, rightMostSpawn);
			Vector2 pos = new Vector2(randX, landerSpawnY.position.y);

			BomberEnemy enemy = Instantiate(bomberPrefab, pos, Quaternion.identity, scroller);
			enemy.Init();

			spawnedBombers.Add(enemy);
		}
		return false;
	}

	public bool SpawnLanders(int enemyCount)
	{
		humans.Clear();
		Human[] allHumans = FindObjectsOfType<Human>();
		foreach (Human human in allHumans)
		{
			if (!human.isTargeted && human.onGround)
			{
				humans.Add(human);
			}
		}

		if (humans.Count == 0 || spawnedLanders.Count >= maxLanders)
		{
			return true;
		}

		for (int i = 0; i < enemyCount; i++)
		{
			if (humans.Count == 0 || spawnedLanders.Count >= maxLanders)
			{
				return true;
			}

			float randX = Random.Range(leftMostSpawn, rightMostSpawn);
			Vector2 pos = new Vector2(randX, landerSpawnY.position.y);
			
			LanderEnemy enemy = Instantiate(landerPrefab, pos, Quaternion.identity, scroller);
			Human nonTargeted = humans[Random.Range(0, humans.Count)];
			enemy.Init();
			enemy.SetTarget(nonTargeted);

			spawnedLanders.Add(enemy);
			humans.Remove(nonTargeted);
		}
		return false;
	}

	public void SetMaxLanders(int max)
	{
		spawnedLanders.Clear();
		maxLanders = max;
	}

	public void SetMaxBombers(int max)
	{
		spawnedBombers.Clear();
		maxBombers = max;
	}

	public int GetAliveEnemyCount()
	{
		int i = 0;
		foreach (EnemyController enemy in spawnedLanders)
		{
			if (enemy != null)
			{
				i++;
			}
		}

		foreach (EnemyController enemy in spawnedBombers)
		{
			if (enemy != null)
			{
				i++;
			}
		}

		MutantEnemy[] spawnedMutants = FindObjectsOfType<MutantEnemy>();
		foreach (EnemyController enemy in spawnedMutants)
		{
			if (enemy != null)
			{
				i++;
			}
		}

		return i;
	}

	public void DestroyEnemies()
	{
		foreach (EnemyController enemy in spawnedLanders)
		{
			if (enemy != null)
			{
				Destroy(enemy.gameObject);
			}
		}

		foreach (EnemyController enemy in spawnedBombers)
		{
			if (enemy != null)
			{
				Destroy(enemy.gameObject);
			}
		}

		MutantEnemy[] spawnedMutants = FindObjectsOfType<MutantEnemy>();
		foreach (EnemyController enemy in spawnedMutants)
		{
			if (enemy != null)
			{
				Destroy(enemy.gameObject);
			}
		}
	}

	public void FreezeEnemies()
	{
		foreach (EnemyController enemy in spawnedLanders)
		{
			if (enemy != null)
			{
				enemy.Freeze();
			}
		}

		foreach (EnemyController enemy in spawnedBombers)
		{
			if (enemy != null)
			{
				enemy.Freeze();
			}
		}

		MutantEnemy[] spawnedMutants = FindObjectsOfType<MutantEnemy>();
		foreach (EnemyController enemy in spawnedMutants)
		{
			if (enemy != null)
			{
				enemy.Freeze();
			}
		}
	}

	public void UnFreezeEnemies()
	{
		foreach (EnemyController enemy in spawnedLanders)
		{
			if (enemy != null)
			{
				enemy.UnFreeze();
			}
		}

		foreach (EnemyController enemy in spawnedBombers)
		{
			if (enemy != null)
			{
				enemy.UnFreeze();
			}
		}

		MutantEnemy[] spawnedMutants = FindObjectsOfType<MutantEnemy>();
		foreach (EnemyController enemy in spawnedMutants)
		{
			if (enemy != null)
			{
				enemy.UnFreeze();
			}
		}
	}
}
