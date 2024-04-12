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
    // public Component component

    private LineRenderer lineRenderer;
    
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();


        DrawLine();
        PositionTileSpot();
    }

    public void DrawLine()
    {
        lineRenderer.SetPosition(0, nodeOne.position);
        lineRenderer.SetPosition(1, nodeTwo.position);
        lineRenderer.enabled = true;
    }

    public void PositionTileSpot()
    {
        Vector3 pos = (nodeOne.position + nodeTwo.position) / 2f;
        tileSpot.position = pos;
        tileSpot.LookAt(nodeOne);
    }
}
