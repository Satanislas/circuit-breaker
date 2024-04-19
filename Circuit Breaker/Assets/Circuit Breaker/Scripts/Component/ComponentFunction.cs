using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentFunction : MonoBehaviour
{
    public int componentType;
    public int value;

    // various components have states. Here is what isActive = false means for all of them
    // Switch: switch is closed
    // Fuse: fuse has not broken
    // Lamp: lamp has not been lit
    // Capacitor: capacitor has no charge
    private bool isActive = false;

    private const int RESISTOR = 0;
    private const int BATTERY = 1;
    private const int SWITCH = 2;
    private const int FUSE = 3;
    private const int LAMP = 4;
    private const int CAPACITOR = 5;

    public void SparkActivate(Spark spark)
    {
        switch (componentType)
        {
            case RESISTOR:
                // removes value amount of charge
                spark.currentValue -= value;
                break;
            case BATTERY:
                // adds value amount of charge
                spark.currentValue += value;
                break;
            case FUSE:
                // breaks if too much charge goes across it
                if(spark.currentValue >= value)
                {
                    isActive = true;
                    // open wire
                }
                break;
            case LAMP:
                //lights if spark has enough to power it
                if(spark.currentValue >= value)
                {
                    spark.currentValue -= value;
                    isActive = true;
                }
                break;
            case CAPACITOR:
                //stores 1 charge for each spark that passes through it
                //can click on capacitor to release all charge at once as a single spark
                isActive = true;
                spark.currentValue--;
                value++;
                break;
            default:
                break;
        }
    }

    private void OnMouseDown()
    {
        switch (componentType)
        {
            case SWITCH:
                //switches the... switch... state
                isActive = !isActive;
                break;
            case CAPACITOR:
                if (isActive)
                {
                    //instantiate a new spark with the current value of value
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