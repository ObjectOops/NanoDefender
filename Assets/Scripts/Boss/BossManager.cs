using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
	public GameManager gameManager;
	public bool started;


	void Start()
	{

	}

	void Update()
	{
		if (gameManager.wave == gameManager.enemies.Count - 1)
		{
			started = true;
		}
	}
}
