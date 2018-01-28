using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable {

    PlayerController pc;
    AudioClip interactSound;
    AudioSource audioSource;

    private void Start()
    {
        interactSound = Resources.Load<AudioClip>("Audio/door-locked");
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController pc = collision.gameObject.GetComponent<PlayerController>();
            pc.setInteractable(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController pc = collision.gameObject.GetComponent<PlayerController>();
            pc.removeInteractable(this);
        }
    }

    public override void PlayInteractSound()
    {
        GetComponent<CircleCollider2D>().enabled = false;
        audioSource.Stop();
        audioSource.clip = interactSound;
        audioSource.loop = false;
        audioSource.Play();
        GetComponent<BoxCollider2D>().enabled = false;
    }
}
