using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public EnemySpawnManager enemySpawnManager;
	public HumanSpawnManager humanSpawnManager;
	public UIManager uiManager;

	private float gameTimer;
	private bool firstSpawn;
	private int points = 0;

	private void Start()
	{
		humanSpawnManager.PopulateLevel(10);

		// SpawnEnemies();
		enemySpawnManager.PopulateLevel(10);

		Application.targetFrameRate = 144;
	}

	private void Update()
	{
		if (gameTimer >= 2f && !firstSpawn)
		{
			SpawnEnemies();
			firstSpawn = true;
			gameTimer = 0;
		}

		if (gameTimer >= 5f)
		{
			SpawnEnemies();
			gameTimer = 0;
		}

		gameTimer += Time.deltaTime;
	}

	public void SpawnEnemies()
	{
		// enemySpawnManager.PopulateLevel(5);
	}

	public void FreezeEnemies()
	{
		enemySpawnManager.FreezeEnemies();
	}

	public void UnFreezeEnemies()
	{
		enemySpawnManager.UnFreezeEnemies();
	}


	public void Bomb()
	{
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		foreach (GameObject enemy in enemies)
		{
			if (enemy.transform.GetChild(0).GetComponent<SpriteRenderer>().isVisible)
			{
				Destroy(enemy);
				++points;
			}
		}
		uiManager.SetPoints(points);
	}
}
