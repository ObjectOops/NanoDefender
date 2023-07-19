using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutantEnemy : EnemyController
{
	private State state;

	public float moveSpeed;
	public float downSpeed;
	private float shootTimer;

	new void Start()
	{
		base.Start();
	}

	void Update()
	{
		if (player == null)
		{
			return;
		}
		if (freeze)
		{
			return;
		}
		ShootActions();

		float xDifAbs = Math.Abs(transform.position.x - player.transform.position.x);
		int dir = Math.Sign(transform.position.x - player.transform.position.x);
		if (xDifAbs > 0.2f)
		{
			spriteRenderer.flipX = dir == -1 ? true : false;
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
		AudioManager.instance.PlaySound("Enemy Death");
		freeze = true;
		GetComponent<Collider2D>().enabled = false;

		animator.SetTrigger("death");
		Invoke("Destroy", 0.517f);
	}

	private void TowardsActions()
	{
		float currentX = transform.position.x;

		int dirTowardsHuman = Math.Sign(player.transform.position.x - currentX);

		float newX = (currentX + (dirTowardsHuman * moveSpeed * Time.deltaTime));

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

		float newY = (currentY - (downSpeed * Time.deltaTime));

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

		float enemyX = transform.position.x;
		float humanX = player.transform.position.x;

		float differenceX = Mathf.Abs(enemyX - humanX);

		if (difference >= 0.5f)
		{
			state = State.TOWARDS_HUMAN;
		}
	}

	private void UpActions()
	{

	}

	private void UpTransitions()
	{

	}

	private void ShootActions()
	{
		if (state.Equals(State.EXPLODE))
		{
			return;
		}

		if (!spriteRenderer.isVisible)
		{
			return;
		}

		if (shootTimer + shootOffset >= 2f)
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
		int rand = UnityEngine.Random.Range(0, 2);
		Vector3 playerPos = (player.transform.position);
		if (rand == 0)
		{
			playerPos += new Vector3(UnityEngine.Random.Range(-3, 4), UnityEngine.Random.Range(-3, 4));
		}

		Vector3 playerDir = (playerPos - transform.position).normalized;
		EnemyBullet bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity, GameObject.Find("Scroller").transform);
		bullet.SetDirection(playerDir);
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
