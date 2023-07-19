using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
	public Button optionsButton;
	public Button quitButton;

	[Header("Menus")]
	public GameObject optionsMenu;

	void Start()
	{
		optionsButton.onClick.AddListener(OpenOptions);
		quitButton.onClick.AddListener(Quit);

		optionsMenu.SetActive(false);
		gameObject.SetActive(false);
	}

	public void CloseOthers()
	{
		optionsMenu.SetActive(false);
	}

	void OpenOptions()
	{
		optionsMenu.SetActive(true);
		gameObject.SetActive(false);
	}

	void Quit()
	{
		Application.Quit();
	}

}
