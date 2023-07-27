using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	[SerializeField][Scene] private string endScene;

	[Header("Managers")]
	[SerializeField] private EnemySpawnManager enemySpawnManager;
	[SerializeField] private HumanSpawnManager humanSpawnManager;
	[SerializeField] private BossManager bossManager;

	[Header("Waves")]
	[SerializeField] private AttackWaveEndManager waveEndManager;
	[SerializeField] private int bossWave = 5;

	[System.Serializable] private struct EnemyCounts
	{
		public int landers;
		public int landerRate;
		public int bombers;
		public int bomberRate;
	}
	[SerializeField] private List<EnemyCounts> enemies;
	[SerializeField] private List<int> humans;
	
	public int spawnTimer = 5;

	[HideInInspector] public int wave = -1;

	private bool waveStarted;
	private bool allSpawned;
	private int landerCount;
	private int bomberCount;
	private int landerSpawnRate;
	private int bomberSpawnRate;
	private int humanCount;
	private float gameTimer;

	private void Start()
	{
		Application.targetFrameRate = -1;
		QualitySettings.vSyncCount = 0;

		Invoke(nameof(StartWave), 3f);
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
		if (bossManager.started)
		{
			return;
		}
		
		if (wave == bossWave)
		{
			StartBossFight();
			return;
		}

		if (!waveStarted)
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

	private void StartBossFight()
	{
		waveStarted = false;
		allSpawned = false;
		humanSpawnManager.DestroyHumans();
		bossManager.StartBoss();
	}

	private IEnumerator EndWave()
	{
		waveStarted = false;
		waveEndManager.gameObject.SetActive(true);
		waveEndManager.EndWave(wave + 1);

		humanSpawnManager.DestroyHumans();
		enemySpawnManager.DestroyEnemies();

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
