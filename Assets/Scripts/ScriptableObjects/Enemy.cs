using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObject/Enemy")]
public class Enemy : ScriptableObject
{
	public Sprite sprite;
	public string enemyName;
	
	public Sprite minimapSprite;
	
	public int pointValue;
}
