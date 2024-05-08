using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public SceneFader sceneFader;
    public GameObject optionsPanel;
    private AudioSource[] audioSources;
    void Start()
    {
        audioSources = FindObjectsOfType<AudioSource>();
    }
    public void NextScene(string sceneName)
    {
        foreach (AudioSource audioSource in audioSources)
        {
            if(audioSource != null)
            {
                if(audioSource.isPlaying)
                {
                    if(audioSource.gameObject.GetComponent<Button>() is not null)
                    {
                        StartCoroutine(isPlaying(audioSource, sceneName));
                    }
                }
            }
            
        }
    }
    IEnumerator isPlaying(AudioSource audioSource, string sceneName)
    {
        while(audioSource.isPlaying)
        {
            yield return null;
        }
        sceneFader.FadeScene(sceneName);
    }
    public void Quit()
    {
        Debug.Log("QUITTING GAME");
        Application.Quit();
    }

    public void Options()
    {

    }
}
