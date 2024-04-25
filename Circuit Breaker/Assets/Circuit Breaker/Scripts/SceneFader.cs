using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour
{
    public void FadeScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
}
