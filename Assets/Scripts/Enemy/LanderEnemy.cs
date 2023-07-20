using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanderEnemy : EnemyController
{
	private State state;

	public float moveSpeed;
	public float downSpeed;

	private float shootTimer;

	private float hitOffset;

	public LayerMask terrainMask;

	new void Start()
	{
		base.Start();
	}


	void Update()
	{

		if (freeze)
		{
			return;
		}

		ShootActions();

		if (human == null)
		{
			return;
		}

		int dir = Math.Sign(transform.position.x - player.transform.position.x);
		spriteRenderer.flipX = dir == -1 ? true : false;


		switch (state)
		{
			case State.TOWARDS_BOTTOM:
				TowardsBottomActions();
				TowardsBottomTransitions();
				break;
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
			case State.EXPLODE:
				ExplodeActions();
				ExplodeTransitions();
				break;
		}
	}

	public override void Die()
	{
		AudioManager.instance.PlaySound("Enemy Death");
		freeze = true;
		GetComponent<Collider2D>().enabled = false;
		if (human != null)
		{
			if (human.transform.parent.Equals(this.transform))
			{
				human.transform.parent = GameObject.Find("Scroller").transform;
				human.isTargeted = false;
			}
		}

		animator.SetTrigger("death");
		Invoke("Destroy", 0.533f);
	}


	public void SetTarget(Human human)
	{
		human.isTargeted = true;
		this.human = human;
	}

	private void TowardsBottomActions()
	{
		float currentX = transform.position.x;

		int dirTowardsHuman = Math.Sign(human.transform.position.x - currentX);

		float newX = (currentX + (dirTowardsHuman * moveSpeed / 5f * Time.deltaTime));

		float currentY = transform.position.y;

		float newY = (currentY - (downSpeed * 1.25f * Time.deltaTime));

		transform.position = new Vector2(newX, newY);
	}

	private void TowardsBottomTransitions()
	{
		if (transform.position.y <= -3)
		{
			state = State.TOWARDS_HUMAN;
		}
	}

	private void TowardsActions()
	{
		float currentX = transform.position.x;

		int dirTowardsHuman = Math.Sign(human.transform.position.x - currentX);

		float newX = (currentX + (dirTowardsHuman * moveSpeed * Time.deltaTime));

		RaycastHit2D hit = Physics2D.Raycast(transform.position + (transform.right * dirTowardsHuman * 0.3f), Vector2.down, 50f, terrainMask);
		if (hit)
		{
			if (hit.distance <= 0.3f)
			{
				hitOffset = 1;
			}
			else
			{
				hitOffset = -1;
			}
		}

		transform.position = new Vector2(newX, transform.position.y + (hitOffset * moveSpeed * Time.deltaTime));
	}

	private void TowardsTransitions()
	{
		float enemyX = transform.position.x;
		float humanX = human.transform.position.x;

		float difference = Mathf.Abs(enemyX - (humanX));

		if (difference < 0.1f)
		{
			state = State.DOWN;
		}
	}

	private void DownActions()
	{
		float currentY = transform.position.y;

		float newY = (currentY - (downSpeed * Time.deltaTime));

		// if (newY <= human.transform.position.y + 0.5f)
		// {
		// 	newY = (currentY + (downSpeed * Time.deltaTime));
		// }

		transform.position = new Vector2(transform.position.x, newY);
	}

	private void DownTransitions()
	{
		float enemyY = transform.position.y;
		float humanY = human.transform.position.y;

		float difference = enemyY - (humanY);

		if (difference < 0.5f)
		{
			human.transform.parent = this.transform;
			human.Frown();
			state = State.UP;
		}
	}

	private void UpActions()
	{
		float currentY = transform.position.y;

		float newY = (currentY + (downSpeed * Time.deltaTime));

		transform.position = new Vector2(transform.position.x, newY);
	}

	private void UpTransitions()
	{
		float enemyY = transform.position.y;
		float topY = 3;

		if (enemyY >= topY)
		{
			state = State.EXPLODE;
		}
	}

	private void ExplodeActions()
	{
		human.transform.parent = GameObject.Find("Scroller").transform;
		// human.transform.position = 
		human.MutantAnimation();
		Destroy(this.gameObject);
	}

	private void ExplodeTransitions()
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

		if (shootTimer + shootOffset >= 3f)
		{
			shootTimer = 0f;
			ShootOnce();
		}

		shootTimer += Time.deltaTime;
	}

	private IEnumerator ShootBurst()
	{
		ShootOnce();
		yield return new WaitForSeconds(0.7f);
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
		animator.SetTrigger("attack");
		AudioManager.instance.PlaySound("LanderShoot");
	}

	public enum State
	{
		TOWARDS_BOTTOM,
		TOWARDS_HUMAN,
		DOWN,
		UP,
		EXPLODE
	}
}
