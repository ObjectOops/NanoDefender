using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
	[Header("Variables")]
	public float moveSpeed;
	public float maxSpeed;
	public float ySpeed;
	public float damping;

	[Header("References")]
	public GameManager manager;
	public PlayerInput input;
	public PlayerAnimator animator;
	public UIManager UIManager;
	public ScrollManager scrollManager;
	public CameraOffset offset;
	public Laser laserPrefab;
	public Transform shootPoint;
	public BoxCollider2D idleCollider;
	public Transform movingShootPoint;
	public BoxCollider2D movingCollider;
	public Transform humanHoldPoint;
	public ParticleSystem deathParticles;
	public GameObject flash;

	public bool holdingHuman;

	/***********************************
	*private variables
	***********************************/
	private Rigidbody2D rb;
	private SpriteRenderer spriteRenderer;
	private Vector2 moveDelta;
	private float velocity;
	private bool flipped;
	private float startX;
	private float moveLerp;
	private bool freezeControls;

	private State state;



	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		startX = spriteRenderer.transform.position.x;
	}

	void Update()
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

	void FixedUpdate()
	{
		moveDelta.x = (velocity) * Time.deltaTime;
		scrollManager.Scroll(moveDelta);


		if (!freezeControls)
		{
			scrollManager.Scroll(new Vector2(-Time.deltaTime, 0));
			YMovement();
		}
	}

	void YMovement()
	{
		float yAxis = Input.GetAxisRaw("Vertical");
		float newY = yAxis * ySpeed * Time.deltaTime;

		transform.position += new Vector3(0f, newY, 0f);

		float clampedY = Mathf.Clamp(transform.position.y, -4.5f, 2.8f);

		transform.position = new Vector3(transform.position.x, clampedY, 0);
	}

	public void AnyActions()
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
			offset.SetXOffset(3);
			transform.localScale = new Vector3(1, 1, 1);
		}
		if (input.GameInput.LeftPressed)
		{
			flipped = true;
			offset.SetXOffset(-3);
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
				foreach (EnemyController enemy in GameObject.FindObjectsOfType<EnemyController>())
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
				animator.BombAnimation();
			}
		}

		if (input.GameInput.HyperspacePressed)
		{
			scrollManager.Scroll(new Vector2(Random.Range(-100, 101), 0f));
			StartCoroutine(Flash());
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
	
	public void AnyTransitions()
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

	public void IdleActions()
	{

	}

	public void IdleTransitions()
	{

	}

	public void MoveActions()
	{
		int dir = flipped ? -1 : 1;
		velocity += dir * moveSpeed * Time.deltaTime;
		velocity = Mathf.Clamp(velocity, -maxSpeed, maxSpeed);

		moveLerp += Time.deltaTime / 2f;
		moveLerp = Mathf.Clamp(moveLerp, 0f, 1f);


		AudioManager.instance.PlaySoundAndWait("Thrust");

		// float lerpedSpriteX = Mathf.Lerp(startX, startX + 1f * lastInputDir, (rightVelocity / maxSpeed) * moveLerp);

		// spriteRenderer.transform.position = new Vector2(lerpedSpriteX, spriteRenderer.transform.position.y);
	}

	public void MoveTransitions()
	{
		if (!input.GameInput.Accelerating)
		{
			state = State.GLIDING;
		}
	}

	public void GlideActions()
	{
		moveLerp -= Time.deltaTime;
		moveLerp = Mathf.Clamp(moveLerp, 0f, 1f);

		// float lerpedSpriteX = Mathf.Lerp(startX, startX + 1f * lastInputDir, moveLerp);

		// spriteRenderer.transform.position = new Vector2(lerpedSpriteX, spriteRenderer.transform.position.y);

		// rightVelocity = Mathf.Lerp(maxSpeed, 0f, frictionLerp);
		velocity *= (1f - damping * Time.deltaTime);

		// frictionLerp += (Mathf.Pow(rightVelocity, 2) / 100f * Time.deltaTime);
	}

	public void GlideTransitions()
	{

	}

	public void DieActions()
	{
		velocity = 0f;
		StartCoroutine(DeathSequence());
	}

	public void DieTransitions()
	{

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
		if (!input.invulnerable)
		{
			state = State.DIE;
			freezeControls = true;
		}
	}

	public void ResetPlayer()
	{
		offset.SetXOffsetInstant(3);
		transform.position = new Vector3(transform.position.x, 0, transform.position.y);
		transform.localScale = new Vector3(1, 1, 1);
		flipped = false;
	}

	private IEnumerator DeathSequence()
	{
		state = State.IDLE;
		offset.freeze = true;

		animator.DeathAnimation();
		deathParticles.Play();
		manager.FreezeEnemies();
		foreach (EnemyBullet bullet in FindObjectsOfType<EnemyBullet>())
		{
			Destroy(bullet.gameObject);
		}
		AudioManager.instance.PlaySound("Player Death");

		yield return new WaitForSeconds(1.4f);

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

		scrollManager.Scroll(new Vector2(Random.Range(-100, 101), 0f));

		offset.freeze = false;
		ResetPlayer();

		yield return new WaitForSeconds(0.15f);
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
