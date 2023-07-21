using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
	public Vector3 direction;
	public float moveSpeed;
	public bool destroyOnLeave;

	private bool entered;
	private bool lastFrameVisible;

	private SpriteRenderer spriteRenderer;
	private float despawnTimer;

	private void Start()
	{
		spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		if (spriteRenderer.isVisible)
		{
			entered = true;
		}
	}

	private void Update()
	{
		transform.position += moveSpeed * Time.deltaTime * direction; // IDE suggested order for performance.
		if(!lastFrameVisible && spriteRenderer.isVisible) {
			entered = true;
		}
		if (destroyOnLeave && entered && !spriteRenderer.isVisible)
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
		
		lastFrameVisible = spriteRenderer.isVisible;
	}

	public void SetDirection(Vector3 direction)
	{
		this.direction = direction;
	}
}
