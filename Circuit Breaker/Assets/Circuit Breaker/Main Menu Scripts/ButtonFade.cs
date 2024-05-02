using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ButtonFade : MonoBehaviour
{
    public Button[] buttons;

    void Start()
    {
        buttons = FindObjectsOfType<Button>();
    }

    public void Hover()
    {
        Debug.Log("HOVERING");
        // BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();
        foreach (Button button in buttons)
        {
            if(button.gameObject != gameObject)
            {
                button.interactable = false;
                if(button.gameObject.name == "Quit Button")
                {
                    button.gameObject.GetComponentInChildren<TextMeshProUGUI>().fontSize = 18;
                }
            }
        }
    }

    public void HoverExit()
    {
        // BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();
        foreach (Button button in buttons)
        {
            if(gameObject != button.gameObject)
            {
                button.interactable = true;
            }
        }
    }
}
