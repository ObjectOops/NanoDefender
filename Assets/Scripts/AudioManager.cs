using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public static AudioManager instance;

	public AudioSource bgMusic;

	[SerializeField]
	private List<string> audioNames;
	[SerializeField]
	private List<AudioClip> audioClips;
	[SerializeField]
	private AudioSource audioComponent;

	private bool heeHee;

	[HideInInspector]
	public Dictionary<string, AudioClip> audioMap = new Dictionary<string, AudioClip>();

	void Start()
	{
		instance = this;
		if (audioNames.Count != audioClips.Count)
		{
			Debug.LogWarning("The number of audio names does not match the number of audio clips.");
		}
		for (int i = 0; i < audioNames.Count; ++i)
		{
			audioMap.Add(audioNames[i], audioClips[i]);
		}

		int rand = UnityEngine.Random.Range(1, 21);
		if (rand == 1)
		{
			heeHee = true;
		}
	}

	public void PlaySound(string name, float volumeScale = 1)
	{
		audioComponent.PlayOneShot(audioMap[name], volumeScale);
	}

	public void PlaySoundAndWait(string name, float volumeScale = 1)
	{
		if (!audioComponent.isPlaying)
		{
			audioComponent.clip = audioMap[name];
			audioComponent.Play();
		}

	}

	private void Update()
	{
		if (heeHee)
		{
			bgMusic.pitch = 1 + Mathf.Clamp(UnityEngine.Random.Range((-Time.time - 3f) / 20f, (Time.time - 3f) / 20f), 0, float.MaxValue);
		}
	}
}
