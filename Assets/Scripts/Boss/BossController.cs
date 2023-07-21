using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
	private Animator animator;
	private State state;
	private SpriteRenderer spriteRenderer;

	public Sprite minimapSprite;
	public int hits;
	public int secondPhaseHits;

	public bool secondPhase;

	[Header("Attack 1")]
	public EnemyBullet bossProjectile;
	public Transform attackOneSpawns;

	[Header("Attack 2")]
	public EnemyBullet bossProjectile2;
	public Transform attackTwoSpawns;
	public Transform attackTwoRotator;
	private Vector3 attackTwoStartPos;

	[Header("Attack 3")]
	public EnemyBullet bossProjectile3;
	public Transform attackThreeSpawns;

	[Header("Attack 4")]
	public EnemyBullet bossProjectile4;
	public Transform attackFourSpawns;
	private float attackFourTimer;
	private int prevProjectile;

	public float timeBetweenAttacks = 5f;
	private float attackTimer;

	private float targetX;

	private Vector3 originalPosition;
	private float moveLerpTimer;
	private bool dead;


	void Start()
	{
		animator = GetComponentInChildren<Animator>();
		spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		attackTwoStartPos = attackTwoRotator.transform.position;
		originalPosition = transform.position;
		FindObjectOfType<ScrollManager>().TintBoss();
		GetComponent<MinimapObject>().Init(minimapSprite);

		targetX = originalPosition.x - 3f;
	}

	public void Damage()
	{
		animator.SetTrigger("hit");
		if (!secondPhase && hits > 0)
		{
			hits--;
			if (hits <= 0)
			{
				secondPhase = true;
				attackFourTimer = 8f;
				attackTimer = 0f;
			}
		}

		if (secondPhase && secondPhaseHits > 0)
		{
			Vector3 newPosition = new Vector3(targetX, transform.position.y, transform.position.z);
			transform.position = Vector3.Lerp(originalPosition, newPosition, moveLerpTimer);
			moveLerpTimer += Time.deltaTime;
			secondPhaseHits--;
			if (secondPhaseHits <= 0)
			{
				Die();
			}
		}
	}

	private void Die()
	{
		animator.SetTrigger("death");
		dead = true;
	}

	void Update()
	{
		if (dead)
		{
			return;
		}
		FindObjectOfType<ScrollManager>().Scroll(new Vector2(Time.deltaTime * 5f, 0f));
		
		if (secondPhase)
		{
			if (attackFourTimer >= 8f)
			{
				AttackFourActions();
				attackFourTimer = 0;
			}
			attackFourTimer += Time.deltaTime;
		}

		switch (state)
		{
			case State.IDLE:
				IdleActions();
				IdleTransitions();
				break;
			case State.ATTACK_1:
				AttackOneActions();
				AttackOneTransitions();
				break;
			case State.ATTACK_2:
				AttackTwoActions();
				AttackTwoTransitions();
				break;
			case State.ATTACK_3:
				AttackThreeActions();
				AttackThreeTransitions();
				break;
			case State.DEAD:
				Debug.Log("DEAD!");
				break;
		}
	}

	private void IdleActions()
	{


	}

	private void IdleTransitions()
	{
		if (attackTimer >= timeBetweenAttacks)
		{
			attackTimer = 0f;

			int rand = UnityEngine.Random.Range(0, 3);

			state = rand switch
			{
				0 => State.ATTACK_1,
				1 => State.ATTACK_2,
				2 => State.ATTACK_3,
				_ => State.IDLE
			};
		}
		attackTimer += Time.deltaTime;
	}

	private void AttackOneActions()
	{
		animator.SetTrigger("attack1");
		Invoke("SpawnAttackOneProjectiles", 0.667f);
	}

	private void SpawnAttackOneProjectiles()
	{
		for (int i = 0; i < attackOneSpawns.childCount; i++)
		{
			Transform spawn = attackOneSpawns.GetChild(i);
			EnemyBullet bossProj = Instantiate(bossProjectile, spawn.position, Quaternion.identity, transform);
			bossProj.SetDirection(spawn.up);
		}
	}

	private void AttackOneTransitions()
	{
		state = State.IDLE;
	}

	private void AttackTwoActions()
	{
		animator.SetTrigger("attack2");
		Invoke("SpawnAttackTwoProjectiles", 0.4f);
		if (secondPhase)
		{

		}
	}

	private void SpawnAttackTwoProjectiles()
	{
		for (int i = 0; i < attackTwoSpawns.childCount; i++)
		{
			Transform spawn = attackTwoSpawns.GetChild(i);
			EnemyBullet bossProj = Instantiate(bossProjectile2, spawn.position, Quaternion.identity, attackTwoRotator);
			// bossProj.SetDirection(spawn.up);
			bossProj.SetDirection(Vector3.zero);
		}

		StartCoroutine(RotateAttackTwo());
	}

	public IEnumerator RotateAttackTwo()
	{
		float time = 0f;
		while (time < 7f)
		{
			attackTwoRotator.transform.position += Vector3.left * 5f * Time.deltaTime;
			attackTwoRotator.transform.rotation *= Quaternion.Euler(0, 0, 90f * Time.deltaTime);
			yield return null;
			time += Time.deltaTime;
		}
		for (int j = 0; j < attackTwoRotator.childCount; j++)
		{
			Transform proj = attackTwoRotator.GetChild(j);
			Destroy(proj.gameObject);
		}
		attackTwoRotator.transform.position = attackTwoStartPos;
		state = State.IDLE;
		yield return null;
	}

	private void AttackTwoTransitions()
	{
		state = State.ROTATE_ATTACK_2;
	}


	private void AttackThreeActions()
	{
		animator.SetTrigger("attack3");
		Invoke("SpawnAttackThreeProjectiles", 0.22f);
	}

	private void SpawnAttackThreeProjectiles()
	{
		for (int i = 0; i < attackThreeSpawns.childCount; i++)
		{
			Transform spawn = attackThreeSpawns.GetChild(i);
			EnemyBullet bossProj = Instantiate(bossProjectile3, spawn.position, spawn.rotation, transform);
			bossProj.SetDirection(spawn.up);
			bossProj.transform.rotation = Quaternion.Euler(spawn.rotation.eulerAngles.x, spawn.rotation.eulerAngles.y, spawn.rotation.eulerAngles.z - 180f);
		}
	}

	private void AttackThreeTransitions()
	{
		state = State.IDLE;
	}

	private void AttackFourActions()
	{
		StartCoroutine(AttackFourCycle());
	}

	private IEnumerator AttackFourCycle()
	{
		int count = 3;
		while (count > 0)
		{
			SpawnAttackFourProjectiles();
			yield return new WaitForSeconds(1f);
			count--;
		}
	}

	private void SpawnAttackFourProjectiles()
	{
		int attackPatterns = attackFourSpawns.childCount;
		int rand = UnityEngine.Random.Range(1, attackPatterns + 1);

		rand = prevProjectile switch
		{
			int i when i == 1 && rand == 3 => 2,
			int i when i == 3 && rand == 1 => 2,
			_ => rand

		};

		Transform attackPattern = attackFourSpawns.Find($"Attack_4_{rand}");

		for (int i = 0; i < attackPattern.childCount; i++)
		{
			Transform spawn = attackPattern.GetChild(i);
			EnemyBullet bossProj = Instantiate(bossProjectile4, spawn.position, Quaternion.identity, transform);
			bossProj.SetDirection(spawn.up);
		}

		prevProjectile = rand;
	}

	private void AttackFourTransitions()
	{
		state = State.IDLE;
	}

	public enum State
	{
		IDLE,
		ATTACK_1,
		ATTACK_2,
		ROTATE_ATTACK_2,
		ATTACK_3,
		DEAD
	}
}
