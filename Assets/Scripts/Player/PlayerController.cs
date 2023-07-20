using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[Header("Variables")]
	public float moveSpeed;
	public float maxSpeed;
	public float ySpeed;
	public float damping;

	[Header("References")] // Stay public for readability.
	public GameManager manager;
	public PlayerInput input;
	public PlayerAnimator animator;
	public UIManager UIManager;
	public ScrollManager scrollManager;
	public CameraOffset cameraOffset;
	public Laser laserPrefab;
	public Transform shootPoint;
	public Transform movingShootPoint;
	public Transform humanHoldPoint;
	public ParticleSystem deathParticles;
	public GameObject flash;

	[Header("Others")]
	public float cameraOffsetX = 3f, flashDuration = 0.03f, deathDuration = 1.4f, resumeDuration = 0.15f;

	[HideInInspector]
	public bool holdingHuman;

	/* 
	 * Private Variables
	 */
	private SpriteRenderer spriteRenderer;
	private Vector2 moveDelta;
	private State state;
	private float velocity, moveLerp;
	private bool flipped, freezeControls;

	/*
	 * The Only Constants
	 */
	private const float lowerBoundY = -4.5f, upperBoundY = 2.8f, leftMapBound = -100, rightMapBound = 101;

	private void Start()
	{
		spriteRenderer = GetComponentInChildren<SpriteRenderer>();
	}

	private void Update()
	{
		if (UIManager.paused)
		{
			return;
		}
		
		AnyActions();
		AnyTransitions();
		switch (state)
		{
			case State.IDLE:
				IdleActions();
				IdleTransitions();
				break;
			case State.MOVING:
				MoveActions();
				MoveTransitions();
				break;
			case State.GLIDING:
				GlideActions();
				GlideTransitions();
				break;
			case State.DIE:
				if(!input.invulnerable) {
					DieActions();
					DieTransitions();
				}
				break;

		}
	}

	private void FixedUpdate()
	{
		moveDelta.x = velocity * Time.deltaTime;
		scrollManager.Scroll(moveDelta);

		if (!freezeControls)
		{
			MovementY();
		}
	}

	private void MovementY()
	{
		float yAxis = Input.GetAxisRaw("Vertical");
		float newY = yAxis * ySpeed * Time.deltaTime;

		transform.position += new Vector3(0f, newY, 0f);

		float clampedY = Mathf.Clamp(transform.position.y, lowerBoundY, upperBoundY);

		transform.position = new Vector3(transform.position.x, clampedY, 0);
	}

	private void AnyActions()
	{
		if (freezeControls)
		{
			return;
		}

		animator.anim.SetBool("holding", holdingHuman);

		if (input.GameInput.RightPressed)
		{
			flipped = false;
			cameraOffset.SetXOffset(cameraOffsetX);
			transform.localScale = new Vector3(1, 1, 1);
		}
		if (input.GameInput.LeftPressed)
		{
			flipped = true;
			cameraOffset.SetXOffset(-cameraOffsetX);
			transform.localScale = new Vector3(-1, 1, 1);
		}
		if (input.GameInput.AttackPressed)
		{
			ShootLaser(input.GameInput.Accelerating);
			AudioManager.instance.PlaySound("Laser");
		}
		if (input.GameInput.BombPressed)
		{
			if (UIManager.UseBomb())
			{
				EnemyController[] enemyControllers = FindObjectsOfType<EnemyController>();
				foreach (EnemyController enemy in enemyControllers)
				{
					SpriteRenderer renderer = enemy.GetComponentInChildren<SpriteRenderer>();
					if (renderer.isVisible)
					{
						enemy.Die();
						UIManager.AddPoints(enemy.GetPointValue());
					}
				}
				// StartCoroutine(BombRinging());
				StartCoroutine(Flash());
			}
		}
	}

	private IEnumerator Flash()
	{
		yield return new WaitForSeconds(flashDuration - 0.01f);
		AudioManager.instance.PlaySound("Smart Bomb");
		flash.SetActive(true);
		yield return new WaitForSeconds(flashDuration);
		flash.SetActive(false);
		yield return new WaitForSeconds(flashDuration);
		flash.SetActive(true);
		yield return new WaitForSeconds(flashDuration);
		flash.SetActive(false);
		yield return new WaitForSeconds(flashDuration);
		flash.SetActive(true);
		yield return new WaitForSeconds(flashDuration);
		flash.SetActive(false);
	}

	// private IEnumerator BombRinging()
	// {
	// 	int iterations = 1;
	// 	for (int i = 0; i < iterations; ++i)
	// 	{
	// 		yield return new WaitForSeconds(flashDuration - 0.01);
	// 		AudioManager.instance.PlaySound("Smart Bomb Ringing", 1 - (float)i / iterations);
	// 	}
	// }

	private void AnyTransitions()
	{
		if (freezeControls)
		{
			return;
		}
		if (input.GameInput.Accelerating)
		{
			state = State.MOVING;
		}
	}

	private void IdleActions()
	{
		// Nothing.
	}

	private void IdleTransitions()
	{
		// Nothing.
	}

	private void MoveActions()
	{
		int dir = flipped ? -1 : 1;
		velocity += dir * moveSpeed * Time.deltaTime;
		velocity = Mathf.Clamp(velocity, -maxSpeed, maxSpeed);

		moveLerp += Time.deltaTime / 2f;
		moveLerp = Mathf.Clamp(moveLerp, 0f, 1f);

		AudioManager.instance.PlaySoundAndWait("Thrust");
	}

	private void MoveTransitions()
	{
		if (!input.GameInput.Accelerating)
		{
			state = State.GLIDING;
		}
	}

	private void GlideActions()
	{
		moveLerp -= Time.deltaTime;
		moveLerp = Mathf.Clamp(moveLerp, 0f, 1f);
		velocity *= (1f - damping * Time.deltaTime);
	}

	private void GlideTransitions()
	{
		// Nothing.
	}

	public void DieActions()
	{
		velocity = 0f;
		StartCoroutine(DeathSequence());
	}

	public void DieTransitions()
	{
		// Nothing.
	}

	private void ShootLaser(bool tilted)
	{
		if (tilted)
		{
			Laser laserInst = Instantiate(laserPrefab, movingShootPoint.position, Quaternion.identity);
			int dir = flipped ? -1 : 1;
			laserInst.SetDirection(dir);
		}
		else
		{
			Laser laserInst = Instantiate(laserPrefab, shootPoint.position, Quaternion.identity);
			int dir = flipped ? -1 : 1;
			laserInst.SetDirection(dir);
		}
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if(!input.invulnerable) {
			if (other.gameObject.TryGetComponent(out EnemyController enemy))
			{
				enemy.Die();
				Die();
			}
			else if (other.gameObject.TryGetComponent(out EnemyBullet bullet))
			{
				Destroy(bullet.gameObject);
				Die();
			}
		}
	}

	private void Die()
	{
		if(!input.invulnerable) {
			state = State.DIE;
			freezeControls = true;
		}
	}
	
	public void ResetPlayer() {
		cameraOffset.SetXOffsetInstant(cameraOffsetX);
		transform.position = new Vector3(transform.position.x, 0, transform.position.y);
		transform.localScale = new Vector3(1, 1, 1);
		flipped = false;
	}

	private IEnumerator DeathSequence()
	{
		state = State.IDLE;
		cameraOffset.freeze = true;

		manager.FreezeEnemies();
		animator.DeathAnimation();
		deathParticles.Play();

		EnemyBullet[] enemyBullets = FindObjectsOfType<EnemyBullet>();
		foreach (EnemyBullet bullet in enemyBullets)
		{
			Destroy(bullet.gameObject);
		}
		AudioManager.instance.PlaySound("Player Death");

		yield return new WaitForSeconds(deathDuration);

		// Don't worry about it.
		bool dead = !UIManager.DecrementHealth();

		if (dead)
		{
			UIManager.ShowGameOver();
			PlayerPrefs.SetInt("score", UIManager.instance.points);
			int highScore = PlayerPrefs.GetInt("highscore", 0);
			if (UIManager.instance.points > highScore)
			{
				PlayerPrefs.SetInt("highscore", UIManager.instance.points);
			}
			StartCoroutine(UIManager.ResetScene());

			spriteRenderer.enabled = false;
			yield break;
		}
		
		UIManager.ShowRefreshScreen();

		scrollManager.Scroll(new Vector2(Random.Range(leftMapBound, rightMapBound), 0f));

		cameraOffset.freeze = false;
		ResetPlayer();

		yield return new WaitForSeconds(resumeDuration);
		UIManager.HideRefreshScreen();

		animator.Reset();
		manager.UnFreezeEnemies();
		freezeControls = false;
	}

	public enum State
	{
		IDLE,
		MOVING,
		GLIDING,
		DIE
	}
}
