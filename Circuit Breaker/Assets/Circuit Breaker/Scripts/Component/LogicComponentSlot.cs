using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicComponentSlot : MonoBehaviour
{
    public GameObject sparkPrefab;

    private int leftCharge, rightCharge;
    public GameObject attachedLogicComponent;
    private Coroutine timer;

    void Start() {
        attachedLogicComponent = null;
        leftCharge = 0;
        rightCharge = 0;
    }

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
            sparkScript.currentValue = charge;
            return;
        }

        if (leftCharge > 0 && rightCharge > 0) {
            StopCoroutine(timer);
            Debug.Log("TOTAL INPUT: " + (leftCharge + rightCharge));
            GameObject outputSpark = Instantiate(sparkPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
            Spark sparkScript = outputSpark.GetComponent<Spark>();
            sparkScript.startNode = transform;
            sparkScript.currentValue = leftCharge + rightCharge;
        } else {
            timer = StartCoroutine(WaitTimer());
        }
    }
}
