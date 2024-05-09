using UnityEngine;
using UnityEngine.EventSystems;
using Quaternion = UnityEngine.Quaternion;

public class DragNDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject componentPrefab;
    public RectTransform canvasRect; 

    private GameObject tempComponent;
    private Camera mainCamera;
    
    public SpeechBubbleManager speechBubbleManager;

    private void Awake()
    {
        canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        mainCamera = Camera.main;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Drag started");
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(eventData.position);
        
        tempComponent = Instantiate(componentPrefab, worldPosition, Quaternion.identity);

        CircuitComponent circuitComp = tempComponent.GetComponent<CircuitComponent>();
        if (circuitComp != null)
        {
            circuitComp.OnMouseDrag();  
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Dragging");
        if (tempComponent != null)
        {
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(eventData.position);
            worldPosition.z = 0;
            tempComponent.transform.position = worldPosition;

            CircuitComponent circuitComp = tempComponent.GetComponent<CircuitComponent>();
            if (circuitComp != null)
            {
                circuitComp.OnMouseDrag();  
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("Drag ended");
        CircuitComponent circuitComp = tempComponent.GetComponent<CircuitComponent>();
        
        if (RectTransformUtility.RectangleContainsScreenPoint(canvasRect, eventData.position, mainCamera))
        {
            if (circuitComp != null)
            {
                Destroy(circuitComp);
            }
            Destroy(tempComponent);
            
        }
        
        if (circuitComp != null)
        {
            //circuitComp.OnMouseDrag();
            circuitComp.OnMouseUp();
            speechBubbleManager.ComponentPlaced();
        }
    }
}
