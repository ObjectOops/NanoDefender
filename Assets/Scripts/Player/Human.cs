using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour
{
	public bool isTargeted;
	public bool onGround = true;
	public float frameFallDistance, fallDamageDistance, groundElevationY;
	public MutantEnemy mutantPrefab;
	public Animator animator;

	private float distanceFallen = 0;

	void Start()
	{
		animator.SetFloat("offset", Random.Range(0, 1f));
	}

	void Update()
	{
		onGround = OnGround();
		if (!onGround && !isTargeted)
		{
			Fall();
		}
		else if (onGround)
		{
			TestFallDamage();
		}
	}

	public void DieSequence()
	{
		animator.SetTrigger("mutant");
		Invoke("SpawnMutant", 0.850f);
	}

	public void SpawnMutant()
	{
		MutantEnemy enemy = Instantiate(mutantPrefab, transform.position, Quaternion.identity, FindObjectOfType<ScrollManager>().transform);
		enemy.Init();
		Destroy(this.gameObject);
	}

	public void Frown()
	{
		animator.SetTrigger("frown");
	}

	public bool OnGround()
	{
		return transform.position.y <= groundElevationY;
	}

	private void Fall()
	{
		transform.position = new Vector2(transform.position.x, transform.position.y - frameFallDistance * Time.deltaTime);
		distanceFallen += frameFallDistance * Time.deltaTime;
	}

	private void TestFallDamage()
	{
		if (distanceFallen >= fallDamageDistance)
		{
			Destroy(this.gameObject);
		}
	}

}
