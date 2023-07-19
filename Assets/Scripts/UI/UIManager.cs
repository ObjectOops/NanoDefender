using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
	public static UIManager instance;

	private List<GameObject> healthObjects = new List<GameObject>();
	public int health;
	private List<GameObject> bombObjects = new List<GameObject>();
	public int smartBombs;
	public TMP_Text pointsText;
	public TMP_Text gameOverText;

	public Transform healthHolder;
	public Transform bombHolder;
	public GameObject refreshScreen;

	public PauseMenuManager pauseMenu;

	public int points;

	[Header("Prefabs")]
	public GameObject healthIcon;
	public GameObject bombIcon;

	private bool flash;
	private float flashTimer;

	public static bool paused;

	void Start()
	{
		instance = this;
		gameOverText.gameObject.SetActive(false);
		for (int i = 0; i < health; i++)
		{
			healthObjects.Add(Instantiate(healthIcon, Vector3.zero, Quaternion.identity, healthHolder));
		}
		for (int i = 0; i < smartBombs; i++)
		{
			bombObjects.Add(Instantiate(bombIcon, Vector3.zero, Quaternion.Euler(0, 0, 90), bombHolder));
		}
	}

	void Update()
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

	public bool UseBomb()
	{
		if (smartBombs > 0)
		{
			SetBombs(smartBombs - 1);
			return true;
		}
		return false;
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
		points += count;
		pointsText.text = $"{points}";
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
