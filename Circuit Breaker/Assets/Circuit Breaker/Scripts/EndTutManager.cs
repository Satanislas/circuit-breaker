using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndTutManager : MonoBehaviour
{
    public GameObject congratsBubble;
    public GameObject returnButton;

    public void ShowCongratsBubble()
    {
        StartCoroutine(ShowCongratsBubbleDelayed());
    }

    private IEnumerator ShowCongratsBubbleDelayed()
    {
        yield return new WaitForSeconds(5f);
        congratsBubble.SetActive(true);
        returnButton.SetActive(true);
    }

    public void ReturnToLevelSelect()
    {
        SceneManager.LoadScene("LevelSelector");
    }
}
