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

	[HideInInspector]
	public Dictionary<string, AudioClip> audioMap = new Dictionary<string, AudioClip>();

	// private bool heeHee;

	private void Start()
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

		// int rand = UnityEngine.Random.Range(1, 21);
		// if (rand == 1)
		// {
		// 	heeHee = true;
		// }
	}

	public IEnumerator PlayBossIntro()
	{
		bgMusic.Stop();
		bgMusic.clip = audioMap["BossStart"];
		bgMusic.loop = false;
		bgMusic.Play();
		while (bgMusic.isPlaying)
		{
			yield return null;
		}
	}

	public void PlayBossMusic()
	{
		bgMusic.Stop();
		bgMusic.clip = audioMap["BossLoop"];
		bgMusic.loop = true;
		bgMusic.Play();
	}

	public void PlaySound(string name, float volumeScale = 1)
	{
		audioComponent.PlayOneShot(audioMap[name], volumeScale);
	}

	public void PlaySoundAndWait(string name)
	{
		if (!audioComponent.isPlaying)
		{
			audioComponent.clip = audioMap[name];
			audioComponent.Play();
		}
	}

	// private void Update()
	// {
	// 	if (heeHee)
	// 	{
	// 		bgMusic.pitch = 1 + Mathf.Clamp(UnityEngine.Random.Range((-Time.time - 3f) / 20f, (Time.time - 3f) / 20f), 0, float.MaxValue);
	// 	}
	// }
}
