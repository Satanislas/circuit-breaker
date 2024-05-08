using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using TMPro;
using UnityEngine;

public class LampUI : MonoBehaviour
{
    public static LampUI Instance {get; private set;}
    public int lampCount = 0;

    public void Awake()
    {
        Instance =  this;
    }
    [Header("LEVEL")]
    public int currentLevel = 1;
    
    [Header("Win Conditions")]
    public bool lampsWin = false;
    private bool isLampsOn = false;
    public bool reachEndWin = false;
    private bool allSparksOn = false;

    [Header("FOR UI")]
    
    public TextMeshProUGUI lampText;
    private int lampsOn = 0;

    private bool gameEnd = false;
    public GameObject EndGameCanvas;

    void Start()
    {
        // lampText.gameObject.SetActive(false);
        if(lampsWin)
        {
            lampCount = 0;
            Node[] nodes = FindObjectsOfType<Node>();
            foreach (var node in nodes)
                if (node.isLamp)
                    lampCount++;
            
            // lampText.gameObject.SetActive(true);
            lampText.SetText($"Lamps: {lampsOn}/{lampCount}");
        }
        
        
        
    }
    

    public void IncreaseLampCount()
    {
        lampsOn++;
        lampText.SetText($"Lamps: {lampsOn}/{lampCount}");
        LampsOn();
    }

    public void LampsOn()
    {
        print("lamps on = " + lampsOn + " || lamps count = " + lampCount);
        if(lampsOn == lampCount)
        {
            isLampsOn = true;
            WinCondition();
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
            if(lampCount == lampsOn)
            {
                gameEnd = true;
                Debug.Log("Win Condition: allLampsOn");
                Win();

            }
        }else if(reachEndWin)
        {
            if(allSparksOn)
            {
                gameEnd = true;
                Debug.Log("Win Condition: reachEnd");
                Win();
            }
        }
    }

    private void Win()
    {
        Debug.Log("LEVEL COMPLETE");
        var sparks = FindObjectsOfType<Spark>();
        foreach (var spark in sparks)
        {
            spark.KillMe();
        }
        EndGameCanvas.SetActive(true);
        Debug.Log("Unlocking Next Level: " + currentLevel+1);
        LevelSelector.Instance.UnlockNextLevel(currentLevel);
    }
}
