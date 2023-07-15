using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public EnemySpawnManager enemySpawnManager;
	public HumanSpawnManager humanSpawnManager;

	private float gameTimer;

	private void Start()
	{
		humanSpawnManager.PopulateLevel(10);
		enemySpawnManager.PopulateLevel(5);
		
		Application.targetFrameRate = 144;
	}

	private void Update()
	{
		if (gameTimer >= 2f)
		{
			enemySpawnManager.PopulateLevel(5);
			gameTimer = 0f;
		}
		
		gameTimer += Time.deltaTime;
	}
}
