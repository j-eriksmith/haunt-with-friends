using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Ghost : NetworkBehaviour {

	private bool playerInBounds = false;
	private bool started = false;
	AudioSource audioSource;

	void Start()
	{
		audioSource = GetComponent<AudioSource>();
	}

	void OnTriggerStay2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player") {
			playerInBounds = true;
		}
	}

	void Update ()
	{
		if (!started && playerInBounds) {
			audioSource.Play ();
			started = true;
		} else if (started && !audioSource.isPlaying) {
			Destroy (this.gameObject);
		}
	}
}