using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BomberEnemy : EnemyController
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
		if (freeze)
		{
			return;
		}

		float xDifAbs = Math.Abs(transform.position.x - player.transform.position.x);
		int dir = Math.Sign(transform.position.x - player.transform.position.x);
		if (xDifAbs > 0.2f)
		{
			spriteRenderer.flipX = dir == -1 ? true : false;
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
		transform.position = new Vector3(transform.position.x + moveSpeed * Time.deltaTime, (Mathf.Sin((shootOffset + Time.time)) * 3f));
	}

	private void SineTransitions()
	{
		if (spriteRenderer.isVisible)
		{
			if (shootTimer >= 2f)
			{
				state = State.BOMB;
				shootTimer = 0f;
			}
			shootTimer += Time.deltaTime;
		}
	}

	public override void Die()
	{
		AudioManager.instance.PlaySound("Enemy Death");
		freeze = true;
		GetComponent<Collider2D>().enabled = false;

		animator.SetTrigger("death");
		Invoke("Destroy", 0.6f);
	}

	public IEnumerator DropBombs()
	{
		int bombCount = UnityEngine.Random.Range(2, 4);
		for (int i = 0; i < bombCount; i++)
		{
			DropBomb();
			yield return new WaitForSeconds(0.3f);
		}
	}

	private void DropBomb()
	{
		EnemyBullet bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity, GameObject.Find("Scroller").transform);
		bullet.SetDirection(Vector2.zero);
		animator.SetTrigger("attack");
		AudioManager.instance.PlaySound("BomberShoot");
	}

	public enum State
	{
		SINE,
		BOMB,
		EXPLODE
	}
}
