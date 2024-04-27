using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ComponentFunction : MonoBehaviour
{
    [Header("Prefab Settings")]
    [Tooltip("Determines what kind of component this is." +
        "\n0: RESISTOR" +
        "\n1: BATTERY" +
        "\n2: SWITCH" +
        "\n3: FUSE" +
        "\n4: LAMP" +
        "\n5: CAPACITOR")]
    public int componentType;
    [Tooltip("Determines amount of charge to interact with. Has different uses per component" +
        "\nRESISTOR: How much charge to remove" +
        "\nBATTERY: How much charge to add" +
        "\nSWITCH: No use" +
        "\nFUSE: How big a spark the fuse can handle." +
        "\nLAMP: How much charge a spark must have in order to light" +
        "\nCAPACITOR: How much charge is currently being stored")]
    public int value;
    [Tooltip("The wire this component is currently on.")]
    public Wire parentWire;

    public bool IsPlaced
    {
        get { return parentWire != null; }
    }

    [Header("Unity Setup")]
    public GameObject sparkPrefab;

    // various components have states. Here is what isActive = false means for all of them
    // Switch: switch is closed
    // Fuse: fuse has not broken
    // Lamp: lamp has not been lit
    // Capacitor: capacitor has no charge
    public bool isActive = false;

    private const int RESISTOR = 0;
    private const int BATTERY = 1;
    private const int SWITCH = 2;
    private const int FUSE = 3;
    private const int LAMP = 4;
    private const int CAPACITOR = 5;

    public void SparkActivate(Spark spark)
    {
        if (!parentWire.IsConnectedTo(spark.startNode))
        {
            return;
        }

        switch (componentType)
        {
            case RESISTOR:
                // removes value amount of charge
                spark.currentValue -= value;
                Debug.Log("Resistor. Spark: " + spark.currentValue);
                break;
            case BATTERY:
                // adds value amount of charge
                spark.currentValue += value;
                Debug.Log("Battery. Spark: " + spark.currentValue);
                break;
            case FUSE:
                // breaks if too much charge goes across it
                if(spark.currentValue >= value)
                {
                    isActive = true;
                    parentWire.isOpen = true;
                    Debug.Log("Fuse Broken. Spark: " + spark.currentValue);
                }
                break;
            case LAMP:
                //lights if spark has enough to power it
                if(spark.currentValue >= value)
                {
                    spark.currentValue -= value;
                    isActive = true;
                    Debug.Log("Lamp lit. Spark: " + spark.currentValue);
                }
                break;
            case CAPACITOR:
                //stores 1 charge for each spark that passes through it
                //can click on capacitor to release all charge at once as a single spark
                isActive = true;
                spark.currentValue--;
                value++;
                Debug.Log("Capacitor. Spark: " + spark.currentValue);
                Debug.Log("Capacitor. Stored: " + value);
                break;
            default:
                break;
        }
    }

    public void ClickInteract()
    {
        if (!IsPlaced)
        {
            return;
        }

        switch (componentType)
        {
            case SWITCH:
                //switches the... switch... state
                isActive = !isActive;
                parentWire.isOpen = !parentWire.isOpen;
                break;
            case CAPACITOR:
                if (isActive)
                {
                    //Instantiate new spark
                    value = 0;
                    isActive = false;
                }
                else
                {
                    //empty capacitor feedback
                }
                break;
            default:
                break;
        }
    }
}