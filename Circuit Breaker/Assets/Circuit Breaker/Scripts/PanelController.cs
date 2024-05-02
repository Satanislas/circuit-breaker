
using UnityEngine;
using UnityEngine.EventSystems;

public class PanelController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public RectTransform panelRect; 
    public float hiddenY; 
    public float shownY;
    public SpeechBubbleManager SpeechBubbleManager;

    public bool isExpanded;
    private bool firstTimeOpened = true; 

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
        isExpanded = !isExpanded;
        UpdatePanelPosition();

        if (isExpanded && firstTimeOpened)
        {
            firstTimeOpened = false;
            SpeechBubbleManager.ShowNextBubbleFromPanel();
            
        }
        
    }

    public void UpdatePanelPosition()
    {
        if (!isExpanded)
        {
            panelRect.anchoredPosition = new Vector2(panelRect.anchoredPosition.x, hiddenY); //hide
        }
        else
        {
            panelRect.anchoredPosition = new Vector2(panelRect.anchoredPosition.x, shownY); //show
        }
    }
    

    public void OnPointerEnter(PointerEventData eventData)
    {
        // isExpanded = true; 
        // UpdatePanelPosition();
        TogglePanel();
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isExpanded = false; 
        UpdatePanelPosition();
    }
}
