using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkInteraction : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enter " + other.transform.name);
    }
}
