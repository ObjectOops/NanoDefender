using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class OptionsMenuManager : MonoBehaviour
{
	[SerializeField]
	private AudioMixer mixer;

	[Header("Menus")]
	[SerializeField]
	private GameObject pauseMenu;

	[Header("UI Elements")]
	[SerializeField]
	private Button backButton;
	[SerializeField]
	private Slider musicSlider, sfxSlider;

	private float musicVolume;
	private float sfxVolume;

	private void Start()
	{
		musicVolume = PlayerPrefs.GetFloat("musicvolume", 1f);
		sfxVolume = PlayerPrefs.GetFloat("sfxvolume", 1f);

		musicSlider.value = musicVolume;
		sfxSlider.value = sfxVolume;

		UpdateMixerFloat("musicVol", musicVolume);
		UpdateMixerFloat("sfxVol", musicVolume);

		backButton.onClick.AddListener(OpenPause);
		musicSlider.onValueChanged.AddListener(UpdateMusicVolume);
		sfxSlider.onValueChanged.AddListener(UpdateSFXVolume);

		Application.quitting += OnQuit;
		SceneManager.sceneUnloaded += OnSceneUnload;
	}

	private void OnQuit()
	{
		PlayerPrefs.SetFloat("musicvolume", musicVolume);
		PlayerPrefs.SetFloat("sfxvolume", sfxVolume);
	}
	
	private void OnSceneUnload(Scene scene) {
		PlayerPrefs.SetFloat("musicvolume", musicVolume);
		PlayerPrefs.SetFloat("sfxvolume", sfxVolume);
	}

	private void OpenPause()
	{
		pauseMenu.SetActive(true);
		gameObject.SetActive(false);
	}

	private void UpdateMusicVolume(float newVolume)
	{
		musicVolume = newVolume;
		UpdateMixerFloat("musicVol", musicVolume);
	}

	private void UpdateSFXVolume(float newVolume)
	{
		sfxVolume = newVolume;
		UpdateMixerFloat("sfxVol", sfxVolume);
	}

	private void UpdateMixerFloat(string name, float newValue)
	{
		// Convert linear values to decibels, which are on a logarithmic scale.
		mixer.SetFloat(name, Mathf.Log10(newValue) * 20);
	}
}
