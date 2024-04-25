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
    [Header("Win Conditions")]
    public bool lampsWin = false;
    private bool isLampsOn = false;
    public bool reachEndWin = false;
    private bool allSparksOn = false;

    [Header("FOR UI")]
    public int lampCount = 0;
    public TextMeshProUGUI lampText;
    private int lampsOn = 0;

    private bool gameEnd = false;

    void Start()
    {
        // lampText.gameObject.SetActive(false);
        if(lampsWin)
        {
            // lampText.gameObject.SetActive(true);
            lampText.SetText($"Lamps: {lampsOn}/{lampCount}");
        }
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

    public void WinCondition()
    {
        if(lampsWin)
        {
            SparksReachEnd();
            GameComplete();
        }else if(reachEndWin)
        {
            SparksReachEnd();
            GameComplete();
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
        if(lampsWin)
        {
            if(isLampsOn && allSparksOn)
            {
                gameEnd = true;
                Debug.Log("Win Condition: allLampsOn");
                Debug.Log("LEVEL COMPLETE");
                
            }
        }else if(reachEndWin)
        {
            if(allSparksOn)
            {
                gameEnd = true;
                Debug.Log("Win Condition: reachEnd");
                Debug.Log("LEVEL COMPLETE");
            }
        }
    }
}
