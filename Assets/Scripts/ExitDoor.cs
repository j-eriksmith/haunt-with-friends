using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().IncrementPlayersDone();
        }
    }

    public void Open()
    {
        GetComponent<AudioSource>().Play();
        GetComponent<CircleCollider2D>().enabled = true;
        GetComponent<Light>().enabled = true;
    }
}
