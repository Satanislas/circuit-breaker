using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

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

    // [Tooltip("Component to default to on wire.\nFill with a CircuitComponent prefab.")]
    // public GameObject defaultComponent;

    // [Header("Unity Settings")]
    // [Tooltip("The transform that controls where the tile is located.\nCan also be used to position components.\nDo not change.")]
    // public Transform tileSpot;
    // [Tooltip("The visual of the empty tile. Child of tileSpot\nCan also be used to position components.\nDo not change.")]
    // public Transform tileIcon;

    public int ConnectionNum
    {
        get { return connectedWires.Length; }
    }

    public bool IsSplit{
        get{ return connectedWires.Length > 1;}
    }

    // private Transform activeComponent;

    // public Transform ActiveComponent{
    //     get { return activeComponent; }
    //     set 
    //     {
    //         // //sets parentWire within ComponentFunction
    //         // if(value != null)
    //         // {
    //         //     value.transform.GetComponent<ComponentFunction>().parentWire = this;
    //         // }
    //         activeComponent = value; 
    //     }
    // }

    // void Start() {
    //     PositionTileSpot();
    //     SpawnComponent();
    // }

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

    // positions the tile graphic
    // public void PositionTileSpot()
    // {
    //     // float lerpX = Mathf.Lerp(nodes[0].position.x, nodes[1].position.x, tileOffset);
    //     // float lerpY = Mathf.Lerp(nodes[0].position.y, nodes[1].position.y, tileOffset);

    //     // Vector3 pos = new Vector3(lerpX, lerpY, 0f);
    //     tileSpot.position = transform.position;
    //     // tileSpot.LookAt(nodes[0], Vector3.up);

    //     // tileIcon.Translate(Vector3.back, Space.World);
    //     // tileIcon.LookAt(tileIcon.position + Vector3.back, tileSpot.up);
    // }

    // // spawns a default component if needed
    // public void SpawnComponent()
    // {
    //     // check if we should spawn with a component
    //     if (defaultComponent == null)
    //     {
    //         return;
    //     }

    //     // check if component already on wire
    //     if (activeComponent != null)
    //     {
    //         return;
    //     }

    //     Instantiate(defaultComponent, transform.position, Quaternion.identity);
    // }

    // public void InteractWithComponent(Spark spark)
    // {
    //     activeComponent.GetComponent<ComponentFunction>().SparkActivate(spark);
    // }
}
