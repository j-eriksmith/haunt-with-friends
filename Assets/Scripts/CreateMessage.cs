using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMessage : MonoBehaviour {
    public static GameObject winText;
    public static GameObject loseText;
    public static GameObject button;

    public void enableWinObjects()
    {
        winText.SetActive(true);
        button.SetActive(true);
        Debug.Log("hello i am activee");
    }

    public void enableLoseObjects()
    {
        loseText.SetActive(true);
        button.SetActive(false);
    }
}
