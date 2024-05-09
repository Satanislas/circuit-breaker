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
    public ParticleSystem sparkParticle;

    [Header("Visuals")] 
    public Gradient gradient;
    public float smallestSize;
    public float biggestSize;
    
    
    [Header("Debug : don't need to be serialized")]
    public int maxValue;
    public int currentValue;
    public Transform targetNode;
    public bool wasIntantiated;
    private Transform lastNode;
    [Tooltip("Used to ignore the capacitor this is spawned from.")]
    public bool wasCapacitor;

    private void Start()
    {
        GetComponent<AudioSource>().Play();
        Debug.Log($"{startNode.gameObject.name} is the starting node");
        if (wasIntantiated) return;
        if (!targetNode) // prevent a death recursive loop
            GetNextNode();
        currentValue = initialValue;
        transform.position = startNode.transform.position;
        UpdateSpeed();
        // GetComponent<AudioSource>().Play();
    }

    private void GetNextNode()
    {
        // GetComponent<AudioSource>().Play();
        try
        {
            Node node = startNode.GetComponent<Node>();


            if (node.IsSplit)
            {
                Split(node);
                Destroy(gameObject);
            }
            if(node.isShort == true)
            {
               targetNode = startNode.GetComponent<Node>().GetNextNode();

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
            // kills spark if at the end of a wire and no other nodes can be found.
            KillMe();
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
        float ratio = (float)currentValue / maxValue;
        GetComponent<Renderer>().material.color = gradient.Evaluate(ratio);
        var sparkParticleMain = sparkParticle.main;
        try {
            sparkParticleMain.startColor = gradient.Evaluate(ratio);
        } catch (NullReferenceException) {
        }

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
        
        if(currentValue < 1)
        {
            KillMe();
        }
        
        UpdateVisual();
        if (Vector3.Distance(transform.position, targetNode.position) <= 0.01f)
        {
            Debug.Log("AUDIO");
            GetComponent<AudioSource>().Play();
            ReachTargetNode();
            return;
        }
        
        transform.position = Vector3.MoveTowards(transform.position, targetNode.position, Time.deltaTime * speed);
    }

    private void ReachTargetNode()
    {
        wasCapacitor = false;
        lastNode = startNode;
        startNode = targetNode;


        Node node = startNode.GetComponent<Node>();
        Spark spark = sparkPrefab.GetComponent<Spark>();
        
        /*
        if(node.isGround){
            //node.GroundSpark(spark);
        }
        */
        
        // If the startNode is a logic node input, destroy the spark and defer handling to the node
        if (node == null) {
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
            currentValue--;
            yield return new WaitForSeconds(0.05f);
        } 
    }
}
