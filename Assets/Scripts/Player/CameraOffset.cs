using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOffset : MonoBehaviour
{
	public float playerOffset;
	public Player player;

	public float lerpPoint;
	private float lerpTimer;

	private bool flipping;

	void Start()
	{

	}

	void Update()
	{
		Vector3 playerPos = player.transform.position;
		playerPos.z = transform.position.z;
        
		if (flipping)
		{
		    float xPos = Mathf.Lerp(lerpPoint, playerPos.x + playerOffset, lerpTimer);
			transform.position = new Vector3(xPos, transform.position.y, transform.position.z);
			lerpTimer += Time.deltaTime;
		}
		else
		{
		    float xLerped = Mathf.Lerp(transform.position.x, playerPos.x + playerOffset, 0.01f);
		    transform.position = new Vector3(xLerped, transform.position.y, transform.position.z);
			// transform.position = Vector3.Lerp(transform.position, playerPos + playerOffset, 0.01f);
		}
	}

	public void SetXOffset(float xOffset)
	{
	    if(playerOffset != xOffset) {
			flipping = true;
			lerpTimer = 0f;

			Vector3 playerPos = player.transform.position;
			playerPos.z = transform.position.z;

			lerpPoint = transform.position.x;
			playerOffset = xOffset;
	    }
	}
}
