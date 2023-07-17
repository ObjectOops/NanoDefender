using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGLooper : MonoBehaviour
{
	public List<GameObject> loopObjects;
	public SpriteRenderer spriteRenderer;
	private Vector3 originalPosition;
	public float maxDif = 16.39f;

	private void Start()
	{
		originalPosition = transform.position;
		// maxDif = spriteRenderer.bounds.max.x;
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
