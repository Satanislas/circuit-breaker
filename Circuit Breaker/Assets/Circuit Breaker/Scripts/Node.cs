using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [Header("Required")]
    [Tooltip("Fill with all the wires that connect to this node. Should never be empty.")]
    public Wire[] connectedWires;

    public int ConnectionNum
    {
        get { return connectedWires.Length; }
    }

    private void Awake()
    {
        Debug.Log(GetWireIndex(connectedWires[1]));
    }

    // returns the next node along a given wire, indicated by wireIndex
    public Transform GetNextNode(int wireIndex)
    {
        // sanity checks
        if(wireIndex >= connectedWires.Length || wireIndex < 0)
        {
            Debug.Log(wireIndex + "Not within connectedWires index range.");
            return null;
        }

        Transform otherNode = connectedWires[wireIndex].GetOtherNode(this.transform);
        Debug.Log(name + " is connected to " + otherNode.name + " by " + connectedWires[wireIndex].name);
        return otherNode;
    }

    // for a given wire, returns the index of that wire within connectedWires.
    // if given wire is not connected to this node, returns -1
    public int GetWireIndex(Wire wireCheck)
    {
        for (int i = 0; i < connectedWires.Length; i++)
        {
            if(connectedWires[i] == wireCheck)
            {
                Debug.Log(wireCheck.name + " has an index of " + i +" in " + name + "'s connectedWires.");
                return i;
            }
        }

        Debug.Log(wireCheck.name + " is not connected to " + name);
        return -1;
    }
}
