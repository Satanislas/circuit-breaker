using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class CircuitComponent : MonoBehaviour {
    public ParticleSystem placeComponentParticles;

    public GameObject hoverHighlight;
    
    private GameObject instantiatedHoverHighlight;
    private ComponentHoverHighlight hoverHighlightScript;

    private GameObject currentlyHoveredTileSpot;
    private Sprite componentSprite;
    private Color highlightColor;

    private bool isActivated;
    private GameObject lastPlacedTileSlot;
    private ComponentFunction componentFunction;


    private PlayBuildManager playBuildManager;

   // public bool dontMove;


    private float clickStartTime;

    void Start() {
        componentSprite = GetComponent<SpriteRenderer>().sprite;
        highlightColor = GetComponent<SpriteRenderer>().color;
        componentFunction = GetComponent<ComponentFunction>();
        highlightColor.a = .39f;
        playBuildManager = PlayBuildManager.instance;
    }


    public void SetLastPlacedTileSlot(GameObject thing){
        lastPlacedTileSlot = thing;
    }

    public void OnMouseDrag()
    {
        // check if in play mode
        if (!playBuildManager.isBuilding)
        {
            return;
        }

       //if (dontMove) return;

        // Check if the mouse has been pressed for some length of time to detect a drag instead of click
        if (Time.time - clickStartTime < .1f) {
            return;
        }

        InteractWithComponent();
        transform.rotation = Quaternion.identity;

        // Get the mouse's position in world space
        Vector3 mousePositionOnScreen =
            Camera.main.ViewportToWorldPoint(
                Camera.main.ScreenToViewportPoint(Input.mousePosition)
            );

        // Search for nearby component slots
        currentlyHoveredTileSpot = null;
        GameObject[] allTileSpots = GameObject.FindGameObjectsWithTag("TileSpot");
        foreach (GameObject tileSpot in allTileSpots) {
            // Ignore Node component slots since circuit components only go on wire
            if (tileSpot.transform.parent.gameObject.GetComponent<Wire>() == null) {
                continue;
            }

            // If nearby a component slot, show a transparent version of the component over the nearby slot
            if (Vector3.Distance(transform.position, tileSpot.transform.position) < 3f) {
                // Ignore tile slots that already have a component on them
                if (tileSpot.transform.parent.gameObject.GetComponent<ComponentSlot>().ActiveComponent != null) {
                    break;
                }
                currentlyHoveredTileSpot = tileSpot;
                if (instantiatedHoverHighlight == null) {
                    instantiatedHoverHighlight = Instantiate(hoverHighlight);
                    hoverHighlightScript = instantiatedHoverHighlight.GetComponent<ComponentHoverHighlight>();
                    instantiatedHoverHighlight.transform.rotation = Quaternion.Euler(0f, 0f, tileSpot.transform.eulerAngles.x);
                    instantiatedHoverHighlight.GetComponent<SpriteRenderer>().sprite = componentSprite;
                    instantiatedHoverHighlight.GetComponent<SpriteRenderer>().color = highlightColor;
                }
                hoverHighlightScript.SnapToComponentSlot(tileSpot.transform);
                break;
            }
        }

        // Destroy the transparent component when out of range
        if (instantiatedHoverHighlight != null && currentlyHoveredTileSpot == null) {
            Destroy(instantiatedHoverHighlight.gameObject);
        }

        // Follow the mouse
        transform.position = new Vector3(mousePositionOnScreen.x, mousePositionOnScreen.y, -2f);
    }

    private void InteractWithComponent() {
        // If the component was previously on a slot, but now is not, clear the previous wire's component slot
        if (lastPlacedTileSlot != null) {
            lastPlacedTileSlot.transform.parent.gameObject.GetComponent<ComponentSlot>().ActiveComponent = null;
            lastPlacedTileSlot = null;
            componentFunction.parentWire = null;
            // sparkParticles.Stop();
        }

        componentFunction.parentWire = null;
    }

    public void OnMouseDown() {
        clickStartTime = Time.time;
    }

    public void OnMouseUp() {
        if (Time.time - clickStartTime < .1f)
        {
            componentFunction.ClickInteract();
            return;
        }

        /*
        if (dontMove)
        {
            componentFunction.ClickInteract();
            return;
        }
        */
        // If the component was being hovered over a component slot, snap it into place and assign it to the wire
        if (currentlyHoveredTileSpot) {
            //Destroy(instantiatedHoverHighlight.gameObject);
            transform.position = new Vector3(currentlyHoveredTileSpot.transform.position.x, currentlyHoveredTileSpot.transform.position.y, -2f);

            float tileSpotYRotation = Mathf.Round(currentlyHoveredTileSpot.transform.eulerAngles.y);
            float yRotation = tileSpotYRotation == 0f ? 180f : 0f;
            float zRotation = tileSpotYRotation == 90f ? 360f - currentlyHoveredTileSpot.transform.eulerAngles.x : currentlyHoveredTileSpot.transform.eulerAngles.x;
            transform.rotation = Quaternion.Euler(0f, yRotation, zRotation);

            //transform.LookAt(transform.position + Vector3.forward, currentlyHoveredTileSpot.transform.up);

            currentlyHoveredTileSpot.transform.parent.gameObject.GetComponent<ComponentSlot>().ActiveComponent = transform;
            Wire parentWireScipt = currentlyHoveredTileSpot.transform.parent.gameObject.GetComponent<Wire>();
            if (parentWireScipt != null) {
                componentFunction.parentWire = parentWireScipt;
                
                /*
                if (componentFunction.componentType == 3) // 3 : SWITCH
                {
                    parentWireScipt.isOpen = true; //to match the starting position of the switch
                } 
                */
            }
            lastPlacedTileSlot = currentlyHoveredTileSpot;
            placeComponentParticles.Play();
            return;
        }
    }
}
