using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
	public FrameInput gameInput { get; private set; }

	private void Update()
	{
		float yMove = Input.GetAxisRaw("Vertical");
		gameInput = new FrameInput
		{
			yMovement = yMove,
			LeftPressed = Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow),
			RightPressed = Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow),
			Accelerating = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.Space),
			AttackPressed = Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.Return),
			BombPressed = Input.GetKeyDown(KeyCode.E)
		};
	}

	public struct FrameInput
	{
		public bool LeftPressed;
		public bool RightPressed;
		public bool Accelerating;
		public bool AttackPressed;
		public bool BombPressed;
		public float yMovement;

	}
}
