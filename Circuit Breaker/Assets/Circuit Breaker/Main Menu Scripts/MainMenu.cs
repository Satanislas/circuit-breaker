using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        Debug.Log("GOING TO LEVEL SELECTOR");
        // SceneManager.LoadScene("LevelSelector");
    }
    public void Quit()
    {
        Debug.Log("QUITTING GAME");
        // Application.Quit();
    }
}
