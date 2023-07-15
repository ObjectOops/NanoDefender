using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollManager : MonoBehaviour
{
	public BoxCollider2D bounds;

	public void Scroll(Vector2 moveDelta)
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			Transform toScroll = transform.GetChild(i);

			toScroll.position -= new Vector3(moveDelta.x, moveDelta.y, 0);
			// if (toScroll.GetComponent<TerrainGenerator>())
			// {
			// 	int children = toScroll.childCount;
			// 	for (int j = 0; j < children; j++)
			// 	{
			// 		Transform terrainPiece = toScroll.GetChild(j);
			// 		if (!bounds.bounds.Contains(terrainPiece.position))
			// 		{
			// 			terrainPiece.position = new Vector2(-terrainPiece.position.x, terrainPiece.position.y);
			// 			continue;
			// 		}
			// 	}
			// 	continue;
			// }

			if (!bounds.bounds.Contains(toScroll.position))
			{
				toScroll.position = new Vector2(-toScroll.position.x, toScroll.position.y);
			}
		}
	}
}
