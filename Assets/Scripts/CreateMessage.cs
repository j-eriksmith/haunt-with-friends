using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateMessage : MonoBehaviour {
    private GameObject winText;
    private GameObject loseText;
    private GameObject button;

    public void enableWinObjects()
    {
        winText = GameObject.FindGameObjectWithTag("WinText");
        button = GameObject.FindGameObjectWithTag("QuitButton");

        winText.GetComponent<Text>().enabled = true;
        button.GetComponent<Button>().enabled = true;
        button.GetComponent<Image>().enabled = true;

        Transform textTransform = button.transform.GetChild(0);
        textTransform.gameObject.GetComponent<Text>().enabled = true;
    }

    public void enableLoseObjects()
    {
        loseText = GameObject.FindGameObjectWithTag("LoseText");
        button = GameObject.FindGameObjectWithTag("QuitButton");

        loseText.GetComponent<Text>().enabled = true;
        button.GetComponent<Button>().enabled = true;
        button.GetComponent<Image>().enabled = true;

        Transform textTransform = button.transform.GetChild(0);
        textTransform.gameObject.GetComponent<Text>().enabled = true;
    }
}
