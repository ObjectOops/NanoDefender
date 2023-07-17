using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField]
    private List<string> audioNames;
    [SerializeField]
    private List<AudioClip> audioClips;
    [SerializeField]
    private AudioSource audioComponent;

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
    }

    public void PlaySound(string name, float volumeScale = 1)
    {
        audioComponent.PlayOneShot(audioMap[name], volumeScale);
    }
}
