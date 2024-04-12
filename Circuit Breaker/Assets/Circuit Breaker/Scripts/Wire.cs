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
    [Tooltip("The transform that controls where the tile is located. Can also be used to position components")]
    public Transform tileSpot;

    [Header("Optional")]
    [Tooltip("Component to default to on wire. Fill with a CircuitComponent prefab.")]
    public GameObject componentToSpawnWith;
    [Tooltip("Turn on to disable changing a component once it's placed")]
    public bool isLocked;

    [HideInInspector]
    public bool isOpen;
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
        if (componentToSpawnWith == null)
        {
            return;
        }

        // check if component already on wire
        if (activeComponent != null)
        {
            return;
        }

        Instantiate(componentToSpawnWith, tileSpot.position, tileSpot.rotation);
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
        Vector3 pos = (nodeOne.position + nodeTwo.position) / 2f;
        tileSpot.position = pos;
        tileSpot.LookAt(nodeOne);
    }

    // if the given node is connected to this wire, return the other node.
    // if not, return null
    public Transform GetOtherNode(Transform node)
    {
        if(node == nodeOne)
        {
            return nodeTwo;
        }

        if (node == nodeTwo)
        {
            return nodeOne;
        }

        return null;
    }

    /*
    public void InteractWithComponent(Spark spark)
    {
        activeComponent.GetComponent<CircuitComponent>().Activate(spark);
    }
    */
}
