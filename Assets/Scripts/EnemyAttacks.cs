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
	}

	// Update is called one per frame
	void Update()
	{
		
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

	private void addCharacter(GameObject player)
	{
		this.player = player;
	}

	void OnCollisionEnter2D(Collision2D Other)
	{
		if (Other.gameObject == player) {
			playerInBounds = true;
		}
	}

	void OnCollisionExit2D(Collision2D Other)
	{
		if (Other.gameObject == player) {
			playerInBounds = false;
		}
	}

	public void dealDamage(int damage)
	{
		if (playerInBounds) {
			print ("Take Damage");
			player.SendMessage ("TakeDamage", damage);
		}
	}


}