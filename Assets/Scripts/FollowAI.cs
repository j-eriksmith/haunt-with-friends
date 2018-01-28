using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class FollowAI : NetworkBehaviour {

	GameObject player;
	private bool playerInRange;
	// Use this for initialization
	void Start()
	{
		player = GameObject.FindGameObjectWithTag ("Player");
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject == player) {
			playerInRange = true;
		}
	}

	void OnCollisionExit2D(Collision2D other)
	{
		if (other.gameObject == player) {
			playerInRange = false;
		}
	}
		
	// Update is called once per frame
	void Update()
	{
		if (playerInRange) {
			transform.position = Vector3.MoveTowards(player.transform.position, this.transform.position, 0);
		}	 
	}
}
