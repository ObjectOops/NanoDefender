using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainGenerator : MonoBehaviour
{
	public GameObject terrainPiece;
	public GameObject terrainUIPiece;
	public Transform minimap;
	public int terrainWidth;
	public int maxUp;
	private bool up;
	private bool down;
	private bool zigzag;
	private int upCount;
	private Vector3 startingPos;
	public Transform scroller;
	public List<GameObject> terrainPoints;
	private Vector2 prevTilePos;


	void Start()
	{
		startingPos = transform.position;
		float scale = 0.1f;
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
				if (upCount >= maxUp)
				{
					upCount = 0;
					up = false;
					down = true;
				}
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
				upCount++;
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

			if (zigzag && !(up || down))
			{
				y += scale;
			}

			zigzag = !zigzag;


			Vector2 tilePos = new Vector2(i - terrainWidth / 2, y);
			GameObject terrainPoint = Instantiate(terrainPiece, tilePos, Quaternion.identity, scroller);
			
			Instantiate(terrainUIPiece, (new Vector3(tilePos.x, tilePos.y) * 4 + minimap.position) + Vector3.down * 32, Quaternion.identity, minimap);
		}
	}
	
	void Update()
	{

	}
}
