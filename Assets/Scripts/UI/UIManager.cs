using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
	// Singleton.
	public static UIManager instance;

	[Header("Parameters")]
	public int health, healthIconsCountInitial = 10;
	public int smartBombs, smartBombIconsCountInitial = 3;
	public int points;

	[Header("HUD")]
	[SerializeField] private TMP_Text pointsText, gameOverText;
	[SerializeField] private Transform healthHolder, bombHolder;
	[SerializeField] private GameObject refreshScreen;

	private List<GameObject> healthObjects = new List<GameObject>();
	private List<GameObject> bombObjects = new List<GameObject>();

	[Header("Additional Menus")]
	[SerializeField] private PauseMenuManager pauseMenu;

	[Header("Prefabs")]
	public GameObject healthIcon;
	public GameObject bombIcon;

	private bool flash;
	private float flashTimer;

	public static bool paused;

	private void Start()
	{
		instance = this;
		gameOverText.gameObject.SetActive(false);
		for (int i = 0; i < healthIconsCountInitial; i++)
		{
			healthObjects.Add(Instantiate(healthIcon, Vector3.zero, Quaternion.identity, healthHolder));
		}
		for (int i = 0; i < smartBombs; i++)
		{
			bombObjects.Add(Instantiate(bombIcon, Vector3.zero, Quaternion.Euler(0, 0, 90), bombHolder));
		}
		SetHealth(health);
		SetBombs(smartBombs);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			paused = !paused;
			pauseMenu.gameObject.SetActive(paused);
			if (!paused)
			{   
				pauseMenu.CloseOthers();
			}
		}

		// Freeze all objects and stop audio.
		if (paused)
		{
			Time.timeScale = 0f;
			AudioListener.pause = true;
		}
		else
		{
			Time.timeScale = 1f;
			AudioListener.pause = false;
		}

		// Add flashing effect to point counter.
		if (flashTimer >= 0.25f && !flash)
		{
			flash = true;
			flashTimer = 0f;
		}
		if (flashTimer >= 0.1f && flash)
		{
			flash = false;
			flashTimer = 0f;
		}
		if (flash)
		{
			pointsText.text = "";
		}
		else
		{
			pointsText.text = $"{points}";
		}
		flashTimer += Time.deltaTime;
	}

	public void SetHealth(int health)
	{
		this.health = health;
		int i = 0;
		foreach (GameObject healthObj in healthObjects)
		{
			if (i < this.health)
			{
				healthObj.SetActive(true);
			}
			else
			{
				healthObj.SetActive(false);
			}
			i++;
		}
	}

	public bool DecrementHealth()
	{
		if (health <= 0)
		{
			return false;
		}
		else
		{
			SetHealth(health - 1);
			return true;
		}
	}

	public void IncrementHealth()
	{
		SetHealth(health + 1);
	}

	public bool UseBomb()
	{
		if (smartBombs > 0)
		{
			SetBombs(smartBombs - 1);
			return true;
		}
		return false;
	}

	public void StockBomb()
	{
		if (smartBombs < smartBombIconsCountInitial)
		{
			SetBombs(smartBombs + 1);
		}
	}

	public void SetBombs(int count)
	{
		smartBombs = count;
		int i = 0;
		foreach (GameObject smartBomb in bombObjects)
		{
			if (i < count)
			{
				smartBomb.SetActive(true);
			}
			else
			{
				smartBomb.SetActive(false);
			}
			i++;
		}
	}

	public void SetPoints(int count)
	{
		points = count;
		pointsText.text = $"{points}";
	}

	public void AddPoints(int count)
	{
		float thousandsPrev = points / 10_000;
		points += count;
		float thousandsNow = points / 10_000;
		pointsText.text = $"{points}";
		if (thousandsNow > thousandsPrev)
		{
			IncrementHealth();
			StockBomb();
		}
	}

	public void ShowRefreshScreen()
	{
		refreshScreen.SetActive(true);
	}

	public void HideRefreshScreen()
	{
		refreshScreen.SetActive(false);
	}

	public void ShowGameOver()
	{
		gameOverText.gameObject.SetActive(true);
	}

	public IEnumerator ResetScene()
	{
		yield return new WaitForSeconds(2f);
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
	}
}
