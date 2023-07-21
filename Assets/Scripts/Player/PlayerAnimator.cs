using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
	[SerializeField]
	private PlayerInput input;
	[SerializeField]
	private GameObject faceObject;

	public Animator anim;

	private Animator faceAnimation;

	private void Start()
	{
		faceAnimation = faceObject.GetComponent<Animator>();
	}

	private void Update()
	{
		if (UIManager.paused)
		{
			return;
		}

		bool moving = input.GameInput.Accelerating;

		anim.SetBool("moving", moving);
		faceAnimation.SetBool("flying", moving); // Discrepancy.

		bool attack = input.GameInput.AttackPressed;
		if (attack)
		{
			anim.SetTrigger("attack");
		}
	}

	public void BombAnimation()
	{
		anim.SetTrigger("bomb");
	}

	public void DeathAnimation()
	{
		anim.SetBool("death", true);
		faceAnimation.SetBool("wasHit", true);
		StartCoroutine(StopHitAnimation());
	}

	private IEnumerator StopHitAnimation()
	{
		yield return new WaitForSeconds(4);
		faceAnimation.SetBool("wasHit", false);
	}

	public void Reset()
	{
		anim.SetBool("death", false);
		anim.SetTrigger("reset");
	}
}
