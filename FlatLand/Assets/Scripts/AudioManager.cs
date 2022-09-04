using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;

    [Header("Audio Clips")]
    public List<AudioClip> musicFiles;

    public void Initialize()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            Debug.LogError("Failed to fetch Audio Source.");

        PlayMusicClip(0);
    }

    /// <summary>
    /// Play an music clip from the list of music files, given index.
    /// </summary>
    public void PlayMusicClip(int index)
    {
        if(musicFiles != null)
        {
            if(index < musicFiles.Count)
            {
                audioSource.PlayOneShot(musicFiles[index]);
            }
        }
    }
}
