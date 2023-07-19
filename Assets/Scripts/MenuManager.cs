using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
	[Scene]
	public string gameScene;

	public TMP_Text scoreText;
	public TMP_Text prevScore;
	public AudioSource source;

	private void Start()
	{
		scoreText.text = $"HIGHSCORE:{PlayerPrefs.GetInt("highscore", 9999)}";
		prevScore.text = $"SCORE:{PlayerPrefs.GetInt("score", 0)}";
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			source.Play();
			Invoke("LoadScene", 0.2f);
		}
	}

	private void LoadScene()
	{
		SceneManager.LoadSceneAsync(gameScene);
	}
}
