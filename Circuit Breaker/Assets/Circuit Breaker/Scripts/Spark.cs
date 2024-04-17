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

    [Header("Visuals")] 
    public Gradient gradient;
    public float smallestSize;
    public float biggestSize;
    
    
    [Header("Debug : not to be serialized")]
    public int currentValue;
    public Transform targetNode;


    private void Start()
    {
        GetNextNode();
        currentValue = initialValue;
        transform.position = startNode.transform.position;
        UpdateSpeed();
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
        }
    }
}
