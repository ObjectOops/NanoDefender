using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Laser : MonoBehaviour
{
	[SerializeField]
	private float speed;
	private int direction;

	public ParticleSystem trail;
	public List<Gradient> trailGradients;

	void Update()
	{
		transform.position += transform.right * direction * speed * Time.deltaTime;
		SpawnTrail();
	}

	public void SetDirection(int dir)
	{
		this.direction = dir;
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.TryGetComponent<EnemyController>(out EnemyController enemy))
		{
			StopLaser();
			UIManager.instance.AddPoints(enemy.GetPointValue());
			enemy.Die();
		}

		if (other.gameObject.TryGetComponent<Human>(out Human human))
		{
			StopLaser();
			human.Die();
		}
	}

	private void SpawnTrail()
	{
		StartCoroutine(LerpColor());
	}

	private IEnumerator LerpColor()
	{
		ParticleSystem.MainModule main = trail.main;

		int rand = UnityEngine.Random.Range(0, trailGradients.Count);
		Gradient trailGradient = trailGradients[rand];
		Color startColor = trailGradient.Evaluate(0);
		Color endColor = trailGradient.Evaluate(1);

		main.startColor = startColor;
		float lerpValue = 0;

		while (lerpValue < 1f)
		{
			main.startColor = Color.Lerp(startColor, endColor, lerpValue);
			lerpValue += Time.deltaTime * 4.53f;
			yield return null;
		}
		main.startColor = endColor;
		// yield return new WaitForSeconds();
		Destroy(transform.gameObject);
	}

	public void StopLaser()
	{
		Destroy(gameObject);
	}

}
