using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
	[SerializeField] private GameManager gameManager;
	[SerializeField] private BossController bossPrefab;

	public Transform bossSpawnPoint;
	public bool started;
	public bool debugStart;
	public int bossWave;

	[Header("Animations")]
	[SerializeField] private GameObject bossSpawn, bossVignette;
	
	private BossController boss;
	private bool lastFramePhase;

	private void Update()
	{
		if (debugStart)
		{
			StartBoss();
		}
		
		if (started)
		{
			FindObjectOfType<CameraOffset>().freeze = true;
		}

		if (boss != null)
		{
			if (!lastFramePhase && boss.secondPhase)
			{
				bossVignette.GetComponent<Animator>().SetTrigger("phase2");
			}
			lastFramePhase = boss.secondPhase;
		}
	}

	public void StartBoss()
	{
		StartCoroutine(MusicIntro());
	}

	private IEnumerator MusicIntro()
	{
		started = true;
		debugStart = false;
		float timer = 0f;
		while (timer < 4f)
		{
			StartCoroutine(AudioManager.instance.PlayBossIntro());
			yield return null;
			timer += Time.deltaTime;
		}
		yield return StartCoroutine(AudioManager.instance.PlayBossIntro());
	
		AudioManager.instance.PlayBossMusic();
		SpawnAnimation();
	}

	private void SpawnAnimation()
	{
		bossSpawn.SetActive(true);
		StartCoroutine(InstantiateBoss());
	}

	private IEnumerator InstantiateBoss()
	{
		yield return new WaitForSeconds(2.5f);
		SpawnBoss();

		bossVignette.SetActive(true);
		yield return new WaitForSeconds(0.1f);
		bossSpawn.SetActive(false);
	}

	private void SpawnBoss()
	{
		boss = Instantiate(bossPrefab, bossSpawnPoint.position, Quaternion.identity);
	}
}
