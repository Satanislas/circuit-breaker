using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBuildManager : MonoBehaviour
{
    public static PlayBuildManager instance;

    [Tooltip("Fill with all Sparkbanks in the scene. Once play mode initiates, sparks will be instantiated accordingly.")]
    public Sparkbank[] sparkBanks;

    [Tooltip("Used to determine build or play mode")]
    public bool isBuilding;

    public void setIsBuilding(bool value) => isBuilding = value;
    
    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            return;
        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            isBuilding = !isBuilding;
            Debug.Log("Building: " + isBuilding);

            if (!isBuilding)
            {
                SpawnSparks();
            }
            else
            {
                DestroySparks();
            }
        }
    }

    private void DestroySparks()
    {
        GameObject[] sparks = GameObject.FindGameObjectsWithTag("Spark");

        foreach (GameObject spark in sparks)
        {
            Destroy(spark);
        }
    }

    private void SpawnSparks()
    {
        foreach (Sparkbank script in sparkBanks)
        {
            script.SpawnSpark();
        }
    }
}
