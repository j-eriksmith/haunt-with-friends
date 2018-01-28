using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Interactable
{

    PlayerController pc;
    AudioClip interactSound;
    AudioSource audioSource;

    private void Start()
    {
        interactSound = Resources.Load<AudioClip>("Audio/keys-rattling");
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayInteractSound();
        }
    }

    public override void PlayInteractSound()
    {
        GetComponent<CircleCollider2D>().enabled = false;
        audioSource.Stop();
        audioSource.clip = interactSound;
        audioSource.loop = false;
        audioSource.Play();
        Destroy(this.gameObject, 2f);
    }
}
