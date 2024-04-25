using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentHoverHighlight : MonoBehaviour {
    public void SnapToComponentSlot(Transform slotTransform) {
        transform.position = new Vector3(slotTransform.position.x, slotTransform.position.y, -2f);
        // Weird rotation to match the tileSpot's rotation
        //float yRotation = slotTransform.eulerAngles.y == 0f ? 0f : 180f;
        //float zRotation = slotTransform.eulerAngles.y == 90f ? slotTransform.eulerAngles.x : -slotTransform.eulerAngles.x;
        //transform.rotation = Quaternion.Euler(0f, yRotation, zRotation);

        transform.LookAt(transform.position + Vector3.back, slotTransform.transform.right);
    }
}
