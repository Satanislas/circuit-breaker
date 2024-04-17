using System;
using UnityEngine;

public class PanelController : MonoBehaviour
{
    public RectTransform panelRect; 
    public float hiddenY; 
    public float shownY; 

    public bool isExpanded; 

    void Start()
    {
        panelRect.anchoredPosition = new Vector2(panelRect.anchoredPosition.x, hiddenY);
        isExpanded = false; 
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TogglePanel();
        }
    }

    public void TogglePanel()
    {
        Debug.Log("Toggle Panel Running");
        if (isExpanded)
        {
            panelRect.anchoredPosition = new Vector2(panelRect.anchoredPosition.x, hiddenY); //hide
            isExpanded = false; 
        }
        else
        {
            panelRect.anchoredPosition = new Vector2(panelRect.anchoredPosition.x, shownY); //show
            isExpanded = true; 
        }
    }
    
}
