using UnityEngine;
using UnityEngine.EventSystems;

public class PanelController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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
        isExpanded = !isExpanded;
        UpdatePanelPosition();
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
        isExpanded = true; 
        UpdatePanelPosition();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isExpanded = false; 
        UpdatePanelPosition();
    }
}
