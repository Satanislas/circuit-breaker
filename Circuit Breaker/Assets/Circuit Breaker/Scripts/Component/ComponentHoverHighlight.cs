using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentHoverHighlight : MonoBehaviour {
    public void SnapToComponentSlot(Transform slotTransform) {
        transform.position = new Vector3(slotTransform.position.x, slotTransform.position.y, -1f);
        transform.rotation = Quaternion.Euler(0f, 0f, slotTransform.rotation.eulerAngles.x);
    }
}
