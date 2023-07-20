using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
	public Vector3 direction;
	public float moveSpeed;
	public bool destroyOnLeave;

	private float despawnTimer;

	private SpriteRenderer spriteRenderer;


	private void Start()
	{
		spriteRenderer = GetComponentInChildren<SpriteRenderer>();
	}

	private void Update()
	{
		transform.position += direction * moveSpeed * Time.deltaTime;
		if (destroyOnLeave && !spriteRenderer.isVisible)
		{
			Destroy(this.gameObject);
		}
		else
		{
			if (despawnTimer > 10f)
			{
				Destroy(this.gameObject);
			}
			despawnTimer += Time.deltaTime;
		}
	}

	public void SetDirection(Vector3 direction)
	{
		this.direction = direction;
	}
}
