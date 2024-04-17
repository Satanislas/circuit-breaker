using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spark : MonoBehaviour
{
    [Header("Required")]
    public int initialValue;
    public Transform startNode; //is also the current node
    public float speed;
    
    
    
    [Header("Debug : not to be serialized")]
    public int currentValue;
    public Transform targetNode;


    private void Start()
    {
        GetNextNode();
        currentValue = initialValue;
        transform.position = startNode.transform.position;
    }

    private void GetNextNode()
    {
        try
        {
            targetNode = startNode.GetComponent<Node>().GetNextNode();
        }
        catch (Exception)
        {
            targetNode = null;
        }
    }

    private void Update()
    {
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
        }
    }
}
