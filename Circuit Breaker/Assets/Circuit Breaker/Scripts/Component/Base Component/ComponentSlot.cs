using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentSlot : MonoBehaviour
{
    [Header("Optional")]
    [Tooltip("Component to default to on wire.\nFill with a CircuitComponent prefab.")]
    public GameObject defaultComponent;

    public Transform activeComponent;

    public bool isLocked;

    [Header("Unity Settings")]
    [Tooltip("The transform that controls where the tile is located.\nCan also be used to position components.\nDo not change.")]
    public Transform tileSpot;
    [Tooltip("The visual of the empty tile. Child of tileSpot\nCan also be used to position components.\nDo not change.")]
    public Transform tileIcon;

    public Transform ActiveComponent{
        get { return activeComponent; }
        set 
        {
            activeComponent = value;

            //sets parentWire within ComponentFunction
            if (value != null)
            {
                Wire test = transform.GetComponent<Wire>();
                if(test != null)
                {
                    activeComponent.GetComponent<ComponentFunction>().parentWire = test;
                    activeComponent.GetComponent<AudioSource>().Play();
                }
            }
        }
    }

    private void Start()
    {
        // only do this if ComponentSlot NOT on a wire
        if(GetComponent<Wire>() == null)
        {
            tileSpot.position = transform.position;
        }
        
        SpawnComponent();
    }

    private void Update(){
        if(activeComponent != null){
            return;
        }

        Wire test = GetComponent<Wire>();

        if (test == null)
        {
            return;
        }

        GetComponent<Wire>().isOpen = false;
    }

    // spawns a default component if needed
    public void SpawnComponent()
    {
        // check if we should spawn with a component
        if (defaultComponent == null)
        {
            return;
        }

        // check if component already on wire
        if (activeComponent != null)
        {
            return;
        }

        Transform newTransform = Instantiate(defaultComponent, tileSpot.position + Vector3.back * 2, Quaternion.identity).transform;
        ActiveComponent = newTransform;

        float tileSpotYRotation = Mathf.Round(tileSpot.transform.eulerAngles.y);
        float yRotation = tileSpotYRotation == 0f ? 180f : 0f;
        float zRotation = tileSpotYRotation == 90f ? 360f - tileSpot.transform.eulerAngles.x : tileSpot.transform.eulerAngles.x;
        newTransform.rotation = Quaternion.Euler(0f, yRotation, zRotation);

        //newTransform.LookAt(newTransform.position + Vector3.forward, tileSpot.transform.right);

        ActiveComponent.GetComponent<CircuitComponent>().SetLastPlacedTileSlot(tileSpot.gameObject);
    }

    // positions the tile graphic
    public void PositionTileSpot(Transform[] nodePositions, float tileOffset)
    {
        float lerpX = Mathf.Lerp(nodePositions[0].position.x, nodePositions[1].position.x, tileOffset);
        float lerpY = Mathf.Lerp(nodePositions[0].position.y, nodePositions[1].position.y, tileOffset);

        Vector3 pos = new Vector3(lerpX, lerpY, 0f);
        tileSpot.position = pos;
        tileSpot.LookAt(nodePositions[0], Vector3.up);

        tileIcon.Translate(Vector3.back, Space.World);
        tileIcon.LookAt(tileIcon.position + Vector3.back, tileSpot.up);
    }
    
    void InteractWithComponent(Spark spark)
    {
        activeComponent.GetComponent<ComponentFunction>().SparkActivate(spark);
    }
}
