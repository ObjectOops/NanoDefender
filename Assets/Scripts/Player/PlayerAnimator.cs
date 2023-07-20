using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
	[SerializeField]
	private PlayerInput input;
	public Animator anim;

	private void Update()
	{
		if (UIManager.paused)
		{
			return;
		}
		
		bool moving = input.GameInput.Accelerating;

		anim.SetBool("moving", moving);

		bool attack = input.GameInput.AttackPressed;
		if (attack)
		{
			anim.SetTrigger("attack");
		}

		bool smartBomb = input.GameInput.BombPressed;
		if (smartBomb)
		{
			anim.SetTrigger("bomb");
		}
	}

	public void DeathAnimation()
	{
		anim.SetBool("death", true);
	}

	public void Reset()
	{
		anim.SetBool("death", false);
		anim.SetTrigger("reset");
	}
}
