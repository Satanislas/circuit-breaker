using UnityEngine;

public class SpeechBubbleManager : MonoBehaviour
{
    public GameObject[] speechBubbles;
    private int currentIndex = 0;
    private bool firstPanelOpen = true; 

    private void Start()
    {
        foreach (GameObject bubble in speechBubbles)
        {
            bubble.SetActive(false);
        }

        if (speechBubbles.Length > 0)
        {
            speechBubbles[0].SetActive(true);
            speechBubbles[0].transform.SetAsLastSibling();
            var rectTransform = speechBubbles[0].GetComponent<RectTransform>();
            rectTransform.localPosition = new Vector3(rectTransform.localPosition.x, rectTransform.localPosition.y, 0);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && currentIndex != 1 && currentIndex != 8 && currentIndex != 9 && currentIndex != 10)
        {
            ShowNextBubble();
        }else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // ShowPreviousBubble();
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

    // public void ShowPreviousBubble()
    // {
    //     if (currentIndex > 0)
    //     {
    //         //hide current 
    //         speechBubbles[currentIndex].SetActive(false);
    //         
    //         //show previous 
    //         currentIndex--;
    //         speechBubbles[currentIndex].SetActive(true);
    //         
    //         //move to front of UI 
    //         speechBubbles[currentIndex].transform.SetAsLastSibling();
    //     }
    // }

    public void ShowNextBubbleFromPanel()
    {
        if (currentIndex == 1 && firstPanelOpen)
        {
            ShowNextBubble();
            firstPanelOpen = false; 
        }
    }

    public void ComponentPlaced()
    {
        if (currentIndex == 8 || currentIndex == 9 || currentIndex == 10)
        {
            ShowNextBubble();
        }
    }
    
}
