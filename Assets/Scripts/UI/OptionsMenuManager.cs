using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsMenuManager : MonoBehaviour
{

	public AudioMixer mixer;

	[Header("Menus")]
	public GameObject pauseMenu;

	[Header("UI Elements")]
	public Button backButton;
	public Slider musicSlider;
	public Slider sfxSlider;

	private float musicVolume;
	private float sfxVolume;

	void Start()
	{
		backButton.onClick.AddListener(OpenPause);

		musicVolume = PlayerPrefs.GetFloat("musicvolume", 1f);
		sfxVolume = PlayerPrefs.GetFloat("sfxvolume", 1f);

		musicSlider.value = musicVolume;
		sfxSlider.value = sfxVolume;

		UpdateMixerFloat("musicVol", musicVolume);
		UpdateMixerFloat("sfxVol", musicVolume);

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

	void OpenPause()
	{
		pauseMenu.SetActive(true);
		gameObject.SetActive(false);
	}

	void UpdateMusicVolume(float newVol)
	{
		musicVolume = newVol;
		UpdateMixerFloat("musicVol", musicVolume);
	}

	void UpdateSFXVolume(float newVol)
	{
		sfxVolume = newVol;
		UpdateMixerFloat("sfxVol", sfxVolume);
	}

	void UpdateMixerFloat(string name, float newValue)
	{
		mixer.SetFloat(name, Mathf.Log10(newValue) * 20);
	}

}
