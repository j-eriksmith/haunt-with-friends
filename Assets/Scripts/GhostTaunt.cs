using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostTaunt : MonoBehaviour
{

    private float t = 0f;
    private bool canPlayTaunt;
    public float timeToRoll = 5f;
    public AudioClip[] tauntClips;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        Debug.Log(t);
        if (t >= timeToRoll)
        {
            int rand = Random.Range(0, 1) * 4;
            GetComponent<AudioSource>().clip = tauntClips[rand];
            if (canPlayTaunt)
            {
                GetComponent<AudioSource>().Play();
            }
            t = 0.0f;
        }
    }
}
