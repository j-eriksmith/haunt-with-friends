using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer) return;
        float x = Input.GetAxis("Horizontal") * Time.deltaTime * 10f;
        float y = Input.GetAxis("Vertical") * Time.deltaTime * 10f;

        transform.Translate(x, y, 0);
	}

    public override void OnStartLocalPlayer()
    {
        //Local player is initialized to blue color
        GetComponent<SpriteRenderer>().color = Color.blue;
    }
}
