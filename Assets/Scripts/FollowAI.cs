using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class FollowAI : NetworkBehaviour {

	GameObject player;
	private float speed = 0.5f;
	private bool playerInRange;
	// Use this for initialization
	void Start()
	{
	}

	void OnTriggerStay2D(Collider2D other)
	{
		if (other.gameObject == player) {
			playerInRange = true;
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject == player) {
			playerInRange = false;
		}
	}
		
	// Update is called once per frame
	void Update()
	{
		if (player == null) {
			player = GameObject.FindGameObjectWithTag ("Player");
		} 
		else if (playerInRange) {
			//Vector2 delta =  player.transform.position
			this.transform.position = Vector3.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
		}	 
	}
}
