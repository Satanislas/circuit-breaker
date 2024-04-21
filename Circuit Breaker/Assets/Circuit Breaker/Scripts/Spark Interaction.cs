using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkInteraction : MonoBehaviour
{
    [Header("Unity Setup")]
    public Spark sparkScript;

    private void OnTriggerEnter(Collider other)
    {
        ComponentFunction functionScript = other.GetComponent<ComponentFunction>();
        if (functionScript.IsPlaced)
        {
            functionScript.SparkActivate(sparkScript);
        }
    }
}
