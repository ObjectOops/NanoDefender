using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGLooper : MonoBehaviour
{
	private Vector3 originalPosition;
	public float maxDif = 16.39f;
	public float size;

	private void Start()
	{
		originalPosition = transform.position;
		// size = GetComponentInChildren<SpriteRenderer>().sprite.bounds.max.x;
	}

	private void Update()
	{
		float posDif = Mathf.Abs(transform.position.x - originalPosition.x);

		if (posDif > (maxDif))
		{
			transform.position = originalPosition;
		}
	}

}
