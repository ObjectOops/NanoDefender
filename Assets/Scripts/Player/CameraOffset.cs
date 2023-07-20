using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOffset : MonoBehaviour
{
	[SerializeField]
	private float playerOffset;
	[SerializeField]
	private PlayerController player;
	[SerializeField]
	private float lerpPoint, lerpSpeed = 0.01f;

	public bool freeze;

	private float lerpTimer;
	private bool flipping;

	void FixedUpdate()
	{
		if (freeze) {
			return;
		}
		
		Vector3 playerPos = player.transform.position;
		playerPos.z = transform.position.z; // Prevent camera from moving forward into foreground.

		if (flipping)
		{
			float xPos = Mathf.Lerp(lerpPoint, playerPos.x + playerOffset, lerpTimer);
			transform.position = new Vector3(xPos, transform.position.y, transform.position.z);
			lerpTimer += Time.deltaTime;
		}
		else
		{
			float xLerped = Mathf.Lerp(transform.position.x, playerPos.x + playerOffset, lerpSpeed);
			transform.position = new Vector3(xLerped, transform.position.y, transform.position.z);
		}
	}

	public void SetXOffset(float xOffset)
	{
		if (playerOffset != xOffset)
		{
			flipping = true;
			lerpTimer = 0f;

			Vector3 playerPos = player.transform.position;
			playerPos.z = transform.position.z;

			lerpPoint = transform.position.x;
			playerOffset = xOffset;
		}
	}

	public void SetXOffsetInstant(float xOffset)
	{
		Vector3 playerPos = player.transform.position;
		playerOffset = xOffset;
		transform.position = new Vector3(playerPos.x + playerOffset, transform.position.y, transform.position.z);
	}
}
