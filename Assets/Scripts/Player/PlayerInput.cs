using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
	public FrameInput gameInput { get; private set; }
	public bool invulnerable;

	private bool listening;

	public List<KeyCode> cheatcode;
	public int codeIndex;

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
			BombPressed = Input.GetKeyDown(KeyCode.E),
		};
		if (Input.GetKeyDown(KeyCode.UpArrow) && !listening)
		{
			listening = true;
			codeIndex++;
		} else if (listening)
		{
			if (codeIndex >= cheatcode.Count)
			{
				invulnerable = true;
				listening = false;
				codeIndex = 0;
			}

			if (Input.GetKeyDown(cheatcode[codeIndex]))
			{
				codeIndex++;
			}
			else if (Input.anyKeyDown)
			{
				listening = false;
				codeIndex = 0;
			}
		}
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
