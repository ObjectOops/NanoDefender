using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour
{
	[Header("Parameters")]
	public bool onGround = true;
	public float fallSpeed, deathDistance, groundElevationY, mutationDuration, deathDuration;
	public int pointValue = 500;

	[Header("Additional Components")]
	[SerializeField] private MutantEnemy mutantPrefab;
	[SerializeField] private Animator animator;
	[SerializeField] private Sprite minimapSprite;

	[HideInInspector] public bool isTargeted, isHeld;

	private float distanceFallen, velocity;
	private bool dead;

	private void Start()
	{
		animator.SetFloat("offset", Random.Range(0f, 1f));
		GetComponent<MinimapObject>().Init(minimapSprite);
	}

	private void Update()
	{
		onGround = OnGround();
		animator.SetBool("onGround", onGround);

		if (!onGround && !isTargeted && !isHeld)
		{
			Fall();
		}

		if (onGround)
		{
			if (isHeld)
			{
				distanceFallen = 0;
				isHeld = false;
				transform.parent = GameObject.Find("Scroller").transform;
				UIManager.instance.AddPoints(pointValue);
				AudioManager.instance.PlaySound("HumanSave");
				FindObjectOfType<PlayerController>().holdingHuman = false;
			}
			else
			{
				TestFallDamage();
			}
		}
	}

	public void MutantAnimation()
	{
		animator.SetTrigger("mutant");
		Invoke(nameof(SpawnMutant), mutationDuration);
	}

	private void SpawnMutant()
	{
		MutantEnemy enemy = Instantiate(mutantPrefab, transform.position, Quaternion.identity, FindObjectOfType<ScrollManager>().transform);
		enemy.Init();
		float enemyY = enemy.transform.position.y;
		enemy.transform.position = new Vector3(enemy.transform.position.x, enemyY > 2.8f ? 2.8f : enemyY);
		Destroy();
	}

	public void Frown()
	{
		animator.SetTrigger("frown");
	}

	private bool OnGround()
	{
		return transform.position.y <= groundElevationY;
	}

	private void Fall()
	{
		velocity += fallSpeed * Time.deltaTime;
		distanceFallen += velocity * Time.deltaTime;

		transform.position = new Vector2(transform.position.x, transform.position.y - velocity * Time.deltaTime);
	}

	private void TestFallDamage()
	{
		if (distanceFallen >= deathDistance)
		{
			Die();
		}
	}

	public void Die()
	{
		dead = true;
		animator.SetTrigger("death");
		Invoke(nameof(Destroy), deathDuration);
	}

	private void Destroy()
	{
		AudioManager.instance.PlaySound("HumanDie");
		Destroy(gameObject);
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.TryGetComponent(out PlayerController player) && 
			!onGround && 
			!isTargeted && 
			!dead)
		{
			transform.parent = player.humanHoldPoint;
			transform.position = player.humanHoldPoint.position;
			isHeld = true;
			player.holdingHuman = true;
		}
	}
}
