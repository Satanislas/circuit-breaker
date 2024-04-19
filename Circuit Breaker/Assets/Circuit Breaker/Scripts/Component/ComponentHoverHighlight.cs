using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentHoverHighlight : MonoBehaviour {
    public void SnapToComponentSlot(Transform slotTransform) {
        transform.position = new Vector3(slotTransform.position.x, slotTransform.position.y, -2f);
        transform.rotation = slotTransform.GetChild(0).transform.rotation;
    }
}
