using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

	private GameObject damage;

	// Use this for initialization
	void Start () {
		damage = GameObject.Find ("enemy");
		damage.SendMessage ("addCharacter", gameObject);
	}
	
	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer) return;
        float x = Input.GetAxis("Horizontal") * Time.deltaTime;
        float y = Input.GetAxis("Vertical") * Time.deltaTime;

        transform.Translate(x, y, 0);
	}

    public override void OnStartLocalPlayer()
    {
        //Local player is initialized to blue color
        //GetComponent<SpriteRenderer>().color = Color.clear;

        GetComponent<SpriteRenderer>().color = Color.blue;
    }
}
