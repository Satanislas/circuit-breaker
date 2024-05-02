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

    private bool hasSpawned;

    private void Awake()
    {
        instance = this;
        hasSpawned = false;
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
                // destroy all sparks
            }
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
