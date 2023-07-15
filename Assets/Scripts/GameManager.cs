using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public EnemySpawnManager enemySpawnManager;
	public HumanSpawnManager humanSpawnManager;
	public UIManager uiManager;

	private float gameTimer;
	private int points = 0;

	private void Start()
	{
		humanSpawnManager.PopulateLevel(10);
		enemySpawnManager.PopulateLevel(5);
		
		Application.targetFrameRate = 144;

		// uiManager.SetPoints(points);
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

	public void AddPoints(int count)
    {
		points += count;
		uiManager.SetPoints(points);
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
