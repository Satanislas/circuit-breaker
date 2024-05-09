using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMusic : MonoBehaviour
{
    public AudioSource[] audioSources;

    void Start()
    {
        audioSources = GetComponents<AudioSource>();
        PlayRandomLevelSong();
    }
    
    public void PlayRandomLevelSong()
    {
        int size = audioSources.Length;
        int randomNum = Random.Range(0, size);
        Debug.Log($"Random Num {randomNum}");
        audioSources[randomNum].Play();
    }
}
