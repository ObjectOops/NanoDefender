using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
	[SerializeField][Scene] private string gameScene;

	[SerializeField] private TMP_Text highScore, prevScore;
	[SerializeField] private AudioSource source;

	[SerializeField] private float sceneTransitionDelay;

	private void Start()
	{
		highScore.text = $"HIGHSCORE:{PlayerPrefs.GetInt("highscore", 9999)}";
		prevScore.text = $"SCORE:{PlayerPrefs.GetInt("score", 0)}";
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			source.Play();
			Invoke(nameof(LoadScene), sceneTransitionDelay);
		}
	}

	private void LoadScene()
	{
		SceneManager.LoadSceneAsync(gameScene);
	}
}
