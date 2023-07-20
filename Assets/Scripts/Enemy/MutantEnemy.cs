using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutantEnemy : EnemyController
{
	// Magic numbers ahead.

	public float moveSpeed, downSpeed, shootCoolDown = 2f, deathDuration = 0.517f;

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
		int dir = (int)Mathf.Sign(transform.position.x - player.transform.position.x);
		if (xDifAbs > 0.2f)
		{
			// dir == -1 ? true : false
			spriteRenderer.flipX = dir == -1;
		}

		switch (state)
		{
			case State.TOWARDS_HUMAN:
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

		int dirTowardsHuman = (int)Mathf.Sign(player.transform.position.x - currentX);

		float newX = currentX + (dirTowardsHuman * moveSpeed * Time.deltaTime);

		transform.position = new Vector2(newX, transform.position.y);
	}
	private void TowardsTransitions()
	{
		float enemyX = transform.position.x;
		float humanX = player.transform.position.x;

		float difference = Mathf.Abs(enemyX - humanX);

		if (difference < 0.1f)
		{
			state = State.DOWN;
		}
	}

	private void DownActions()
	{
		float currentY = transform.position.y;

		float newY = currentY - (downSpeed * Time.deltaTime);

		transform.position = new Vector2(transform.position.x, newY);
	}

	private void DownTransitions()
	{
		float enemyY = transform.position.y;
		float humanY = player.transform.position.y;

		float difference = Mathf.Abs(enemyY - humanY);

		if (difference < 0.1f)
		{
			state = State.UP;
		}

		if (difference >= 0.5f)
		{
			state = State.TOWARDS_HUMAN;
		}
	}

	private void UpActions()
	{
		// Nothing.
	}

	private void UpTransitions()
	{
		// Nothing.
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
		int rand = Random.Range(0, 2);
		Vector3 playerPos = player.transform.position;
		if (rand == 0)
		{
			playerPos += new Vector3(Random.Range(-3, 4), Random.Range(-3, 4));
		}

		Vector3 playerDir = (playerPos - transform.position).normalized;
		EnemyBullet bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity, GameObject.Find("Scroller").transform);
		bullet.SetDirection(playerDir);
		animator.SetTrigger("attack");
		AudioManager.instance.PlaySound("MutantShoot");
	}

	public enum State
	{
		TOWARDS_HUMAN,
		DOWN,
		UP,
		EXPLODE
	}
}
