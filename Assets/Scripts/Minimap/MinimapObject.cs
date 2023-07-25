using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapObject : MonoBehaviour
{
	[SerializeField] private GameObject minimapObjectPrefab;

	private Enemy enemy;
	private Sprite sprite;

	private BoxCollider2D worldBounds;
	private RectTransform minimapBounds;
	private Transform minimapHolder;
	private GameObject spawnedMinimap;

	private void Start()
	{
		// Hmm.
		worldBounds = GameObject.Find("MinimapWorldBounds").GetComponent<BoxCollider2D>();
		minimapBounds = GameObject.Find("MinimapBounds").GetComponent<RectTransform>();
		minimapHolder = minimapBounds.transform;

		spawnedMinimap = Instantiate(minimapObjectPrefab, minimapHolder.position, Quaternion.identity, minimapHolder);
		if (enemy != null)
		{
			spawnedMinimap.GetComponent<Image>().sprite = enemy.minimapSprite;
		}
		if (sprite != null)
		{
			spawnedMinimap.GetComponent<Image>().sprite = sprite;
		}
	}

	public void Init(Enemy enemy)
	{
		this.enemy = enemy;
	}

	public void Init(Sprite sprite)
	{
		this.sprite = sprite;
	}

	private void Update()
	{
		spawnedMinimap.transform.localPosition = WorldToMinimapSpace(transform.position);
	}

	private void OnDestroy()
	{
		Destroy(spawnedMinimap);
	}

	public Vector3 WorldToMinimapSpace(Vector3 worldSpace)
	{
		// World: (8.494, 2.641)
		// Canvas: (31.1, 43.2)
		// Calculated Scale Factor: (3.6614080527, 16.3574403635)
		// Vector3 minimapVec = new Vector3(worldSpace.x * 3.6614080527f, worldSpace.y * 16.3574403635f);

		Vector2 maxWorld = new Vector2(worldBounds.bounds.max.x, worldBounds.bounds.max.y);
		Vector2 maxMinimap = new Vector2(minimapBounds.rect.max.x, minimapBounds.rect.max.y);
		Vector2 scaleFactor = maxMinimap / maxWorld;
		Vector2 minimapCoords = worldSpace * scaleFactor;
		return minimapCoords;
	}
}
