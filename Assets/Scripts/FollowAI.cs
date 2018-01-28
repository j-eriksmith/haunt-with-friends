using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class FollowAI : NetworkBehaviour {

	HashSet<GameObject> container = new HashSet<GameObject>();
	public float speed = 0.5f;
	private bool playerInRange;
	// Use this for initialization
	void Start()
	{
	}

	void OnTriggerStay2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player") {
			playerInRange = true;
			container.Add(other.gameObject);
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player") {
			playerInRange = false;
			container.Remove(other.gameObject);
		}
	}

	GameObject chooseClosestPlayer()
	{
		GameObject closest_target = null; 
		float min_distance = 1000f;
		foreach (GameObject player in container) {
			float distance = Vector3.Distance (this.transform.position, player.transform.position);
			if (distance <= min_distance) {
				closest_target = player;
				min_distance = distance;
			}
		}
		return closest_target;
	}
		
	// Update is called once per frame
	void Update()
	{
		if (playerInRange) {
			//Vector2 delta =  player.transform.position
			GameObject targ = chooseClosestPlayer();
			this.transform.position = Vector3.MoveTowards(this.transform.position, targ.transform.position, speed * Time.deltaTime);
		}	 
	}
}
