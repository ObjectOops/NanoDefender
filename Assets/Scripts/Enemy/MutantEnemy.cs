using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutantEnemy : EnemyController
{
	// Magic numbers ahead.

	public float moveSpeed, downUpSpeed, shootCoolDown = 2f, deathDuration = 0.517f;

	private State state;
	private float shootTimer;

	new void Start()
	{
		base.Start();
	}

	void Update()
	{
		if (player == null || freeze)
		{
			return;
		}

		ShootActions();

		float xDifAbs = Mathf.Abs(transform.position.x - player.transform.position.x);
		int dir = Math.Sign(transform.position.x - player.transform.position.x);
		if (xDifAbs > 0.2f)
		{
			// dir == -1 ? true : false
			spriteRenderer.flipX = dir == -1;
		}

		switch (state)
		{
			case State.TOWARDS_PLAYER:
				TowardsActions();
				TowardsTransitions();
				break;
			case State.DOWN:
				DownActions();
				DownTransitions();
				break;
			case State.UP:
				UpActions();
				UpTransitions();
				break;
		}
	}

	public override void Die()
	{
		GetComponent<Collider2D>().enabled = false;
		freeze = true;
		AudioManager.instance.PlaySound("Enemy Death");

		animator.SetTrigger("death");
		Invoke(nameof(Destroy), deathDuration);
	}

	private void TowardsActions()
	{
		float currentX = transform.position.x;

		int dirTowardsPlayer = (int)Mathf.Sign(player.transform.position.x - currentX);

		float newX = currentX + (dirTowardsPlayer * moveSpeed * Time.deltaTime);

		transform.position = new Vector2(newX, transform.position.y);
	}
	private void TowardsTransitions()
	{
		float enemyX = transform.position.x, enemyY = transform.position.y;
		float playerX = player.transform.position.x, playerY = player.transform.position.y;

		float differenceX = Mathf.Abs(enemyX - playerX), dirY = Mathf.Sign(enemyY - playerY);

		if (differenceX < 0.1f && dirY == 1)
		{
			state = State.DOWN;
		}
		else if (differenceX < 0.1f && dirY == -1)
		{
			state = State.UP;
		}
	}

	private void DownActions()
	{
		float currentY = transform.position.y;

		float newY = currentY - (downUpSpeed * Time.deltaTime);

		transform.position = new Vector2(transform.position.x, newY);
	}

	private void DownTransitions()
	{
		float enemyY = transform.position.y, enemyX = transform.position.x;
		float playerY = player.transform.position.y, playerX = player.transform.position.x;

		float differenceX = Mathf.Abs(enemyX - playerX), dirY = Mathf.Sign(enemyY - playerY);

		if (dirY == -1)
		{
			state = State.UP;
		}

		if (differenceX >= 0.5f)
		{
			state = State.TOWARDS_PLAYER;
		}
	}

	private void UpActions()
	{
		float currentY = transform.position.y;

		float newY = currentY + (downUpSpeed * Time.deltaTime);

		transform.position = new Vector2(transform.position.x, newY);
	}

	private void UpTransitions()
	{
		float enemyY = transform.position.y;
		float enemyX = transform.position.x;
		
		float playerY = player.transform.position.y;
		float playerX = player.transform.position.x;

		float differenceX = Mathf.Abs(enemyX - playerX), dirY = Mathf.Sign(enemyY - playerY);

		if (dirY == 1)
		{
			state = State.DOWN;
		}

		if (differenceX >= 0.5f)
		{
			state = State.TOWARDS_PLAYER;
		}
	}

	private void ShootActions()
	{
		if (state.Equals(State.EXPLODE) || !spriteRenderer.isVisible)
		{
			return;
		}

		if (shootTimer + shootOffset >= shootCoolDown)
		{
			shootTimer = 0f;
			StartCoroutine(ShootBurst());
		}

		shootTimer += Time.deltaTime;
	}

	private IEnumerator ShootBurst()
	{
		ShootOnce();
		yield return new WaitForSeconds(0.5f);
		ShootOnce();
	}

	private void ShootOnce()
	{
		// Adds inaccuracy to enemy projectiles.
		int rand = UnityEngine.Random.Range(0, 2);
		Vector3 playerPos = player.transform.position;
		if (rand == 0)
		{
			playerPos += new Vector3(UnityEngine.Random.Range(-3, 4), UnityEngine.Random.Range(-3, 4));
		}

		Vector3 playerDir = (playerPos - transform.position).normalized;
		EnemyBullet bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity, GameObject.Find("Scroller").transform);
		bullet.SetDirection(playerDir);
		animator.SetTrigger("attack");
		AudioManager.instance.PlaySound("MutantShoot");
	}

	public enum State
	{
		TOWARDS_PLAYER,
		DOWN,
		UP,
		EXPLODE
	}
}
