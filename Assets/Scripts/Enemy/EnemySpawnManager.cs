using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
	public LanderEnemy landerPrefab;
	public BomberEnemy bomberPrefab;
	public Transform landerSpawnY;

	public Transform scroller;
	public int maxLanders;
	public int maxBombers;

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

			float randX = UnityEngine.Random.Range(-50, 51);
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

		if (humans.Count == 0)
		{
			return true;
		}

		if (spawnedLanders.Count >= maxLanders)
		{
			return true;
		}

		for (int i = 0; i < enemyCount; i++)
		{
			if (humans.Count == 0)
			{
				return true;
			}

			if (spawnedLanders.Count >= maxLanders)
			{
				return true;
			}

			float randX = UnityEngine.Random.Range(-50, 51);
			Vector2 pos = new Vector2(randX, landerSpawnY.position.y);
			LanderEnemy enemy = Instantiate(landerPrefab, pos, Quaternion.identity, scroller);

			Human nonTargeted = humans[UnityEngine.Random.Range(0, humans.Count)];

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
		this.maxLanders = max;
	}

	public void SetMaxBombers(int max)
	{
		spawnedBombers.Clear();
		this.maxBombers = max;
	}


	public int GetAliveEnemies()
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

		foreach (EnemyController enemy in FindObjectsOfType<MutantEnemy>())
		{
			if (enemy != null)
			{
				i++;
			}
		}
		return i;
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

		foreach (EnemyController enemy in FindObjectsOfType<MutantEnemy>())
		{
			enemy.Freeze();
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

		foreach (EnemyController enemy in FindObjectsOfType<MutantEnemy>())
		{
			enemy.UnFreeze();
		}
	}
}
