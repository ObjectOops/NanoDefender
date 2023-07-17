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
			LeftPressed = Input.GetKeyDown(KeyCode.A),
			RightPressed = Input.GetKeyDown(KeyCode.D),
			Accelerating = Input.GetKey(KeyCode.LeftShift),
			AttackPressed = Input.GetKeyDown(KeyCode.K),
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
