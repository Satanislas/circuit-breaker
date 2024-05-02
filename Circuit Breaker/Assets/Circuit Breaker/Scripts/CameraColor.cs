using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraColor : MonoBehaviour
{
    public Color buildColor;
    public Color playColor;

    private void Update()
    {
        if (PlayBuildManager.instance.isBuilding)
        {
            SetBGColor(buildColor);
        }
        else
        {
            SetBGColor(playColor);
        }
    }

    public void SetBGColor(Color color)
    {
        GetComponent<Camera>().backgroundColor = color;
    }


}
