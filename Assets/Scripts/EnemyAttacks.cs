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

    private void Start()
    {
        //GetComponent<AudioSource>().PlayDelayed(5f);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (firstCollision)
            {
                dealDamage(damage, other.gameObject);
                firstCollision = false;
            }
            currTime += Time.deltaTime;                
            if (currTime > timeBetweenDamage)
            {
                dealDamage(damage, other.gameObject);
                currTime = 0;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        currTime = 0;

    }

	public void dealDamage(int damage, GameObject player)
	{
		player.SendMessage ("TakeDamage", damage);
	}
}