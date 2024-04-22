using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    public void SelectLevel(string levelName)
    {
        Debug.Log($"GOING TO {levelName}");
    }
}
