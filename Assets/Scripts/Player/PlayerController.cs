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
	public ParticleSystem deathParticles;

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
				DieActions();
				DieTransitions();
				break;

		}
	}

	void FixedUpdate()
	{
		moveDelta.x = (velocity) * Time.deltaTime;
		scrollManager.Scroll(moveDelta);

		if (!freezeControls)
		{
			YMovement();
		}
	}

	void YMovement()
	{
		float yAxis = Input.GetAxisRaw("Vertical");
		float newY = yAxis * ySpeed * Time.deltaTime;

		transform.position += new Vector3(0f, newY, 0f);

		float clampedY = Mathf.Clamp(transform.position.y, -5f, 3f);

		transform.position = new Vector3(transform.position.x, clampedY, 0);
	}

	public void AnyActions()
	{
		if (freezeControls)
		{
			return;
		}
		if (input.gameInput.RightPressed)
		{
			flipped = false;
			offset.SetXOffset(3);
			transform.localScale = new Vector3(1, 1, 1);
		}
		if (input.gameInput.LeftPressed)
		{
			flipped = true;
			offset.SetXOffset(-3);
			transform.localScale = new Vector3(-1, 1, 1);
		}
		if (input.gameInput.AttackPressed)
		{
			ShootLaser();
		}
		if (input.gameInput.BombPressed)
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
			}
		}
	}

	public void AnyTransitions()
	{
		if (freezeControls)
		{
			return;
		}
		if (input.gameInput.Accelerating)
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

		// float lerpedSpriteX = Mathf.Lerp(startX, startX + 1f * lastInputDir, (rightVelocity / maxSpeed) * moveLerp);

		// spriteRenderer.transform.position = new Vector2(lerpedSpriteX, spriteRenderer.transform.position.y);
	}

	public void MoveTransitions()
	{
		if (!input.gameInput.Accelerating)
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

	private void ShootLaser()
	{
		Laser laserInst = Instantiate(laserPrefab, shootPoint.position, Quaternion.identity);
		int dir = flipped ? -1 : 1;
		laserInst.SetDirection(dir);
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.TryGetComponent<EnemyController>(out EnemyController enemy))
		{
			enemy.Die();
			Die();
		}
	}

	private void Die()
	{
		state = State.DIE;
		freezeControls = true;
	}

	private IEnumerator DeathSequence()
	{
		state = State.IDLE;
		offset.freeze = true;
        
        animator.DieAnim();
        deathParticles.Play();
		manager.FreezeEnemies();    
		yield return new WaitForSeconds(1.4f);
		
		bool dead = !UIManager.DecrementHealth();
		if(dead) {
		    UIManager.ShowGameOver();
		    StartCoroutine(UIManager.ResetScene());
		    
		    spriteRenderer.enabled = false;
		    yield break;
		}
		UIManager.ShowRefreshScreen();
        
        offset.freeze = false;
		offset.SetXOffsetInstant(3);
		transform.position = new Vector3(transform.position.x, 0, transform.position.y);
		transform.localScale = new Vector3(1, 1, 1);
		flipped = false;

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
