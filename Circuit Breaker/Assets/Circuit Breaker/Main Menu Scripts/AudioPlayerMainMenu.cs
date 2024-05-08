using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayerMainMenu : MonoBehaviour
{
    public AudioSource[] audioSources;
    public SceneFader sceneFader;
    public void PlayHoverAudio()
    {
        audioSources[0].Play();
    }

    public void PlayPressedAudio()
    {
        audioSources[1].Play();
    }

    public void AfterPressed(string level)
    {
        sceneFader.FadeScene(level);
    }

    public void ReturnToMainMenuAudio(string level)
    {
        PlayPressedAudio();
        StartCoroutine(Wait(level));
    }

    IEnumerator Wait(string level)
    {
        yield return new WaitForSeconds(0.5f);
        AfterPressed(level);
    }
}
