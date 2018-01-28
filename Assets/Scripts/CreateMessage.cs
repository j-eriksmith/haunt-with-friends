using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateMessage : MonoBehaviour {
    private GameObject winText;
    //public static GameObject loseText;
    private GameObject button;

    public void enableWinObjects()
    {
        winText = GameObject.FindGameObjectWithTag("WinText");
        button = GameObject.FindGameObjectWithTag("QuitButton");

        winText.GetComponent<Text>().enabled = true;
        button.GetComponent<Button>().enabled = true;
        button.GetComponent<Image>().enabled = true;
        Debug.Log("hello i am activee");
    }

    public void enableLoseObjects()
    {
        winText = GameObject.FindGameObjectWithTag("WinText");
        button = GameObject.FindGameObjectWithTag("QuitButton");

        //loseText.SetActive(true);
        button.SetActive(false);
    }
}
