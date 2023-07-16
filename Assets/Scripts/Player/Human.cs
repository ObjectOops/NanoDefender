using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour
{
	public bool isTargeted, onGround;
	public float frameFallDistance, fallDamageDistance, groundElevationY;
	public MutantEnemy mutantPrefab;
	public Transform scroller;

	private float distanceFallen = 0;

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

    public void SpawnMutant()
	{
		Instantiate(mutantPrefab, transform.position, Quaternion.identity, FindObjectOfType<ScrollManager>().transform);
		Destroy(this.gameObject);
	}

	public bool OnGround()
    {
		return transform.position.y <= groundElevationY;
    }

	private void Fall()
    {
		transform.position = new Vector2(transform.position.x, transform.position.y - frameFallDistance);
		distanceFallen += frameFallDistance;
	}

	private void TestFallDamage()
    {
		if (distanceFallen >= fallDamageDistance)
		{
			Destroy(this.gameObject);
		}
	}

}
