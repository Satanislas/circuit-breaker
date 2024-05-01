using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentSlot : MonoBehaviour
{
    [Header("Optional")]
    [Tooltip("Component to default to on wire.\nFill with a CircuitComponent prefab.")]
    public GameObject defaultComponent;

    private Transform activeComponent;

    [Header("Unity Settings")]
    [Tooltip("The transform that controls where the tile is located.\nCan also be used to position components.\nDo not change.")]
    public Transform tileSpot;
    [Tooltip("The visual of the empty tile. Child of tileSpot\nCan also be used to position components.\nDo not change.")]
    public Transform tileIcon;

    public Transform ActiveComponent{
        get { return activeComponent; }
        set 
        {
            //sets parentWire within ComponentFunction
            // if(value != null)
            // {
            //     value.transform.GetComponent<ComponentFunction>().parentWire = this;
            // }
            activeComponent = value; 
        }
    }

    private void Awake()
    {
        tileSpot.position = transform.position;
        SpawnComponent();
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
        StartCoroutine(SpawnDefaultComponent());
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

    IEnumerator SpawnDefaultComponent()
    {
        yield return new WaitForSeconds(1f);
        Instantiate(defaultComponent, tileSpot.position, tileSpot.rotation);
    }
}
