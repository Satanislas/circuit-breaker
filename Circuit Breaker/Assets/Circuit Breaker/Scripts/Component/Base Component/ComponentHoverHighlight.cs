using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentHoverHighlight : MonoBehaviour {
    public void SnapToComponentSlot(Transform slotTransform) {
        transform.position = new Vector3(slotTransform.position.x, slotTransform.position.y, -2f);
        // Weird rotation to match the tileSpot's rotation
            float tileSpotYRotation = Mathf.Round(slotTransform.transform.eulerAngles.y);
            float yRotation = tileSpotYRotation == 0f ? 180f : 0f;
            float zRotation = tileSpotYRotation == 90f ? 360f - slotTransform.transform.eulerAngles.x : slotTransform.transform.eulerAngles.x;
            transform.rotation = Quaternion.Euler(0f, yRotation, zRotation);

        // transform.LookAt(transform.position, slotTransform.transform.right);
    }
}
