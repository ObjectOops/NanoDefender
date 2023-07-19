using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManagerEnd : MonoBehaviour
{
	[Scene]
	public string gameScene;

	public TMP_Text scoreText;
	public AudioSource source;

	private void Start()
	{
		scoreText.text = $"SCORE:{PlayerPrefs.GetInt("score", 9999)}";
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			source.Play();
			
			Invoke("LoadScene", 0.2f);
		}

	}
	
	private void LoadScene() {
		SceneManager.LoadSceneAsync(gameScene);
	}
}
