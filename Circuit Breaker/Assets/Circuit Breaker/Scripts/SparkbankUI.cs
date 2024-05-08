using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SparkbankUI : MonoBehaviour
{
    public TextMeshProUGUI chargeText;

    private void Awake()
    {
        chargeText.text = GetComponent<Sparkbank>().value.ToString();
    }
}
