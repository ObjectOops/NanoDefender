using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{

	public PlayerInput input;
	public Animator anim;

	private void Update()
	{
		if (UIManager.paused)
		{
			return;
		}
		
		bool moving = input.gameInput.Accelerating;

		anim.SetBool("moving", moving);

		bool attack = input.gameInput.AttackPressed;
		if (attack)
		{
			anim.SetTrigger("attack");
		}

		bool smartBomb = input.gameInput.BombPressed;
		if (smartBomb)
		{
			anim.SetTrigger("bomb");
		}
	}

	public void DieAnim()
	{
		anim.SetBool("death", true);
	}

	public void Reset()
	{
		anim.SetBool("death", false);
		anim.SetTrigger("reset");
	}
}