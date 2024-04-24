using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Node : MonoBehaviour
{
    [Header("Required")]
    [Tooltip("Fill with all the wires that connect to this node.\nShould never be empty.")]
    public Wire[] connectedWires;

    [Header("Optional")]
    [Tooltip("Turn on to make this node Ground. Will destroy a Spark when encountered.")]
    public bool isGround;
    [Header("Lamp")]
    public bool isLamp; //A LAMP NODE CANNOT ALSO BE A SPLIT NODE
    public GameObject lamp;
    public int amountOfChargeNeeded = 1;

    void Start()
    {
        if (isLamp)
        {
            LampUI.Instance.lampCount++;
            Debug.Log($"LAMPCOUNT: {LampUI.Instance.lampCount}");
        }
    }

    public int ConnectionNum
    {
        get { return connectedWires.Length; }
    }

    public bool IsSplit{
        get{ return connectedWires.Length > 2;}
    }

    // returns the next node along the wire.
    // only used for wires with no splits.
    public Transform GetNextNode()
    {
        if (connectedWires.Length == 0){
            //Debug.Log(name + " has no connections.");
            return null;
        }
        
        Transform otherNode = connectedWires[0].GetOtherNode(this.transform);
        //Debug.Log(name + " is connected to " + otherNode.name + " by " + connectedWires[0].name);
        return otherNode;
    }

    // returns the next node along a given wire, indicated by wireIndex
    public Transform GetNextNode(int wireIndex)
    {
        if (connectedWires.Length == 0){
            //Debug.Log(name + " has no connections.");
            return null;
        }

        // sanity checks
        if(wireIndex >= connectedWires.Length || wireIndex < 0)
        {
            //Debug.Log(wireIndex + "Not within connectedWires index range.");
            return null;
        }

        Transform otherNode = connectedWires[wireIndex].GetOtherNode(this.transform);
        //Debug.Log(name + " is connected to " + otherNode.name + " by " + connectedWires[wireIndex].name);
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
                //Debug.Log(wireCheck.name + " has an index of " + i +" in " + name + "'s connectedWires.");
                return i;
            }
        }

        //Debug.Log(wireCheck.name + " is not connected to " + name);
        return -1;
    }

    public void TurnOnLamp()
    {
        lamp.SetActive(true);
    }

    



    /*
    public void GroundSpark(Spark spark){
        Destroy(spark.gameObject);
    }
    */
}
