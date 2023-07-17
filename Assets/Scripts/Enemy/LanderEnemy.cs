using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanderEnemy : EnemyController
{
	private State state;

	public float moveSpeed;
	public float downSpeed;

	void Update()
	{
		if (human == null)
		{
			return;
		}
		if (freeze)
		{
			return;
		}
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
		freeze = true;
		GetComponent<Collider2D>().enabled = false;
		if (human.transform.parent.Equals(this.transform))
		{
			human.transform.parent = GameObject.Find("Scroller").transform;
			human.isTargeted = false;
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

		transform.position = new Vector2(newX, transform.position.y);
	}
	private void TowardsTransitions()
	{
		float enemyX = transform.position.x;
		float humanX = human.transform.position.x;

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
		float humanY = human.transform.position.y;

		float difference = Mathf.Abs(enemyY - humanY);

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
		human.DieSequence();
		Destroy(this.gameObject);
	}

	private void ExplodeTransitions()
	{

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
