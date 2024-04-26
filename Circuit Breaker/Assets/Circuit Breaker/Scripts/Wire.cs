using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Wire : MonoBehaviour
{
    [Header("Required")]
    [Tooltip("Nodes this wire connects. Will only ever be two nodes.\nDo not leave empty.")]
    public Transform[] nodes;

    [Header("Optional")]
    [Tooltip("Component to default to on wire.\nFill with a CircuitComponent prefab.")]
    public GameObject defaultComponent;
    [Tooltip("Turn on to disable changing a component once it's placed.")]
    public bool isLocked;
    [Tooltip("Turn on to force a Spark to only traverse from nodes[0] to nodes[1].\nEffectively disables reverse traversal.")]
    public bool isPolarized;
    [Tooltip("Turn on to open this wire. Will not allow traversal across in any direction.")]
    public bool isOpen;
    [Tooltip("Turn on to short circuit this wire. Will force an entire Spark to traverse without splitting at the node.")]
    public bool isShort;

    [Header("Prefab Settings")]
    [Range(0, 1)]
    [Tooltip("Specifies where on the wire the tileSpot resides.\nCloser to 0 indicates closer to nodes[0].\nCloser to 1 indicates closer to nodes[1].\n0.5 indicates perfectly in the middle.")]
    public float tileOffset;

    private LineRenderer lineRenderer;
    
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();

        DrawLine();
        GetComponent<ComponentSlot>().PositionTileSpot(nodes, tileOffset);
    }

    private void Update()
    {
        UpdateColor();
    }

    private void UpdateColor()
    {
        if (isOpen)
        {
            lineRenderer.startColor = Color.red;
            lineRenderer.endColor = Color.red;
            return;
        }

        if (isShort)
        {
            lineRenderer.startColor = Color.red;
            lineRenderer.endColor = Color.red;
            return;
        }

        lineRenderer.startColor = Color.black;
        lineRenderer.endColor = Color.black;
    }

    // draws the line between nodes
    public void DrawLine()
    {
        lineRenderer.SetPosition(0, nodes[0].position);
        lineRenderer.SetPosition(1, nodes[1].position);
        lineRenderer.enabled = true;
    }


    // if the given node is connected to this wire, return the other node.
    // if not, return null
    public Transform GetOtherNode(Transform node)
    {
        if (!IsConnectedTo(node))
        {
            return null;
        }

        if(node == nodes[0])
        {
            return nodes[1];
        }
        
        return nodes[0];
    }

    // returns true if the given node is one of the nodes this wire connects
    public bool IsConnectedTo(Transform node)
    {
        return node == nodes[0] || node == nodes[1];
    }

    public void SetPolarized(int i)
    {
        // sanity check
        if(i != 0 || i != 1)
        {
            //Debug.Log(i + " is not one of the two nodes.");
            return;
        }

        isPolarized = true;

        // if the provided index is 1, swap the node order in nodes[].
        if(i == 1)
        {
            Transform temp = nodes[0];
            nodes[0] = nodes[1];
            nodes[1] = temp;
        }
    }

    public void DisablePolarization()
    {
        isPolarized = false;
    }

    // Given a node, will return the index of the node within nodes[]
    // if not found, and therefore is not conected, returns -1
    public int GetNodeIndex(Transform node)
    {
        for (int i = 0; i < nodes.Length; i++)
        {
            if (nodes[i] == node)
            {
                //Debug.Log(node.name + " has an index of " + i + " in " + name + "'s nodes.");
                return i;
            }
        }

        //Debug.Log(node.name + " is not connected to " + name);
        return -1;
    }

    // Given a node, will return whether or not it is possible to traverse this wire starting from the given node.
    public bool CanTraverse(Transform node)
    {
        if (isOpen)
        {
            //Debug.Log("Wire is open. Cannot traverse.");
            return false;
        }

        if (!IsConnectedTo(node))
        {
            //Debug.Log("Node not connected. Cannot traverse from " + node.name);
            return false;
        }

        if (isPolarized)
        {
            if(GetNodeIndex(node) != 0)
            {
                //Debug.Log("Wire is polarized. " + node.name + " is on the wrong side.");
                return false;
            }
        }

        //Debug.Log("Wire is able to be traversed from " + node.name);
        return true;
    }

    // public void InteractWithComponent(Spark spark)
    // {
    //     activeComponent.GetComponent<ComponentFunction>().SparkActivate(spark);
    // }
}
