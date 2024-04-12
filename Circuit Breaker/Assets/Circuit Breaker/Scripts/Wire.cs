using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Wire : MonoBehaviour
{
    [Header("Required")]
    [Tooltip("The first node to connect to. Do not leave empty.")]
    public Transform nodeOne;
    [Tooltip("The second node to connect to. Do not leave empty.")]
    public Transform nodeTwo;

    [Header("Prefab Settings")]
    [Tooltip("The transform that controls where the tile is located. Can also be used to position components.")]
    public Transform tileSpot;
    [Range(0,1)]
    [Tooltip("Specifies where on the wire the tileSpot resides. 0.5 indicates perfectly in the middle.")]
    public float tileOffset;

    [Header("Optional")]
    [Tooltip("Component to default to on wire. Fill with a CircuitComponent prefab.")]
    public GameObject defaultComponent;
    [Tooltip("Turn on to disable changing a component once it's placed")]
    [Space]
    public bool isLocked;

    [HideInInspector]
    public bool isOpen;
    [HideInInspector]
    public Transform activeComponent;

    private LineRenderer lineRenderer;
    
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();

        DrawLine();
        PositionTileSpot();
        SpawnComponent();
    }

    // spawns a default component if needed
    public void SpawnComponent()
    {
        // check if we should spawn with a component
        if (defaultComponent == null)
        {
            return;
        }

        // check if component already on wire
        if (activeComponent != null)
        {
            return;
        }

        Instantiate(defaultComponent, tileSpot.position, tileSpot.rotation);
    }

    // draws the line between nodes
    public void DrawLine()
    {
        lineRenderer.SetPosition(0, nodeOne.position);
        lineRenderer.SetPosition(1, nodeTwo.position);
        lineRenderer.enabled = true;
    }

    // positions the tile graphic
    public void PositionTileSpot()
    {
        float lerpX = Mathf.Lerp(nodeOne.position.x, nodeTwo.position.x, tileOffset);
        float lerpY = Mathf.Lerp(nodeOne.position.y, nodeTwo.position.y, tileOffset);

        Vector3 pos = new Vector3(lerpX, lerpY, 0f);
        tileSpot.position = pos;
        tileSpot.LookAt(nodeOne);
    }

    // if the given node is connected to this wire, return the other node.
    // if not, return null
    public Transform GetOtherNode(Transform node)
    {
        if (!IsConnectedTo(node))
        {
            return null;
        }

        if(node == nodeOne)
        {
            return nodeTwo;
        }
        
        return nodeOne;
    }

    // returns true if the given node is one of the nodes this wire connects
    public bool IsConnectedTo(Transform node)
    {
        return node == nodeOne || node == nodeTwo;
    }

    /*
    public void InteractWithComponent(Spark spark)
    {
        activeComponent.GetComponent<CircuitComponent>().Activate(spark);
    }
    */
}
