using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;
    public void FadeScene(string SceneName)
    {
        StartCoroutine(TransitionToLevel());
        SceneManager.LoadScene(SceneName);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            FadeScene("Main Menu");
        }
    }

    IEnumerator TransitionToLevel()
    {
        transition.SetTrigger("beginTransition");
        yield return new WaitForSeconds(transitionTime);
    }

}
