using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    public static LevelSelector Instance { get; set;}
    public Button[] levelButtons;


    private void Awake() {
        Instance = this;
    }


    void Start()
    {
        // PlayerPrefs.DeleteKey("Level");
        int levelReached = PlayerPrefs.GetInt("Level", 1);
        for(int i = 0; i < levelButtons.Length; i++)
        {
            if(i + 1 > levelReached)
            {
                Debug.Log(levelButtons[i].gameObject.name);
                levelButtons[i].interactable = false;
            }
        }
    }

    public void UnlockNextLevel(int currentLevel)
    {
        int nextLevel = currentLevel + 1;
        if(PlayerPrefs.GetInt("Level") < nextLevel)
        {
            PlayerPrefs.SetInt("Level", nextLevel);
        }
    }


}
