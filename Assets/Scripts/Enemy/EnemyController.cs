using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyController : MonoBehaviour
{
	public PlayerController player;
	public Human human;
	public Enemy enemyData;
	public EnemyBullet bulletPrefab;
	private int points;

	internal SpriteRenderer spriteRenderer;
	internal float shootOffset;
	internal Animator animator;
	internal bool freeze;

	public void Start()
	{
		animator = GetComponentInChildren<Animator>();
		spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		animator.SetFloat("offset", UnityEngine.Random.Range(0f, 1f));
		player = FindObjectOfType<PlayerController>();
		shootOffset = UnityEngine.Random.Range(0f, 1f);
		
		GetComponent<MinimapObject>().Init(enemyData);
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
