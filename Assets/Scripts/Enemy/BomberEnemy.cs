using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberEnemy : EnemyController
{
	public float moveSpeed, downSpeed, shootCoolDown = 2f, bombTimeSpacing = 0.3f, deathDuration = 0.6f;

	private State state;
	private float shootTimer;

	new private void Start()
	{
		base.Start();
	}

	private void Update()
	{
		if (freeze)
		{
			return;
		}

		float absDiffX = Mathf.Abs(transform.position.x - player.transform.position.x);
		int dir = (int)Mathf.Sign(transform.position.x - player.transform.position.x);

		if (absDiffX > 0.2f)
		{
			// dir == -1 ? true : false
			spriteRenderer.flipX = dir == -1;
		}

		switch (state)
		{
			case State.SINE:
				SineActions();
				SineTransitions();
				break;
			case State.BOMB:
				BombActions();
				BombTransitions();
				break;
		}
	}

	private void BombActions()
	{
		StartCoroutine(DropBombs());
	}

	private void BombTransitions()
	{
		state = State.SINE;
	}

	private void SineActions()
	{
		transform.position = new Vector2(
			transform.position.x + moveSpeed * Time.deltaTime, 
			Mathf.Sin(shootOffset + Time.time) * 3f
		);
	}

	private void SineTransitions()
	{
		if (!spriteRenderer.isVisible)
		{
			return;
		}
		if (shootTimer >= shootCoolDown)
		{
			state = State.BOMB;
			shootTimer = 0f;
		}
		shootTimer += Time.deltaTime;
	}

	public override void Die()
	{
		GetComponent<Collider2D>().enabled = false;
		freeze = true;
		animator.SetTrigger("death");
		AudioManager.instance.PlaySound("Enemy Death");
		Invoke(nameof(Destroy), deathDuration);
	}

	public IEnumerator DropBombs()
	{
		int bombCount = Random.Range(2, 4);
		for (int i = 0; i < bombCount; i++)
		{
			DropBomb();
			yield return new WaitForSeconds(bombTimeSpacing);
		}
	}

	private void DropBomb()
	{
		EnemyBullet bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity, GameObject.Find("Scroller").transform);
		bullet.SetDirection(Vector2.zero); // Mines don't move.
		animator.SetTrigger("attack");
		AudioManager.instance.PlaySound("BomberShoot");
	}

	public enum State
	{
		SINE,
		BOMB,
	}
}
