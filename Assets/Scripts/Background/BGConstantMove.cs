using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGConstantMove : MonoBehaviour
{
	[SerializeField] private float moveSpeed;

	private void Update()
	{
		transform.position -= new Vector3(moveSpeed * Time.deltaTime, 0f, 0f);
	}
}
