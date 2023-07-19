using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
	public Vector3 direction;
	public float moveSpeed;

	private SpriteRenderer spriteRenderer;

	private void Start()
	{
		spriteRenderer = GetComponentInChildren<SpriteRenderer>();
	}

	private void Update()
	{
		transform.position += direction * moveSpeed * Time.deltaTime;
		if (!spriteRenderer.isVisible)
		{
			Destroy(this.gameObject);
		}
	}

	public void SetDirection(Vector3 direction)
	{
		this.direction = direction;
	}
}
