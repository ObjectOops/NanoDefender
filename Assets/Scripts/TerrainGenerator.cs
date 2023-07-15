using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
	public GameObject terrainPiece;
	public int terrainWidth;
	private bool up;
	private bool down;
	private Vector3 startingPos;

	public Transform scroller;

	void Start()
	{
		startingPos = transform.position;
		float scale = terrainPiece.transform.localScale.x;
		float pieceAmount = (float)terrainWidth / scale;
		for (float i = 0; i < terrainWidth; i += scale)
		{
			float y = transform.position.y;
			int rand = UnityEngine.Random.Range(1, 21);
			if (rand == 20 && !down)
			{
				up = true;
			}
			if (up)
			{
				int rand2 = UnityEngine.Random.Range(1, 11);
				if (rand2 == 1)
				{
					up = false;
					down = true;
				}
				else
				{
					y += scale;
					transform.position = new Vector3(transform.position.x, y, transform.position.z);
				}
			}
			if (down)
			{
				y -= scale;
				transform.position -= new Vector3(0, scale, 0);
				if (transform.position.y <= startingPos.y)
				{
					down = false;
				}
			}
			Instantiate(terrainPiece, new Vector3(i - terrainWidth / 2, y, transform.position.z), Quaternion.identity);
		}
	}

	void Update()
	{


	}
}
