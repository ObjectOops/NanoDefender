using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
	public LanderEnemy landerPrefab;
	public Transform landerSpawnY;
	
	public Transform scroller;

	private List<Human> humans = new List<Human>();

	public void PopulateLevel(int enemyCount)
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
			return;
		}

		for (int i = 0; i < enemyCount; i++)
		{
			if (humans.Count == 0)
			{
				return;
			}
			
			float randX = UnityEngine.Random.Range(-50, 51);
			Vector2 pos = new Vector2(randX, landerSpawnY.position.y);
			LanderEnemy enemy = Instantiate(landerPrefab, pos, Quaternion.identity, scroller);

			Human nonTargeted = humans[UnityEngine.Random.Range(0, humans.Count)];

			enemy.SetTarget(nonTargeted);

			humans.Remove(nonTargeted);
		}
	}

	private void Update()
	{

	}
}
