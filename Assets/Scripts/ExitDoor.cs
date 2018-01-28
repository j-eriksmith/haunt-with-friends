using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Open()
    {
        GetComponent<AudioSource>().Play();
        GetComponent<CircleCollider2D>().enabled = true;
    }
}
