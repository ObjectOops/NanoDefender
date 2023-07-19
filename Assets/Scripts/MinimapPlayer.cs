using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapPlayer : MonoBehaviour
{

	public PlayerController player;
	public CameraOffset camOffset;
	public GameObject testObject;
	public GameObject testWorldObject;
	public BoxCollider2D worldBounds;
	public RectTransform minimapBounds;

	//what the fuck
	//how does any of this work
	//gets player position and scales it to be in bounds of minimap

	private void Update()
	{
		Vector3 worldXVec = camOffset.transform.position;
		float unscaledX = Camera.main.WorldToScreenPoint(worldXVec).x;

		float targetX = (worldXVec.x * -10);

		transform.localPosition = new Vector3(targetX, transform.localPosition.y, transform.localPosition.z);

		Vector3 screenPoint = Camera.main.WorldToScreenPoint(player.transform.position);

		float yValue = (float)Scale((int)screenPoint.y, 0, Screen.height, 768 - 130, Screen.height);

		Vector3 minimapPoint = new Vector3(transform.position.x, yValue, transform.position.z);
		transform.position = minimapPoint;
	}

	private double Scale(int value, int min, int max, int minScale, int maxScale)
	{
		double scaled = minScale + (double)(value - min) / (max - min) * (maxScale - minScale);
		return scaled;
	}
}
