using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManagerEnd : MonoBehaviour
{
	[SerializeField][Scene] private string gameScene;

	[SerializeField] private TMP_Text scoreText;
	[SerializeField] private AudioSource source;

	[SerializeField] private float sceneTransitionDelay;

	private void Start()
	{
		scoreText.text = $"SCORE:{PlayerPrefs.GetInt("score", 9999)}";
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			source.Play();
			Invoke(nameof(LoadScene), sceneTransitionDelay);
		}
	}
	
	private void LoadScene() {
		SceneManager.LoadSceneAsync(gameScene);
	}
}
