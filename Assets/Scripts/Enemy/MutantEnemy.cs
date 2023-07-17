using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutantEnemy : EnemyController
{
	private State state;

	public float moveSpeed;
	public float downSpeed;

	new void Start()
	{
        player = FindObjectOfType<PlayerController>();
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
		freeze = true;
		Destroy(this.gameObject);
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

	public enum State
	{
		TOWARDS_HUMAN,
		DOWN,
		UP,
		EXPLODE
	}
}
