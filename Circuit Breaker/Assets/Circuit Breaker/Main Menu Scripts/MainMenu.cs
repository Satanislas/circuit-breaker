using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button[] buttons;
    void Awake()
    {
        if(!PlayerPrefs.HasKey("Level1"))
        {
            PlayerPrefs.SetInt("Level1", 0);
        }
    }
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
