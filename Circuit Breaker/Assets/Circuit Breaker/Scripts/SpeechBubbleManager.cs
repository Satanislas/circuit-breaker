using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechBubbleManager : MonoBehaviour
{
    public GameObject[] speechBubbles;
    private int currentIndex = 0;

    private void Start()
    {
        foreach (GameObject bubble in speechBubbles)
        {
            bubble.SetActive(false);
        }

        if (speechBubbles.Length > 0)
        {
            speechBubbles[0].SetActive(true);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ShowNextBubble();
        }else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ShowPreviousBubble();
        }
    }

    public void ShowNextBubble()
    {
        if (currentIndex < speechBubbles.Length - 1)
        {
            //hide current bubble 
            speechBubbles[currentIndex].SetActive(false);
            
            //show next one
            currentIndex++;
            speechBubbles[currentIndex].SetActive(true);
        }
    }

    public void ShowPreviousBubble()
    {
        if (currentIndex > 0)
        {
            //hide current 
            speechBubbles[currentIndex].SetActive(false);
            
            //show previous 
            currentIndex--;
            speechBubbles[currentIndex].SetActive(true);
        }
    }
    
}
