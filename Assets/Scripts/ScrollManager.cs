using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ScrollManager : MonoBehaviour
{
	public BoxCollider2D bounds;
	public BoxCollider2D minimapBounds;
	public Transform minimapScroll;

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


		for (int i = 0; i < minimapScroll.childCount; i++)
		{
			Transform toScroll = minimapScroll.GetChild(i);

			toScroll.localPosition -= new Vector3(moveDelta.x, moveDelta.y, 0) * 4;

			if (!minimapBounds.bounds.Contains(toScroll.position))
			{
				int sign = Math.Sign(moveDelta.x);
				toScroll.localPosition = new Vector2(toScroll.localPosition.x + (int)(399 * sign), toScroll.localPosition.y);
			}
		}
	}
}
