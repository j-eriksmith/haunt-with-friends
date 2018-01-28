using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerHealth : NetworkBehaviour {
	public static int maxHealth = 3;
    //[SyncVar]
	// Use this for initialization
	void Start () {
	}

    public void TakeDamage(int amount)
    {
        if (!isServer) return;
        maxHealth -= amount;
        if (maxHealth <= 0)
        {
			Destroy (this.gameObject);
        }
    }
}
