using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using TMPro;
using UnityEngine;

public class LampUI : MonoBehaviour
{
    public static LampUI Instance {get; private set;}

    void Awake()
    {
        Instance = this;
    }

    public int lampCount = 0;
    public TextMeshProUGUI lampText;
    private int lampsOn = 0;
    private bool isLampsOn = false;
    private bool allSparksOn = false;
    private bool gameEnd = false;

    void Start()
    {
        lampText.SetText($"Lamps: {lampsOn}/{lampCount}");
    }
    

    public void IncreaseLampCount()
    {
        lampsOn++;
        lampText.SetText($"Lamps: {lampsOn}/{lampCount}");
        Debug.Log(lampsOn);
        LampsOn();
    }

    public void LampsOn()
    {
        if(lampsOn == lampCount)
        {
            isLampsOn = true;
        }
    }

    public void SparksReachEnd()
    {
        Spark[] sparks = FindObjectsOfType<Spark>();
        foreach (Spark spark in sparks)
        {
            if(spark.IsEndNode())
            {
                allSparksOn = true;
            }else{
                allSparksOn = false;
                break;
            }
            Debug.Log($"ALLSPARKSON in for loop: {allSparksOn}");
        }
        Debug.Log($"ALL SPARKS ON: {allSparksOn}");
    }

    public void GameComplete()
    {
        if(isLampsOn && allSparksOn)
        {
            gameEnd = true;
            Debug.Log("LEVEL COMPLETE");
            Debug.Log("GO TO NEXT LEVEL");
        }
    }
}
