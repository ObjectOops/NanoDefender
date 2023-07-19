
using UnityEngine;
using UnityEngine.UI;

public class MinimapObject : MonoBehaviour
{
	private Enemy enemy;
	private Sprite sprite;

	private BoxCollider2D worldBounds;
	private RectTransform minimapBounds;
	private Transform minimapHolder;

	private GameObject spawnedMinimap;
	public GameObject minimapObjectPrefab;

	void Start()
	{
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

	void Update()
	{
		Vector3 worldPoint = Camera.main.WorldToScreenPoint(transform.position);
		spawnedMinimap.transform.localPosition = WorldToMinimapSpace(transform.position);
	}

	private void OnDestroy()
	{
		Destroy(spawnedMinimap.gameObject);
	}


	public Vector3 WorldToMinimapSpace(Vector3 worldSpace)
	{
		Vector2 maxWorld = new Vector2(worldBounds.bounds.max.x, worldBounds.bounds.max.y);
		Vector2 maxMinimap = new Vector2(minimapBounds.rect.max.x, minimapBounds.rect.max.y);
		// Vector2 maxMinimapLocal = minimapBounds.transform.InverseTransformPoint(maxMinimap);
		// Debug.Log("maxWorld: " + maxWorld);
		// Debug.Log("maxMinimapLocal: " + maxMinimap);
		Vector2 scaleFactor = maxMinimap / maxWorld;
		//8.494, 2.641 world
		//31.1, 43.2 Canvas
		//3.6614080527, 16.3574403635 scale factor
		Vector2 minimapCoords = worldSpace * scaleFactor;
		// Vector3 minimapVec = new Vector3(worldSpace.x * 3.6614080527f, worldSpace.y * 16.3574403635f);
		return minimapCoords;
	}
}