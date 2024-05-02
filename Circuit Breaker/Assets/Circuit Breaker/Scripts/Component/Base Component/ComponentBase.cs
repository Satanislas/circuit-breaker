using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentBase : MonoBehaviour
{
    public ParticleSystem sparkParticles;

    public GameObject hoverHighlight;
    
    private GameObject instantiatedHoverHighlight;
    private ComponentHoverHighlight hoverHighlightScript;

    private GameObject currentlyHoveredTileSpot;
    private Sprite componentSprite;
    private Color highlightColor;

    private bool isActivated;
    private GameObject lastPlacedTileSlot;
    private ComponentFunction componentFunction;

    private string componentType;

    private float clickStartTime;

    void Start() {
        componentSprite = GetComponent<SpriteRenderer>().sprite;
        highlightColor = GetComponent<SpriteRenderer>().color;
        componentFunction = GetComponent<ComponentFunction>();
        highlightColor.a = .39f;

        if (GetComponent<CircuitComponent>() != null) {
            componentType = "circuit";
        } else {
            componentType = "logic";
        }
    }

    void OnMouseDrag() {
        // Check if the mouse has been pressed for some length of time to detect a drag instead of click
        if (Time.time - clickStartTime < .1f) {
            return;
        }

        ClearComponentSlot();
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
            // Ignore Wire component slots if this is a logic component
            if (tileSpot.transform.parent.gameObject.GetComponent<Wire>() != null) {
                if (componentType == "logic") {
                    continue;
                }
            } else {  // Ignore Node component slots if this is a circuit component
                if (componentType == "circuit") {
                    continue;
                }
            }

            // If nearby a component slot, show a transparent version of the component over the nearby slot
            if (Vector3.Distance(transform.position, tileSpot.transform.position) < 3f) {
                // Ignore tile slots that already have a component on them
                if (componentType == "circuit" && tileSpot.transform.parent.gameObject.GetComponent<ComponentSlot>().ActiveComponent != null) {
                    continue;
                }
                if (componentType == "logic" && tileSpot.transform.parent.gameObject.GetComponent<LogicComponentSlot>().attachedLogicComponent != null) {
                    continue;
                }

                currentlyHoveredTileSpot = tileSpot;
                if (instantiatedHoverHighlight == null) {
                    instantiatedHoverHighlight = Instantiate(hoverHighlight);
                    hoverHighlightScript = instantiatedHoverHighlight.GetComponent<ComponentHoverHighlight>();
                    instantiatedHoverHighlight.transform.rotation = Quaternion.Euler(0f, 0f, tileSpot.transform.eulerAngles.x);
                    // Set hover highlight sprite to be the same as the component
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

    // Clear the active component from the slot the component was on
    private void ClearComponentSlot() {
        // For logic components
        if (componentType == "logic") {
            if (lastPlacedTileSlot != null) {
                lastPlacedTileSlot.transform.parent.GetComponent<LogicComponentSlot>().attachedLogicComponent = null;
                lastPlacedTileSlot = null;
            }
            return;
        }

        // For circuit components
        if (lastPlacedTileSlot != null) {
            lastPlacedTileSlot.transform.parent.gameObject.GetComponent<ComponentSlot>().ActiveComponent = null;
            lastPlacedTileSlot = null;
            componentFunction.parentWire = null;
            // sparkParticles.Stop();
        }

        componentFunction.parentWire = null;
    }

    void OnMouseDown() {
        clickStartTime = Time.time;
    }

    void OnMouseUp() {
        if (Time.time - clickStartTime < .1f) {
            componentFunction.ClickInteract();
        }

        // If the component was being hovered over a component slot, snap it into place and assign it to the wire
        if (currentlyHoveredTileSpot != null) {
            // TODO: Move this into the Logic Component class
            // If a logic component is being placed, set the logic component's slot active component to be this component
            if (componentType == "logic") {
                currentlyHoveredTileSpot.transform.parent.GetComponent<LogicComponentSlot>().attachedLogicComponent = gameObject;
            }

            if (componentType == "circuit") {
                float tileSpotYRotation = Mathf.Round(currentlyHoveredTileSpot.transform.eulerAngles.y);
                float yRotation = tileSpotYRotation == 0f ? 180f : 0f;
                float zRotation = tileSpotYRotation == 90f ? 360f - currentlyHoveredTileSpot.transform.eulerAngles.x : currentlyHoveredTileSpot.transform.eulerAngles.x;
                transform.rotation = Quaternion.Euler(0f, yRotation, zRotation);
                currentlyHoveredTileSpot.transform.parent.gameObject.GetComponent<ComponentSlot>().ActiveComponent = transform;
                Wire parentWireScipt = currentlyHoveredTileSpot.transform.parent.gameObject.GetComponent<Wire>();
                if (parentWireScipt != null) {
                    componentFunction.parentWire = parentWireScipt;
                }
            }

            Destroy(instantiatedHoverHighlight.gameObject);
            transform.position = new Vector3(currentlyHoveredTileSpot.transform.position.x, currentlyHoveredTileSpot.transform.position.y, -2f);
            lastPlacedTileSlot = currentlyHoveredTileSpot;
            // sparkParticles.Play();
            return;
        }
    }
}
