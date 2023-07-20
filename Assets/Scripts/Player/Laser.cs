using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
	[SerializeField]
	private float speed, lerpSpeed;

	public ParticleSystem trail;
	public List<Gradient> trailGradients;

	private int direction;

	void Update()
	{
		transform.position += direction * speed * Time.deltaTime * transform.right; // Ordered this way for performance.
		SpawnTrail();
	}

	public void SetDirection(int direction)
	{
		this.direction = direction;
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.TryGetComponent(out EnemyController enemy))
		{
			StopLaser();
			enemy.Die();
			UIManager.instance.AddPoints(enemy.GetPointValue());
			return;
		}

		if (other.gameObject.TryGetComponent(out Human human))
		{
			if (!human.isHeld)
            {
				StopLaser();
				human.Die();
            }
			return;
		}

		//if (other.gameObject.TryGetComponent<BossController>(out BossController boss))
		//{
		//	StopLaser();
		//	boss.Damage();
		//	return;
		//}
	}

	private void SpawnTrail()
	{
		StartCoroutine(LerpColor());
	}

	private IEnumerator LerpColor()
	{
		ParticleSystem.MainModule main = trail.main;

		int rand = Random.Range(0, trailGradients.Count);
		Gradient trailGradient = trailGradients[rand];
		Color startColor = trailGradient.Evaluate(0);
		Color endColor = trailGradient.Evaluate(1);

		main.startColor = startColor;
		float lerpValue = 0;

		while (lerpValue < 1f)
		{
			main.startColor = Color.Lerp(startColor, endColor, lerpValue);
			lerpValue += Time.deltaTime * lerpSpeed;
			yield return null;
		}
		main.startColor = endColor;
		Destroy(transform.gameObject);
	}

	public void StopLaser()
	{
		Destroy(gameObject);
	}
}
