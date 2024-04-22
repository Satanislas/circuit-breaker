using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spark : MonoBehaviour
{
    [Header("Required")]
    public int initialValue;
    public Transform startNode; //is also the current node
    private float speed;
    public float maxSpeed;
    public float minSpeed;
    public GameObject sparkPrefab;

    [Header("Visuals")] 
    public Gradient gradient;
    public float smallestSize;
    public float biggestSize;
    
    
    [Header("Debug : don't need to be serialized")]
    public int currentValue;
    public Transform targetNode;
    public bool wasIntantiated;

    [Header("Lamp UI")]
    public LampUI lampUI;


    private void Start()
    {
        Debug.Log($"{startNode.gameObject.name} is the starting node");
        if (wasIntantiated) return;
        if (!targetNode) // prevent a death recursive loop
            GetNextNode();
        currentValue = initialValue;
        transform.position = startNode.transform.position;
        UpdateSpeed();
    }

    private void GetNextNode()
    {
        try
        {
            Node node = startNode.GetComponent<Node>();

            if (node.IsSplit)
            {
                Split(node);
                Destroy(this.gameObject);
            }
            else if(node.isLamp)
            {
                Debug.Log($"{node.gameObject.name} is a lamp");
                lampUI.IncreaseLampCount();
                node.TurnOnLamp();
                targetNode = startNode.GetComponent<Node>().GetNextNode();
            }
            else
            {
                targetNode = startNode.GetComponent<Node>().GetNextNode();
            }
        }
        catch (Exception)
        {
            targetNode = null;
        }
    }

    private void Split(Node node)
    {
        int numberOfNodes = node.connectedWires.Length;
        foreach (Wire wire in node.connectedWires)
        {
            Spark spark = Instantiate(sparkPrefab,transform.position,Quaternion.identity).GetComponent<Spark>();
            spark.initialValue = initialValue;
            spark.currentValue = currentValue / numberOfNodes;
            spark.startNode = node.transform;
            spark.targetNode = wire.GetOtherNode(node.transform);
            spark.wasIntantiated = true;
            spark.gradient = gradient;
            spark.smallestSize = smallestSize;
            spark.biggestSize = biggestSize;
            spark.minSpeed = minSpeed;
            spark.maxSpeed = maxSpeed;
        }
    }

    private void UpdateVisual()
    {
        var ratio = (float)currentValue / initialValue;
        GetComponent<Renderer>().material.color = gradient.Evaluate(ratio);

        var size = Mathf.Lerp(smallestSize, biggestSize, ratio);
        transform.localScale = new Vector3(size,size,size);
        
        UpdateSpeed();
    }

    private void UpdateSpeed()
    {
        speed = Mathf.Lerp(minSpeed, maxSpeed, (float)currentValue / initialValue);
    }

    private void Update()
    {
        UpdateVisual();
        if (Vector3.Distance(transform.position, targetNode.position) <= 0.01f)
        {
            ReachTargetNode();
            return;
        }
        
        transform.position = Vector3.MoveTowards(transform.position, targetNode.position, Time.deltaTime * speed);
    }

    private void ReachTargetNode()
    {
        startNode = targetNode;
        GetNextNode();
        
        //if there is no wire connected
        if (!targetNode)
        {
            //disable the script
            print("Spark " + name + " arrived with a value of " + currentValue);
            enabled = false;
            WinCondition();
        }
    }

    //not sure if we want to check end node like this or if we want to actually make a node an endnode by bool up top
    public bool IsEndNode()
    {
        if(targetNode == null)
        {
            return true;
        }
        return false;
    }

    public void WinCondition()
    {
        LampUI.Instance.SparksReachEnd();
        LampUI.Instance.GameComplete();
    }

}
