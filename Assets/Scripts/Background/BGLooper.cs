using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGLooper : MonoBehaviour
{
	private Vector3 originalPosition;

	[HideInInspector]
	public float maxDif = 16.39f;

	private void Start()
	{
		originalPosition = transform.position;
	}

	private void Update()
	{
		float posDif = Mathf.Abs(transform.position.x - originalPosition.x);

		if (posDif > maxDif)
		{
			transform.position = originalPosition;
		}
	}
}
