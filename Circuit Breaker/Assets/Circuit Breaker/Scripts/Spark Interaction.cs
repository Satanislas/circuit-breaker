using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class SparkInteraction : MonoBehaviour
{
    [Header("Unity Setup")]
    public Spark sparkScript;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("COLLIDING WITH: " + other.name);
        ComponentFunction functionScript = other.GetComponent<ComponentFunction>();

        // For interaction with circuit components on wires
        if (functionScript.IsPlaced)
        {
            functionScript.SparkActivate(sparkScript);
        }
    }

    public void HitLogicComponentInput(Transform logicComponentInput) {
        Debug.Log("Triggered!");

        // For interaction with logic components on nodes
        logicComponentInput.gameObject.GetComponent<NodeLogicComponentInput>().StoreChargeInLogicComponent(sparkScript.currentValue);
        Debug.Log("Hit a node input with charge " + sparkScript.currentValue);
        return;
    }
}
