using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EnemyAttacks : NetworkBehaviour {

	GameObject player;	
	public int damage;
	private bool playerInBounds = false;
	public float timeBetweenDamage = 1.5f;
	private float currTime = 0;
	private bool firstCollision = true;
	// Use this for initialization
	void Start()
	{
		this.player = GameObject.FindGameObjectWithTag ("Player");
	}

	// Update is called one per frame
	void Update()
	{
		if (player == null) {
			this.player = GameObject.FindGameObjectWithTag ("Player");
		} else {
			checkInBounds ();
			if (!playerInBounds) {
				firstCollision = true;
				currTime = 0;
			} else if (playerInBounds && firstCollision) {
				dealDamage (damage);
				currTime += Time.deltaTime;
				firstCollision = false;
			} else {
				currTime += Time.deltaTime;
				if (currTime > timeBetweenDamage) {
					dealDamage (damage);
					currTime = 0;
				}
			}
		}

	}

	void checkInBounds()
	{
		playerInBounds = Vector3.Distance (player.transform.position, this.transform.position) <= 0.1;
	}

	public void dealDamage(int damage)
	{
		if (playerInBounds) {
			print ("Take Damage");
			player.SendMessage ("TakeDamage", damage);
		}
	}


}