using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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


    [Header("UI")] public GameObject textCanvas;
    public TextMeshProUGUI textValue;

    [Header("Visuals")] 
    public Gradient gradient;
    public float smallestSize;
    public float biggestSize;
    
    
    [Header("Debug : don't need to be serialized")]
    public int currentValue;
    public Transform targetNode;
    public bool wasIntantiated;
    private Transform lastNode;
    private bool mouseOver;

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
                Destroy(gameObject);
            }
            else if(node.isLamp && !node.isLit)
            {
                if(currentValue >= node.lampChargeNeeded)
                {
                    Debug.Log($"{node.gameObject.name} is a lamp");
                    LampUI.Instance.IncreaseLampCount();
                    node.TurnOnLamp();
                    
                    //We don't need the spark anymore
                    Destroy(gameObject);
                }
            }
            
            //assign the next node
            if (node.connectedWires[0].isOpen)
            {
                targetNode = null;
                return;
            }
            targetNode = node.GetNextNode();
        }
        catch (Exception)
        {
            targetNode = null;
        }
    }

    private void Split(Node node)
    {
        int numberOfNodes = node.connectedWires.Length;
        
        //check if we don't come back from last node
        foreach (var wire in node.connectedWires)
        {
            if (wire.GetOtherNode(node.transform) == lastNode || wire.isOpen)
            {
                numberOfNodes--;
                break;
            }
        }
        
        foreach (Wire wire in node.connectedWires)
        {
            if (wire.GetOtherNode(node.transform) == lastNode || wire.isOpen)
            {
                //Debug.Log(node.name + "is same as " + wire.GetOtherNode(node.transform).name);
                continue;
            }
            
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
        //if target node gets destroyed or anything happen
        if (!targetNode) return;
        if (!mouseOver)
        {
            if (textCanvas.activeSelf)
            {
                textCanvas.SetActive(false);
            }   
        }
        mouseOver = false;
        
        if(currentValue < 1)
        {
            KillMe();
        }
        
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
        lastNode = startNode;
        startNode = targetNode;

        // If the startNode is a logic node input, destroy the spark and defer handling to the node
        if (startNode.GetComponent<Node>() == null) {
            GetComponent<SparkInteraction>().HitLogicComponentInput(startNode);
            KillMe();
            return;
        }

        GetNextNode();
        
        //if there is no wire connected
        if (!targetNode)
        {
            //disable the script
            print("Spark " + name + " arrived with a value of " + currentValue);
            enabled = false;
            LampUI.Instance.WinCondition();
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


    public void KillMe()
    {
        Destroy(gameObject);
    }

    private IEnumerator Shrink() //we'll do an animation instead
    {
        while (transform.localScale.x > 0.01)
        {
            transform.localScale = new Vector3(transform.localScale.x * 0.8f ,transform.localScale.y* 0.8f,transform.localScale.z* 0.8f);
            if (currentValue > 0) currentValue--;
            yield return new WaitForSeconds(0.05f);
        } 
    }

    private void OnMouseExit()
    {
        textCanvas.SetActive(false);
        textValue.text = currentValue.ToString();
        mouseOver = false;
    }

    private void OnMouseOver()
    {
        mouseOver = true;
        textCanvas.SetActive(true);
        textValue.text = currentValue.ToString();
    }
}
