using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NodeLogicComponentInput : MonoBehaviour
{
    public string side;

    private LogicComponentSlot parentLogicComponentSlot;

    void Start() {
        parentLogicComponentSlot = transform.parent.GetComponent<LogicComponentSlot>();
        Debug.Log(parentLogicComponentSlot);
    }

    public void StoreChargeInLogicComponent(int charge) {
        parentLogicComponentSlot.SetCharge(side, charge);
    }

    public Node GetParentNodeScript() {
        return transform.parent.gameObject.GetComponent<Node>();
    }
}
