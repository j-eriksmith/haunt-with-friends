using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolPicker : MonoBehaviour {
    public static Color brushColor{
        get;
        private set;
    }

    public static int brushSize {
        get;
        private set;
    }

    private void Start()
    {
        brushColor = Color.cyan;
        brushSize = 3;
    }

    public void changeTool(int tool)
    {
        if(tool == 0)
        {
            brushColor = Color.cyan;
            brushSize = 3;
            Debug.Log("Pen Selected");
        }

        else if(tool == 1)
        {
            brushColor = Color.clear;
            brushSize = 5;
            Debug.Log("Color Selected");
        }
    }
}
