using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
	[Header("Menus")]
	[SerializeField] private GameObject optionsMenu;

	[Header("UI Elements")]
	[SerializeField] private Button optionsButton, quitButton;

	private void Start()
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

	private void OpenOptions()
	{
		optionsMenu.SetActive(true);
		gameObject.SetActive(false);
	}

	private void Quit()
	{
		Application.Quit();
	}
}
