using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG_ConstantMove : MonoBehaviour
{
	public float moveSpeed;

	void Update()
	{
		transform.position -= new Vector3(moveSpeed * Time.deltaTime, 0f, 0f);
	}
}
