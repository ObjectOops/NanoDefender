using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyController : MonoBehaviour
{
	public PlayerController player;
	public Human human;
	public Enemy enemyData;
	private int points;


	internal Animator animator;
	internal bool freeze;

	public void Start()
	{
		animator = GetComponentInChildren<Animator>();
		animator.SetFloat("offset", UnityEngine.Random.Range(0f, 1f));
	}

	public void Init()
	{
		points = enemyData.pointValue;
	}

	public abstract void Die();

	public void Destroy()
	{
		Destroy(this.gameObject);
	}

	public int GetPointValue()
	{
		return points;
	}

	public void Freeze()
	{
		freeze = true;
	}

	public void UnFreeze()
	{
		freeze = false;
	}
}
