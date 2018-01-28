using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestController : MonoBehaviour{

    public bool moving = false;
    private const float SPEED = 2.4f;
    private Rigidbody2D rb;
    private bool[] dir = new bool[4];
    private AudioClip clipWood, clipCarpet, clipCon;
    private AudioSource audioSrc;
    private float longStepWait;

    // Use this for initialization
    void Start () {
        rb = this.GetComponent<Rigidbody2D>();
        audioSrc = GetComponent<AudioSource>();
        clipWood = Resources.Load<AudioClip>("Audio/wooden_footsteps");
        clipCarpet = Resources.Load<AudioClip>("Audio/");
        longStepWait = clipWood.length / 9;
	}

    IEnumerator wood_step_sound_guard() // Terminates a tiny amount early so that it can be called again without conflict
    {
        float time = 0.05f;
        int stepcount = 0;
        while(time < clipWood.length-0.05f)
        {
            time += Time.deltaTime;
            if(stepcount <= (int)(time/longStepWait))
            {
                stepcount++;
                print("step");
            }
            yield return null;
        }
    }
	
	// Update is called once per frame
	void Update () {

        // Movement code
        dir = new bool[] { Input.GetKey(KeyCode.UpArrow), Input.GetKey(KeyCode.DownArrow), 
            Input.GetKey(KeyCode.LeftArrow), Input.GetKey(KeyCode.RightArrow) };
        rb.velocity = new Vector2((System.Convert.ToInt32(Input.GetKey(KeyCode.RightArrow)) - System.Convert.ToInt32(Input.GetKey(KeyCode.LeftArrow))) * SPEED, 
            (System.Convert.ToInt32(Input.GetKey(KeyCode.UpArrow)) - System.Convert.ToInt32(Input.GetKey(KeyCode.DownArrow))) * SPEED);
        if((dir[0] ^ dir[1]) || (dir[2] ^ dir[3]))
        {
            moving = true;
        }
        else
        {
            moving = false;
        }

        // Footsteps code
        if(moving)
        {
            if(!audioSrc.isPlaying)
            {
                print("start");
                audioSrc.PlayOneShot(clipWood);
                StartCoroutine("wood_step_sound_guard");
            }
        }
        else
        {
            print("stop");
            audioSrc.Stop();
            StopCoroutine("wood_step_sound_guard");
        }
	}
}
