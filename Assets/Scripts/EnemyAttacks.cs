using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EnemyAttacks : NetworkBehaviour {

	GameObject player;	
	public int damage;
	public float timeBetweenDamage = 1.5f;
	private float currTime = 0;
	private bool firstCollision = true;

    private void Start()
    {
        //GetComponent<AudioSource>().PlayDelayed(5f);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
		if (other.gameObject.tag == "Player" && checkMinimumDistance(other.gameObject))
        {
			if (firstCollision) {
				dealDamage (damage, other.gameObject);
				firstCollision = false;
            
				currTime += Time.deltaTime;  
			}
            if (currTime > timeBetweenDamage)
            {
                dealDamage(damage, other.gameObject);
                currTime = 0;
            }
        }
    }

	bool checkMinimumDistance(GameObject player)
	{
		return Vector3.Distance (player.transform.position, this.transform.position) <= 0.35f;
	}

    private void OnTriggerExit2D(Collider2D collision)
    {
        currTime = 0;
		firstCollision = true;

    }

	public void dealDamage(int damage, GameObject player)
	{
		print ("Damage taken");
		player.SendMessage ("TakeDamage", damage);
	}
}