using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public EnemySpawnManager enemySpawnManager;
	public HumanSpawnManager humanSpawnManager;
	[Scene]
	public string endScene;

	private float gameTimer;
	private bool firstSpawn;
	private int points = 0;

	private void Start()
	{
		humanSpawnManager.PopulateLevel(15);

		// SpawnEnemies();
		enemySpawnManager.PopulateLevel(5);

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

		if (enemySpawnManager.GetAliveEnemies() == 0)
		{
			PlayerPrefs.SetInt("score", UIManager.instance.points);
			int highScore = PlayerPrefs.GetInt("highscore", 9999);
			if (UIManager.instance.points > highScore)
			{
				PlayerPrefs.SetInt("highscore", UIManager.instance.points);
			}
			SceneManager.LoadScene(endScene);
		}
	}

	public void SpawnEnemies()
	{
		enemySpawnManager.PopulateLevel(5);
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
		UIManager.instance.SetPoints(points);
	}
}
