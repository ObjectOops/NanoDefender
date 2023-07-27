using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	// Public for readability.

	[Header("Variables")]
	public float moveSpeed;
	public float maxSpeed;
	public float ySpeed;
	public float dampening;
	public float deathDuration = 1.4f;

	[Header("References")]
	public PlayerInput input;
	public PlayerAnimator animator;

	public GameManager manager;
	public UIManager UIManager;
	public ScrollManager scrollManager;

	public BoxCollider2D idleCollider;
	public BoxCollider2D movingCollider;

	public Transform shootPoint;
	public Transform movingShootPoint;
	public Transform humanHoldPoint;

	public CameraOffset cameraOffset;
	public Laser laserPrefab;
	public ParticleSystem deathParticles;
	public GameObject flash;

	[HideInInspector] public bool holdingHuman;

	/*
	 * Private variables.
	 */

	private SpriteRenderer spriteRenderer;
	private Vector2 moveDelta;
	private State state;
	private float velocity, startX, moveLerp;
	private bool flipped, freezeControls;

	/*
	 * Constant.
	 */
	private float minY = -4.5f, maxY= 2.8f, offsetLeft = -3, offsetRight = 3;
	private float mapLeftBound = -100, mapRightBound = 101;

	private void Start()
	{
		spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		startX = spriteRenderer.transform.position.x;
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
				if (!input.invulnerable)
				{
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
			scrollManager.Scroll(new Vector2(Time.deltaTime * -1, 0));
			MovementY();
		}
	}

	private void MovementY()
	{
		float yAxis = Input.GetAxisRaw("Vertical");
		float newY = yAxis * ySpeed * Time.deltaTime;

		transform.position += new Vector3(0f, newY, 0f);

		float clampedY = Mathf.Clamp(transform.position.y, minY, maxY);

		transform.position = new Vector3(transform.position.x, clampedY, 0);
	}

	private void AnyActions()
	{
		if (freezeControls)
		{
			return;
		}

		animator.anim.SetBool("holding", holdingHuman);
		movingCollider.enabled = input.GameInput.Accelerating;
		idleCollider.enabled = !input.GameInput.Accelerating;

		if (input.GameInput.RightPressed)
		{
			flipped = false;
			cameraOffset.SetXOffset(offsetRight);
			transform.localScale = new Vector3(1, 1, 1);
		}
		if (input.GameInput.LeftPressed)
		{
			flipped = true;
			cameraOffset.SetXOffset(offsetLeft);
			transform.localScale = new Vector3(-1, 1, 1);
		}
		if (input.GameInput.AttackPressed)
		{
			ShootLaser(input.GameInput.Accelerating);
			AudioManager.instance.PlaySound("Laser");
		}
		if (input.GameInput.BombPressed)
		{
			bool success = UIManager.UseBomb();
			if (success)
			{
				EnemyController[] enemies = FindObjectsOfType<EnemyController>();
				foreach (EnemyController enemy in enemies)
				{
					SpriteRenderer renderer = enemy.GetComponentInChildren<SpriteRenderer>();
					if (renderer.isVisible)
					{
						enemy.Die();
						UIManager.AddPoints(enemy.GetPointValue());
					}
				}
				StartCoroutine(Flash());
				animator.BombAnimation();
			}
		}

		if (input.GameInput.HyperspacePressed)
		{
			scrollManager.Scroll(new Vector2(Random.Range(mapLeftBound, mapRightBound), 0f));
			StartCoroutine(HyperSpace());
		}
	}

	private IEnumerator Flash()
	{
		yield return new WaitForSeconds(0.2f);
		AudioManager.instance.PlaySound("Smart Bomb");
		flash.SetActive(true);
		yield return new WaitForSeconds(0.03f);
		flash.SetActive(false);
		yield return new WaitForSeconds(0.03f);
		flash.SetActive(true);
		yield return new WaitForSeconds(0.03f);
		flash.SetActive(false);
		yield return new WaitForSeconds(0.03f);
		flash.SetActive(true);
		yield return new WaitForSeconds(0.03f);
		flash.SetActive(false);
	}
	
	private IEnumerator HyperSpace() {
		AudioManager.instance.PlaySound("Hyperspace");
		flash.SetActive(true);
		yield return new WaitForSeconds(0.5f);
		flash.SetActive(false);
	}
	
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

		velocity *= 1f - dampening * Time.deltaTime;
	}

	private void GlideTransitions()
	{
		// Nothing.
	}

	private void DieActions()
	{
		velocity = 0f;
		StartCoroutine(DeathSequence());
	}

	private void DieTransitions()
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
		if (!input.invulnerable)
		{
			if (other.collider.gameObject.TryGetComponent(out EnemyController enemy))
			{
				enemy.Die();
				Die();
				return;
			}

			if (other.collider.gameObject.TryGetComponent(out EnemyBullet bullet))
			{
				Destroy(bullet.gameObject);
				Die();
			}
		}
	}

	private void Die()
	{
		// Also catches the edge-case where multiple bullets hit simultaneously.
		if (!input.invulnerable && !freezeControls)
		{
			state = State.DIE;
			freezeControls = true;
		}
	}

	public void ResetPlayer()
	{
		cameraOffset.SetXOffsetInstant(offsetRight);
		transform.position = new Vector3(transform.position.x, 0, transform.position.y);
		transform.localScale = new Vector3(1, 1, 1);
		holdingHuman = flipped = false;
	}

	private IEnumerator DeathSequence()
	{
		manager.FreezeEnemies();
		animator.DeathAnimation();
		deathParticles.Play();
		AudioManager.instance.PlaySound("Player Death");

		state = State.IDLE;
		cameraOffset.freeze = true;

		EnemyBullet[] bullets = FindObjectsOfType<EnemyBullet>();
		foreach (EnemyBullet bullet in bullets)
		{
			Destroy(bullet.gameObject);
		}
		// Catch edge case where a cell is being held.
		foreach (Transform child in transform)
		{
			child.BroadcastMessage("Die", SendMessageOptions.DontRequireReceiver);
		}

		yield return new WaitForSeconds(deathDuration);

		// Don't worry about it.
		bool dead = !UIManager.DecrementHealth();
		if (dead)
		{
			UIManager.ShowGameOver();
			AudioManager.instance.bgMusic.Stop();
			AudioManager.instance.PlaySound("Lose");
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

		scrollManager.Scroll(new Vector2(Random.Range(mapLeftBound, mapRightBound), 0f));

		cameraOffset.freeze = false;
		ResetPlayer();

		yield return new WaitForSeconds(0.15f);
		UIManager.HideRefreshScreen();

		animator.ResetAnimation();
		manager.UnFreezeEnemies();
		freezeControls = false;
	}

	private enum State
	{
		IDLE,
		MOVING,
		GLIDING,
		DIE
	}
}
