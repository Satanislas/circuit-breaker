using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SparkUI : MonoBehaviour
{
    private bool mouseHover;
    public GameObject canvas;
    public TextMeshProUGUI text;
    private Spark spark;


    private void Start()
    {
        spark = GetComponent<Spark>();
    }

    private void OnMouseEnter()
    {
        canvas.SetActive(true);
        text.text = spark.currentValue.ToString();
    }

    private void OnMouseOver()
    {
        text.text = spark.currentValue.ToString();
    }

    private void OnMouseExit()
    {
        canvas.SetActive(false);
    }
}
