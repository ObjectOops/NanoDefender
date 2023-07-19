using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour
{
	public bool isTargeted;
	public bool onGround = true;
	public bool isHeld;
	public float fallSpeed, deathDistance, groundElevationY;
	public MutantEnemy mutantPrefab;
	public Animator animator;

	public Sprite minimapSprite;

	private float distanceFallen = 0;
	private float velocity;

	void Start()
	{
		animator.SetFloat("offset", Random.Range(0, 1f));
		GetComponent<MinimapObject>().Init(minimapSprite);
	}

	void Update()
	{
		onGround = OnGround();
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
				animator.SetBool("frown", false);
				UIManager.instance.AddPoints(500);
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
		Invoke("SpawnMutant", 0.850f);
	}

	private void SpawnMutant()
	{

		MutantEnemy enemy = Instantiate(mutantPrefab, transform.position, Quaternion.identity, FindObjectOfType<ScrollManager>().transform);
		enemy.Init();
		Destroy();
	}

	public void Frown()
	{
		animator.SetBool("frown", true);
	}

	public bool OnGround()
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
		} else {
			animator.SetBool("frown", false);
		}
	}

	public void Die()
	{
		animator.SetTrigger("death");
		AudioManager.instance.PlaySound("HumanDie");
		Invoke("Destroy", 0.433f);
	}

	private void Destroy()
	{
		Destroy(this.gameObject);
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.TryGetComponent<PlayerController>(out PlayerController player))
		{
			if (!onGround && !isTargeted)
			{
				transform.parent = player.humanHoldPoint;
				transform.position = player.humanHoldPoint.position;
				isHeld = true;
				UIManager.instance.AddPoints(500);
				player.holdingHuman = true;
			}
		}
	}

}
