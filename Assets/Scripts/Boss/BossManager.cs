using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BossManager : MonoBehaviour
{
	public GameManager gameManager;
	public bool started;
	public bool debugStart;
	public int bossWave;

	public BossController bossPrefab;
	private BossController boss;
	public Transform bossSpawnPoint;

	[Header("Animations")]
	public GameObject bossSpawn;
	public GameObject bossVignette;
	
	public Volume postProcess;

	private bool lastFramePhase;

	void Start()
	{

	}

	void Update()
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

	public IEnumerator MusicIntro()
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

	public void SpawnAnimation()
	{
		bossSpawn.SetActive(true);
		StartCoroutine(InstantiateBoss());
	}

	public IEnumerator InstantiateBoss()
	{
		yield return new WaitForSeconds(2.5f);
		SpawnBoss();

		bossVignette.SetActive(true);
		yield return new WaitForSeconds(0.1f);
		bossSpawn.SetActive(false);
	}

	public void SpawnBoss()
	{
		boss = Instantiate(bossPrefab, bossSpawnPoint.position, Quaternion.identity);
	}

}
