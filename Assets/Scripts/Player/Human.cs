using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour
{
	public bool isTargeted;
	public MutantEnemy mutantPrefab;
	public Transform scroller;

	public void SpawnMutant()
	{
		Instantiate(mutantPrefab, transform.position, Quaternion.identity, FindObjectOfType<ScrollManager>().transform);
		Destroy(this.gameObject);
	}

}
