using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sparkbank : MonoBehaviour
{
    [Header("Spark Settings")]
    [Tooltip("Charge to spawn the spark with.")]
    public int value;
    [Header("Unity Setup")]
    public GameObject sparkPrefab;
    private Transform parentNode;

    private void Awake()
    {
        parentNode = transform.parent;
    }

    public void SpawnSpark()
    {
        GameObject newSpark = Instantiate(sparkPrefab, parentNode.position, Quaternion.identity);
        Spark sparkScript = newSpark.GetComponent<Spark>();
        sparkScript.wasIntantiated = true;
        sparkScript.currentValue = value;
        sparkScript.startNode = parentNode;

        // sparkbank will only ever be on a node with one connection, so this will work
        sparkScript.targetNode = parentNode.GetComponent<Node>().GetNextNode();
    }
}
