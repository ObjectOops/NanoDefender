using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ScrollManager : MonoBehaviour
{
	public BoxCollider2D bounds;
	public Color normalTintColor;
	public Color bossTintColor;

	void Start()
	{
		TintNormal();
	}

	public void Scroll(Vector2 moveDelta)
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			Transform toScroll = transform.GetChild(i);
			float parallaxMult = 1f;
			if (toScroll.TryGetComponent<ParallaxScroller>(out ParallaxScroller parallax))
			{
				parallaxMult = parallax.scrollMultiplier;
			}

			toScroll.position -= new Vector3(moveDelta.x * parallaxMult, moveDelta.y * parallaxMult, 0);

			if (!bounds.bounds.Contains(toScroll.position))
			{
				int sign = Math.Sign(moveDelta.x);
				toScroll.position = bounds.bounds.ClosestPoint(new Vector2(toScroll.position.x + (int)(100 * sign), toScroll.position.y));
				continue;
			}
		}
	}

	public void TintNormal()
	{
		Transform bg = transform.Find("BG");
		Transform fg = transform.Find("FG");
		Transform fg_vein = transform.Find("FG_Vein_1");
		Transform fg_vein2 = transform.Find("FG_Vein_2");
		Tilemap tilemap = transform.Find("FG_Tilemap").GetComponentInChildren<Tilemap>();

		List<SpriteRenderer> toTint = new List<SpriteRenderer>();
		toTint.AddRange(bg.GetComponentsInChildren<SpriteRenderer>());
		toTint.AddRange(fg.GetComponentsInChildren<SpriteRenderer>());
		toTint.AddRange(fg_vein.GetComponentsInChildren<SpriteRenderer>());
		toTint.AddRange(fg_vein2.GetComponentsInChildren<SpriteRenderer>());

		foreach (SpriteRenderer spriteRenderer in toTint)
		{
			Color oldColor = spriteRenderer.color;
			Color newColor = new Color(normalTintColor.r, normalTintColor.g, normalTintColor.b, oldColor.a);
			spriteRenderer.color = newColor;
		}

		tilemap.color = normalTintColor;
	}

	public void TintBoss()
	{
		Transform bg = transform.Find("BG");
		Transform fg = transform.Find("FG");
		Transform fg_vein = transform.Find("FG_Vein_1");
		Transform fg_vein2 = transform.Find("FG_Vein_2");
		Tilemap tilemap = transform.Find("FG_Tilemap").GetComponentInChildren<Tilemap>();


		List<SpriteRenderer> toTint = new List<SpriteRenderer>();
		toTint.AddRange(bg.GetComponentsInChildren<SpriteRenderer>());
		toTint.AddRange(fg.GetComponentsInChildren<SpriteRenderer>());
		toTint.AddRange(fg_vein.GetComponentsInChildren<SpriteRenderer>());
		toTint.AddRange(fg_vein2.GetComponentsInChildren<SpriteRenderer>());


		foreach (SpriteRenderer spriteRenderer in toTint)
		{
			Color oldColor = spriteRenderer.color;
			Color newColor = new Color(bossTintColor.r, bossTintColor.g, bossTintColor.b, oldColor.a);
			spriteRenderer.color = newColor;
		}

		tilemap.color = bossTintColor;
	}
}
