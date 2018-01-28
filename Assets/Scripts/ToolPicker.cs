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
        brushColor = new Color32(255, 127, 0, 255);
        brushSize = 3;
    }

    public void changeTool(int tool)
    {
        if(tool == 0)
        {
            brushColor = new Color32(255, 127, 0, 255);
            brushSize = 3;
            Debug.Log("Red Selected");
        }

        else if (tool == 1)
        {
            brushColor = new Color32(127, 0, 255, 255);
            brushSize = 3;
            Debug.Log("Green Selected");
        }

        else if (tool == 2)
        {
            brushColor = new Color32(0, 255, 0, 255);
            brushSize = 3;
            Debug.Log("Blue Selected");
        }

        else if(tool == 3)
        {
            brushColor = Color.clear;
            brushSize = 15;
            Debug.Log("Eraser Selected");
        }
    }
}
