using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class CircuitComponent : MonoBehaviour {
    public ParticleSystem sparkParticles;

    public GameObject hoverHighlight;
    
    private GameObject instantiatedHoverHighlight;
    private ComponentHoverHighlight hoverHighlightScript;

    private GameObject currentlyHoveredTileSpot;
    private Sprite componentSprite;
    private Color highlightColor;

    private bool isActivated;
    private GameObject lastPlacedTileSlot;

    void Start() {
        componentSprite = GetComponent<SpriteRenderer>().sprite;
        highlightColor = GetComponent<SpriteRenderer>().color;
        highlightColor.a = .39f;

    }

    void OnMouseDrag() {
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
            // If nearby a component slot, show a transparent version of the component over the nearby slot
            if (Vector3.Distance(transform.position, tileSpot.transform.position) < 3f) {
                // Ignore tile slots that already have a component on them
                if (tileSpot.transform.parent.gameObject.GetComponent<Wire>().ActiveComponent != null) {
                    break;
                }
                currentlyHoveredTileSpot = tileSpot;
                if (instantiatedHoverHighlight == null) {
                    instantiatedHoverHighlight = Instantiate(hoverHighlight);
                    hoverHighlightScript = instantiatedHoverHighlight.GetComponent<ComponentHoverHighlight>();
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

    void OnMouseDown() {

        // If the component was previously on a slot, but now is not, clear the previous wire's component slot
        if (lastPlacedTileSlot != null) {
            lastPlacedTileSlot.transform.parent.gameObject.GetComponent<Wire>().ActiveComponent = null;
            lastPlacedTileSlot = null;
            // sparkParticles.Stop();
        }
    }

    void OnMouseUp() {

        // If the component was being hovered over a component slot, snap it into place and assign it to the wire
        if (currentlyHoveredTileSpot) {
            Destroy(instantiatedHoverHighlight.gameObject);
            transform.position = new Vector3(currentlyHoveredTileSpot.transform.position.x, currentlyHoveredTileSpot.transform.position.y, -1f);
            // float yRotation = currentlyHoveredTileSpot.transform.rotation.y
            transform.rotation = Quaternion.Euler(0f, currentlyHoveredTileSpot.transform.rotation.y, currentlyHoveredTileSpot.transform.rotation.eulerAngles.x);
            currentlyHoveredTileSpot.transform.parent.gameObject.GetComponent<Wire>().ActiveComponent = transform;
            lastPlacedTileSlot = currentlyHoveredTileSpot;
            // sparkParticles.Play();
        }

    }
}
