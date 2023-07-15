using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanSpawnManager : MonoBehaviour
{
	public GameObject humanprefab;
	public Transform humanSpawnY;
	public Transform scroller;

	public void PopulateLevel(int humanCount)
	{
		//5 enemies
		for (int i = 0; i < humanCount; i++)
		{
			float randX = UnityEngine.Random.Range(-50, 51);
			Vector2 pos = new Vector2(randX, humanSpawnY.position.y);
			GameObject human = Instantiate(humanprefab, pos, Quaternion.identity, scroller);
		}
	}
}
