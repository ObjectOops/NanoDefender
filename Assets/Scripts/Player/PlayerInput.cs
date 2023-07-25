using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
	public FrameInput GameInput { get; private set; }
	public bool invulnerable;

	[SerializeField] private List<KeyCode> cheatcode;

	private int codeIndex;
	private bool listening;

	private void Update()
	{
		float yMove = Input.GetAxisRaw("Vertical");
		GameInput = new FrameInput
		{
			yMovement = yMove,
			LeftPressed = Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow),
			RightPressed = Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow),
			Accelerating = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.Space),
			AttackPressed = Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.Return),
			BombPressed = Input.GetKeyDown(KeyCode.L),
			HyperspacePressed = Input.GetKeyDown(KeyCode.J),
		};

		if (Input.GetKeyDown(KeyCode.UpArrow) && !listening)
		{
			listening = true;
			codeIndex++;
		}
		else if (listening)
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
		public bool HyperspacePressed;
		public float yMovement;
	}
}
