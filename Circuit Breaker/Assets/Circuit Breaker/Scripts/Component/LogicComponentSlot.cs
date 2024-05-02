using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LogicComponentSlot : MonoBehaviour
{
    public GameObject sparkPrefab;
    public string logicGateName;

    private int leftCharge, rightCharge;
    public GameObject attachedLogicComponent;
    private Coroutine timer;

    void Start() {
        attachedLogicComponent = null;
        leftCharge = 0;
        rightCharge = 0;
    }

    // Wait 5 seconds. If no other sparks arrive within that time, reset both inputs
    private IEnumerator WaitTimer() {
        Debug.Log("Waiting for a charge");
        yield return new WaitForSeconds(5);
        Debug.Log("Done waiting");
        leftCharge = 0;
        rightCharge = 0;
    }

    public void SetCharge(string side, int charge) {
        if (side == "left") {
            leftCharge = charge;
        } else if (side == "right") {
            rightCharge = charge;
        }

        Debug.Log("left charge: " + leftCharge);
        Debug.Log("right charge: " + rightCharge);

        // If no logic component is attached, do not store a charge, just pass it through
        if (attachedLogicComponent == null) {
            GameObject outputSpark = Instantiate(sparkPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
            Spark sparkScript = outputSpark.GetComponent<Spark>();
            sparkScript.startNode = transform;
            sparkScript.initialValue = charge;
            return;
        }

        // Once both inputs have received a charge, produce an output
        if (leftCharge > 0 && rightCharge > 0) {
            StopCoroutine(timer);  // Cancel the 5 second timer
            OutputSpark();
        // If one spark input has been received, start a timer to wait for another spark
        } else {
            timer = StartCoroutine(WaitTimer());
        }
    }

    // Calculate the new charge for the output spark, then spawn it
    private void OutputSpark() {
        int outputSparkCharge = 0;
        switch (logicGateName) {
            case "and":
                outputSparkCharge = leftCharge + rightCharge;
                break;
            case "or":
                outputSparkCharge = Mathf.Min(leftCharge, rightCharge);
                break;
            case "xor":
                outputSparkCharge = Mathf.Abs(leftCharge - rightCharge);
                break;
            default:
                Debug.Log("ERROR: " + name + " has not been assigned a logic gate");
                break;
        }
        // Reset stored charges
        leftCharge = 0;
        rightCharge = 0;

        if (outputSparkCharge <= 0) {
            Debug.Log("Spark output is 0!");
            return;
        }

        // Spawn the new spark
        GameObject outputSpark = Instantiate(sparkPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        Spark sparkScript = outputSpark.GetComponent<Spark>();
        sparkScript.startNode = transform;
        sparkScript.initialValue = outputSparkCharge;
        Debug.Log("output spark charge: " + sparkScript.currentValue);
    }
}
