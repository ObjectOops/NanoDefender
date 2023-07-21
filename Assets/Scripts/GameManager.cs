using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public EnemySpawnManager enemySpawnManager;
	public HumanSpawnManager humanSpawnManager;
	public BossManager bossManager;

	[Scene]
	public string endScene;

	private float gameTimer;

	[Header("Waves")]
	public AttackWaveEndManager waveEndManager;
	public List<EnemyCounts> enemies;
	public List<int> humans;
	public int spawnTimer = 5;

	private bool waveStarted;
	private bool allSpawned;
	[HideInInspector]
	public int wave = -1;
	private int landerCount;
	private int bomberCount;
	private int landerSpawnRate;
	private int bomberSpawnRate;
	private int humanCount;


	[System.Serializable]
	public struct EnemyCounts
	{
		public int landers;
		public int landerRate;
		public int bombers;
		public int bomberRate;
	}


	private void Start()
	{
		Application.targetFrameRate = -1;
		QualitySettings.vSyncCount = 0;

		// SpawnEnemies();
		Invoke("StartWave", 3f);
	}

	private void StartWave()
	{
		wave++;

		gameTimer = 0f;

		landerCount = enemies[wave].landers;
		bomberCount = enemies[wave].bombers;
		landerSpawnRate = enemies[wave].landerRate;
		bomberSpawnRate = enemies[wave].bomberRate;
		humanCount = humans[wave];

		enemySpawnManager.SetMaxLanders(landerCount);
		enemySpawnManager.SetMaxBombers(bomberCount);

		humanSpawnManager.PopulateLevel(humanCount);
		enemySpawnManager.SpawnLanders(landerSpawnRate);
		enemySpawnManager.SpawnBombers(bomberSpawnRate);

		waveStarted = true;
	}

	private void Update()
	{
		if (!waveStarted)
		{
			return;
		}

		if (bossManager.bossWave == wave)
		{
			EndWaveForBoss();
			return;
		}

		if (bossManager.started)
		{
			return;
		}

		if (gameTimer >= spawnTimer)
		{
			bool allSpawned = SpawnEnemies(landerSpawnRate, bomberSpawnRate);
			this.allSpawned = allSpawned;
			gameTimer = 0;
		}

		gameTimer += Time.deltaTime;

		if (enemySpawnManager.GetAliveEnemyCount() == 0 && allSpawned)
		{
			StartCoroutine(EndWave());
		}
	}

	public void EndWaveForBoss()
	{
		waveStarted = false;
		humanSpawnManager.DestroyHumans();
		bossManager.StartBoss();
	}

	public IEnumerator EndWave()
	{
		waveStarted = false;
		waveEndManager.gameObject.SetActive(true);
		waveEndManager.EndWave(wave + 1);

		humanSpawnManager.DestroyHumans();

		yield return new WaitForSeconds(3f);
		FindObjectOfType<PlayerController>().ResetPlayer();
		waveEndManager.gameObject.SetActive(false);
		yield return new WaitForSeconds(2f);
		StartWave();
	}

	public void EndGame()
	{
		PlayerPrefs.SetInt("score", UIManager.instance.points);
		int highScore = PlayerPrefs.GetInt("highscore", 9999);
		if (UIManager.instance.points > highScore)
		{
			PlayerPrefs.SetInt("highscore", UIManager.instance.points);
		}
		SceneManager.LoadScene(endScene);
	}

	public bool SpawnEnemies(int landerCount, int bomberCount)
	{
		bool landersSpawned = enemySpawnManager.SpawnLanders(landerCount);
		bool bombersSpawned = enemySpawnManager.SpawnBombers(bomberCount);
		return landersSpawned && bombersSpawned;
	}

	public void FreezeEnemies()
	{
		enemySpawnManager.FreezeEnemies();
	}

	public void UnFreezeEnemies()
	{
		enemySpawnManager.UnFreezeEnemies();
	}

}
